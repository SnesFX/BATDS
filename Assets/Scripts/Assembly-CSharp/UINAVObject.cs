using UnityEngine;
using UnityEngine.UI;

public class UINAVObject : MonoBehaviour
{
	public Selectable UIObject;

	[Header("NavDirections")]
	public UINAVObject Up;

	public UINAVObject Down;

	public UINAVObject Left;

	public UINAVObject Right;

	[Header("Overrides")]
	public bool overrideColors;

	public Color DeselectedOverride = Color.white;

	public Color SelectedOverride = Color.white;

	public bool Disabled;

	public float DisabledMultiplier = 1f;

	public void Awake()
	{
		if (UIObject == null)
		{
			UIObject = GetComponent<Selectable>();
		}
		if (UINAVManager.Instance.currentNavObject != this)
		{
			Deselect();
		}
	}

	public void Select()
	{
		UINAVManager.Instance.Select(this);
	}

	public void SetSelected()
	{
		if (UIObject == null)
		{
			UIObject = GetComponent<Selectable>();
		}
		if (!overrideColors)
		{
			UIObject.targetGraphic.color = UINAVManager.Instance.SelectedColor;
		}
		else
		{
			UIObject.targetGraphic.color = SelectedOverride;
		}
		if (Disabled)
		{
			UIObject.targetGraphic.color *= DisabledMultiplier;
		}
	}

	public void Deselect()
	{
		if (!overrideColors)
		{
			UIObject.targetGraphic.color = UINAVManager.Instance.DeselectedColor;
		}
		else
		{
			UIObject.targetGraphic.color = DeselectedOverride;
		}
		if (Disabled)
		{
			UIObject.targetGraphic.color *= DisabledMultiplier;
		}
	}
}
