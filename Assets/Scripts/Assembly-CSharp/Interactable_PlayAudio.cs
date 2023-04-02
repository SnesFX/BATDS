using UnityEngine;

public class Interactable_PlayAudio : BaseInteractable
{
	public Animator Anim;

	public override void Start()
	{
		base.Start();
	}

	public override void DoInteraction()
	{
		base.DoInteraction();
		if ((bool)Anim)
		{
			Anim.SetTrigger("Activate");
		}
	}
}
