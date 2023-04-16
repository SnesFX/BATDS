using System.Collections;
using UnityEngine;

public class KBCharacterController : MonoBehaviour
{
	public enum InputControllerMode
	{
		NONE = 0,
		PLAYER = 1,
		AI_MELEE = 2,
		AI_MONSTER = 3
	}

	public enum EdgeResponse
	{
		DONT_FALL = 0
	}

	public enum AiDetectionMode
	{
		NOTHING = 0,
		FOLLOW_TARGET = 1
	}

	private Vector3 m_PreviousLocationForAnim;

	[Header("Initialization")]
	[Tooltip("cannot be changed at runtime")]
	public InputControllerMode m_ControlMode;

	[Header("Character Props")]
	public EdgeResponse m_EdgeRespose;

	[Header("AI Props")]
	public float SightDistance = 28f;

	[Range(0f, 360f)]
	public float SightAngle = 60f;

	public float LoseTrackingDistance = 1000f;

	public LayerMask TargetableLayer;

	public AiDetectionMode DetectionMode;

	public float SpotTargetDelayTime;

	public LayerMask m_SightBlockingLayers;

	public LayerMask m_AllyLayer;

	public bool DisableCameraFX;

	[Header("Audio Props")]
	public AudioClip[] SpottedPlayerClips;

	public AudioClip[] LostPlayerClips;

	public AudioClip ChaseMusic;

	public AudioClip CaptureMusic;

	public AudioSource DialogueSource;

	public AudioSource IdleAudio;

	public bool DisableIdleAudioOnChase;

	[Header("Physics Props")]
	public float m_MaxHP;

	public float m_WalkSpeed = 1.2f;

	public float m_RunSpeed = 2.04f;

	public float m_JumpPower = 2f;

	[Header("Object References")]
	public Animator m_Animator;

	[HideInInspector]
	public CameraController m_CameraController;

	public Transform CharacterArt;

	public Joystick MobileJoystick;

	[Header("Physics Props")]
	[SerializeField]
	public LayerMask m_ColisionLayers = 1;

	[SerializeField]
	private float m_ColisionRadious = 0.5f;

	[SerializeField]
	private float m_CharacterHeight = 1f;

	internal float SpeedDifference;

	public IntLock m_MovementLock = new IntLock("Character_Movement");

	[HideInInspector]
	public IntLock m_ActionLock = new IntLock("Character_Actions");

	private float m_GravityAmmount = 0.008f;

	private Vector3 m_PlayerCalculatedPosition;

	private Vector3 m_ExternalForce;

	private Vector3 m_movementDifference;

	private float ZeroFix;

	private float m_HP;

	private float m_ActiveGravity;

	private bool m_IsGrounded;

	private bool m_HasJumped;

	public bool CharacterAbleToRun = true;

	internal bool m_CanRun = true;

	internal KBInputController m_InputController;

	private Collider m_AttachedCollider;

	public Vector3 m_PreviousLocation { get; private set; }

	public bool IsGrounded => m_IsGrounded;

	public void Start()
	{
		switch (m_ControlMode)
		{
		case InputControllerMode.PLAYER:
			m_InputController = new PlayerController_TopDown();
			break;
		case InputControllerMode.AI_MELEE:
			m_InputController = new AIController_Melee();
			break;
		case InputControllerMode.AI_MONSTER:
			m_InputController = new AIController_Monster();
			break;
		default:
			m_InputController = new KBInputController();
			break;
		}
		m_InputController.Initialize(this);
		m_HP = m_MaxHP;
		m_PlayerCalculatedPosition = base.transform.position;
	}

	public void OnEnable()
	{
		if (CharacterArt == null)
		{
			base.enabled = false;
		}
		StartCoroutine(CheckMovementChange());
	}

	public void OnDisable()
	{
		StopAllCoroutines();
	}

	public virtual void Update()
	{
		if (m_ControlMode != 0)
		{
			m_InputController.DOUpdate();
			m_MovementLock.DoUpdate();
			m_ActionLock.DoUpdate();
			SolveMovementAnimation();
		}
	}

