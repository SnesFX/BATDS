using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class QuitGame : MonoBehaviour
{
	public Image Fader;

	public void Quit()
	{
		Fader.enabled = true;
		Fader.color = new Color(0f, 0f, 0f, 0f);
		StartCoroutine(DoQuit());
	}

	private IEnumerator DoQuit()
	{
		while (Fader.color.a < 1f)
		{
			Fader.color += new Color(0f, 0f, 0f, 5f * Time.unscaledDeltaTime);
			yield return new WaitForSecondsRealtime(0.001f);
		}
		if (GameManager.Instance != null)
		{
			Time.timeScale = 1f;
			GameManager.SpawnPointIndex = 0;
			LevelGenerator.CurrentTheme = LevelTheme.HUB_1;
			GameManager.Day = 1;
			GameManager.SavedItems.Clear();
			CosmeticController.HoldingBone = false;
			SceneManager.LoadScene("MainMenu");
		}
		else
		{
			Application.Quit();
		}
	}
}
