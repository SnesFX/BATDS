using UnityEngine;
using UnityEngine.UI;

public class Interactable_ShoppingCollectable : BaseInteractable
{
	public Sprite Icon;

	internal Image ConnectedUI;

	public ShoppingItem itemType;

	public override void Start()
	{
		base.Start();
		GameManager.Instance.GAME_UI_MANAGER.RegisterShoppingCollectable(Icon, out ConnectedUI);
		GameManager.Instance.ShoppingListGoal++;
	}

	public override void DoInteraction()
	{
		base.DoInteraction();
		ConnectedUI.color = Color.white;
		GameManager.Instance.GetShoppingItem(this);
		base.enabled = false;
		base.gameObject.SetActive(value: false);
	}

	public void AddIconToListOnly()
	{
		GameManager.Instance.GAME_UI_MANAGER.RegisterShoppingCollectable(Icon, out ConnectedUI);
		ConnectedUI.color = Color.white;
	}
}
