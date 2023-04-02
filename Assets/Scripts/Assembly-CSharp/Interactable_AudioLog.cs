using UnityEngine;

public class Interactable_AudioLog : BaseInteractable
{
	private int LogNumber;

	public Animator Anim;

	private bool WasPlaying;

	public override void Start()
	{
		base.Start();
		LogNumber = Random.Range(0, Audio.Clips.Length);
	}

	public override void DoInteraction()
	{
		if ((bool)Audio && !Audio.IsPlaying)
		{
			if ((bool)Audio)
			{
				Audio.PlayClip(LogNumber);
			}
			Anim.SetBool("IsPlaying", value: true);
			WasPlaying = true;
		}
	}

	public override void Update()
	{
		base.Update();
		if ((bool)Audio && !Audio.IsPlaying && WasPlaying)
		{
			Anim.SetBool("IsPlaying", value: false);
			WasPlaying = false;
		}
	}
}
