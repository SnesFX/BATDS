using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroController : MonoBehaviour
{
	public float CutsceneRuntime = 33f;

	public LevelTheme LoadTheme;

	public GameObject SkipButton;

	public bool IsSkippable;

	private bool CanSkip;

	public GameObject Blocker;

	public AudioSource Music;

	private IEnumerator Start()
	{
		if (IsSkippable && PlayerPrefs.GetInt("CanSkipA", 0) == 1)
		{
			SkipButton.SetActive(value: true);
			CanSkip = true;
		}
		LoadingController.LoadingComplete();
		yield return new WaitForSeconds(33f);
		LoadingController.IsLoading();
		yield return new WaitForSeconds(3f);
		if (IsSkippable && PlayerPrefs.GetInt("CanSkipA", 0) == 0)
		{
			PlayerPrefs.SetInt("CanSkipA", 1);
		}
		LevelGenerator.CurrentTheme = LoadTheme;
		SceneManager.LoadScene("GameScene");
	}

	private void Update()
	{
		if ((CanSkip && TDInputManager.Run == InputButtonState.DOWN) || TDInputManager.Interact == InputButtonState.DOWN)
		{
			Skip();
		}
	}

	public void Skip()
	{
		Music.Stop();
		Blocker.SetActive(value: true);
		LoadingController.IsLoading();
		LevelGenerator.CurrentTheme = LoadTheme;
		SceneManager.LoadScene("GameScene");
	}
}
