using UnityEngine;

public class SteamJumpscareController : MonoBehaviour
{
	private bool hasFired;

	public ParticleSystem particles;

	public AudioClipPlayer clip;

	private void Start()
	{
		if (Random.Range(0, 55) != 1)
		{
			base.enabled = false;
		}
	}

	private void Update()
	{
		if (Vector3.Distance(base.transform.position, GameManager.Instance.Player.transform.position) < 5f)
		{
			hasFired = true;
			particles.Play();
			clip.PlayClip();
			base.enabled = false;
		}
	}
}
