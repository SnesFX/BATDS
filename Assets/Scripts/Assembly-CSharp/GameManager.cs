using System;
using System.Collections;
using System.Collections.Generic;
using InControl;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
	public BuildMode internalBuildMode;

	public static int SpawnPointIndex = 0;

	public static int Day = 1;

	public static PauseState CurrentPauseState = PauseState.UNPAUSED;

	public static int Seed = 12;

	public static GameObject SubMenu;

	[Header("CONTROLLERS")]
	public LevelGenerator LEVEL_GENERATOR;

	public PathfinderManager PATH_MANAGER;

	public GameUIManager GAME_UI_MANAGER;

	public MusicManager MUSIC_MANAGER;

	public GameObject GAME_UI_PARENT;

	public OptionsManager OPTIONS_MANAGER;

	[Header("CHARACTER REF's")]
	public KBCharacterController[] AvailableCharacters;

	internal KBCharacterController Monster;

	internal KBCharacterController Player;

	internal Interactable_Elevator Elevator;

	[Header("STARTING POINTS")]
	public Transform[] SpawnPoints;

	internal Camera MainCamRef;

	internal bool isPlayerDead;

	public System.Random LevelGenRandom;

	internal List<Interactable_ShoppingCollectable> collectedItems = new List<Interactable_ShoppingCollectable>();

	public static List<Interactable_ShoppingCollectable> SavedItems = new List<Interactable_ShoppingCollectable>();

	internal List<Interactable_TeleportWall> TeleportLocations = new List<Interactable_TeleportWall>();

	internal float BorisStamina = 1f;

	private const float BorisStaminaDrain = 0.03f;

	internal int ShoppingListCount;

	internal int ShoppingListGoal;

	private static GameManager instance;

	public static BuildMode ActiveBuildMode => Instance.internalBuildMode;

	public static GameManager Instance => instance;

	public void OnEnable()
	{
		instance = UnityEngine.Object.FindObjectOfType<GameManager>();
		SaveManager.Load();
	}

	private void Start()
	{
		SubMenu = null;
		InputManager.OnDeviceDetached += delegate
		{
			SetPaused(force: true);
		};
		OPTIONS_MANAGER.LoadLaunchQualitySettings();
		StartCoroutine(TDInputManager.CalculateInputs());
		Seed = UnityEngine.Random.Range(0, 1000000);
		LevelGenRandom = new System.Random(Seed);
		if (ActiveBuildMode == BuildMode.MOBILE)
		{
			Application.targetFrameRate = 30;
		}
		else
		{
			Application.targetFrameRate = 60;
		}
		MainCamRef = Camera.main;
		string floorArea = "";
		int floorNumber = LevelGenRandom.Next(-25, 68);
		switch (LevelGenerator.CurrentTheme)
		{
		case LevelTheme.PROJECTIONIST_FLOOR:
			floorArea = "Day " + Day;
			break;
		case LevelTheme.REGLUAR_FL0OR:
			floorArea = "Day 414";
			floorNumber = 414;
			break;
		default:
			floorArea = "Day " + Day;
			break;
		case LevelTheme.HUB_1:
		case LevelTheme.ALICE_FLOOR:
			break;
		}
		GAME_UI_MANAGER.SetFloorName(floorArea, floorNumber);
		BuildLevel();
		MUSIC_MANAGER.ChangeMusic(MUSIC_MANAGER.AmbienceTrack, 3f, 0.3f);
		collectedItems.AddRange(SavedItems);
		SavedItems.Clear();
		for (int i = 0; i < collectedItems.Count; i++)
		{
			collectedItems[i].AddIconToListOnly();
		}
	}

	public void TryInteract()
	{
		if (GAME_UI_MANAGER.currentInteractable != null)
		{
			GAME_UI_MANAGER.currentInteractable.CheckInteractionSuccess();
		}
	}

	public void SetPaused(bool force = false)
	{
		if (!force && SubMenu != null)
		{
			SubMenu = null;
			return;
		}
		if (force)
		{
			GAME_UI_MANAGER.lockMinigame.ForceClose();
			GAME_UI_MANAGER.recordPlayer.ForceClose();
			GAME_UI_MANAGER.TapePlayer.ForceClose();
			CurrentPauseState = PauseState.PAUSED;
		}
		else if (CurrentPauseState == PauseState.PAUSED)
		{
			CurrentPauseState = PauseState.UNPAUSING;
			StartCoroutine(DoUnpause());
		}
		else
		{
			CurrentPauseState = PauseState.PAUSED;
		}
		if (ActiveBuildMode == BuildMode.MOBILE)
		{
			GAME_UI_PARENT.SetActive(CurrentPauseState != PauseState.PAUSED);
		}
		Time.timeScale = ((CurrentPauseState == PauseState.PAUSED) ? 0f : 1f);
		GAME_UI_MANAGER.PauseMenu.SetActive(CurrentPauseState == PauseState.PAUSED);
		GAME_UI_MANAGER.OptionsWindow.SetActive(value: false);
		GAME_UI_MANAGER.QuitWindow.SetActive(value: false);
	}

	private IEnumerator DoUnpause()
	{
		yield return new WaitForSeconds(0.01f);
		if (CurrentPauseState != PauseState.PAUSED)
		{
			CurrentPauseState = PauseState.UNPAUSED;
		}
	}

	private void Update()
	{
		if (TDInputManager.Menu == InputButtonState.DOWN)
		{
			SetPaused();
		}
		if (!Application.isFocused && CurrentPauseState != PauseState.PAUSED)
		{
			SetPaused();
		}
	}

	public void BuildLevel()
	{
		LEVEL_GENERATOR.GenerateLevel();
	}

	public void OnLevelBuildComplete()
	{
		BuildPaths();
	}

	public void BuildPaths()
	{
		LEVEL_GENERATOR.NavSurface.BuildNavMesh();
		PATH_MANAGER.BuildPaths();
	}

	public void OnBuildPathsComplete()
	{
		Elevator = UnityEngine.Object.FindObjectOfType<Interactable_Elevator>();
		ActivateEntities();
	}

	public void ActivateEntities()
	{
		Monster = AvailableCharacters[(int)LEVEL_GENERATOR.ActiveTheme.MonsterToActivate];
		UseableCharacter useableCharacter = LEVEL_GENERATOR.ActiveTheme.PlayerToActivate;
		if (useableCharacter == UseableCharacter.BORIS)
		{
			useableCharacter = (UseableCharacter)SaveManager.DATA.C;
			if (useableCharacter != UseableCharacter.LOST_ONE && useableCharacter != UseableCharacter.SAMMY)
			{
				useableCharacter = UseableCharacter.BORIS;
			}
		}
		Player = AvailableCharacters[(int)useableCharacter];
		JumpscareManager.currentMonster = LEVEL_GENERATOR.ActiveTheme.MonsterToActivate;
		if (Player == null)
		{
			Debug.LogError("NO PLAYER?!");
			return;
		}
		if (Monster != null)
		{
			Monster.m_ControlMode = KBCharacterController.InputControllerMode.AI_MONSTER;
			if (Day > 2 || LevelGenerator.CurrentTheme != LevelTheme.REGULAR_FLOOR)
			{
				SpawnMonster();
			}
		}
		else if (SpawnPointIndex != 0)
		{
			Player.transform.position = SpawnPoints[SpawnPointIndex].transform.position;
		}
		Player.m_ControlMode = KBCharacterController.InputControllerMode.PLAYER;
		Player.gameObject.SetActive(value: true);
		LoadingController.LoadingComplete();
		GAME_UI_MANAGER.Fader.TransitionOut();
	}

	public void SpawnMonster()
	{
		if (!(Monster == null) && !Monster.gameObject.activeSelf)
		{
			PATH_MANAGER.RegisterBendySpawnPoints();
			PathfinderNode pathfinderNode = PATH_MANAGER.BendySpawnableNodeList[UnityEngine.Random.Range(0, PATH_MANAGER.BendySpawnableNodeList.Count)];
			Monster.transform.position = pathfinderNode.transform.position + Vector3.up * 1f;
			Monster.gameObject.SetActive(value: true);
		}
	}

	public void DeactivatePlayer()
	{
		Player.gameObject.SetActive(value: false);
	}

	public void ReactivatePlayer()
	{
		Player.gameObject.SetActive(value: true);
	}

	public void KillPlayer(KBCharacterController Attacker)
	{
		isPlayerDead = true;
		DeactivatePlayer();
		StartCoroutine(KillPlayerSequence());
		GAME_UI_PARENT.SetActive(value: false);
		LevelGenerator.CurrentTheme = LevelTheme.HUB_1;
		SpawnPointIndex = 1;
		MUSIC_MANAGER.SFXModifier = 0f;
		MUSIC_MANAGER.ChangeMusic(Attacker.CaptureMusic, 10f, 1f, StopTrnasitions: true, Loop: false);
	}

	public IEnumerator KillPlayerSequence()
	{
		GAME_UI_MANAGER.ActivateDeathBlackout();
		yield return new WaitForSeconds(2.5f);
		SceneManager.LoadScene("DeathScene");
	}

	public void GetShoppingItem(Interactable_ShoppingCollectable item)
	{
		collectedItems.Add(item);
		ShoppingListCount++;
		if (ShoppingListCount >= ShoppingListGoal)
		{
			UIPopupMessage.ShowUniqueMessage("Run for the {elevator}!!!");
			Elevator.IsActive = true;
		}
		else if (ShoppingListCount > 3)
		{
			if (Day == 1)
			{
				SpawnMonster();
			}
		}
		else if (ShoppingListCount > 0 && Day == 2)
		{
			SpawnMonster();
		}
	}

	public void DrainBorisStamina()
	{
		BorisStamina = Mathf.Clamp01(BorisStamina - 0.03f * Time.deltaTime);
		if (BorisStamina <= 0f)
		{
			Player.m_CanRun = false;
			Player.m_InputController.isRunning = false;
		}
		else
		{
			Player.m_CanRun = true;
		}
	}

	public void IncreaseBorisStamina(float ammount)
	{
		BorisStamina = Mathf.Clamp01(BorisStamina += ammount);
		Player.m_CanRun = true;
	}

	public void decreaseBorisStamina(float ammount)
	{
		BorisStamina = Mathf.Clamp01(BorisStamina -= ammount);
	}

	public void LevelWon()
	{
		switch (LevelGenerator.CurrentTheme)
		{
		case LevelTheme.ALICE_FLOOR:
			if (!SaveManager.DATA.FOUND_A)
			{
				UIPopupMessage.ShowUniqueMessage("{Alice Angel} Encounters Unlocked");
				SaveManager.DATA.FOUND_A = true;
			}
			break;
		case LevelTheme.PROJECTIONIST_FLOOR:
			SaveManager.DATA.FOUND_B = true;
			break;
		}
		LevelGenerator.CurrentTheme = LevelTheme.HUB_1;
		SpawnPointIndex = 1;
		SavedItems.AddRange(collectedItems);
		for (int i = 0; i < SavedItems.Count; i++)
		{
			switch (SavedItems[i].itemType)
			{
			case ShoppingItem.BONE:
				SaveManager.DATA.Bones++;
				break;
			case ShoppingItem.BOOK:
				SaveManager.DATA.Books++;
				break;
			case ShoppingItem.GEAR:
				SaveManager.DATA.Gears++;
				break;
			case ShoppingItem.PLUNGER:
				SaveManager.DATA.Plungers++;
				break;
			case ShoppingItem.RADIO:
				SaveManager.DATA.Radios++;
				break;
			case ShoppingItem.SPOON:
				SaveManager.DATA.Spoons++;
				break;
			case ShoppingItem.WRENCH:
				SaveManager.DATA.Wrenches++;
				break;
			}
		}
		Day++;
		if (Day > SaveManager.DATA.HighestDayCount)
		{
			SaveManager.DATA.HighestDayCount = Day;
		}
		SaveManager.Save();
		StartCoroutine(LevelWonSequence());
	}

	private IEnumerator LevelWonSequence()
	{
		yield return new WaitForSeconds(3.5f);
		MUSIC_MANAGER.SFXModifier = 0f;
		GAME_UI_MANAGER.Fader.TransitionIn();
		GAME_UI_MANAGER.GameUIGroup.TransitionOut();
		LoadingController.IsLoading();
		yield return new WaitForSeconds(3f);
		SceneManager.LoadScene("GameScene");
	}

	public void LoadNewLevel(LevelTheme lvl, bool Force = false)
	{
		if (lvl == LevelTheme.REGULAR_FLOOR && Day > 2)
		{
			int num = UnityEngine.Random.Range(0, 23);
			if (SaveManager.DATA.FOUND_A && UnityEngine.Random.Range(0, 6) == 1)
			{
				lvl = LevelTheme.REGULAR_A;
			}
			else if (SaveManager.DATA.FOUND_B && UnityEngine.Random.Range(0, 10) == 1)
			{
				lvl = LevelTheme.REGULAR_P;
			}
			else if (SaveManager.DATA.FOUND_D && UnityEngine.Random.Range(0, 4) == 1)
			{
				lvl = LevelTheme.REGULAR_B;
			}
			else if (Day > 10 && UnityEngine.Random.Range(0, 3000) == 414)
			{
				lvl = LevelTheme.REGLUAR_FL0OR;
			}
			else if (num <= Mathf.Clamp(SaveManager.DATA.HeartCount, 0, 4))
			{
				lvl = LevelTheme.PROJECTIONIST_FLOOR;
			}
		}
		LevelGenerator.CurrentTheme = lvl;
		if (Force)
		{
			SceneManager.LoadScene("GameScene");
		}
		else
		{
			StartCoroutine(LevelWonSequence());
		}
	}

	public void SetIsRunning(bool isRunning)
	{
		if ((bool)Player)
		{
			Player.SetIsRunning(isRunning);
		}
	}

	private void OnDestroy()
	{
		instance = null;
	}
}
