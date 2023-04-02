using UnityEngine;
using UnityEngine.UI;

public class UI_CollectionIcon : MonoBehaviour
{
	public FollowElementType ColorBasedOn;

	private void Start()
	{
		float num = 1f;
		FollowElementType colorBasedOn = ColorBasedOn;
		if (colorBasedOn == FollowElementType.ALICE_KEY && !SaveManager.DATA.KEY)
		{
			num = 0.5f;
		}
		GetComponent<Image>().color = Color.white * num;
	}
}
