using TNetSdk;
using UnityEngine;

public class CMutiplyGame
{
	public enum kMutiplyState
	{
		None = 0,
		Connect = 1,
		Login = 2,
		SearchRoom = 3,
		WaitForPlay = 4,
		WaitToSearch = 5
	}

	protected static CMutiplyGame m_Instance;

	protected iDataCenter m_DataCenter;

	protected iGameState m_GameState;

	protected kMutiplyState m_State;

	public CMutiplyGame()
	{
		m_State = kMutiplyState.None;
	}

	public static CMutiplyGame GetInstance()
	{
		if (m_Instance == null)
		{
			m_Instance = new CMutiplyGame();
		}
		return m_Instance;
	}

	public void Initialize()
	{
		m_GameState = iGameApp.GetInstance().m_GameState;
		iGameData gameData = iGameApp.GetInstance().m_GameData;
		if (gameData != null)
		{
			m_DataCenter = gameData.GetDataCenter();
		}
	}

	public void OnConnectSuccess(TNetEventData tEvent)
	{
		Debug.Log("OnConnectSuccess");
		TNetManager.GetInstance().Login(m_GameState.UserName, string.Empty);
	}

	public void OnLogin(TNetEventData tEvent)
	{
		SysLoginResCmd.Result result = (SysLoginResCmd.Result)(int)tEvent.data["result"];
		Debug.Log("OnLogin result = " + result);
		if (result != SysLoginResCmd.Result.ok)
		{
			DisConnect();
			return;
		}
		iGameApp.GetInstance().EnterScene(kGameSceneEnum.Room);
		m_GameState.BattleLevel = Random.Range(0, 150);
		CRoomManager.CRoomCharInfo cRoomCharInfo = new CRoomManager.CRoomCharInfo();
		if (cRoomCharInfo != null)
		{
			cRoomCharInfo.m_nCharID = m_DataCenter.CurCharID;
			cRoomCharInfo.m_nBattleLevel = m_GameState.BattleLevel;
			CRoomManager.GetInstance().SendUserVariable(cRoomCharInfo);
		}
		CRoomManager.GetInstance().Initialize();
		CRoomManager.GetInstance().SearchRoom(m_GameState.BattleLevel);
	}

	public void Connect()
	{
		TNetManager.GetInstance().Connect("192.168.0.190", 7000);
		TNetObject netObject = TNetManager.GetInstance().NetObject;
		if (netObject != null)
		{
			netObject.AddEventListener(TNetEventSystem.CONNECTION, OnConnectSuccess);
			netObject.AddEventListener(TNetEventSystem.LOGIN, OnLogin);
		}
	}

	public void DisConnect()
	{
		TNetManager.GetInstance().DisConnect();
		UnRegisterEvent();
	}

	public void UnRegisterEvent()
	{
		TNetObject netObject = TNetManager.GetInstance().NetObject;
		if (netObject != null)
		{
			netObject.RemoveEventListener(TNetEventSystem.CONNECTION, OnConnectSuccess);
			netObject.RemoveEventListener(TNetEventSystem.LOGIN, OnLogin);
		}
	}
}
