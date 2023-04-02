using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathReset : MonoBehaviour
{
	public AudioSource Audio;

	public IEnumerator Jumpscare()
	{
		Audio.Play();
		yield return new WaitForSeconds(6f);
		SceneManager.LoadScene("GameScene");
	}

	public void DoJumpscare()
	{
		StartCoroutine(Jumpscare());
	}
}
