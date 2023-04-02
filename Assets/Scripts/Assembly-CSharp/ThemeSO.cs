using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Theme", menuName = "Create New Theme")]
public class ThemeSO : ScriptableObject
{
	public Color SkyColorModifier = new Color(1f, 1f, 1f, 1f);

	[Header("Theme Monster")]
	public UseableCharacter MonsterToActivate;

	[Header("Theme Player")]
	public UseableCharacter PlayerToActivate;

	[Header("Starting Room")]
	public StartingRoom StartingRoom;

	[Header("Room List")]
	public List<RoomProperties> Rooms;

	[Header("Shopping Items")]
	public List<Interactable_ShoppingCollectable> ShoppingItems;

	[Header("Generation Overrides")]
	public bool CanSpawnHearts = true;

	public bool CanSpawnLogs = true;

	public int ForceRoomCount = -1;
}
