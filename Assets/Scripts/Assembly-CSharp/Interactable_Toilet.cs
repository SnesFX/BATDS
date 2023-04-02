using UnityEngine;

public class Interactable_Toilet : BaseInteractable
{
	public Animator Anim;

	public Transform TapeLocation;

	public Interactable_Special chosenItem;

	private Transform spawnpoint;

	public override void Start()
	{
		base.Start();
		if ((bool)chosenItem)
		{
			chosenItem = Object.Instantiate(chosenItem);
			spawnpoint = TapeLocation;
			chosenItem.transform.SetParent(spawnpoint);
			chosenItem.transform.localPosition = Vector3.zero;
			chosenItem.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
			chosenItem.transform.localScale = Vector3.one;
			chosenItem.gameObject.SetActive(value: false);
		}
	}

	public override void DoInteraction()
	{
		base.DoInteraction();
		if (base.enabled)
		{
			GameManager.Instance.GAME_UI_MANAGER.flushMinigame.Activate(this);
		}
	}

	public void Open()
	{
		base.enabled = false;
		Anim.SetTrigger("Activate");
		if ((bool)chosenItem)
		{
			chosenItem.gameObject.SetActive(value: true);
		}
	}
}
