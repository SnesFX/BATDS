using System.Collections;
using UnityEngine;

public class Interactable_Mask : BaseInteractable
{
	public GameObject Song;

	public UI_WorldFollowElement MaskIcon;

	public override void Start()
	{
		base.Start();
		if (SaveManager.DATA.Candles != 5 || SaveManager.DATA.FOUND_E)
		{
			IsActive = false;
		}
	}

	public override void DoInteraction()
	{
		StartCoroutine(UnlockSammy());
		Object.FindObjectOfType<Interactable_InkToy>().MakeAvailable(UseableCharacter.SAMMY);
	}

	private IEnumerator UnlockSammy()
	{
		SaveManager.DATA.FOUND_E = true;
		SaveManager.Save();
		IsActive = false;
		GameManager.Instance.Player.m_MovementLock.Lock(isStatic: true);
		GameManager.Instance.GAME_UI_MANAGER.Fader.TransitionIn();
		GameManager.Instance.GAME_UI_MANAGER.GameUIGroup.TransitionOut();
		yield return new WaitForSeconds(2f);
		Audio.PlayClip();
		Song.SetActive(value: true);
		MaskIcon.gameObject.SetActive(MaskIcon.CheckVisiblility(FollowElementType.MASK));
		yield return new WaitForSeconds(7f);
		GameManager.Instance.GAME_UI_MANAGER.Fader.TransitionOut();
		GameManager.Instance.GAME_UI_MANAGER.GameUIGroup.TransitionIn();
		yield return new WaitForSeconds(2f);
		GameManager.Instance.Player.m_MovementLock.UnlockStatic();
		GameManager.Instance.GAME_UI_MANAGER.ShowCharacterPopup("Sammy Lawrence", "Ink Portals");
	}
}
