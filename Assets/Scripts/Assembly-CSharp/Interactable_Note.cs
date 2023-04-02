using UnityEngine;

public class Interactable_Note : BaseInteractable
{
	public string From;

	[Header("Content")]
	[TextArea]
	public string Content;

	public override void DoInteraction()
	{
		GameManager.Instance.GAME_UI_MANAGER.NoteViewer.Open(this, From, Content);
		base.DoInteraction();
	}
}
