using UnityEngine;

public class Fuseball_Fuse : MonoBehaviour
{
	private SpriteRenderer Light;

	private Vector3 LocalTargetPos;

	private void OnEnable()
	{
		LocalTargetPos = base.transform.localPosition;
		Light = GetComponentInChildren<SpriteRenderer>();
	}

	private void Update()
	{
		base.transform.localPosition = Vector3.MoveTowards(base.transform.localPosition, LocalTargetPos, 5f * Time.deltaTime);
	}

	public void Activate()
	{
		LocalTargetPos = new Vector3(base.transform.localPosition.x, base.transform.localPosition.y, -0.9f);
		Light.enabled = true;
	}

	public void BallHit()
	{
		Fuseball_Manager.Instance.FuseHit(this);
		ResetFuse();
	}

	public void ResetFuse()
	{
		LocalTargetPos = new Vector3(base.transform.localPosition.x, base.transform.localPosition.y, -0.53f);
		Light.enabled = false;
	}
}
