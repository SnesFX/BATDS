using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Fuseball_Manager : MonoBehaviour
{
	private static Fuseball_Manager instance;

	private int Score;

	public Fuseball_Fuse[] Fuses;

	public Fuseball_Ball Ball;

	public Fuseball_Pattle Pattle;

	public ParticleSystem deathParticles;

	private Fuseball_Fuse lastFuse;

	public Transform BallLaunchPosition;

	private bool BallLaunched;

	public Image InteractPopup;

	public Sprite[] InteractIcons;

	public GameObject PauseMenu;

	public GameObject MobileUI;

	public UITransitionHelper Fader;

	public Text T_Score;

	public Text T_HighScore;

	public AudioSource MusicPlayer;

	public AudioClip[] SongList;

	public AudioClipPlayer SFXPlayer;

	public AudioClip Ambience;

	public bool GameStarted;

	private Fuseball_Fuse SelectedFuse;

	public static Fuseball_Manager Instance
	{
		get
		{
			if (instance == null)
			{
				instance = Object.FindObjectOfType<Fuseball_Manager>();
			}
			return instance;
		}
	}

	private IEnumerator Start()
	{
		Ball.Reset();
		Fader.TransitionOut();
		SaveManager.Load();
		LoadingController.LoadingComplete();
		T_HighScore.text = SaveManager.DATA.FBHS.ToString();
		int @int = PlayerPrefs.GetInt("CurrentTrack", 0);
		if (@int >= 0)
		{
			MusicPlayer.clip = SongList[@int];
		}
		else
		{
			MusicPlayer.clip = Ambience;
		}
		MusicPlayer.Play();
		SetInteractIcon();
		InteractPopup.enabled = false;
		yield return new WaitForSeconds(2f);
		Vector3 tar2 = new Vector3(1.4f, -0.7f, -1f);
		while (Pattle.transform.localPosition != tar2)
		{
			Pattle.transform.localPosition = Vector3.MoveTowards(Pattle.transform.localPosition, tar2, Pattle.PattleSpeed * Time.deltaTime);
			yield return new WaitForEndOfFrame();
		}
		yield return new WaitForSeconds(0.5f);
		tar2 = new Vector3(0f, -0.7f, -1f);
		while (Pattle.transform.localPosition != tar2)
		{
			Pattle.transform.localPosition = Vector3.MoveTowards(Pattle.transform.localPosition, tar2, Pattle.PattleSpeed * Time.deltaTime);
			yield return new WaitForEndOfFrame();
		}
		yield return new WaitForSeconds(0.5f);
		ActivateRandomFuse();
		yield return new WaitForSeconds(0.5f);
		InteractPopup.enabled = true;
		GameStarted = true;
	}

	public void SetInteractIcon()
	{
		switch (ControlSwapManager.CurrentControlMode)
		{
		case ControlMode.MOBILE:
			InteractPopup.sprite = InteractIcons[0];
			MobileUI.SetActive(value: true);
			break;
		case ControlMode.KEYBOARD:
			InteractPopup.sprite = InteractIcons[1];
			MobileUI.SetActive(value: false);
			break;
		case ControlMode.JOYSTICK:
			InteractPopup.sprite = InteractIcons[2];
			MobileUI.SetActive(value: false);
			break;
		}
	}

	private void Update()
	{
		MusicPlayer.volume = 0.5f * (1f - Fader.m_Graphic.color.a);
		if (Fader.IsTransitionComplete && GameStarted)
		{
			if (TDInputManager.Interact == InputButtonState.DOWN && Time.timeScale == 1f)
			{
				LaunchBall();
			}
			if (TDInputManager.Menu == InputButtonState.DOWN || TDInputManager.Run == InputButtonState.DOWN)
			{
				Pause();
			}
		}
	}

	public void LaunchBall()
	{
		if (!BallLaunched)
		{
			Ball.gameObject.SetActive(value: true);
			BallLaunched = true;
			InteractPopup.enabled = false;
			ActivateRandomFuse();
			SFXPlayer.PlayClip(0);
		}
	}

	public void BallLost()
	{
		if (Score > SaveManager.DATA.FBHS)
		{
			SaveManager.DATA.FBHS = Score;
			SaveManager.Save();
			T_HighScore.text = SaveManager.DATA.FBHS.ToString();
			if (Score >= 15 && !SaveManager.DATA.FOUND_C)
			{
				SaveManager.DATA.FOUND_C = true;
				SaveManager.Save();
				SFXPlayer.PlayClip(6);
			}
			else
			{
				SFXPlayer.PlayClip(4);
			}
		}
		else
		{
			SFXPlayer.PlayClip(3);
		}
		SelectedFuse.ResetFuse();
		SelectedFuse = null;
		Score = 0;
		deathParticles.transform.position = Ball.transform.position;
		deathParticles.Emit(8);
		BallLaunched = false;
		Ball.Reset();
		T_Score.text = Score.ToString();
		InteractPopup.enabled = true;
	}

	private void ActivateRandomFuse()
	{
		if (!(SelectedFuse != null))
		{
			SelectedFuse = Fuses[Random.Range(0, Fuses.Length)];
			if (SelectedFuse == lastFuse)
			{
				SelectedFuse = null;
				ActivateRandomFuse();
			}
			else
			{
				SelectedFuse.Activate();
				SFXPlayer.PlayClip(2);
			}
		}
	}

	public void FuseHit(Fuseball_Fuse last)
	{
		Score++;
		lastFuse = last;
		SelectedFuse = null;
		ActivateRandomFuse();
		T_Score.text = Score.ToString();
	}

	public void Pause()
	{
		if (Fader.IsTransitionComplete)
		{
			if (Time.timeScale == 1f)
			{
				Time.timeScale = 0f;
				PauseMenu.SetActive(value: true);
			}
			else
			{
				Resume();
			}
		}
	}

	public void Resume()
	{
		Time.timeScale = 1f;
		PauseMenu.SetActive(value: false);
	}

	public void Quit()
	{
		if (Score > SaveManager.DATA.FBHS)
		{
			SaveManager.DATA.FBHS = Score;
			SaveManager.Save();
			T_HighScore.text = SaveManager.DATA.FBHS.ToString();
			SFXPlayer.PlayClip(4);
		}
		Time.timeScale = 0.95f;
		PauseMenu.SetActive(value: false);
		Fader.TransitionIn();
		StartCoroutine(QuitEnum());
		GameManager.SpawnPointIndex = 2;
		LoadingController.IsLoading();
	}

	private IEnumerator QuitEnum()
	{
		while (!Fader.IsTransitionComplete)
		{
			yield return new WaitForEndOfFrame();
		}
		Time.timeScale = 1f;
		SceneManager.LoadScene("GameScene");
	}
}
