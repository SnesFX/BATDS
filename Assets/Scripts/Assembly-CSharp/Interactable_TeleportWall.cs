using System.Collections;
using UnityEngine;

public class Interactable_TeleportWall : BaseInteractable
{
	public override void Start()
	{
		base.Start();
		if (SaveManager.DATA.C != 8)
		{
			IsActive = false;
		}
		else
		{
			GameManager.Instance.TeleportLocations.Add(this);
		}
	}

	public override void DoInteraction()
	{
		if (GameManager.Instance.BorisStamina > 0.1f && GameManager.Instance.TeleportLocations.Count > 1)
		{
			StartCoroutine(TeleportPlayer());
			Audio.PlayClip(0);
		}
		else
		{
			Audio.PlayClip(1);
		}
	}

	private IEnumerator TeleportPlayer()
	{
		IsActive = false;
		GameManager.Instance.decreaseBorisStamina(0.2f);
		GameManager.Instance.GAME_UI_MANAGER.GameUIGroup.TransitionOut(2f);
		GameManager.Instance.GAME_UI_MANAGER.Fader.TransitionIn(2f);
		yield return new WaitForSeconds(0.5f);
		GameManager.Instance.Player.gameObject.SetActive(value: false);
		yield return new WaitForSeconds(0.25f);
		Interactable_TeleportWall newLocation = null;
		while (newLocation == null || newLocation == this)
		{
			newLocation = GameManager.Instance.TeleportLocations[Random.Range(0, GameManager.Instance.TeleportLocations.Count)];
			if (Physics.CheckSphere(newLocation.transform.position, 10f, LayerMask.GetMask("Enemy")))
			{
				Debug.LogError("ENEMY TOO CLOSE!");
				newLocation = null;
			}
			else
			{
				yield return new WaitForSeconds(0.15f);
			}
		}
		if (newLocation == null)
		{
			newLocation = this;
		}
		newLocation.IsActive = false;
		yield return new WaitForSeconds(0.25f);
		GameManager.Instance.Player.TeleportTo(newLocation.transform.position);
		GameManager.Instance.Player.gameObject.SetActive(value: true);
		GameManager.Instance.GAME_UI_MANAGER.GameUIGroup.TransitionIn(2f);
		GameManager.Instance.GAME_UI_MANAGER.Fader.TransitionOut(2f);
		yield return new WaitForSeconds(2f);
		IsActive = true;
		newLocation.IsActive = true;
	}
}
