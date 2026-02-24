using UnityEngine;

public class iGameApp
{
	public static iGameApp m_Instance;

	public DebugGUI m_Debug;

	public iGizmos m_Gizmos;

	public iGameSceneBase m_GameScene;

	public iGameState m_GameState;

	public iGameData m_GameData;

	public iClearMemory m_ClearMemory;

	public iIAPManager m_IAPManager;

	public static iGameApp GetInstance()
	{
		if (m_Instance == null)
		{
			m_Instance = new iGameApp();
			m_Instance.Initialize();
		}
		return m_Instance;
	}

	public void Initialize()
	{
		MyUtils.SimulatePlatform = PlatformEnum.IOS;
		GameObject gameObject = new GameObject("_GizmosManager");
		if (gameObject != null)
		{
			gameObject.transform.position = Vector3.zero;
			gameObject.transform.rotation = Quaternion.identity;
			m_Gizmos = gameObject.AddComponent<iGizmos>();
		}
		gameObject = new GameObject("_DebugGUI");
		if (gameObject != null)
		{
			gameObject.transform.position = Vector3.zero;
			gameObject.transform.rotation = Quaternion.identity;
			m_Debug = gameObject.AddComponent<DebugGUI>();
			m_Debug.m_nMaxCount = 10;
		}
		gameObject = new GameObject("_ClearMemoryObject");
		if (gameObject != null)
		{
			gameObject.transform.position = Vector3.zero;
			gameObject.transform.rotation = Quaternion.identity;
			m_ClearMemory = gameObject.AddComponent<iClearMemory>();
		}
		gameObject = new GameObject("_IAPManager");
		if (gameObject != null)
		{
			gameObject.transform.position = Vector3.zero;
			gameObject.transform.rotation = Quaternion.identity;
			m_IAPManager = gameObject.AddComponent<iIAPManager>();
		}
		PrefabManager.Initialize();
		m_GameData = new iGameData();
		m_GameData.Load();
		m_GameState = new iGameState();
		m_GameState.Initialize();
		Debug.Log(MyUtils.Make32(5, 50));
		Screen.autorotateToLandscapeLeft = true;
		Screen.autorotateToLandscapeRight = true;
	}

	public void Destroy()
	{
	}

	public string GetKey()
	{
		return "helloworld";
	}

	public void EnterScene(string sName)
	{
		m_GameState.CurScene = kGameSceneEnum.OutOfGame;
		m_GameState.m_sLoadScene = sName;
		Application.LoadLevel("SceneLoad");
	}

	public void EnterScene(kGameSceneEnum gotoscene)
	{
		if (m_GameState.CurScene == kGameSceneEnum.Game)
		{
			DestroyScene();
		}
		switch (gotoscene)
		{
		case kGameSceneEnum.Game:
		{
			CUISound.GetInstance().Stop("BGM_theme");
			GameLevelInfo gameLevelInfo = m_GameData.GetGameLevelInfo(m_GameState.GameLevel);
			if (gameLevelInfo != null)
			{
				m_GameState.CurScene = kGameSceneEnum.Game;
				m_GameState.m_sLoadScene = gameLevelInfo.sSceneName;
				Application.LoadLevel("SceneLoad");
			}
			break;
		}
		case kGameSceneEnum.Map:
			CUISound.GetInstance().Play("BGM_theme");
			m_GameState.CurScene = kGameSceneEnum.Map;
			m_GameState.m_sLoadScene = "Scene_Map";
			Application.LoadLevel("SceneLoad");
			break;
		case kGameSceneEnum.Room:
			m_GameState.CurScene = kGameSceneEnum.Room;
			Application.LoadLevelAsync("SceneRoom");
			break;
		case kGameSceneEnum.Home:
			CUISound.GetInstance().Play("BGM_theme");
			m_GameState.CurScene = kGameSceneEnum.Home;
			m_GameState.m_sLoadScene = "Scene_MainMenu";
			Application.LoadLevel("SceneLoad");
			break;
		}
	}

