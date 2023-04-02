using UnityEngine;

public class Interactable_Locker : BaseInteractable
{
	private static bool HasShownDay1Scrap;

	public Animator Anim;

	public Transform RecordLocation;

	public Transform ScrapLocation;

	public Transform SpecialBoneLocation;

	public Transform FuseLocation;

	private int SpawnType;

	public Interactable_Special chosenItem;

	private Transform spawnpoint;

	public override void Start()
	{
		base.Start();
		if ((bool)chosenItem)
		{
			chosenItem = Object.Instantiate(chosenItem);
			switch (chosenItem.ObjType)
			{
			case SpecialItemType.BONE:
				spawnpoint = SpecialBoneLocation;
				break;
			case SpecialItemType.RECORD:
				spawnpoint = RecordLocation;
				break;
			case SpecialItemType.SCRAP:
				spawnpoint = ScrapLocation;
				break;
			case SpecialItemType.FUSE:
				spawnpoint = FuseLocation;
				break;
			case SpecialItemType.KEY2:
				spawnpoint = FuseLocation;
				break;
			default:
				spawnpoint = FuseLocation;
				break;
			}
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
			GameManager.Instance.GAME_UI_MANAGER.lockMinigame.Activate(this);
		}
	}

	public void Open()
	{
		base.enabled = false;
		Anim.SetTrigger("Activate");
		if ((GameManager.Day != 1 || (!SaveManager.DATA.Scrap_1 && !HasShownDay1Scrap)) && (bool)chosenItem)
		{
			HasShownDay1Scrap = true;
			chosenItem.gameObject.SetActive(value: true);
		}
	}
}
