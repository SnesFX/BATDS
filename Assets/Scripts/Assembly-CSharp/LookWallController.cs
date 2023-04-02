using UnityEngine;
using UnityEngine.UI;

public class LookWallController : MonoBehaviour
{
	public Image Overlay;

	internal bool IsOpen;

	private Interactable_Lookwall interact;

	[Header("Common")]
	public Sprite[] Common;

	[Header("Rare")]
	public Sprite Bendy;

	public Sprite Meatly;

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
		interact.enabled = true;
		Overlay.enabled = false;
		GameManager.Instance.Player.m_MovementLock.UnlockStatic();
		IsOpen = false;
	}

	public void Look(Interactable_Lookwall I)
	{
		interact = I;
		cooldown = 3;
		if (IsOpen)
		{
			Close();
			return;
		}
		if (!I.IsInitialized)
		{
			I.Index = Random.Range(0, Common.Length);
			int num = Random.Range(0, 100);
			if (num < 10)
			{
				I.Index = 101;
			}
			else if (num == 99)
			{
				I.Index = 102;
			}
			I.IsInitialized = true;
		}
		if (I.Index == 101)
		{
			Overlay.sprite = Bendy;
		}
		else if (I.Index == 102)
		{
			Overlay.sprite = Meatly;
		}
		else
		{
			Overlay.sprite = Common[I.Index];
		}
		I.enabled = false;
		Overlay.enabled = true;
		GameManager.Instance.Player.m_MovementLock.Lock(isStatic: true);
		IsOpen = true;
	}
}
