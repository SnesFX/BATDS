using UnityEngine;
using UnityEngine.UI;

public class NoteController : MonoBehaviour
{
	internal bool IsOpen;

	private Interactable_Note interact;

	public Text From;

	public Text Content;

	private int cooldown;

	private void Update()
	{
		if (cooldown > 0)
		{
			cooldown--;
		}
		else
		{
			if (!IsOpen)
			{
				return;
			}
			if (GameManager.ActiveBuildMode == BuildMode.PC)
			{
				if (TDInputManager.Interact == InputButtonState.DOWN || TDInputManager.Menu == InputButtonState.DOWN)
				{
					Close();
				}
			}
			else if (IsOpen && Input.GetMouseButtonUp(0))
			{
				Close();
			}
		}
	}

	public void Close()
	{
		cooldown = 3;
		GetComponent<UITransitionHelper>().TransitionOut();
		interact.enabled = true;
		GameManager.Instance.Player.m_MovementLock.UnlockStatic();
		IsOpen = false;
	}

	public void Open(Interactable_Note I, string FROM, string MSG)
	{
		interact = I;
		cooldown = 3;
		if (IsOpen)
		{
			Close();
			return;
		}
		From.text = "-" + FROM;
		Content.text = MSG;
		GetComponent<UITransitionHelper>().TransitionIn();
		I.enabled = false;
		GameManager.Instance.Player.m_MovementLock.Lock(isStatic: true);
		IsOpen = true;
	}
}
