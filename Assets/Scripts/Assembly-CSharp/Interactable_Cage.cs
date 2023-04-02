using System.Collections;
using UnityEngine;

public class Interactable_Cage : BaseInteractable
{
	public static bool ForceGangLevel;

	public DisableObjectsBasedOnSaveData Cage;

	public override void Start()
	{
		base.Start();
		if (SaveManager.DATA.KEY2 < 5 || SaveManager.DATA.FOUND_D)
		{
			IsActive = false;
		}
	}

	public override void DoInteraction()
	{
		base.DoInteraction();
		StartCoroutine(UnlockCage());
	}

	private IEnumerator UnlockCage()
	{
		SaveManager.DATA.FOUND_D = true;
		SaveManager.Save();
		IsActive = false;
		GameManager.Instance.Player.m_MovementLock.Lock(isStatic: true);
		GameManager.Instance.GAME_UI_MANAGER.Fader.SetAlpha(1f);
		GameManager.Instance.GAME_UI_MANAGER.GameUIGroup.SetAlpha(0f);
		Cage.gameObject.SetActive(value: true);
		Cage.Check();
		ForceGangLevel = true;
		yield return new WaitForSeconds(4f);
		GameManager.Instance.GAME_UI_MANAGER.Fader.TransitionOut();
		GameManager.Instance.GAME_UI_MANAGER.GameUIGroup.TransitionIn();
		yield return new WaitForSeconds(2f);
		GameManager.Instance.Player.m_MovementLock.UnlockStatic();
		UIPopupMessage.ShowUniqueMessage("{Butcher Gang} encounters unlocked!");
	}
}
