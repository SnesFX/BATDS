using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Interactable_Scraps : BaseInteractable
{
	public override void Start()
	{
		base.Start();
	}

	public override void DoInteraction()
	{
		base.DoInteraction();
		GameManager.Instance.Player.m_MovementLock.Lock(isStatic: true);
		GameManager.Instance.GAME_UI_MANAGER.Fader.TransitionIn();
		GameManager.Instance.GAME_UI_MANAGER.GameUIGroup.TransitionOut();
		GameManager.Instance.MUSIC_MANAGER.ChangeMusic(null, 2f);
		GameManager.Instance.MUSIC_MANAGER.SFXModifier = 0f;
		base.enabled = false;
		StartCoroutine(Wait());
	}

	private IEnumerator Wait()
	{
		yield return new WaitForSeconds(3f);
		SceneManager.LoadScene("Finalie", LoadSceneMode.Additive);
		yield return new WaitForSeconds(35f);
		SceneManager.UnloadSceneAsync("Finalie");
		GameManager.Instance.GAME_UI_MANAGER.Fader.TransitionOut();
		GameManager.Instance.GAME_UI_MANAGER.GameUIGroup.TransitionIn();
		GameManager.Instance.MUSIC_MANAGER.SFXModifier = 1f;
		GameManager.Instance.MUSIC_MANAGER.ChangeMusic(GameManager.Instance.MUSIC_MANAGER.AmbienceTrack, 2f);
		yield return new WaitForSeconds(1f);
		GameManager.Instance.Player.m_MovementLock.UnlockStatic();
	}
}
