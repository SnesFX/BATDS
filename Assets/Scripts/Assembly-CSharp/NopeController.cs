using UnityEngine;

public class NopeController : MonoBehaviour
{
	public static AudioSource Source;

	public void Start()
	{
		Source = GetComponent<AudioSource>();
	}

	public static void Nope()
	{
		if (!Source.isPlaying)
		{
			Source.Play();
		}
	}
}
