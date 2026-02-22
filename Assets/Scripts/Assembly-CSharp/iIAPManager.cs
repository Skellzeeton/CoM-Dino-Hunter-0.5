using System.Collections;
using EventCenter;
using UnityEngine;
using gyIAPSystem;

public class iIAPManager : MonoBehaviour
{
	protected enum kPingState
	{
		None = 0,
		Pinging = 1,
		Success = 2,
		Fail = 3
	}

	protected CIAPCenter m_IAPCenter;

	protected int m_nCurPurchase;

	protected bool m_bSendPurchase;

	protected kPingState m_PingState;

	private void Awake()
	{
		Object.DontDestroyOnLoad(base.gameObject);
		m_IAPCenter = new CIAPCenter();
		m_IAPCenter.Load();
		m_bSendPurchase = false;
		m_nCurPurchase = -1;
		m_PingState = kPingState.None;
	}

	private void Start()
	{
	}

	private void Update()
	{
		Update(Time.deltaTime);
		if (Input.GetKeyDown(KeyCode.Alpha6))
		{
			Purchase(1);
		}
	}

	protected void Update(float deltaTime)
	{
		if (m_nCurPurchase == -1)
		{
			return;
		}
		if (!m_bSendPurchase)
		{
			if (m_PingState == kPingState.Pinging)
			{
				return;
			}
			if (m_PingState == kPingState.Success)
			{
				m_PingState = kPingState.None;
				CIAPInfo cIAPInfo = m_IAPCenter.Get(m_nCurPurchase);
				if (cIAPInfo != null)
				{
					IAPPlugin.NowPurchaseProduct(cIAPInfo.sKey, "1");
					m_bSendPurchase = true;
				}
			}
			else if (m_PingState == kPingState.Fail)
			{
				m_PingState = kPingState.None;
				m_nCurPurchase = -1;
				OnPurchaseFailed(m_nCurPurchase);
			}
			return;
		}
		int purchaseStatus = IAPPlugin.GetPurchaseStatus();
		if (purchaseStatus != 0)
		{
			if (purchaseStatus == 1)
			{
				OnPurchaseSuccess(m_nCurPurchase);
				m_nCurPurchase = -1;
			}
			else if (purchaseStatus < 0)
			{
				OnPurchaseFailed(m_nCurPurchase);
				m_nCurPurchase = -1;
			}
		}
	}

	public IEnumerator TestPingApple()
	{
		m_PingState = kPingState.Pinging;
		WWW www = new WWW("http://www.apple.com/?rand=" + Random.Range(10, 99999));
		Debug.Log(www.url);
		yield return www;
		if (www.error != null)
		{
			Debug.Log("test ping failed " + www.error);
			m_PingState = kPingState.Fail;
		}
		else
		{
			Debug.Log("test ping successed ");
			m_PingState = kPingState.Success;
		}
	}

	public bool Purchase(int nID)
	{
		if (m_nCurPurchase != -1)
		{
			return false;
		}
		CIAPInfo cIAPInfo = m_IAPCenter.Get(nID);
		if (cIAPInfo == null)
		{
			return false;
		}
		m_bSendPurchase = false;
		m_nCurPurchase = nID;
		StartCoroutine(TestPingApple());
		return true;
	}

	public void OnPurchaseSuccess(int nID)
	{
		iGameData gameData = iGameApp.GetInstance().m_GameData;
		if (gameData == null)
		{
			return;
		}
		iDataCenter dataCenter = gameData.GetDataCenter();
		if (dataCenter == null)
		{
			return;
		}
		CIAPInfo cIAPInfo = m_IAPCenter.Get(nID);
		if (cIAPInfo != null)
		{
			if (cIAPInfo.isCrystal)
			{
				dataCenter.AddCrystal(cIAPInfo.nValue);
			}
			else
			{
				dataCenter.AddGold(cIAPInfo.nValue);
			}
			dataCenter.Save();
			TUIGameInfo tUIGameInfo = new TUIGameInfo();
			tUIGameInfo.player_info = new TUIPlayerInfo();
			tUIGameInfo.player_info.gold = dataCenter.Gold;
			tUIGameInfo.player_info.crystal = dataCenter.Crystal;
			global::EventCenter.EventCenter.Instance.Publish(this, new TUIEvent.BackEvent_SceneIAP("TUIEvent_IAPResult", tUIGameInfo, true));
		}
	}

	public void OnPurchaseFailed(int nID)
	{
		global::EventCenter.EventCenter.Instance.Publish(this, new TUIEvent.BackEvent_SceneIAP("TUIEvent_IAPResult"));
	}
}
