using UnityEngine;

public class Interactable_Old : BaseInteractable
{
	public Animator Anim;

	public override void DoInteraction()
	{
		base.DoInteraction();
		if ((bool)Anim)
		{
			Anim.SetTrigger("Activate");
		}
		SaveManager.DATA.RemainingBoxCount++;
	}
}
