using UnityEngine;
using UnityEngine.UI;

public class UI_CollectionCounter : MonoBehaviour
{
	public FollowElementType CountBasedOn;

	public int max;

	private void Start()
	{
		int num = 0;
		switch (CountBasedOn)
		{
		case FollowElementType.SCRAPS:
			num = ScrapsManager.GetScrapCount();
			break;
		case FollowElementType.ALICE_KEY:
			if (SaveManager.DATA.KEY)
			{
				num = 1;
			}
			break;
		case FollowElementType.FUSES:
			num = SaveManager.DATA.FUSES;
			break;
		case FollowElementType.CAGE_KEY:
			num = SaveManager.DATA.KEY2;
			break;
		case FollowElementType.CANDLES:
			num = SaveManager.DATA.Candles;
			break;
		case FollowElementType.MASK:
			if (SaveManager.DATA.Candles == 5)
			{
				num = 1;
			}
			break;
		}
		GetComponent<Text>().text = num + "/" + max;
	}
}
