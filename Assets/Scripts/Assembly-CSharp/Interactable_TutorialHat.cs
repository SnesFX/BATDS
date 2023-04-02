using UnityEngine;

public class Interactable_TutorialHat : BaseInteractable
{
	public Animator Door;

	public GameObject BorisHat;

	public override void Start()
	{
		if (PlayerPrefs.GetInt("TutorialComplete", 1) == 2)
		{
			base.gameObject.SetActive(value: false);
			return;
		}
		base.Start();
		Door.SetBool("IsClosed", value: true);
		BorisHat.SetActive(value: false);
	}

	public override void DoInteraction()
	{
		base.enabled = false;
		base.gameObject.SetActive(value: false);
		Door.SetTrigger("Open");
		Audio.PlayClip(0);
		Audio.PlayClip(1);
		BorisHat.SetActive(value: true);
	}
}