	public void CreateScene()
	{
		iGameData gameData = GetInstance().m_GameData;
		if (gameData == null)
		{
			return;
		}
		iDataCenter dataCenter = gameData.GetDataCenter();
		if (dataCenter != null)
		{
			for (int i = 0; i < 3; i++)
			{
				CarryWeapon(i, dataCenter.GetSelectWeapon(i));
			}
			int gameLevel = m_GameState.GameLevel;
			switch (gameLevel)
			{
			case 0:
				m_GameScene = new iGameScene0();
				break;
			case 1:
				m_GameScene = new iGameScene1();
				break;
			case 2:
				m_GameScene = new iGameScene2();
				break;
			default:
				m_GameScene = new iGameSceneBase();
				break;
			}
			if (m_GameScene != null)
			{
				m_GameScene.Initialize();
				m_GameScene.InitializeGameLevel(gameLevel);
				m_GameScene.StartGame();
				PrefabManager.PreLoad();
			}
		}
	}

	public void DestroyScene()
	{
		if (m_GameScene != null)
		{
			m_GameScene.Destroy();
			m_GameScene = null;
			PrefabManager.DestroyPreLoad();
		}
	}

	public void ResetScene()
	{
	}

	public void Update(float deltaTime)
	{
		if (m_GameScene != null)
		{
			m_GameScene.Update(deltaTime);
		}
	}

	public void FixedUpdate(float deltaTime)
	{
		if (m_GameScene != null)
		{
			m_GameScene.FixedUpdate(deltaTime);
		}
	}

	public void LateUpdate(float deltaTime)
	{
		if (m_GameScene != null)
		{
			m_GameScene.LateUpdate(deltaTime);
		}
	}

	public void CarryWeapon(int nIndex, int nWeaponID)
	{
		iDataCenter dataCenter = m_GameData.GetDataCenter();
		if (dataCenter == null)
		{
			return;
		}
		int weaponLevel = dataCenter.GetWeaponLevel(nWeaponID);
		if (weaponLevel < 1)
		{
			return;
		}
		CWeaponInfoLevel weaponInfo = m_GameData.GetWeaponInfo(nWeaponID, weaponLevel);
		if (weaponInfo != null)
		{
			CWeaponBase cWeaponBase = null;
			switch (weaponInfo.nAttackMode)
			{
			case 1:
				cWeaponBase = new CWeaponMelee();
				break;
			case 2:
				cWeaponBase = new CWeaponShoot();
				break;
			case 3:
				cWeaponBase = new CWeaponSpawn();
				break;
			case 4:
				cWeaponBase = new CWeaponSpawnWithHead();
				break;
			case 5:
				cWeaponBase = new CWeaponHoldy();
				break;
			case 6:
				cWeaponBase = new CWeaponShotgun();
				break;
			}
			if (cWeaponBase != null)
			{
				cWeaponBase.Initialize(nWeaponID, weaponLevel);
				m_GameState.CarryWeapon(nIndex, cWeaponBase);
			}
		}
	}

	public void SetGizmosPoint(string sKey, Vector3 p, Color color)
	{
		if (!(m_Gizmos == null))
		{
			m_Gizmos.SetPoint(sKey, p, color);
		}
	}

	public void SetGizmosLine(string sKey, Vector3 p1, Vector3 p2, Color color)
	{
		if (!(m_Gizmos == null))
		{
			m_Gizmos.SetLine(sKey, p1, p2, color);
		}
	}

	public void SetGizmosRay(string sKey, Vector3 p, Vector3 dir, Color color)
	{
		if (!(m_Gizmos == null))
		{
			m_Gizmos.SetRay(sKey, p, dir, color);
		}
	}

	public void ScreenLog(string str)
	{
		if (!(m_Debug == null))
		{
			m_Debug.Debug(str);
		}
	}

	public void ClearMemory()
	{
		if (!(m_ClearMemory == null))
		{
			m_ClearMemory.ClearMemory();
		}
	}
}
