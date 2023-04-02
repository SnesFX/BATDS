using UnityEngine;

public class Interactable_Patron : BaseInteractable
{
	private Animator Anim;

	public override void Start()
	{
		base.Start();
		Anim = GetComponent<Animator>();
	}

	public override void DoInteraction()
	{
		base.DoInteraction();
		if ((bool)Anim)
		{
			Anim.SetTrigger("Talk");
		}
	}
}
