using UnityEngine;

public class PillarScaleRandomizer : MonoBehaviour
{
	private void Awake()
	{
		base.transform.localScale += Vector3.up * Random.Range(0f, 0.012f);
	}
}
