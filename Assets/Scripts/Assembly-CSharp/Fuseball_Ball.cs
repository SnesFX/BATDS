using UnityEngine;

public class Fuseball_Ball : MonoBehaviour
{
	public float Acceleration;

	public float MaxXAcceleration;

	private float ActiveGravity;

	public LayerMask BounceLayers;

	public float UpwardBounceForce;

	private Vector3 previousPos;

	private float XPower;

	public void Reset()
	{
		base.transform.position = Fuseball_Manager.Instance.BallLaunchPosition.position;
		previousPos = base.transform.position;
		XPower = Random.Range((0f - MaxXAcceleration) * 0.5f, MaxXAcceleration * 0.5f);
		ActiveGravity = -0.1f;
		base.gameObject.SetActive(value: false);
	}

	private void Update()
	{
		ActiveGravity += Acceleration * Time.deltaTime;
		base.transform.position += Vector3.down * ActiveGravity * Time.deltaTime;
		base.transform.position += Vector3.right * XPower * Time.deltaTime;
		Vector3 vector = Raycast();
		if (vector == Vector3.up)
		{
			ActiveGravity = 0f - UpwardBounceForce;
			Fuseball_Manager.Instance.SFXPlayer.PlayClip(5, 0.1f);
		}
		else if (vector == Vector3.down)
		{
			ActiveGravity = 0f;
		}
		else if (vector == Vector3.right || vector == Vector3.left)
		{
			XPower = 0f - XPower;
			Fuseball_Manager.Instance.SFXPlayer.PlayClip(1, 0.1f);
		}
		previousPos = base.transform.position;
	}

	private Vector3 Raycast()
	{
		Debug.DrawLine(previousPos, base.transform.position, Color.red);
		if (Physics.Linecast(previousPos, base.transform.position, out var hitInfo, BounceLayers, QueryTriggerInteraction.Ignore))
		{
			base.transform.position = previousPos;
		}
		if ((bool)hitInfo.transform && hitInfo.transform.tag == "Fuseball_Pattle")
		{
			float num = Mathf.Clamp((base.transform.position.x - hitInfo.transform.position.x) * 3f, -1f, 1f);
			XPower = num * MaxXAcceleration;
		}
		else if ((bool)hitInfo.transform && hitInfo.transform.tag == "Fuseball_Fuse")
		{
			hitInfo.transform.BroadcastMessage("BallHit");
		}
		else if ((bool)hitInfo.transform && hitInfo.transform.tag == "Fuseball_Bottom")
		{
			Fuseball_Manager.Instance.BallLost();
		}
		return hitInfo.normal;
	}
}
