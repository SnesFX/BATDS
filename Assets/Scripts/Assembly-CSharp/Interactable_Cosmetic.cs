public class Interactable_Cosmetic : BaseInteractable
{
	public enum CosmeticType
	{
		BONE = 0
	}

	public CosmeticType ObjType;

	public override void Start()
	{
		base.Start();
		if (ObjType == CosmeticType.BONE && (CosmeticController.HoldingBone || !SaveManager.DATA.SpecialBone))
		{
			base.gameObject.SetActive(value: false);
		}
	}

	public override void DoInteraction()
	{
		base.DoInteraction();
		if (ObjType == CosmeticType.BONE)
		{
			CosmeticController.Instance.GiveBone();
			base.gameObject.SetActive(value: false);
			base.enabled = false;
		}
	}
}
