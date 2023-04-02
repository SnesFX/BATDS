using UnityEngine;

public class CameraController : MonoBehaviour
{
	public CameraMode m_Mode;

	[HideInInspector]
	public KBCharacterController m_FollowCharacter;

	[HideInInspector]
	public Camera m_Camera;

	private Transform m_CameraParent;

	[Header("1ST PERSON SETTINGS")]
	public float FP_CharacterHeight = 1f;

	public Vector2 FP_Sensitivity = new Vector2(1f, 1f);

	private float pitch;

	[Header("3RD PERSON SETTINGS")]
	public float TP_FollowDistance = 5f;

	public float TP_FollowHeight = 3f;

	public float TP_CameraTurnSensitivity = 0.25f;

	[HideInInspector]
	public bool m_UseMouse;

	[Header("TOP DOWN SETTINGS")]
	public Transform TD_PivotPoint;

	public Vector3 ShiftOffset;

	public float TD_FollowSpeed = 0.1f;

	public float TD_ShiftSpeed = 0.01f;

	private float ThirdPersonRotationDelta;

	private Vector2 FirstPersonMouseInput;

	private Vector3 LastPlayerPosition;

	private Vector3 MovementDir;

	public Transform m_Pitch;

	private Vector3 PitchPosition;

	private void Start()
	{
		Init();
	}

	public void Init()
	{
		if (Camera.main == null)
		{
			Debug.Log("A camera is needed in your scene!");
			return;
		}
		m_Camera = Camera.main;
		switch (m_Mode)
		{
		case CameraMode.THIRD_PERSON:
			m_FollowCharacter.m_RunSpeed = -0.2f;
			break;
		case CameraMode.FIRST_PERSON:
			m_FollowCharacter.m_RunSpeed = -0.2f;
			break;
		}
	}

	private void Update()
	{
		switch (m_Mode)
		{
		case CameraMode.THIRD_PERSON:
			Update3rdPerson();
			break;
		case CameraMode.FIRST_PERSON:
			Update1stPerson();
			break;
		}
	}

	private void FixedUpdate()
	{
		switch (m_Mode)
		{
		case CameraMode.THIRD_PERSON:
			Update3rdPerson();
			break;
		case CameraMode.FIRST_PERSON:
			Update1stPerson();
			break;
		case CameraMode.TOP_DOWN:
			UpdateTopDown();
			break;
		}
	}

	private void Update1stPerson()
	{
	}

	private void UpdateTopDown()
	{
		float num = 0f;
		if (Physics.Linecast(m_FollowCharacter.transform.position + Vector3.up * 1f, m_FollowCharacter.transform.position + Vector3.back * 5f + Vector3.up * 7f))
		{
			num = 10f;
		}
		TD_PivotPoint.localEulerAngles = Vector3.Lerp(TD_PivotPoint.localEulerAngles, new Vector3(num, 0f, 0f), 0.01f);
		base.transform.position = Vector3.Lerp(base.transform.position, m_FollowCharacter.transform.position, TD_FollowSpeed);
		Vector3 vector = m_FollowCharacter.CharacterArt.forward;
		if (num != 0f)
		{
			vector.z = 0f;
		}
		if (!m_FollowCharacter.isActiveAndEnabled)
		{
			vector = Vector3.zero;
		}
		TD_PivotPoint.localPosition = Vector3.MoveTowards(TD_PivotPoint.localPosition, ShiftOffset + vector * 3f, TD_ShiftSpeed);
		m_Pitch.transform.position = (PitchPosition = Vector3.Lerp(PitchPosition, TD_PivotPoint.position, 0.1f));
	}

	private void Update3rdPerson()
	{
		_ = ThirdPersonRotationDelta;
		_ = 0f;
	}

	public void Rotate3rdPerson(float DIR)
	{
	}
}
