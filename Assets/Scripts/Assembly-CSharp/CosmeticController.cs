using UnityEngine;

public class CosmeticController : MonoBehaviour
{
	public static CosmeticController Instance;

	public static bool HoldingBone;

	public GameObject Bone;

	private void Awake()
	{
		Instance = this;
		if (HoldingBone)
		{
			GiveBone();
		}
	}

	public void GiveBone()
	{
		Bone.SetActive(value: true);
		HoldingBone = true;
	}
}
