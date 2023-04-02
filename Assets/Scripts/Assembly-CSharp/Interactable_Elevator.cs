using UnityEngine;

public class Interactable_Elevator : BaseInteractable
{
	public Animator Anim;

	public Transform CharacterParent;

	public bool IsSafehouseElevator;

	public override void DoInteraction()
	{
		base.DoInteraction();
		KBCharacterController player = GameManager.Instance.Player;
		player.CharacterArt.transform.SetParent(CharacterParent);
		player.CharacterArt.localPosition = Vector3.zero;
		player.CharacterArt.localEulerAngles = Vector3.zero;
		player.gameObject.SetActive(value: false);
		player.m_Animator.SetFloat("Speed", 0f);
		IsActive = false;
		if (!IsSafehouseElevator)
		{
			Anim.SetTrigger("Up");
			GameManager.Instance.MUSIC_MANAGER.ChangeMusic(GameManager.Instance.MUSIC_MANAGER.Victory, 2f, 1f, StopTrnasitions: true, Loop: false);
			GameManager.Instance.LevelWon();
			return;
		}
		Anim.SetTrigger("Down");
		PlayerPrefs.SetInt("TutorialComplete", 2);
		PlayerPrefs.Save();
		GameManager.Instance.MUSIC_MANAGER.ChangeMusic(null, 0.3f, 1f, StopTrnasitions: true);
		if (Interactable_Cage.ForceGangLevel)
		{
			Interactable_Cage.ForceGangLevel = false;
			GameManager.Instance.LoadNewLevel(LevelTheme.REGULAR_B);
		}
		else
		{
			GameManager.Instance.LoadNewLevel(LevelTheme.REGULAR_FLOOR);
		}
	}
}
