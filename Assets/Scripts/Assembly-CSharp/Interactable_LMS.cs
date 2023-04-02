using UnityEngine;

public class Interactable_LMS : BaseInteractable
{
	private bool InUse;

	public Animator Anim;

	private float cooldown;

	public override void DoInteraction()
	{
		if (cooldown > 0f)
		{
			return;
		}
		Anim.SetTrigger("Activate");
		if (!InUse)
		{
			InUse = true;
			if ((bool)Audio)
			{
				Audio.PlayClip(0);
			}
			GameManager.Instance.GAME_UI_MANAGER.InsideLMS.TransitionIn();
			GameManager.Instance.DeactivatePlayer();
			GameManager.Instance.Player.TeleportTo(new Vector3(base.transform.position.x, GameManager.Instance.Player.transform.position.y, base.transform.position.z));
		}
		else
		{
			InUse = false;
			if ((bool)Audio)
			{
				Audio.PlayClip(1);
			}
			GameManager.Instance.GAME_UI_MANAGER.InsideLMS.TransitionOut();
			GameManager.Instance.Player.m_InputController.ForceFacingDirection(base.transform.forward);
			GameManager.Instance.ReactivatePlayer();
		}
	}

	public override void Update()
	{
		base.Update();
		if (cooldown > 0f)
		{
			cooldown -= Time.deltaTime;
		}
	}
}
