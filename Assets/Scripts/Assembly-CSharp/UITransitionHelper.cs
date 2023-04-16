using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UITransitionHelper : MonoBehaviour
{
	private enum UiTransitionType
	{
		NONE = 0,
		FADE = 1,
		FILL = 2,
		SLIDE_VERTICAL = 3,
		SLIDE_HORIZONTAL = 4
	}

	[SerializeField]
	private UiTransitionType m_TransitionInType;

	[SerializeField]
	private UiTransitionType m_TransitionOutType;

	public bool StartOnLoad;

	public float FadeInDelay;

	public float FadeOutDelay;

	public float SlideSpeed = 400f;

	public Vector2 SlideInPositions;

	public float SlideOutPosition;

	public bool AutoFadeOut = true;

	public bool EffectRaycastTargetable = true;

	public MaskableGraphic m_Graphic;

	public CanvasGroup m_CanvasGroup;

	private Color Col;

	private bool m_TransitionInComplete;

	private bool m_IsTransitionComplete;

	private Coroutine m_ActiveCoroutine;

	private AudioClipPlayer Audio;

	public bool IsTransitionComplete => m_IsTransitionComplete;

	private void Start()
	{
		if (!m_Graphic)
		{
			m_Graphic = GetComponent<MaskableGraphic>();
		}
		if (!m_Graphic)
		{
			m_Graphic = base.gameObject.AddComponent<Image>();
			m_Graphic.enabled = false;
		}
		Audio = GetComponent<AudioClipPlayer>();
		Col = m_Graphic.color;
		if (StartOnLoad)
		{
			switch (m_TransitionInType)
			{
			case UiTransitionType.FADE:
				Col.a = 0f;
				m_Graphic.color = Col;
				if ((bool)m_CanvasGroup)
				{
					m_CanvasGroup.alpha = Col.a;
				}
				break;
			case UiTransitionType.FILL:
				((Image)m_Graphic).fillAmount = 0f;
				break;
			case UiTransitionType.SLIDE_VERTICAL:
			{
				RectTransform rectTransform2 = m_Graphic.rectTransform;
				rectTransform2.anchoredPosition = new Vector2(rectTransform2.anchoredPosition.x, SlideInPositions.x);
				break;
			}
			case UiTransitionType.SLIDE_HORIZONTAL:
			{
				RectTransform rectTransform = m_Graphic.rectTransform;
				rectTransform.anchoredPosition = new Vector2(SlideInPositions.x, rectTransform.anchoredPosition.y);
				break;
			}
			}
			TransitionIn();
		}
		else
		{
			m_IsTransitionComplete = true;
		}
	}

	public void TransitionIn(float Multiplier = 1f)
	{
		m_IsTransitionComplete = false;
		if (m_ActiveCoroutine != null)
		{
			StopCoroutine(m_ActiveCoroutine);
		}
		switch (m_TransitionInType)
		{
		case UiTransitionType.FADE:
			m_ActiveCoroutine = CoroutineRunner.Instance.StartCoroutine(FadeTransition(true, Multiplier));
			break;
		case UiTransitionType.FILL:
			m_ActiveCoroutine = CoroutineRunner.Instance.StartCoroutine(FillTransition(isTransitionIn: true));
			break;
		case UiTransitionType.SLIDE_VERTICAL:
			m_ActiveCoroutine = CoroutineRunner.Instance.StartCoroutine(SlideTransition(isTransitionIn: true, isVertical: true));
			break;
		case UiTransitionType.SLIDE_HORIZONTAL:
			m_ActiveCoroutine = CoroutineRunner.Instance.StartCoroutine(SlideTransition(isTransitionIn: true, isVertical: false));
			break;
		default:
			OnlyTransitionOut();
			break;
		}
	}

	public void TransitionOut(float Multiplier = 1f)
	{
		if (m_ActiveCoroutine != null)
		{
			StopCoroutine(m_ActiveCoroutine);
		}
		switch (m_TransitionOutType)
		{
		case UiTransitionType.FADE:
			m_ActiveCoroutine = CoroutineRunner.Instance.StartCoroutine(FadeTransition(false, Multiplier));
			break;
		case UiTransitionType.FILL:
			m_ActiveCoroutine = CoroutineRunner.Instance.StartCoroutine(FillTransition(isTransitionIn: false));
			break;
		case UiTransitionType.SLIDE_VERTICAL:
			m_ActiveCoroutine = CoroutineRunner.Instance.StartCoroutine(SlideTransition(isTransitionIn: false, isVertical: true));
			break;
		case UiTransitionType.SLIDE_HORIZONTAL:
			m_ActiveCoroutine = CoroutineRunner.Instance.StartCoroutine(SlideTransition(isTransitionIn: false, isVertical: false));
			break;
		}
	}

	public void ForceReset()
	{
		if (m_ActiveCoroutine != null)
		{
			StopCoroutine(m_ActiveCoroutine);
		}
		Col = m_Graphic.color;
		switch (m_TransitionInType)
		{
		case UiTransitionType.FADE:
			Col.a = 0f;
			m_Graphic.color = Col;
			if ((bool)m_CanvasGroup)
			{
				m_CanvasGroup.alpha = Col.a;
			}
			break;
		case UiTransitionType.FILL:
			((Image)m_Graphic).fillAmount = 0f;
			break;
		case UiTransitionType.SLIDE_VERTICAL:
		{
			RectTransform rectTransform2 = m_Graphic.rectTransform;
			rectTransform2.anchoredPosition = new Vector2(rectTransform2.anchoredPosition.x, SlideInPositions.x);
			break;
		}
		case UiTransitionType.SLIDE_HORIZONTAL:
		{
			RectTransform rectTransform = m_Graphic.rectTransform;
			rectTransform.anchoredPosition = new Vector2(SlideInPositions.x, rectTransform.anchoredPosition.y);
			break;
		}
		}
	}

	public void SetAlpha(float Alpha)
	{
		Col = m_Graphic.color;
		Col.a = Alpha;
		m_Graphic.color = Col;
		if ((bool)m_CanvasGroup)
		{
			m_CanvasGroup.alpha = Col.a;
		}
	}

	private void OnlyTransitionOut()
	{
		if (m_TransitionOutType != 0)
		{
			TransitionOut();
		}
	}

	private IEnumerator FadeTransition(bool isTransitionIn, float Multiplier = 1f)
	{
		float seconds = (isTransitionIn ? FadeInDelay : FadeOutDelay);
		yield return new WaitForSeconds(seconds);
		if (isTransitionIn && EffectRaycastTargetable)
		{
			m_Graphic.raycastTarget = true;
		}
		if ((bool)Audio && isTransitionIn)
		{
			Audio.PlayClip();
		}
		float toValue = (isTransitionIn ? 1f : 0f);
		while (Col.a != toValue)
		{
			Col.a = Mathf.MoveTowards(Col.a, toValue, 2f * Multiplier * Time.deltaTime);
			m_Graphic.color = Col;
			if ((bool)m_CanvasGroup)
			{
				m_CanvasGroup.alpha = Col.a;
			}
			yield return new WaitForEndOfFrame();
		}
		if (AutoFadeOut && isTransitionIn && m_TransitionOutType != 0)
		{
			TransitionOut();
			yield break;
		}
		if (!isTransitionIn && EffectRaycastTargetable)
		{
			m_Graphic.raycastTarget = false;
		}
		m_IsTransitionComplete = true;
	}

	private IEnumerator FillTransition(bool isTransitionIn)
	{
		float seconds = (isTransitionIn ? FadeInDelay : FadeOutDelay);
		yield return new WaitForSeconds(seconds);
		Image image = (Image)m_Graphic;
		if (isTransitionIn && EffectRaycastTargetable)
		{
			m_Graphic.raycastTarget = true;
		}
		if ((bool)Audio && isTransitionIn)
		{
			Audio.PlayClip();
		}
		float toValue = (isTransitionIn ? 1f : 0f);
		while (image.fillAmount != toValue)
		{
			image.fillAmount = Mathf.MoveTowards(image.fillAmount, toValue, 2f * Time.deltaTime);
			yield return new WaitForEndOfFrame();
		}
		if (AutoFadeOut && isTransitionIn && m_TransitionOutType != 0)
		{
			TransitionOut();
			yield break;
		}
		if (!isTransitionIn && EffectRaycastTargetable)
		{
			m_Graphic.raycastTarget = false;
		}
		m_IsTransitionComplete = true;
	}

	private IEnumerator SlideTransition(bool isTransitionIn, bool isVertical)
	{
		float seconds = (isTransitionIn ? FadeInDelay : FadeOutDelay);
		yield return new WaitForSeconds(seconds);
		RectTransform box = m_Graphic.rectTransform;
		Vector2 myVector = Vector2.zero;
		if (isTransitionIn && EffectRaycastTargetable)
		{
			m_Graphic.raycastTarget = true;
		}
		if ((bool)Audio && isTransitionIn)
		{
			Audio.PlayClip();
		}
		Vector2 toValue;
		if (isVertical)
		{
			toValue = new Vector2(box.anchoredPosition.x, SlideOutPosition);
			if (isTransitionIn)
			{
				toValue.y = SlideInPositions.y;
			}
		}
		else
		{
			toValue = new Vector2(SlideOutPosition, box.anchoredPosition.y);
			if (isTransitionIn)
			{
				toValue.x = SlideInPositions.y;
			}
		}
		while (box.anchoredPosition != toValue)
		{
			box.anchoredPosition = Vector3.MoveTowards(box.anchoredPosition, toValue, SlideSpeed * Time.deltaTime);
			yield return new WaitForEndOfFrame();
		}
		if (AutoFadeOut && isTransitionIn && m_TransitionOutType != 0)
		{
			TransitionOut();
			yield break;
		}
		if (!isTransitionIn && EffectRaycastTargetable)
		{
			m_Graphic.raycastTarget = false;
		}
		m_IsTransitionComplete = true;
	}
}