	private void FixedUpdate()
	{
		m_InputController.DOFixedUpdate();
		m_PreviousLocation = base.transform.position;
		if (m_PlayerCalculatedPosition != m_PreviousLocation && !m_MovementLock.IsLocked())
		{
			base.transform.position = m_PlayerCalculatedPosition;
		}
		Debug.DrawRay(base.transform.position + Vector3.up * 1.5f, (m_PlayerCalculatedPosition - m_PreviousLocation).normalized * 5f);
		if ((bool)CharacterArt && Physics.Raycast(base.transform.position + Vector3.up * 1.5f, (m_PlayerCalculatedPosition - m_PreviousLocation).normalized, 5f, m_AllyLayer, QueryTriggerInteraction.Ignore))
		{
			base.transform.position += CharacterArt.right * 3f * Time.deltaTime;
		}
		m_PlayerCalculatedPosition = base.transform.position;
	}

	public IEnumerator CheckMovementChange()
	{
		while (true)
		{
			yield return new WaitForSeconds(0.05f);
			m_movementDifference = base.transform.position - m_PreviousLocationForAnim;
			m_PreviousLocationForAnim = base.transform.position;
		}
	}

	public void Move(Vector3 direction, float multiplier = 1f)
	{
		float num = (m_InputController.isRunning ? m_RunSpeed : m_WalkSpeed);
		m_PlayerCalculatedPosition = base.transform.position + direction.normalized * multiplier * (num * 0.1f);
	}

	public void MoveTo(Vector3 newPosition)
	{
		m_PlayerCalculatedPosition = newPosition;
	}

	public void TeleportTo(Vector3 newPosition)
	{
		base.transform.position = newPosition;
		m_PlayerCalculatedPosition = newPosition;
		m_PreviousLocation = newPosition;
	}

	public void Jump(bool SetJumpFlag = true)
	{
		if (m_IsGrounded && !m_HasJumped)
		{
			m_ActiveGravity += m_JumpPower * 0.1f;
			if (SetJumpFlag)
			{
				m_HasJumped = true;
			}
		}
	}

	public void JumpReset()
	{
		m_HasJumped = false;
	}

	private void CalculateDiff()
	{
		Vector3 playerCalculatedPosition = m_PlayerCalculatedPosition;
		Vector3 previousLocation = m_PreviousLocation;
		playerCalculatedPosition.y = 0f;
		previousLocation.y = 0f;
	}

	private void SolveColision()
	{
		_ = Vector3.zero;
		Vector3 CurrentPosition = base.transform.position;
		Vector3 vector = base.transform.position - m_PreviousLocation;
		RaycastHit hitInfo = default(RaycastHit);
		if (Physics.Linecast(m_PreviousLocation, CurrentPosition, out hitInfo, m_ColisionLayers, QueryTriggerInteraction.Ignore))
		{
			CurrentPosition += new Vector3(hitInfo.point.x, CurrentPosition.y, hitInfo.point.z) - (CurrentPosition + vector.normalized * m_ColisionRadious);
		}
		for (int i = -1; i < 2; i++)
		{
			for (int j = -1; j < 2; j++)
			{
				if (i != 0 || j != 0)
				{
					CurrentPosition += ColliderRaycast(i, j, ref CurrentPosition, ref hitInfo);
				}
			}
		}
		base.transform.position = CurrentPosition;
	}

	private Vector3 ColliderRaycast(float x, float z, ref Vector3 CurrentPosition, ref RaycastHit hit)
	{
		Vector3 normalized = new Vector3(x, 0f, z).normalized;
		if (Physics.Raycast(CurrentPosition, normalized, out hit, m_ColisionRadious * normalized.magnitude, m_ColisionLayers, QueryTriggerInteraction.Ignore) && hit.transform != base.transform)
		{
			return new Vector3(hit.point.x, CurrentPosition.y, hit.point.z) - (CurrentPosition + normalized * m_ColisionRadious);
		}
		return Vector3.zero;
	}

	private void SolveGravity()
	{
		RaycastHit hitInfo = default(RaycastHit);
		bool flag = Physics.Raycast(base.transform.position + Vector3.up * (m_CharacterHeight / 2f), Vector3.down, out hitInfo, m_CharacterHeight, m_ColisionLayers, QueryTriggerInteraction.Ignore);
		m_IsGrounded = false;
		if (m_ActiveGravity <= 0f && flag)
		{
			m_IsGrounded = true;
			base.transform.position = hitInfo.point + Vector3.up * (m_CharacterHeight / 2f);
		}
		if (m_IsGrounded)
		{
			m_ActiveGravity = 0f;
			return;
		}
		m_ActiveGravity -= m_GravityAmmount;
		base.transform.position += Vector3.up * m_ActiveGravity;
	}

