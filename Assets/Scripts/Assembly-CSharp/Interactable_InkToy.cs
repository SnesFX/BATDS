using System.Collections.Generic;
using UnityEngine;

public class Interactable_InkToy : BaseInteractable
{
	public Animator Anim;

	public ParticleSystem Particles;

	private List<UseableCharacter> av = new List<UseableCharacter>();

	private int index;

	public GameObject[] CharacterObjects;

	public override void Start()
	{
		base.Start();
		av.Add(UseableCharacter.BORIS);
		if (SaveManager.DATA.PAPER >= 5)
		{
			av.Add(UseableCharacter.LOST_ONE);
		}
		if (SaveManager.DATA.FOUND_E)
		{
			av.Add(UseableCharacter.SAMMY);
		}
		index = av.IndexOf((UseableCharacter)SaveManager.DATA.C);
		SetMesh();
	}

	public override void DoInteraction()
	{
		base.DoInteraction();
		if ((bool)Anim)
		{
			Anim.SetTrigger("Activate");
		}
		Particles.Emit(10);
		index++;
		if (index == av.Count)
		{
			index = 0;
		}
		SaveManager.DATA.C = (int)av[index];
		SaveManager.Save();
		SetMesh();
	}

	private void SetMesh()
	{
		for (int i = 0; i < CharacterObjects.Length; i++)
		{
			CharacterObjects[i].SetActive(value: false);
		}
		switch ((UseableCharacter)SaveManager.DATA.C)
		{
		case UseableCharacter.LOST_ONE:
			CharacterObjects[1].SetActive(value: true);
			break;
		case UseableCharacter.SAMMY:
			CharacterObjects[2].SetActive(value: true);
			break;
		default:
			CharacterObjects[0].SetActive(value: true);
			break;
		}
	}

	public void MakeAvailable(UseableCharacter newC)
	{
		av.Add(newC);
	}
}
