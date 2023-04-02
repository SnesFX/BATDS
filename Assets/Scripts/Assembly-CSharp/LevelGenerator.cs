using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class LevelGenerator : MonoBehaviour
{
	[Header("TESTING")]
	public int TestDay;

	public LevelTheme TestTheme;

	[Header("Current Theme")]
	public static LevelTheme CurrentTheme;

	internal ThemeSO ActiveTheme;

	[Header("Theme List")]
	public List<ThemeSO> ThemeList;

	[Header("Starting Room List")]
	public GameObject[] StartingRooms;

	[Header("Other Items")]
	public GameObject AudioLog;

	public GameObject Heart;

	public GameObject LostPaper;

	public List<Interactable_Special> SpecialItems;

	private int CurrentRoomCount;

	private bool hasPlacedLog;

	internal List<DoorwayController> OpenDoorways = new List<DoorwayController>();

	internal List<Vector2> ChunkPositions = new List<Vector2>();

	private const int MaxRoomCount = 5;

	internal NavMeshSurface NavSurface;

	public static int reloadCounter;

	public void GenerateLevel()
	{
		ActiveTheme = ThemeList[(int)CurrentTheme];
		if (TestTheme != 0)
		{
			ActiveTheme = ThemeList[(int)TestTheme];
		}
		StartingRooms[(int)ActiveTheme.StartingRoom].SetActive(value: true);
		SetAmbientSkyColor();
		OpenDoorways.AddRange(Object.FindObjectsOfType<DoorwayController>());
		ChunkPositions.Add(Vector2.zero);
		int num = 0;
		int num2 = Mathf.Clamp(5 + GameManager.Day, 0, 13);
		if (ActiveTheme.ForceRoomCount > -1)
		{
			num2 = ActiveTheme.ForceRoomCount;
		}
		if (OpenDoorways.Count > 0)
		{
			while (num < 1000 && CurrentRoomCount < num2)
			{
				num++;
				DoorwayController doorwayController = OpenDoorways[GameManager.Instance.LevelGenRandom.Next(0, OpenDoorways.Count)];
				RoomProperties roomProperties = ActiveTheme.Rooms[GameManager.Instance.LevelGenRandom.Next(0, ActiveTheme.Rooms.Count)];
				Transform transform = Object.Instantiate(roomProperties.ChunkRefrencePointsParent, doorwayController.RoomSpawnPoint.transform.position, doorwayController.transform.rotation);
				Vector2[] array = CheckChunkPositions(transform);
				if (array.Length == 0)
				{
					Object.Destroy(transform.gameObject);
					continue;
				}
				ChunkPositions.AddRange(array);
				GameObject gameObject = Object.Instantiate(roomProperties.gameObject, doorwayController.RoomSpawnPoint.transform.position, doorwayController.transform.rotation);
				gameObject.transform.SetParent(base.transform);
				OpenDoorways.AddRange(gameObject.GetComponent<RoomProperties>().Doorways);
				doorwayController.DoorConnected = true;
				CurrentRoomCount++;
				OpenDoorways.Remove(doorwayController);
				Object.Destroy(transform.gameObject);
				if (OpenDoorways.Count == 0)
				{
					break;
				}
			}
		}
		foreach (DoorwayController openDoorway in OpenDoorways)
		{
			if (openDoorway.isActiveAndEnabled)
			{
				openDoorway.CheckForConnectingDoors();
			}
		}
		PlaceItems();
		if (reloadCounter > 0)
		{
			reloadCounter--;
			SceneManager.LoadScene("GameScene");
		}
		else
		{
			NavSurface = Object.FindObjectOfType<NavMeshSurface>();
			GameManager.Instance.OnLevelBuildComplete();
		}
	}

	private Vector2[] CheckChunkPositions(Transform pointParent)
	{
		List<Vector2> list = new List<Vector2>();
		for (int i = 0; i < pointParent.childCount; i++)
		{
			Vector3 position = pointParent.GetChild(i).transform.position;
			Vector2 item = new Vector2(Mathf.Round(position.x), Mathf.Round(position.z));
			if (ChunkPositions.Contains(item))
			{
				return new Vector2[0];
			}
			list.Add(item);
		}
		return list.ToArray();
	}

	public void PlaceItems()
	{
		List<ShoppingItemSpawnNode> list = new List<ShoppingItemSpawnNode>();
		ShoppingItemSpawnNode[] array = Object.FindObjectsOfType<ShoppingItemSpawnNode>();
		if (array.Length == 0 || ActiveTheme.ShoppingItems.Count == 0)
		{
			SpawnLockerItem();
			return;
		}
		list.AddRange(array);
		int num = Mathf.Clamp(list.Count, 0, 6);
		for (int i = 0; i < num; i++)
		{
			if (i >= 6)
			{
				return;
			}
			ShoppingItemSpawnNode shoppingItemSpawnNode = list[GameManager.Instance.LevelGenRandom.Next(0, list.Count)];
			list.Remove(shoppingItemSpawnNode);
			Object.Instantiate(ActiveTheme.ShoppingItems[GameManager.Instance.LevelGenRandom.Next(0, ActiveTheme.ShoppingItems.Count)], shoppingItemSpawnNode.transform.position, shoppingItemSpawnNode.transform.rotation);
		}
		if (((ActiveTheme.CanSpawnLogs && Random.Range(0, 2) == 1) || GameManager.Day == 2) && list.Count > 0)
		{
			if (!hasPlacedLog)
			{
				ShoppingItemSpawnNode shoppingItemSpawnNode2 = list[GameManager.Instance.LevelGenRandom.Next(0, list.Count)];
				list.Remove(shoppingItemSpawnNode2);
				Object.Instantiate(AudioLog, shoppingItemSpawnNode2.transform.position, shoppingItemSpawnNode2.transform.rotation);
				hasPlacedLog = true;
			}
		}
		else
		{
			hasPlacedLog = true;
		}
		if (ActiveTheme.CanSpawnHearts && Random.Range(0, 3) == 1 && list.Count > 0)
		{
			ShoppingItemSpawnNode shoppingItemSpawnNode3 = list[GameManager.Instance.LevelGenRandom.Next(0, list.Count)];
			list.Remove(shoppingItemSpawnNode3);
			Object.Instantiate(Heart, shoppingItemSpawnNode3.transform.position, shoppingItemSpawnNode3.transform.rotation);
			Debug.Log("SPAWNED HEART");
		}
		if (Random.Range(0, 2) == 1 && list.Count > 0)
		{
			ShoppingItemSpawnNode shoppingItemSpawnNode4 = list[GameManager.Instance.LevelGenRandom.Next(0, list.Count)];
			list.Remove(shoppingItemSpawnNode4);
			Object.Instantiate(LostPaper, shoppingItemSpawnNode4.transform.position, shoppingItemSpawnNode4.transform.rotation);
			Debug.Log("SPAWNED LOST PAPER");
		}
		SpawnLockerItem();
		SpawnDeadBorisItem();
		SpawnToiletItem();
	}

	private void SpawnLockerItem()
	{
		Interactable_Special[] array = new Interactable_Special[3];
		int num = GameManager.Instance.LevelGenRandom.Next(0, 10);
		int num2 = GameManager.Instance.LevelGenRandom.Next(0, 10);
		int num3 = GameManager.Instance.LevelGenRandom.Next(0, 10);
		if (num < 7 || GameManager.Day == 1)
		{
			if (GameManager.Instance.LevelGenRandom.Next(0, 10) == 1 && !SaveManager.DATA.SpecialBone)
			{
				array[0] = SpecialItems[2];
			}
			if (array[0] == null)
			{
				switch (GameManager.Instance.LevelGenRandom.Next(0, 2))
				{
				case 0:
				{
					array[0] = SpecialItems[0];
					if (GameManager.Day < 5)
					{
						array[0].ItemNumber = GameManager.Day;
						break;
					}
					List<int> list2 = new List<int>();
					if (!SaveManager.DATA.Record_1)
					{
						list2.Add(1);
					}
					if (!SaveManager.DATA.Record_2)
					{
						list2.Add(2);
					}
					if (!SaveManager.DATA.Record_3)
					{
						list2.Add(3);
					}
					if (!SaveManager.DATA.Record_4)
					{
						list2.Add(4);
					}
					if (!SaveManager.DATA.Record_5)
					{
						list2.Add(5);
					}
					if (!SaveManager.DATA.Record_6)
					{
						list2.Add(6);
					}
					if (list2.Count > 0)
					{
						array[0].ItemNumber = list2[GameManager.Instance.LevelGenRandom.Next(0, list2.Count)];
					}
					else
					{
						array[0] = null;
					}
					break;
				}
				case 1:
				{
					array[0] = SpecialItems[1];
					if (GameManager.Day < 7)
					{
						array[0].ItemNumber = GameManager.Day;
						break;
					}
					List<int> list = new List<int>();
					if (!SaveManager.DATA.Scrap_1)
					{
						list.Add(1);
					}
					if (!SaveManager.DATA.Scrap_2)
					{
						list.Add(2);
					}
					if (!SaveManager.DATA.Scrap_3)
					{
						list.Add(3);
					}
					if (!SaveManager.DATA.Scrap_4)
					{
						list.Add(4);
					}
					if (!SaveManager.DATA.Scrap_5)
					{
						list.Add(5);
					}
					if (!SaveManager.DATA.Scrap_6)
					{
						list.Add(6);
					}
					if (!SaveManager.DATA.Scrap_7)
					{
						list.Add(7);
					}
					if (list.Count > 0)
					{
						array[0].ItemNumber = list[GameManager.Instance.LevelGenRandom.Next(0, list.Count)];
					}
					else
					{
						array[0] = null;
					}
					break;
				}
				}
			}
			if (GameManager.Day == 1)
			{
				array[0] = SpecialItems[1];
			}
		}
		if (num2 < 7 && GameManager.Instance.LevelGenRandom.Next(0, 2) == 1 && SaveManager.DATA.FUSES < 6)
		{
			array[1] = SpecialItems[5];
		}
		if (num3 < 7)
		{
			if (GameManager.Instance.LevelGenRandom.Next(0, 2) == 0)
			{
				if (SaveManager.DATA.KEY2 < 5)
				{
					array[2] = SpecialItems[6];
				}
			}
			else if (SaveManager.DATA.Candles < 4)
			{
				array[2] = SpecialItems[8];
			}
			else if (SaveManager.DATA.Candles == 4)
			{
				array[2] = SpecialItems[9];
			}
		}
		for (int i = 0; i < array.Length; i++)
		{
			List<Interactable_Locker> list3 = new List<Interactable_Locker>();
			list3.AddRange(Object.FindObjectsOfType<Interactable_Locker>());
			if (!(array[i] != null) || list3.Count <= 0)
			{
				continue;
			}
			Debug.Log("SPAWNED SPECIAL ITEM: " + array[i].ObjType);
			if (GameManager.Day == 1 && i == 0)
			{
				foreach (Interactable_Locker item in list3)
				{
					item.chosenItem = array[i];
				}
				break;
			}
			Interactable_Locker interactable_Locker = list3[GameManager.Instance.LevelGenRandom.Next(0, list3.Count)];
			interactable_Locker.chosenItem = array[i];
			list3.Remove(interactable_Locker);
		}
	}

	private void SpawnDeadBorisItem()
	{
		Interactable_Special interactable_Special = null;
		if (GameManager.Instance.LevelGenRandom.Next(0, 10) < 7)
		{
			interactable_Special = SpecialItems[3];
			List<int> list = new List<int>();
			if (!SaveManager.DATA.Tape_1)
			{
				list.Add(1);
			}
			if (!SaveManager.DATA.Tape_2)
			{
				list.Add(2);
			}
			if (!SaveManager.DATA.Tape_3)
			{
				list.Add(3);
			}
			if (!SaveManager.DATA.Tape_4)
			{
				list.Add(4);
			}
			if (!SaveManager.DATA.Tape_5)
			{
				list.Add(5);
			}
			if (list.Count <= 0)
			{
				interactable_Special = (SaveManager.DATA.KEY ? null : SpecialItems[4]);
			}
			else
			{
				interactable_Special.ItemNumber = list[0];
			}
		}
		if (interactable_Special != null)
		{
			Interactable_DeadBoris[] array = Object.FindObjectsOfType<Interactable_DeadBoris>();
			if (array.Length != 0)
			{
				Debug.Log("SPAWNED SPECIAL ITEM: " + interactable_Special.ObjType);
				array[GameManager.Instance.LevelGenRandom.Next(0, array.Length)].chosenItem = interactable_Special;
			}
		}
	}

	private void SpawnToiletItem()
	{
		Interactable_Special interactable_Special = null;
		if (GameManager.Instance.LevelGenRandom.Next(0, 10) < 7 && SaveManager.DATA.Blueprints < 5)
		{
			interactable_Special = SpecialItems[10];
		}
		if (interactable_Special != null)
		{
			Interactable_Toilet[] array = Object.FindObjectsOfType<Interactable_Toilet>();
			if (array.Length != 0)
			{
				Debug.Log("SPAWNED SPECIAL ITEM: " + interactable_Special.ObjType);
				array[GameManager.Instance.LevelGenRandom.Next(0, array.Length)].chosenItem = interactable_Special;
			}
		}
	}

	public void SetAmbientSkyColor()
	{
		RenderSettings.ambientSkyColor = ActiveTheme.SkyColorModifier;
	}
}