	private void EdgeDetection()
	{
		if (!(base.transform.position != m_PreviousLocation))
		{
			return;
		}
		Vector3 vector = base.transform.position;
		if (!Physics.Raycast(vector + Vector3.up, Vector3.down, 6f, m_ColisionLayers, QueryTriggerInteraction.Ignore))
		{
			vector.z = m_PreviousLocation.z;
			if (!Physics.Raycast(vector + Vector3.up, Vector3.down, 6f, m_ColisionLayers, QueryTriggerInteraction.Ignore))
			{
				vector.z = base.transform.position.z;
				vector.x = m_PreviousLocation.x;
			}
			if (!Physics.Raycast(vector + Vector3.up, Vector3.down, 6f, m_ColisionLayers, QueryTriggerInteraction.Ignore))
			{
				vector = m_PreviousLocation;
			}
		}
		base.transform.position = vector;
	}

	private void EdgeJump()
	{
		if (base.transform.position != m_PreviousLocation && !Physics.Raycast(base.transform.position + Vector3.up, Vector3.down, 6f, m_ColisionLayers, QueryTriggerInteraction.Ignore) && m_IsGrounded)
		{
			Jump(SetJumpFlag: false);
		}
	}

	private void SolveExternalForce()
	{
		if (m_ExternalForce.magnitude > 0f)
		{
			base.transform.position += m_ExternalForce;
			m_ExternalForce = Vector3.MoveTowards(m_ExternalForce, Vector3.zero, 0.05f);
		}
	}

	private void Knockback(Vector3 Direction)
	{
		m_ExternalForce = Direction * 0.5f + Vector3.up * 0.7f;
	}

	public void ApplyDamage(Vector3 Attackpoint, float DamageAmmount, bool DoKnockback = false)
	{
		Vector3 direction = -(Attackpoint - base.transform.position).normalized;
		m_HP -= DamageAmmount;
		if (m_HP > 0f && DoKnockback)
		{
			Knockback(direction);
		}
	}

	private void SolveMovementAnimation()
	{
		float b = 0f;
		if (m_movementDifference.magnitude > m_WalkSpeed * 0.025f)
		{
			b = 0.5f;
			if (m_InputController.isRunning)
			{
				b = 1f;
			}
		}
		SpeedDifference = Mathf.Lerp(SpeedDifference, b, 20f * Time.deltaTime);
		SetAnimationProperty("Speed", SpeedDifference);
	}

	public void SmoothRotateCharacterArt(Quaternion newrot)
	{
	}

	public void SetAnimationProperty(string property, float value, bool UseMovementLock = false, bool UseActionLock = false)
	{
		if ((bool)m_Animator && (!UseMovementLock || !m_MovementLock.IsLocked()) && (!UseActionLock || !m_ActionLock.IsLocked()))
		{
			m_Animator.SetFloat(property, value, 0.07f, Time.deltaTime);
		}
	}

	public void SetAnimationPropertyHard(string property, float value, bool UseMovementLock = false, bool UseActionLock = false)
	{
		if ((bool)m_Animator && (!UseMovementLock || !m_MovementLock.IsLocked()) && (!UseActionLock || !m_ActionLock.IsLocked()))
		{
			m_Animator.SetFloat(property, value);
		}
	}

	public void SetAnimationProperty(string property, bool value, bool UseMovementLock = false, bool UseActionLock = false)
	{
		if ((bool)m_Animator && (!UseMovementLock || !m_MovementLock.IsLocked()) && (!UseActionLock || !m_ActionLock.IsLocked()))
		{
			m_Animator.SetBool(property, value);
		}
	}

	public void SetAnimationTrigger(string property, bool UseMovementLock = false, bool UseActionLock = false)
	{
		if ((bool)m_Animator && (!UseMovementLock || !m_MovementLock.IsLocked()) && (!UseActionLock || !m_ActionLock.IsLocked()))
		{
			m_Animator.SetTrigger(property);
		}
	}

	public void SetIsRunning(bool isRunning)
	{
		m_InputController.SetIsRunning(isRunning);
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.cyan;
		Gizmos.DrawWireCube(base.transform.position, new Vector3(m_ColisionRadious + 0.5f, m_CharacterHeight, m_ColisionRadious + 0.5f));
		if (m_InputController != null)
		{
			m_InputController.OnDrawGizmos();
		}
	}
}
