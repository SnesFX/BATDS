using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class JumpscareManager : MonoBehaviour
{
	public static UseableCharacter currentMonster = UseableCharacter.BENDY;

	public AudioSource Audio;

	[Header("JUMPSCARES")]
	public Animator Bendy;

	public Animator Alice;

	public Animator Projectionist;

	public Animator Boris;

	public Animator Butcher;

	private Animator selectedMonster;

	private void Start()
	{
		switch (currentMonster)
		{
		case UseableCharacter.BENDY:
			selectedMonster = Bendy;
			break;
		case UseableCharacter.ALICE:
			selectedMonster = Alice;
			break;
		case UseableCharacter.PROJECTIONIST:
			selectedMonster = Projectionist;
			break;
		case UseableCharacter.BORKIS:
			selectedMonster = Boris;
			break;
		case UseableCharacter.GANG:
			selectedMonster = Butcher;
			break;
		}
		StartCoroutine(DoJumpscare());
	}

	private IEnumerator DoJumpscare()
	{
		yield return new WaitForSeconds(0.1f);
		if ((bool)selectedMonster)
		{
			selectedMonster.SetTrigger("Go");
		}
		yield return new WaitForSeconds(0.2f);
		Audio.Play();
		yield return new WaitForSeconds(4f);
		SceneManager.LoadScene("GameScene");
	}
}
