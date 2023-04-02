using System.Collections.Generic;
using UnityEngine;

public class ScrapsManager : MonoBehaviour
{
	public List<GameObject> ScrapPieces;

	public int scrapCount;

	public BaseInteractable AllScrapsComplete;

	private void Start()
	{
		if (SaveManager.DATA.Scrap_1)
		{
			ScrapPieces[0].SetActive(value: true);
			scrapCount++;
		}
		if (SaveManager.DATA.Scrap_2)
		{
			ScrapPieces[1].SetActive(value: true);
			scrapCount++;
		}
		if (SaveManager.DATA.Scrap_3)
		{
			ScrapPieces[2].SetActive(value: true);
			scrapCount++;
		}
		if (SaveManager.DATA.Scrap_4)
		{
			ScrapPieces[3].SetActive(value: true);
			scrapCount++;
		}
		if (SaveManager.DATA.Scrap_5)
		{
			ScrapPieces[4].SetActive(value: true);
			scrapCount++;
		}
		if (SaveManager.DATA.Scrap_6)
		{
			ScrapPieces[5].SetActive(value: true);
			scrapCount++;
		}
		if (SaveManager.DATA.Scrap_7)
		{
			ScrapPieces[6].SetActive(value: true);
			scrapCount++;
		}
		if (scrapCount == 7)
		{
			AllScrapsComplete.enabled = true;
		}
	}

	public static int GetScrapCount()
	{
		int num = 0;
		if (SaveManager.DATA.Scrap_1)
		{
			num++;
		}
		if (SaveManager.DATA.Scrap_2)
		{
			num++;
		}
		if (SaveManager.DATA.Scrap_3)
		{
			num++;
		}
		if (SaveManager.DATA.Scrap_4)
		{
			num++;
		}
		if (SaveManager.DATA.Scrap_5)
		{
			num++;
		}
		if (SaveManager.DATA.Scrap_6)
		{
			num++;
		}
		if (SaveManager.DATA.Scrap_7)
		{
			num++;
		}
		return num;
	}
}
