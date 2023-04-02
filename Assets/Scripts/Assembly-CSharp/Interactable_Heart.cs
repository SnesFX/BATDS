using UnityEngine;

public class Interactable_Heart : BaseInteractable
{
	public MeshRenderer MeshToDisable;

	public override void Start()
	{
		base.Start();
	}

	public override void DoInteraction()
	{
		if ((bool)Audio)
		{
			Audio.PlayClip(0);
			Audio.PlayClip(1);
		}
		MeshToDisable.enabled = false;
		base.enabled = false;
		SaveManager.DATA.HeartCount++;
		SaveManager.Save();
	}
}
