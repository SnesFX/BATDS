using System.Collections;
using InControl;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
	public AudioSource Music;

	public UITransitionHelper FadeOut;

	public Button PlayButton;

	private bool MenuAnimComplete;

	private int waitCount;

	public Animator introAnim;

	public UITransitionHelper Logo;

	public UITransitionHelper Menu;

	public UITransitionHelper copyright;

	public UITransitionHelper DLC;

	public CanvasGroup MenuGroup;

	private int DeleteDataCount;

	public GameObject DeleteDataWindow;

	private void Start()
	{
		LoadingController.LoadingComplete();
	}

	private void Update()
	{
		Music.volume = 1f - FadeOut.m_Graphic.color.a;
		if (InputManager.AnyKeyIsPressed || InputManager.ActiveDevice.AnyButtonIsPressed || Input.GetMouseButtonDown(0))
		{
			introAnim.SetTrigger("ForceComplete");
			Logo.SetAlpha(1f);
			Menu.SetAlpha(1f);
			copyright.SetAlpha(1f);
			DLC.SetAlpha(1f);
		}
		if (!MenuAnimComplete && Menu.m_Graphic.color.a >= 1f)
		{
			MenuGroup.blocksRaycasts = true;
			PlayButton.GetComponent<UINAVObject>().Select();
			MenuAnimComplete = true;
			StartCoroutine(TDInputManager.CalculateInputs());
		}
		if (MenuAnimComplete && waitCount < 3)
		{
			waitCount++;
		}
	}

	public void Play()
	{
		SaveManager.Load();
		LoadingController.IsLoading();
		if (waitCount >= 3)
		{
			FadeOut.TransitionIn();
			StartCoroutine(DelayPlay());
		}
	}

	private IEnumerator DelayPlay()
	{
		yield return new WaitForSeconds(4f);
		if (PlayerPrefs.GetInt("TutorialComplete", 1) == 2)
		{
			SceneManager.LoadScene("GameScene");
		}
		else
		{
			SceneManager.LoadScene("Intro");
		}
	}

	public void TryDeleteData()
	{
		if (DeleteDataCount < 4)
		{
			DeleteDataCount++;
			return;
		}
		DeleteDataCount = 0;
		DeleteDataWindow.SetActive(value: true);
	}

	public void DeleteData()
	{
		PlayerPrefs.DeleteAll();
	}
}
