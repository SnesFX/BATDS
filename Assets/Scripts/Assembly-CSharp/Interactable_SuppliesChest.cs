using UnityEngine;

public class Interactable_SuppliesChest : BaseInteractable
{
	public Animator Anim;

	public BaseInteractable[] Interactables;

	public UITransitionHelper VictoryMessage;

	public UI_WorldFollowElement SupplyChestIcon;

	public override void Start()
	{
		base.Start();
		if (GameManager.Instance.collectedItems.Count <= 0)
		{
			base.enabled = false;
			InteractablesActive(isActive: true);
		}
		else
		{
			InteractablesActive(isActive: false);
		}
	}

	public override void DoInteraction()
	{
		base.DoInteraction();
		Anim.SetTrigger("Activate");
		VictoryMessage.TransitionIn();
		SupplyChestIcon.Deactivate();
		for (int i = 0; i < GameManager.Instance.collectedItems.Count; i++)
		{
			GameManager.Instance.collectedItems[i].ConnectedUI.enabled = false;
		}
		GameManager.Instance.collectedItems.Clear();
		base.enabled = false;
		InteractablesActive(isActive: true);
	}

	private void InteractablesActive(bool isActive)
	{
		BaseInteractable[] interactables = Interactables;
		for (int i = 0; i < interactables.Length; i++)
		{
			interactables[i].enabled = isActive;
		}
	}
}
