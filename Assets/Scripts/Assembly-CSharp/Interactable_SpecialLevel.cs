using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Interactable_SpecialLevel : BaseInteractable
{
	public float StartDelay;

	public LevelTheme NewLevel;

	public override void Start()
	{
		base.Start();
		if (NewLevel == LevelTheme.PROJECTIONIST_FLOOR && !SaveManager.DATA.FOUND_B)
		{
			base.gameObject.SetActive(value: false);
		}
		if (NewLevel == LevelTheme.ALICE_FLOOR)
		{
			if (!SaveManager.DATA.Tape_5)
			{
				base.gameObject.SetActive(value: false);
			}
			if (!SaveManager.DATA.KEY)
			{
				IsActive = false;
			}
		}
		if (NewLevel == LevelTheme.FUSE_GAME && SaveManager.DATA.FUSES < 6)
		{
			IsActive = false;
		}
	}

	public override void DoInteraction()
	{
		base.DoInteraction();
		GameManager.Instance.Player.m_MovementLock.Lock(isStatic: true);
		base.enabled = false;
		StartCoroutine(Wait());
	}

	private IEnumerator Wait()
	{
		yield return new WaitForSeconds(StartDelay);
		GameManager.Instance.GAME_UI_MANAGER.Fader.TransitionIn();
		GameManager.Instance.GAME_UI_MANAGER.GameUIGroup.TransitionOut();
		GameManager.Instance.MUSIC_MANAGER.ChangeMusic(null, 2f);
		GameManager.Instance.MUSIC_MANAGER.SFXModifier = 0f;
		yield return new WaitForSeconds(3f);
		if (NewLevel == LevelTheme.ALICE_FLOOR)
		{
			SceneManager.LoadScene("AliceCutscene");
		}
		if (NewLevel == LevelTheme.FUSE_GAME)
		{
			SceneManager.LoadScene("BallGame");
			yield break;
		}
		GameManager.Instance.LoadNewLevel(NewLevel);
		LoadingController.IsLoading();
	}
}
