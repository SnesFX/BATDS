using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUIManager : MonoBehaviour
{
	[Header("WINDOWS")]
	public GameObject PauseMenu;

	public GameObject OptionsWindow;

	public GameObject QuitWindow;

	[Header("SCREEN FX")]
	public Image DeathBlackout;

	public UITransitionHelper BendySpotted;

	public CameraShake CamShake;

	public UITransitionHelper InsideLMS;

	[Header("TRANSITIONERS")]
	public UITransitionHelper Fader;

	public UITransitionHelper GameUIGroup;

	[Header("INTERACTION")]
	public BaseInteractable currentInteractable;

	public Image InteractPopup;

	public Image BorisStaminaBar;

	[Header("RECT")]
	public RectTransform CanvasRect;

	public RectTransform IOS_Scaler;

	[Header("TEXT AND ICONS")]
	public Text SectionName;

	public Text FloorNumber;

	public Sprite[] InteractIcons;

	[Header("POPUP")]
	public UITransitionHelper UIPopup;

	public Text UIPopupText;

	[Header("CHARACTER UP")]
	public UITransitionHelper CharacterPopup;

	public Text CharacterPopupNameText;

	public Text CharacterPopupExtraText;

	public List<string> UIPopupMessages = new List<string>();

	[Header("USEABLE THINGS")]
	public LockMinigame lockMinigame;

	public RecordPlayerController recordPlayer;

	public LookWallController LookWall;

	public ZipperMinigame zipperMinigame;

	public TapePlayerController TapePlayer;

	public NoteController NoteViewer;

	public FlushMinigame flushMinigame;

	private char[] FloorLeters = new char[26]
	{
		'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J',
		'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T',
		'U', 'V', 'W', 'X', 'Y', 'Z'
	};

	[Header("Shopping List")]
	public Image ShoppingListEmplate;

	public void Start()
	{
		switch (GameManager.ActiveBuildMode)
		{
		case BuildMode.MOBILE:
			InteractPopup.sprite = InteractIcons[0];
			break;
		case BuildMode.PC:
			InteractPopup.sprite = InteractIcons[1];
			break;
		}
	}

	public void ShowUIPopup(string PopupToAdd)
	{
		UIPopupMessages.Add(PopupToAdd);
	}

	public void ActivateDeathBlackout()
	{
		DeathBlackout.enabled = true;
	}

	public void SetInputUI(bool isJoystick)
	{
		if (GameManager.ActiveBuildMode != BuildMode.MOBILE)
		{
			if (isJoystick)
			{
				InteractPopup.sprite = InteractIcons[2];
			}
			else
			{
				InteractPopup.sprite = InteractIcons[1];
			}
		}
	}

	public void SetFloorName(string FloorArea, int floorNumber)
	{
		if (!(FloorArea == ""))
		{
			SectionName.text = FloorArea;
			if (floorNumber <= 0)
			{
				FloorNumber.text = "Level " + FloorLeters[Mathf.Abs(floorNumber)];
			}
			else
			{
				FloorNumber.text = "Level " + floorNumber;
			}
		}
	}

	public void Update()
	{
		if (UIPopupMessages.Count > 0 && UIPopup.IsTransitionComplete)
		{
			UIPopupText.text = UIPopupMessages[0];
			UIPopup.TransitionIn();
			UIPopupMessages.RemoveAt(0);
		}
		if ((bool)currentInteractable && currentInteractable.enabled)
		{
			SetInteractWorldPosition(currentInteractable.transform.position);
			if (!currentInteractable.CheckDistanceToPlayer())
			{
				currentInteractable = null;
			}
			InteractPopup.gameObject.SetActive(value: true);
		}
		else if (InteractPopup.gameObject.activeInHierarchy)
		{
			InteractPopup.gameObject.SetActive(value: false);
		}
		BorisStaminaBar.rectTransform.sizeDelta = new Vector2(Mathf.Clamp01(GameManager.Instance.BorisStamina / 0.005f) * 11f, 200f * GameManager.Instance.BorisStamina);
	}

	public void SetInteractWorldPosition(Vector3 worldPos)
	{
		Vector2 vector = GameManager.Instance.MainCamRef.WorldToViewportPoint(worldPos + Vector3.up * 4f);
		Vector2 anchoredPosition = new Vector2(vector.x * CanvasRect.sizeDelta.x - CanvasRect.sizeDelta.x * 0.5f, vector.y * CanvasRect.sizeDelta.y - CanvasRect.sizeDelta.y * 0.5f);
		InteractPopup.rectTransform.anchoredPosition = anchoredPosition;
	}

	public void RegisterShoppingCollectable(Sprite Icon, out Image ConnectedUI)
	{
		GameObject obj = Object.Instantiate(ShoppingListEmplate.gameObject, ShoppingListEmplate.transform.parent);
		obj.name = "Item_" + Icon.name;
		obj.transform.localScale = Vector3.one;
		obj.SetActive(value: true);
		Image component = obj.GetComponent<Image>();
		component.sprite = Icon;
		ConnectedUI = component;
	}

	public void ShowCharacterPopup(string CharacterName, string CharacterAbility = "")
	{
		CharacterPopupNameText.text = "Unlocked: " + CharacterName;
		string text = "";
		if (CharacterAbility != "")
		{
			text = "Special Ability: " + CharacterAbility;
		}
		CharacterPopupExtraText.text = text;
		CharacterPopup.TransitionIn();
	}
}
