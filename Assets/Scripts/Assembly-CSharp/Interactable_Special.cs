using UnityEngine;

public class Interactable_Special : BaseInteractable
{
	public SpecialItemType ObjType;

	public int ItemNumber;

	public override void Start()
	{
		base.Start();
		base.gameObject.SetActive(!hasbeencollected());
	}

	public override void DoInteraction()
	{
		base.DoInteraction();
		switch (ObjType)
		{
		case SpecialItemType.RECORD:
			UIPopupMessage.ShowUniqueMessage("new safehouse {tune} found!");
			switch (ItemNumber)
			{
			case 1:
				SaveManager.DATA.Record_1 = true;
				break;
			case 2:
				SaveManager.DATA.Record_2 = true;
				break;
			case 3:
				SaveManager.DATA.Record_3 = true;
				break;
			case 4:
				SaveManager.DATA.Record_4 = true;
				break;
			case 5:
				SaveManager.DATA.Record_5 = true;
				break;
			case 6:
				SaveManager.DATA.Record_6 = true;
				break;
			}
			break;
		case SpecialItemType.SCRAP:
			UIPopupMessage.ShowUniqueMessage("new safehouse {scrap} found!");
			switch (ItemNumber)
			{
			case 1:
				SaveManager.DATA.Scrap_1 = true;
				break;
			case 2:
				SaveManager.DATA.Scrap_2 = true;
				break;
			case 3:
				SaveManager.DATA.Scrap_3 = true;
				break;
			case 4:
				SaveManager.DATA.Scrap_4 = true;
				break;
			case 5:
				SaveManager.DATA.Scrap_5 = true;
				break;
			case 6:
				SaveManager.DATA.Scrap_6 = true;
				break;
			case 7:
				SaveManager.DATA.Scrap_7 = true;
				break;
			}
			break;
		case SpecialItemType.BONE:
			UIPopupMessage.ShowUniqueMessage("{favorite bone} Recovered!");
			CosmeticController.Instance.GiveBone();
			SaveManager.DATA.SpecialBone = true;
			break;
		case SpecialItemType.TAPE:
			UIPopupMessage.ShowUniqueMessage("new safehouse {Tape} found!");
			switch (ItemNumber)
			{
			case 1:
				SaveManager.DATA.Tape_1 = true;
				break;
			case 2:
				SaveManager.DATA.Tape_2 = true;
				break;
			case 3:
				SaveManager.DATA.Tape_3 = true;
				break;
			case 4:
				SaveManager.DATA.Tape_4 = true;
				break;
			case 5:
				SaveManager.DATA.Tape_5 = true;
				break;
			}
			break;
		case SpecialItemType.KEY:
			UIPopupMessage.ShowUniqueMessage("safehouse {Key} found!");
			SaveManager.DATA.KEY = true;
			break;
		case SpecialItemType.FUSE:
			UIPopupMessage.ShowUniqueMessage("found An {Odd fuse}!");
			SaveManager.DATA.FUSES++;
			break;
		case SpecialItemType.KEY2:
			UIPopupMessage.ShowUniqueMessage("Padlock {Key} found!");
			SaveManager.DATA.KEY2++;
			break;
		case SpecialItemType.LOST_PAPER:
			SaveManager.DATA.PAPER++;
			if (SaveManager.DATA.PAPER >= 5)
			{
				GameManager.Instance.GAME_UI_MANAGER.ShowCharacterPopup("Lost One");
			}
			else
			{
				UIPopupMessage.ShowUniqueMessage("{LOST Paper} found!");
			}
			break;
		case SpecialItemType.CANDLE:
			SaveManager.DATA.Candles++;
			if (SaveManager.DATA.Candles == 4)
			{
				UIPopupMessage.ShowUniqueMessage("all {Candles} found!");
			}
			else
			{
				UIPopupMessage.ShowUniqueMessage("found A {Candle}!");
			}
			break;
		case SpecialItemType.MASK:
			SaveManager.DATA.Candles = 5;
			UIPopupMessage.ShowUniqueMessage("{Sammys mask} found!");
			break;
		case SpecialItemType.BLUEPRINT:
			SaveManager.DATA.Blueprints++;
			UIPopupMessage.ShowUniqueMessage("{Dance Blueprint} found!");
			break;
		}
		Debug.Log(ObjType.ToString() + " " + ItemNumber + " Collected");
		SaveManager.Save();
		base.enabled = false;
		base.gameObject.SetActive(value: false);
	}

	private bool hasbeencollected()
	{
		if (ObjType == SpecialItemType.RECORD)
		{
			switch (ItemNumber)
			{
			case 1:
				return SaveManager.DATA.Record_1;
			case 2:
				return SaveManager.DATA.Record_2;
			case 3:
				return SaveManager.DATA.Record_3;
			case 4:
				return SaveManager.DATA.Record_4;
			case 5:
				return SaveManager.DATA.Record_5;
			case 6:
				return SaveManager.DATA.Record_6;
			}
		}
		else if (ObjType == SpecialItemType.SCRAP)
		{
			switch (ItemNumber)
			{
			case 1:
				return SaveManager.DATA.Scrap_1;
			case 2:
				return SaveManager.DATA.Scrap_2;
			case 3:
				return SaveManager.DATA.Scrap_3;
			case 4:
				return SaveManager.DATA.Scrap_4;
			case 5:
				return SaveManager.DATA.Scrap_5;
			case 6:
				return SaveManager.DATA.Scrap_6;
			case 7:
				return SaveManager.DATA.Scrap_7;
			}
		}
		else if (ObjType == SpecialItemType.TAPE)
		{
			switch (ItemNumber)
			{
			case 1:
				return SaveManager.DATA.Tape_1;
			case 2:
				return SaveManager.DATA.Tape_2;
			case 3:
				return SaveManager.DATA.Tape_3;
			case 4:
				return SaveManager.DATA.Tape_4;
			case 5:
				return SaveManager.DATA.Tape_5;
			}
		}
		else
		{
			if (ObjType == SpecialItemType.BONE)
			{
				return SaveManager.DATA.SpecialBone;
			}
			if (ObjType == SpecialItemType.KEY)
			{
				return SaveManager.DATA.KEY;
			}
			if (ObjType == SpecialItemType.FUSE)
			{
				return SaveManager.DATA.FUSES >= 6;
			}
			if (ObjType == SpecialItemType.KEY2)
			{
				return SaveManager.DATA.KEY2 >= 5;
			}
			if (ObjType == SpecialItemType.LOST_PAPER)
			{
				return SaveManager.DATA.PAPER >= 5;
			}
			if (ObjType == SpecialItemType.CANDLE)
			{
				return SaveManager.DATA.Candles >= 5;
			}
			if (ObjType == SpecialItemType.MASK)
			{
				return SaveManager.DATA.Candles == 5;
			}
		}
		return false;
	}
}
