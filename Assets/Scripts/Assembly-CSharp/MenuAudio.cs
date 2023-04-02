using UnityEngine;

public class MenuAudio : MonoBehaviour
{
	public AudioSource Source;

	public void Play()
	{
		float timeScale = Time.timeScale;
		Time.timeScale = 1f;
		Source.PlayOneShot(Source.clip);
		Time.timeScale = timeScale;
	}
}
