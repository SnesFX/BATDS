using UnityEngine;

public class DisableObjectsBasedOnSaveData : MonoBehaviour
{
	public bool invert;

	public bool FuseBall;

	public bool Highscore;

	public bool Alice;

	public bool Gang;

	public bool Lost;

	public bool Candles;

	public bool DarkR;

	public bool sammy;

	public int count;

	public GameObject Swap;

	private void Start()
	{
		Check();
	}

	public void Check()
	{
		if (FuseBall && SaveManager.DATA.FUSES < 6)
		{
			base.gameObject.SetActive(invert);
		}
		else if (Highscore && SaveManager.DATA.FOUND_C)
		{
			base.gameObject.SetActive(invert);
		}
		else if (Alice && !SaveManager.DATA.FOUND_A)
		{
			base.gameObject.SetActive(invert);
		}
		else if (Gang && !SaveManager.DATA.FOUND_D)
		{
			base.gameObject.SetActive(invert);
		}
		else if (Lost && SaveManager.DATA.PAPER < 5)
		{
			base.gameObject.SetActive(invert);
		}
		else if (Candles && SaveManager.DATA.Candles < count)
		{
			base.gameObject.SetActive(invert);
		}
		else if (DarkR && !SaveManager.DATA.FOUND_W)
		{
			base.gameObject.SetActive(invert);
		}
		else if (sammy && !SaveManager.DATA.FOUND_E)
		{
			base.gameObject.SetActive(invert);
		}
		if ((bool)Swap)
		{
			Swap.SetActive(!base.gameObject.activeInHierarchy);
		}
	}
}
