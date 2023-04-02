using UnityEngine;

public class ArtCritic : MonoBehaviour
{
	public Transform[] standpoints;

	private void Start()
	{
		base.transform.position = standpoints[Random.Range(0, standpoints.Length)].position;
		base.transform.Rotate(0f, 90f + Random.Range(-10f, 10f), 0f);
	}
}
