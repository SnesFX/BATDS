using UnityEngine;
using UnityEngine.UI;

public class UIFollowObject : MonoBehaviour
{
	public Image Icon;

	public Text Tex;

	public BaseInteractable followTarget;

	public Vector3 Offset = new Vector3(0f, 5f, 3f);

	public bool greyOutIcon = true;

	public GameObject Child;

	private void Start()
	{
		if ((bool)Icon)
		{
			Icon.enabled = false;
		}
		if ((bool)Tex)
		{
			Tex.enabled = false;
		}
	}

	private void Update()
	{
		if ((bool)Icon)
		{
			if ((bool)followTarget && followTarget.isActiveAndEnabled)
			{
				Icon.enabled = true;
				if (!followTarget.IsActive && Icon.color == Color.white && greyOutIcon)
				{
					Icon.color *= 0.5f;
				}
			}
			else if (Icon.enabled)
			{
				Icon.enabled = false;
			}
			if ((bool)Child && Child.activeSelf != Icon.enabled)
			{
				Child.SetActive(Icon.enabled);
			}
		}
		if ((bool)Tex)
		{
			if ((bool)followTarget && followTarget.gameObject.activeInHierarchy)
			{
				Tex.enabled = true;
			}
			else if (Tex.enabled)
			{
				Tex.enabled = false;
			}
		}
	}
}
