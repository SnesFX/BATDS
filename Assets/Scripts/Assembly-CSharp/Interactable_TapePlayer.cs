public class Interactable_TapePlayer : BaseInteractable
{
	public TapePlayerController TapeController;

	public override void Start()
	{
		base.Start();
		TapeController.Init();
	}

	public override void DoInteraction()
	{
		base.DoInteraction();
		TapeController.Open();
		base.enabled = false;
	}
}
