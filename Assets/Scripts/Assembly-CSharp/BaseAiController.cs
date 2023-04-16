using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class BaseAiController : KBInputController
{
	[SerializeField]
	public Transform Target;

	public Transform EnemyTarget;

	[SerializeField]
	private List<Vector3> m_path = new List<Vector3>();

	private GameObject LastKnownPoint = new GameObject();

	private Vector3 TargetPreviousPosition;

	private Vector3 MoveToPoint = Vector3.zero;

	private Vector3 SmoothMoveToPoint = Vector3.zero;

	private Vector3 avoidancePoint;

	private bool isAvoiding;

	private bool ForceFollow;

	public override void DOFixedUpdate()
	{
		base.DOFixedUpdate();
		KBCharacterController.AiDetectionMode detectionMode = m_BaseController.DetectionMode;
		if (detectionMode == KBCharacterController.AiDetectionMode.FOLLOW_TARGET)
		{
			LookForAndFollowTarget();
		}
	}

	public void SetEnemyAsTarget(Transform newTarget, bool Force = false)
	{
		ForceFollow = Force;
		EnemyTarget = newTarget;
		Target = EnemyTarget;
	}

	public void LookForAndFollowTarget()
	{
		if (EnemyTarget == null)
		{
			Roam();
			Collider[] array = Physics.OverlapSphere(m_BaseController.transform.position, m_BaseController.SightDistance, m_BaseController.TargetableLayer);
			for (int i = 0; i < array.Length; i++)
			{
				Vector3 normalized = (array[i].transform.position - m_BaseController.transform.position).normalized;
				float num = Vector3.Distance(m_BaseController.transform.position, array[i].transform.position);
				if ((Vector3.Angle(m_BaseController.CharacterArt.transform.forward, normalized) < m_BaseController.SightAngle || num < 15f) && IsInLineOfSight(array[i].transform))
				{
					SetEnemyAsTarget(array[i].transform);
					if (m_BaseController.SpotTargetDelayTime > 0f)
					{
						m_BaseController.CharacterArt.rotation = Quaternion.LookRotation(normalized, Vector3.up);
						OnEnemySpotted();
					}
					break;
				}
			}
		}
		else if ((Vector3.Distance(m_BaseController.transform.position, EnemyTarget.position) > m_BaseController.LoseTrackingDistance && !ForceFollow) || !EnemyTarget.gameObject.activeSelf)
		{
			LastKnownPoint.transform.position = EnemyTarget.transform.position;
			Target = LastKnownPoint.transform;
			m_path.Clear();
			EnemyTarget = null;
			OnEnemyLost();
		}
		if (!(Target != null))
		{
			return;
		}
		if (!Target.gameObject.activeInHierarchy)
		{
			m_BaseController.m_MovementLock.Lock(false, 0.7f);
			Target = null;
			return;
		}
		if (m_path.Count == 0)
		{
			FindPathToPoint(Target.position);
			return;
		}
		bool flag = false;
		Vector3 vector = new Vector3(m_path[0].x, m_BaseController.transform.position.y, m_path[0].z);
		if (Vector3.Distance(m_BaseController.transform.position, vector) > 1f)
		{
			MoveToPoint = vector;
			flag = true;
		}
		else
		{
			m_path.RemoveAt(0);
			if (m_path.Count > 0)
			{
				vector = new Vector3(m_path[0].x, m_BaseController.transform.position.y, m_path[0].z);
				flag = true;
			}
		}
		if (Vector3.Distance(m_BaseController.transform.position, Target.position) < 30f && Vector3.Distance(TargetPreviousPosition, Target.position) > 10f)
		{
			FindPathToPoint(Target.position);
		}
		SmoothMoveToPoint = Vector3.Lerp(SmoothMoveToPoint, MoveToPoint, 8f * Time.fixedDeltaTime);
		Vector3 normalized2 = (SmoothMoveToPoint - m_BaseController.transform.position).normalized;
		if (flag)
		{
			m_BaseController.Move(normalized2);
			if ((bool)m_BaseController.CharacterArt && !m_BaseController.m_MovementLock.IsLocked())
			{
				Quaternion rotation = Quaternion.Lerp(m_BaseController.CharacterArt.rotation, Quaternion.LookRotation(normalized2, Vector3.up), 15f * Time.deltaTime);
				m_BaseController.CharacterArt.rotation = rotation;
			}
		}
	}

	public void FindPathToPoint(Vector3 point)
	{
		NavMeshPathRequestManager.Instance.RequestClosestPath(m_BaseController.transform.position, point, m_path, 2f);
		TargetPreviousPosition = Target.position;
	}

	private bool IsInLineOfSight(Transform newTarget)
	{
		Vector3 start = new Vector3(m_BaseController.transform.position.x, m_BaseController.transform.position.y + 5f, m_BaseController.transform.position.z);
		Vector3 end = new Vector3(newTarget.position.x, m_BaseController.transform.position.y + 3.5f, newTarget.position.z);
		return !Physics.Linecast(start, end, m_BaseController.m_SightBlockingLayers, QueryTriggerInteraction.Ignore);
	}

	public void Roam()
	{
		if (Target == null)
		{
			Target = GameManager.Instance.PATH_MANAGER.GlobalPathNodeList[UnityEngine.Random.Range(0, GameManager.Instance.PATH_MANAGER.GlobalPathNodeList.Count)].transform;
		}
		else if (m_path.Count == 0)
		{
			m_BaseController.m_MovementLock.Lock(false, 2f);
			m_path.Clear();
			Target = null;
		}
	}

	public bool CheckAvoidance()
	{
		float num = 4f;
		if (isAvoiding)
		{
			num += 2f;
		}
		Vector3 normalized = (SmoothMoveToPoint - m_BaseController.transform.position).normalized;
		Debug.DrawRay(m_BaseController.transform.position, normalized * num, Color.yellow);
		if (avoidanceRaycast(normalized, num))
		{
			if (isAvoiding)
			{
				MoveToPoint = avoidancePoint;
				if (Vector3.Distance(avoidancePoint, m_BaseController.transform.position) < 2f)
				{
					isAvoiding = false;
				}
				return true;
			}
			List<Vector3> list = new List<Vector3>();
			for (int i = -1; i < 2; i += 2)
			{
				Vector3 vector = new Vector3(normalized.z, normalized.y, 0f - normalized.x) * i;
				if (!avoidanceRaycast(vector, 8f))
				{
					list.Add(vector * 8f);
				}
			}
			float num2 = 1000f;
			Vector3 vector2 = Vector3.zero;
			for (int j = 0; j < list.Count; j++)
			{
				Vector3 vector3 = m_BaseController.transform.position + list[j];
				Debug.DrawLine(vector3, m_BaseController.transform.position, Color.cyan);
				float num3 = Vector3.Distance(vector3, MoveToPoint);
				if (num3 < num2)
				{
					num2 = num3;
					vector2 = vector3;
				}
			}
			if (vector2 != Vector3.zero)
			{
				isAvoiding = true;
				avoidancePoint = vector2;
				MoveToPoint = vector2;
				return true;
			}
		}
		else
		{
			isAvoiding = false;
		}
		return false;
	}

	private bool avoidanceRaycast(Vector3 dir, float dist)
	{
		return Physics.Raycast(m_BaseController.transform.position, dir.normalized, dist, m_BaseController.m_ColisionLayers, QueryTriggerInteraction.Ignore);
	}

	public virtual void OnEnemySpotted()
	{
		m_path.Clear();
		m_BaseController.m_MovementLock.Lock(false, m_BaseController.SpotTargetDelayTime);
		m_BaseController.m_Animator.SetTrigger("TargetSeen");
	}

	public virtual void OnEnemyLost()
	{
	}
}
