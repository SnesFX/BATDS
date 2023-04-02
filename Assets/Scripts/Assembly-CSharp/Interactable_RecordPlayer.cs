public class Interactable_RecordPlayer : BaseInteractable
{
	public RecordPlayerController RecordController;

	public override void Start()
	{
		base.Start();
		RecordController.Init();
	}

	public override void DoInteraction()
	{
		base.DoInteraction();
		RecordController.Open();
		RecordController.m_RecordPlayers.SetActive(isActive: false);
	}
}
