using UnityEngine;

public class Interactable_SammyInstrument : BaseInteractable
{
	public bool BorisCanUse;

	public int CheckPreviousInSequence;

	public int NextInSequence;

	public int CheckPreviousAlternate;

	public int NextInSequenceAlternate;

	public GameObject ON;

	public GameObject OFF;

	private Animator anim;

	public AudioSource UnlockAudio;

	public override void Start()
	{
		base.Start();
		anim = GetComponent<Animator>();
		ActivateGate();
		if (SaveManager.DATA.C != 8 && !BorisCanUse)
		{
			IsActive = false;
		}
	}

	public override void DoInteraction()
	{
		if ((bool)Audio)
		{
			Audio.PlayClip();
		}
		anim.SetTrigger("Activate");
		if (SaveManager.DATA.SSequence == CheckPreviousInSequence)
		{
			SaveManager.DATA.SSequence = NextInSequence;
			Debug.Log(base.gameObject.name + " Next:" + NextInSequence);
			if (SaveManager.DATA.SSequence == 414 && !SaveManager.DATA.FOUND_W)
			{
				SaveManager.DATA.FOUND_W = true;
				SaveManager.Save();
				Debug.Log("SUCCESS");
				ActivateGate(playAudio: true);
			}
		}
		else if (SaveManager.DATA.SSequence == CheckPreviousAlternate && NextInSequenceAlternate != 0)
		{
			SaveManager.DATA.SSequence = NextInSequenceAlternate;
			Debug.Log("Next:" + NextInSequenceAlternate);
		}
		else
		{
			SaveManager.DATA.SSequence = 0;
			Debug.Log("SEQUENCE FAIL");
			if (SaveManager.DATA.C == 8)
			{
				NopeController.Nope();
			}
		}
	}

	public void ActivateGate(bool playAudio = false)
	{
		bool fOUND_W = SaveManager.DATA.FOUND_W;
		if ((bool)ON)
		{
			if (fOUND_W && playAudio)
			{
				UnlockAudio.Play();
			}
			ON.gameObject.SetActive(fOUND_W);
			OFF.gameObject.SetActive(!fOUND_W);
		}
	}
}
