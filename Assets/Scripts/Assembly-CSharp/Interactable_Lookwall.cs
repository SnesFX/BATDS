public class Interactable_Lookwall : BaseInteractable
{
	public int Index;

	public bool IsInitialized;

	public bool DidLook;

	public override void DoInteraction()
	{
		GameManager.Instance.GAME_UI_MANAGER.LookWall.Look(this);
		if (!DidLook && Index == 101)
		{
			Audio.PlayClip();
		}
		DidLook = true;
	}
}
