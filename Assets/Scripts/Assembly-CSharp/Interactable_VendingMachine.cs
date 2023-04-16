using UnityEngine;

public class Interactable_VendingMachine : BaseInteractable
{
	private const float CooldownTime = 60f;

	public Animator Anim;

	public MeshRenderer Renderer;

	private Color Col = new Color(1f, 0.687f, 0.2122f, 1f);

	private int Uses = 3;

	private float Cooldown;

	public override void DoInteraction()
	{
		if (!GameManager.Instance.Player.m_MovementLock.IsLocked())
		{
			base.DoInteraction();
			Anim.SetTrigger("Activate");
			GameManager.Instance.IncreaseBorisStamina(1f);
			GameManager.Instance.Player.m_MovementLock.Lock(isStatic: false, 2.1f);
			GameManager.Instance.Player.SetAnimationTrigger("Drink");
		}
	}

	private void SetUseableState(float isUsable)
	{
		MaterialPropertyBlock materialPropertyBlock = new MaterialPropertyBlock();
		Renderer.GetPropertyBlock(materialPropertyBlock);
		Color col = Col;
		col *= isUsable;
		materialPropertyBlock.SetColor("_EmissionColor", col);
		Renderer.SetPropertyBlock(materialPropertyBlock);
	}
}
