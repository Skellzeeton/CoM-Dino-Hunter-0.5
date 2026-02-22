using System.Collections.Generic;
using UnityEngine;
using gyAchievementSystem;
using gyTaskSystem;

public class iGameUIBase : MonoBehaviour
{
	protected Transform mPreLoadNode;

	protected gyUIManager m_UIManager;

	protected gyUIAimCross m_UIAimCross;

	protected int m_nCurWeaponType;

	protected iGameSceneBase m_GameScene;

	protected iGameData m_GameData;

	protected iGameState m_GameState;

	protected CPoolManage m_NGUIDamage;

	protected CPoolManage m_NGUILifeBar;

	protected CPoolManage m_NGUIScreenTip;

	protected CPoolManage m_NGUIGoldTip;

	protected CPoolManage m_NGUIMaterialTip;

	protected CPoolManage m_NGUITextTip;

	protected Dictionary<int, gyLifeBarHUD> m_dictLifeBar;

	protected iGameTaskUIPlane m_GameTaskUIPlane;

	protected CUIProtraitInfo m_MainPlayerPortraitInfo;

	protected Dictionary<int, CUIProtraitInfo> m_dictTeamMate;

	protected Transform[] m_TeamMateNode;

	protected int m_nCurWeaponIndex;

	private void Awake()
	{
		GameObject gameObject = GameObject.Find("UI Root (2D)");
		if (gameObject == null)
		{
			Debug.LogError("cant find UI Root (2D)");
			return;
		}
		m_UIManager = gameObject.GetComponent<gyUIManager>();
		if (m_UIManager == null)
		{
			Debug.LogError("cant find gyUIManager");
			return;
		}
		if (m_UIManager.mToolPanel != null)
		{
			m_UIManager.mToolPanel.gameObject.SetActiveRecursively(false);
		}
		if (m_UIManager.mPause != null)
		{
			m_UIManager.mPause.gameObject.SetActiveRecursively(false);
		}
		if (mPreLoadNode == null)
		{
			GameObject gameObject2 = GameObject.Find("GamePreLoad");
			if (gameObject2 == null)
			{
				gameObject2 = new GameObject("GamePreLoad");
			}
			if (gameObject2 != null)
			{
				mPreLoadNode = gameObject2.transform;
			}
		}
		m_dictLifeBar = new Dictionary<int, gyLifeBarHUD>();
		m_GameTaskUIPlane = GetControl<iGameTaskUIPlane>("_AnchorTop/NGUITaskUIPlane");
		m_MainPlayerPortraitInfo = new CUIProtraitInfo();
		m_MainPlayerPortraitInfo.Initialize(m_UIManager.mHeadPortrait);
		float num = (float)Screen.height / 320f;
		float num2 = (float)Screen.width / 480f;
		float num3 = ((!(num < num2)) ? num2 : num);
		foreach (Transform item in m_UIManager.mParent)
		{
			if (item.name.IndexOf("Anchor") != -1)
			{
				item.localScale *= num3;
			}
		}
		Debug.Log("Screen size = " + Screen.width + " * " + Screen.height);
		Debug.Log("Screen scale multiplier = " + num3);
	}

	private void Update()
	{
		if (CAchievementManager.GetInstance().GetTipCount() > 0 && m_UIManager != null && m_UIManager.mAchievementTip != null && !m_UIManager.mAchievementTip.isActive)
		{
			CAchievementTip cAchievementTip = CAchievementManager.GetInstance().PopTip();
			if (cAchievementTip != null)
			{
				m_UIManager.mAchievementTip.ShowTip(cAchievementTip.sName, cAchievementTip.nStep);
			}
		}
	}

	public void Initialize()
	{
		m_GameScene = iGameApp.GetInstance().m_GameScene;
		m_GameData = iGameApp.GetInstance().m_GameData;
		m_GameState = iGameApp.GetInstance().m_GameState;
		m_GameTaskUIPlane.Initialize();
		InitTaskUI();
		m_NGUIDamage = new CPoolManage();
		m_NGUIDamage.Initialize("Artist/GameUI/NGUIDamage", mPreLoadNode, m_UIManager.mParent, 10);
		m_NGUILifeBar = new CPoolManage();
		m_NGUILifeBar.Initialize("Artist/GameUI/NGUILifeBar", mPreLoadNode, m_UIManager.mParent, 10);
		m_NGUIScreenTip = new CPoolManage();
		m_NGUIScreenTip.Initialize("Artist/GameUI/NGUIScreenTip", mPreLoadNode, m_UIManager.mParent, 1);
		m_NGUIGoldTip = new CPoolManage();
		m_NGUIGoldTip.Initialize("Artist/GameUI/NGUIGoldTip", mPreLoadNode, m_UIManager.mParent, 10);
		m_NGUIMaterialTip = new CPoolManage();
		m_NGUIMaterialTip.Initialize("Artist/GameUI/NGUIMaterialTip", mPreLoadNode, m_UIManager.mParent, 5);
		m_NGUITextTip = new CPoolManage();
		m_NGUITextTip.Initialize("Artist/GameUI/NGUITextTip", mPreLoadNode, m_UIManager.mParent, 5);
		Reset();
	}

	public void Destroy()
	{
		foreach (gyLifeBarHUD value in m_dictLifeBar.Values)
		{
			if (value.gameObject != null)
			{
				Object.Destroy(value.gameObject);
			}
		}
		m_dictLifeBar.Clear();
		ClearTeamMateProtrait();
	}

	public void InitTaskUI()
	{
		if (m_GameScene.m_TaskManager == null)
		{
			return;
		}
		List<CTaskBase> taskList = m_GameScene.m_TaskManager.GetTaskList();
		if (taskList == null)
		{
			return;
		}
		m_GameTaskUIPlane.Clear();
		foreach (CTaskBase item in taskList)
		{
			CTaskInfo taskInfo = item.GetTaskInfo();
			if (taskInfo != null)
			{
				iGameTaskUIBase iGameTaskUIBase2 = m_GameTaskUIPlane.Add(taskInfo.nType);
				if (!(iGameTaskUIBase2 == null))
				{
					iGameTaskUIBase2.Initialize(item);
				}
			}
		}
	}

	public void Reset()
	{
		ShowSuccess(false);
		ShowFailed(false);
	}

	public T GetControl<T>(string path) where T : MonoBehaviour
	{
		Transform transform = m_UIManager.mParent.Find(path);
		if (transform == null)
		{
			return (T)null;
		}
		return transform.GetComponent<T>();
	}

	public GameObject GetControl(string path)
	{
		Transform transform = m_UIManager.mParent.Find(path);
		if (transform == null)
		{
			return null;
		}
		return transform.gameObject;
	}

	public GameObject AddControl(int nPrefab, Transform parent)
	{
		GameObject gameObject = PrefabManager.Get(nPrefab);
		if (gameObject == null)
		{
			return null;
		}
		Vector3 localScale = gameObject.transform.localScale;
		GameObject gameObject2 = (GameObject)Object.Instantiate(gameObject);
		if (gameObject2 == null)
		{
			return null;
		}
		if (parent != null)
		{
			gameObject2.transform.parent = parent;
		}
		else
		{
			gameObject2.transform.parent = m_UIManager.mParent;
		}
		gameObject2.transform.localScale = localScale;
		return gameObject2;
	}

	public void AddDmgUI(float fValue, Vector2 v2Pos, Color color, gyUILabelDmg.kMode mode)
	{
		GameObject gameObject = m_NGUIDamage.Get();
		if (!(gameObject == null))
		{
			gyUILabelDmg component = gameObject.GetComponent<gyUILabelDmg>();
			if (!(component == null))
			{
				gameObject.transform.parent = m_UIManager.mParent;
				component.SetLabel(fValue.ToString());
				component.SetColor(color);
				component.Go(v2Pos, mode);
			}
		}
	}

	public void AddExpText(Vector2 v2Pos, int nExp)
	{
		GameObject gameObject = m_NGUIDamage.Get();
		if (!(gameObject == null))
		{
			gyUILabelDmg component = gameObject.GetComponent<gyUILabelDmg>();
			if (!(component == null))
			{
				gameObject.transform.parent = m_UIManager.mParent;
				component.SetLabel("EXP:" + nExp);
				component.SetColor(new Color(0f, 0.15f, 0.9f));
				component.Go(v2Pos, gyUILabelDmg.kMode.Mode5);
			}
		}
	}

	public void AddGoldUI(float fValue, Vector3 v3Pos)
	{
		GameObject gameObject = m_NGUIGoldTip.Get();
		if (!(gameObject == null))
		{
			gyUILabelDmg component = gameObject.GetComponent<gyUILabelDmg>();
			if (!(component == null))
			{
				gameObject.transform.parent = m_UIManager.mParent;
				component.SetLabel(fValue.ToString());
				component.Go(v3Pos);
			}
		}
	}

	public void AddMaterialUI(Vector3 v3Pos, string sIcon, int nCount)
	{
		GameObject gameObject = m_NGUIMaterialTip.Get();
		if (gameObject == null)
		{
			return;
		}
		gyUIMaterials component = gameObject.GetComponent<gyUIMaterials>();
		if (!(component == null))
		{
			gameObject.transform.parent = m_UIManager.mParent;
			gameObject.transform.localPosition = Vector3.zero;
			component.SetIcon(sIcon);
			component.SetValue(nCount);
			Vector3 vector = Vector3.zero;
			if (m_UIManager != null && m_UIManager.mHeadPortrait != null)
			{
				vector = m_UIManager.mHeadPortrait.transform.localPosition;
			}
			component.Go(v3Pos, vector);
		}
	}

	public void SetWeaponIcon(string sName)
	{
		if (!(m_UIManager == null) && !(m_UIManager.mWeapon == null))
		{
			iGameUIWeapon component = m_UIManager.mWeapon.GetComponent<iGameUIWeapon>();
			if (!(component == null))
			{
				component.SetIcon(sName);
			}
		}
	}

	public void SetSkillIcon(string sName)
	{
		if (!(m_UIManager == null) && !(m_UIManager.mSkill == null))
		{
			m_UIManager.mSkill.SetIcon(sName);
		}
	}

	public gyLifeBarHUD CreateLifeBar(CCharBase target)
	{
		if (target == null)
		{
			return null;
		}
		GameObject gameObject = m_NGUILifeBar.Get();
		if (gameObject == null)
		{
			return null;
		}
		gyLifeBarHUD component = gameObject.GetComponent<gyLifeBarHUD>();
		if (component == null)
		{
			return null;
		}
		component.Initialize(target);
		m_dictLifeBar.Add(target.UID, component);
		return component;
	}

	public void RemoveLifeBar(int nUID)
	{
		if (m_dictLifeBar.ContainsKey(nUID))
		{
			gyUIPoolObject component = m_dictLifeBar[nUID].GetComponent<gyUIPoolObject>();
			if (component != null)
			{
				component.TakeBack(0f);
			}
			else
			{
				Object.Destroy(m_dictLifeBar[nUID].gameObject);
			}
			m_dictLifeBar.Remove(nUID);
		}
	}

	public void ShootLifeBar(int nUID)
	{
		foreach (KeyValuePair<int, gyLifeBarHUD> item in m_dictLifeBar)
		{
			if (nUID != item.Key)
			{
				item.Value.SetTime(0f, 0.5f);
			}
		}
	}

	public gyUIScreenTip CreateScreenTip(GameObject actor, GameObject target)
	{
		if (actor == null || target == null)
		{
			return null;
		}
		GameObject gameObject = m_NGUIScreenTip.Get();
		if (gameObject == null)
		{
			return null;
		}
		gyUIScreenTip component = gameObject.GetComponent<gyUIScreenTip>();
		if (component == null)
		{
			return null;
		}
		component.Initialize(actor, target);
		return component;
	}

	public void RemoveScreenTip(gyUIScreenTip screentip)
	{
		if (!(screentip.gameObject == null))
		{
			screentip.Clear();
			gyUIPoolObject component = screentip.gameObject.GetComponent<gyUIPoolObject>();
			if (component != null)
			{
				component.TakeBack(0f);
			}
			else
			{
				Object.Destroy(screentip.gameObject);
			}
		}
	}

	public iGameTaskUIPlane GetTaskPlane()
	{
		return m_GameTaskUIPlane;
	}

	public void ShowSuccess(bool bShow)
	{
		if (m_UIManager == null || m_UIManager.mPanelMissionComplete == null)
		{
			return;
		}
		m_UIManager.mPanelMissionComplete.Show(bShow);
		if (bShow)
		{
			if (m_GameScene.CurGameLevelInfo != null)
			{
				m_UIManager.mPanelMissionComplete.SetGainExp(m_GameScene.CurGameLevelInfo.nRewardExp);
				m_UIManager.mPanelMissionComplete.SetGainGold(m_GameScene.CurGameLevelInfo.nRewardGold);
			}
			m_UIManager.mPanelMissionComplete.SetGainGoldEarned(m_GameState.GainGoldInGame);
		}
	}

	public void ShowFailed(bool bShow)
	{
		if (!(m_UIManager == null) && !(m_UIManager.mPanelMissionFailed == null))
		{
			m_UIManager.mPanelMissionFailed.Show(bShow);
		}
	}

	public void ShowRevive(bool bShow)
	{
		if (m_UIManager == null || m_UIManager.mPanelRevive == null)
		{
			return;
		}
		m_UIManager.mPanelRevive.Show(bShow);
		if (!bShow)
		{
			return;
		}
		if (m_GameState.HasAnyMaterial())
		{
			m_UIManager.mPanelRevive.ShowStatistcs(true);
			for (int i = 0; i < iMacroDefine.GainMaterialFromGameMax; i++)
			{
				CMaterialInfo gainMaterial = m_GameState.GetGainMaterial(i);
				if (gainMaterial == null || gainMaterial.nItemID == -1)
				{
					continue;
				}
				m_UIManager.mPanelRevive.ShowItem(i, true);
				m_UIManager.mPanelRevive.SetCount(i, gainMaterial.nItemCount);
				CItemInfoLevel itemInfo = m_GameData.GetItemInfo(gainMaterial.nItemID, 1);
				if (itemInfo != null)
				{
					GameObject gameObject = PrefabManager.Get("Artist/Atlas/Material/" + itemInfo.sIcon);
					if (gameObject != null)
					{
						m_UIManager.mPanelRevive.SetIcon(i, gameObject.GetComponent<UIAtlas>());
					}
				}
			}
		}
		m_UIManager.mPanelRevive.SetReviveTime(10f);
	}

	public void ShowMaterial(bool bShow)
	{
		if (m_UIManager == null || m_UIManager.mPanelMaterial == null)
		{
			return;
		}
		m_UIManager.mPanelMaterial.Show(bShow);
		if (!bShow)
		{
			return;
		}
		for (int i = 0; i < iMacroDefine.GainMaterialFromGameMax; i++)
		{
			CMaterialInfo gainMaterial = m_GameState.GetGainMaterial(i);
			if (gainMaterial == null || gainMaterial.nItemID == -1)
			{
				continue;
			}
			m_UIManager.mPanelMaterial.ShowItem(i, true);
			m_UIManager.mPanelMaterial.SetCount(i, gainMaterial.nItemCount);
			CItemInfoLevel itemInfo = m_GameData.GetItemInfo(gainMaterial.nItemID, 1);
			if (itemInfo != null)
			{
				GameObject gameObject = PrefabManager.Get("Artist/Atlas/Material/" + itemInfo.sIcon);
				if (gameObject != null)
				{
					m_UIManager.mPanelMaterial.SetIcon(i, gameObject.GetComponent<UIAtlas>());
				}
			}
		}
		iDataCenter dataCenter = m_GameData.GetDataCenter();
		if (dataCenter != null)
		{
			m_UIManager.mPanelMaterial.SetStashMax(dataCenter.StashCountMax);
			m_UIManager.mPanelMaterial.SetStashCur(dataCenter.StashCount);
		}
	}

	public void ShowLevelUp(bool bShow)
	{
		if (!(m_UIManager == null) && !(m_UIManager.mPanelLevelUp == null))
		{
			m_UIManager.mPanelLevelUp.Show(bShow);
			if (bShow)
			{
				m_UIManager.mPanelLevelUp.SetLevelContext(m_GameState.nLastLevel, m_GameState.nNowLevel);
				m_UIManager.mPanelLevelUp.SetHPContext(m_GameState.nLastHP, m_GameState.nNowHP);
			}
		}
	}

	public void AddTeamMateProtrait(int nUID)
	{
	}

	public void DelTeamMateProtrait(int nUID)
	{
	}

	public void ClearTeamMateProtrait()
	{
	}

	public void ResortTeamMateProtrait()
	{
	}

	public CUIProtraitInfo GetProtrait(int nUID)
	{
		if (nUID == -1)
		{
			return m_MainPlayerPortraitInfo;
		}
		return null;
	}

	public void SetProtraitIcon(string sName, int nUID = -1)
	{
		CUIProtraitInfo protrait = GetProtrait(nUID);
		if (protrait != null)
		{
			protrait.SetIcon(sName);
		}
	}

	public void SetProtraitName(string sName, int nUID = -1)
	{
		CUIProtraitInfo protrait = GetProtrait(nUID);
		if (protrait != null)
		{
			protrait.SetName(sName);
		}
	}

	public void SetProtraitLife(float fRate, int nUID = -1)
	{
		CUIProtraitInfo protrait = GetProtrait(nUID);
		if (protrait != null)
		{
			protrait.SetLife(fRate);
		}
	}

	public void SetProtraitExp(int nTimes, float fExpRate, int nUID = -1)
	{
		CUIProtraitInfo protrait = GetProtrait(nUID);
		if (protrait != null)
		{
			protrait.SetExp(fExpRate);
		}
	}

	public void SetProtraitLevel(int nLevel, int nUID = -1)
	{
		CUIProtraitInfo protrait = GetProtrait(nUID);
		if (protrait != null)
		{
			protrait.SetLevel(nLevel);
		}
	}

	public void PlayProtraitLevelAnim(int nUID = -1)
	{
		CUIProtraitInfo protrait = GetProtrait(nUID);
		if (protrait != null)
		{
			protrait.ShowLevelAnim();
		}
	}

	public void ShowGameUI()
	{
		if (m_UIManager.mHeadPortrait != null)
		{
			m_UIManager.mHeadPortrait.SetActiveRecursively(true);
		}
		if (m_UIManager.mWeapon != null)
		{
			m_UIManager.mWeapon.gameObject.SetActiveRecursively(true);
		}
		if (m_UIManager.mSkill != null)
		{
			m_UIManager.mSkill.gameObject.SetActiveRecursively(true);
		}
		if (m_UIManager.mWheelMove != null)
		{
			m_UIManager.mWheelMove.gameObject.SetActiveRecursively(true);
		}
		if (m_UIManager.mWheelShoot != null)
		{
			m_UIManager.mWheelShoot.gameObject.SetActiveRecursively(true);
		}
		if (m_UIManager.mTaskPlane != null)
		{
			m_UIManager.mTaskPlane.SetActiveRecursively(true);
		}
		if (m_UIAimCross != null)
		{
			m_UIAimCross.gameObject.SetActiveRecursively(true);
		}
	}

	public void HideGameUI()
	{
		if (m_UIManager.mHeadPortrait != null)
		{
			m_UIManager.mHeadPortrait.SetActiveRecursively(false);
		}
		if (m_UIManager.mWeapon != null)
		{
			m_UIManager.mWeapon.gameObject.SetActiveRecursively(false);
		}
		if (m_UIManager.mSkill != null)
		{
			m_UIManager.mSkill.gameObject.SetActiveRecursively(false);
		}
		if (m_UIManager.mWheelMove != null)
		{
			m_UIManager.mWheelMove.gameObject.SetActiveRecursively(false);
		}
		if (m_UIManager.mWheelShoot != null)
		{
			m_UIManager.mWheelShoot.gameObject.SetActiveRecursively(false);
		}
		if (m_UIManager.mTaskPlane != null)
		{
			m_UIManager.mTaskPlane.SetActiveRecursively(false);
		}
		if (m_UIAimCross != null)
		{
			m_UIAimCross.gameObject.SetActiveRecursively(false);
		}
	}

	public void FadeIn(float fTime)
	{
		if (!(m_UIManager.mScreenMask == null))
		{
			m_UIManager.mScreenMask.FadeIn(fTime);
		}
	}

	public void FadeOut(float fTime)
	{
		if (!(m_UIManager.mScreenMask == null))
		{
			m_UIManager.mScreenMask.FadeOut(fTime);
		}
	}

	public void MovieUIIn(float fTime)
	{
		if (!(m_UIManager.mMovieMask == null))
		{
			m_UIManager.mMovieMask.MoveIn(false);
		}
	}

	public void MovieUIOut(float fTime)
	{
		if (!(m_UIManager.mMovieMask == null))
		{
			m_UIManager.mMovieMask.MoveOut(true);
		}
	}

	public void InitAimCross(int nWeaponType, float fPrecise)
	{
		if (m_nCurWeaponType != nWeaponType && m_UIAimCross != null)
		{
			Object.Destroy(m_UIAimCross.gameObject);
			m_UIAimCross = null;
		}
		m_nCurWeaponType = nWeaponType;
		if (m_UIAimCross == null)
		{
			GameObject original = null;
			switch (nWeaponType)
			{
			case 2:
				original = PrefabManager.Get("Artist/GameUI/NGUIAimPoint_AutoRifle");
				break;
			case 0:
				original = PrefabManager.Get("Artist/GameUI/NGUIAimPoint_HandGun");
				break;
			case 4:
				original = PrefabManager.Get("Artist/GameUI/NGUIAimPoint_HoldGun");
				break;
			case 1:
				original = PrefabManager.Get("Artist/GameUI/NGUIAimPoint_Melee");
				break;
			case 5:
				original = PrefabManager.Get("Artist/GameUI/NGUIAimPoint_Rocket");
				break;
			case 3:
				original = PrefabManager.Get("Artist/GameUI/NGUIAimPoint_ShootGun");
				break;
			}
			GameObject gameObject = Object.Instantiate(original) as GameObject;
			if (gameObject != null)
			{
				if (m_UIManager != null && m_UIManager.mAchorCenter != null)
				{
					gameObject.transform.parent = m_UIManager.mAchorCenter;
					gameObject.transform.localScale = Vector3.one;
					gameObject.transform.localPosition = Vector3.zero;
				}
				m_UIAimCross = gameObject.GetComponent<gyUIAimCross>();
			}
		}
		if (!(m_UIAimCross == null))
		{
			m_UIAimCross.Initialize(fPrecise);
		}
	}

	public void ExpandAimCross()
	{
		if (!(m_UIAimCross == null))
		{
			m_UIAimCross.Expand();
		}
	}

	public void ShowAchievementTip(string str, int star)
	{
		if (!(m_UIManager.mAchievementTip == null))
		{
			m_UIManager.mAchievementTip.ShowTip(str, star);
		}
	}

	public void ShowTip(string str)
	{
		GameObject gameObject = m_NGUITextTip.Get();
		if (!(gameObject == null))
		{
			gyUILabelDmg component = gameObject.GetComponent<gyUILabelDmg>();
			if (!(component == null))
			{
				component.SetLabel(str);
				component.Go(new Vector2(-Screen.width / 2, 0f));
			}
		}
	}

	public void ShowScreenBlood(bool bShow, float fAlpha = 1f, float fTime = 0.5f)
	{
		if (!(m_UIManager == null) && !(m_UIManager.mScreenBloodMask == null))
		{
			m_UIManager.mScreenBloodMask.ShowMask(bShow, fAlpha);
			if (bShow)
			{
				m_UIManager.mScreenBloodMask.FadeIn(fTime);
			}
		}
	}

	public void SetBulletNum(int nNum, int nMax)
	{
		if (!(m_UIManager == null) && !(m_UIManager.mBulletNum == null) && !(m_UIManager.mBulletIcon == null))
		{
			if (nMax == 0)
			{
				m_UIManager.mBulletNum.gameObject.active = false;
				m_UIManager.mBulletIcon.gameObject.active = false;
			}
			else
			{
				m_UIManager.mBulletNum.gameObject.active = true;
				m_UIManager.mBulletIcon.gameObject.active = true;
				m_UIManager.mBulletNum.text = nNum + "/" + nMax;
			}
		}
	}

	public void RegisterEvent()
	{
		gyUIEventRegister gyUIEventRegister2 = null;
		gyUIEventRegister2 = GetEventRegister(m_UIManager.mPause);
		gyUIEventRegister2.RegisterOnClick(Event_Pause);
		gyUIEventRegister2 = GetEventRegister(m_UIManager.mWeapon);
		gyUIEventRegister2.RegisterOnClick(Event_SwitchWeapon);
		gyUIEventRegister2 = GetEventRegister(m_UIManager.mSkill.gameObject);
		gyUIEventRegister2.RegisterOnClick(Event_UseSkill);
		gyUIEventRegister2 = GetEventRegister(m_UIManager.mScreenTouch);
		gyUIEventRegister2.RegisterOnDrag(Event_SlipScreen);
		gyUIEventRegister2.RegisterOnClick(Event_Continue);
		gyUIEventRegister2 = GetEventRegister(m_UIManager.mWheelMove.gameObject);
		gyUIEventRegister2.RegisterOnPress(Event_Move);
		gyUIEventRegister2.RegisterOnDrag(Event_Move);
		gyUIEventRegister2 = GetEventRegister(m_UIManager.mWheelShoot.gameObject);
		gyUIEventRegister2.RegisterOnPress(Event_Shoot);
		gyUIEventRegister2.RegisterOnDrag(Event_SlipScreen);
		gyUIEventRegister2.RegisterOnHold(Event_SlipScreen);
		gyUIEventRegister2.SetHoldTime(0.1f);
		if (m_UIManager.mPanelRevive != null)
		{
			gyUIEventRegister2 = GetEventRegister(m_UIManager.mPanelRevive.mReviveButton);
			gyUIEventRegister2.RegisterOnClick(Event_Revive);
		}
		if (m_UIManager.mPanelMaterial != null)
		{
			gyUIEventRegister2 = GetEventRegister(m_UIManager.mPanelMaterial.mTakeAllButton);
			gyUIEventRegister2.RegisterOnClick(Event_ClickTakeAllMaterial);
			gyUIEventRegister2 = GetEventRegister(m_UIManager.mPanelMaterial.mBackButton);
			gyUIEventRegister2.RegisterOnClick(Event_ClickBackToHome);
			m_UIManager.mPanelMaterial.RegisterOnClickCell(Event_ClickMaterial);
		}
		if (m_UIManager.mToolPanel != null)
		{
			gyUIEventRegister2 = GetEventRegister(m_UIManager.mToolPanel.gameObject);
			gyUIEventRegister2.RegisterOnClick(Event_ClickToolPanel);
			m_UIManager.mToolPanel.RegisterOnClickCell(Event_ClickTool);
		}
	}

	public void RegisterEvent_Windows()
	{
		gyUIEventRegister gyUIEventRegister2 = null;
		gyUIEventRegister2 = GetEventRegister(m_UIManager.mPause);
		gyUIEventRegister2.RegisterOnClick(Event_Pause);
		gyUIEventRegister2 = GetEventRegister(m_UIManager.mScreenTouch);
		gyUIEventRegister2.RegisterOnPress(Event_Shoot);
		gyUIEventRegister2.RegisterOnClick(Event_Continue);
		gyUIEventRegister2 = GetEventRegister(m_UIManager.mSkill.gameObject);
		gyUIEventRegister2.RegisterOnClick(Event_UseSkill);
		if (m_UIManager.mPanelRevive != null)
		{
			gyUIEventRegister2 = GetEventRegister(m_UIManager.mPanelRevive.mReviveButton);
			gyUIEventRegister2.RegisterOnClick(Event_Revive);
		}
		if (m_UIManager.mPanelMaterial != null)
		{
			gyUIEventRegister2 = GetEventRegister(m_UIManager.mPanelMaterial.mTakeAllButton);
			gyUIEventRegister2.RegisterOnClick(Event_ClickTakeAllMaterial);
			gyUIEventRegister2 = GetEventRegister(m_UIManager.mPanelMaterial.mBackButton);
			gyUIEventRegister2.RegisterOnClick(Event_ClickBackToHome);
			m_UIManager.mPanelMaterial.RegisterOnClickCell(Event_ClickMaterial);
		}
		if (m_UIManager.mToolPanel != null)
		{
			gyUIEventRegister2 = GetEventRegister(m_UIManager.mToolPanel.gameObject);
			gyUIEventRegister2.RegisterOnClick(Event_ClickToolPanel);
			m_UIManager.mToolPanel.RegisterOnClickCell(Event_ClickTool);
		}
	}

	protected void Event_Pause()
	{
		Debug.Log("click pause");
		iGameApp.GetInstance().EnterScene(kGameSceneEnum.Map);
	}

	protected void Event_Move(bool bPressed)
	{
		CCharUser user = m_GameScene.GetUser();
		if (user == null || !user.IsCanMove())
		{
			return;
		}
		if (bPressed)
		{
			Vector2 wheelOffSet = m_UIManager.mWheelMove.WheelOffSet;
			if (!(wheelOffSet == Vector2.zero))
			{
				user.MoveByCompass(wheelOffSet.x, wheelOffSet.y);
			}
		}
		else
		{
			user.MoveStop();
		}
	}

	protected void Event_Move(Vector2 delta)
	{
		CCharUser user = m_GameScene.GetUser();
		if (!(user == null) && user.IsCanMove())
		{
			Vector2 wheelOffSet = m_UIManager.mWheelMove.WheelOffSet;
			if (wheelOffSet == Vector2.zero)
			{
				user.MoveStop();
			}
			else
			{
				user.MoveByCompass(wheelOffSet.x, wheelOffSet.y);
			}
		}
	}

	protected void Event_Shoot(bool bPressed)
	{
		if ((m_GameScene.GameStatus != iGameSceneBase.kGameStatus.Gameing && m_GameScene.GameStatus != iGameSceneBase.kGameStatus.GameOver_ShowTime) || (MyUtils.SimulatePlatform == PlatformEnum.Windows && ((bPressed && !Input.GetMouseButtonDown(0)) || (!bPressed && !Input.GetMouseButtonUp(0)))))
		{
			return;
		}
		CCharUser user = m_GameScene.GetUser();
		if (!(user == null))
		{
			user.SetFire(bPressed);
			if (bPressed)
			{
				m_GameScene.AssistAim_Start();
			}
			else
			{
				m_GameScene.AssistAim_Stop();
			}
		}
	}

	protected void Event_SwitchWeapon()
	{
		CCharUser user = m_GameScene.GetUser();
		if (user == null)
		{
			return;
		}
		int num = m_nCurWeaponIndex + 1;
		while (num != m_nCurWeaponIndex && m_GameState.GetWeapon(num) == null)
		{
			num++;
			if (num >= 3)
			{
				num = 0;
			}
		}
		m_nCurWeaponIndex = num;
		user.SwitchWeapon(m_nCurWeaponIndex);
	}

	protected void Event_UseSkill()
	{
		CCharUser user = m_GameScene.GetUser();
		if (!(user == null) && user.IsCanAttack() && !user.IsSkillCD())
		{
			user.UseSkill(user.SkillID);
			if (m_UIManager != null && m_UIManager.mSkill != null)
			{
				m_UIManager.mSkill.SetCD(user.CurSkillCD);
			}
		}
	}

	protected void Event_SlipScreen(Vector2 delta)
	{
		CCharUser user = m_GameScene.GetUser();
		iCameraTrail iCameraTrail2 = m_GameScene.GetCamera();
		if (!(delta != Vector2.zero))
		{
			return;
		}
		if (delta.x != 0f)
		{
			iCameraTrail2.Yaw(delta.x / (iMacroDefine.SlipRateWidth * (float)Screen.width));
			if (user.IsCanAim())
			{
				user.SetYaw(iCameraTrail2.GetYaw());
			}
		}
		if (delta.y != 0f)
		{
			iCameraTrail2.Pitch(delta.y / (iMacroDefine.SlipRateHeight * (float)Screen.height));
		}
		if (Mathf.Abs(delta.x) > 1f && Mathf.Abs(delta.y) > 1f && m_GameScene.IsAssistAim())
		{
			m_GameScene.AssistAim_Stop();
		}
	}

	protected void Event_SlipScreen()
	{
		CCharUser user = m_GameScene.GetUser();
		iCameraTrail iCameraTrail2 = m_GameScene.GetCamera();
		Vector2 wheelOffSet = m_UIManager.mWheelShoot.WheelOffSet;
		if (Mathf.Abs(wheelOffSet.x) < 0.5f && Mathf.Abs(wheelOffSet.y) < 0.5f)
		{
			if (!m_GameScene.IsAssistAim())
			{
				Debug.Log("start ");
				m_GameScene.AssistAim_Start();
			}
			return;
		}
		iCameraTrail2.Yaw(wheelOffSet.x * 0.8f / (iMacroDefine.SlipRateWidth * (float)Screen.width));
		if (user.IsCanAim())
		{
			user.SetYaw(iCameraTrail2.GetYaw());
		}
		iCameraTrail2.Pitch(wheelOffSet.y * 0.8f / (iMacroDefine.SlipRateHeight * (float)Screen.height));
	}

	protected void Event_Revive()
	{
		CCharUser user = m_GameScene.GetUser();
		if (!(user == null) && user.isDead && m_GameScene.GameStatus == iGameSceneBase.kGameStatus.GameOver_Revive)
		{
			m_GameScene.ReviveGame();
		}
	}

	protected void Event_Continue()
	{
		if (m_GameScene.GameStatus != iGameSceneBase.kGameStatus.GameOver || m_GameScene.GameStatus != iGameSceneBase.kGameStatus.GameOver)
		{
			return;
		}
		if (m_GameScene.isMissionSuccess)
		{
			if (m_UIManager == null || m_UIManager.mPanelMissionComplete == null)
			{
				return;
			}
			if (m_GameScene.GameStatusStep == 1)
			{
				if (m_UIManager.mPanelMissionComplete.IsContextHop())
				{
					m_UIManager.mPanelMissionComplete.StopContextHop();
					m_GameScene.GameStatusStep = 2;
				}
				return;
			}
			if (m_GameScene.GameStatusStep == 2)
			{
				CCharUser user = m_GameScene.GetUser();
				if (user == null || m_GameScene.CurGameLevelInfo == null)
				{
					return;
				}
				if (m_GameState.nLastLevel < user.Level)
				{
					m_GameState.nNowLevel = user.Level;
					CCharacterInfoLevel characterInfo = m_GameData.GetCharacterInfo(user.ID, m_GameState.nLastLevel);
					CCharacterInfoLevel characterInfo2 = m_GameData.GetCharacterInfo(user.ID, user.Level);
					if (characterInfo != null && characterInfo2 != null)
					{
						m_GameState.nLastHP = (int)characterInfo.fLifeBase;
						m_GameState.nNowHP = (int)characterInfo2.fLifeBase;
					}
					m_GameScene.GameStatusStep = 3;
					m_GameScene.GameStatusStepTime = 2f;
					ShowSuccess(false);
					ShowLevelUp(true);
				}
				else if (m_GameState.HasAnyMaterial())
				{
					ShowSuccess(false);
					m_GameScene.GameOver_TakeMaterial();
				}
				else
				{
					iGameApp.GetInstance().EnterScene(kGameSceneEnum.Map);
				}
			}
			if (m_GameScene.GameStatusStep == 3)
			{
			}
			if (m_GameScene.GameStatusStep == 4)
			{
				if (m_GameState.HasAnyMaterial())
				{
					ShowLevelUp(false);
					m_GameScene.GameOver_TakeMaterial();
				}
				else
				{
					iGameApp.GetInstance().EnterScene(kGameSceneEnum.Map);
				}
			}
		}
		else
		{
			iGameApp.GetInstance().EnterScene(kGameSceneEnum.Map);
		}
	}

	protected void Event_ClickToolPanel()
	{
		if (!(m_UIManager == null) && !(m_UIManager.mToolPanel == null) && !m_UIManager.mToolPanel.IsMoveIn)
		{
			m_UIManager.mToolPanel.MoveIn();
		}
	}

	protected void Event_ClickTool(int nIndex)
	{
		Debug.Log("click cell " + nIndex);
	}

	protected void Event_ClickMaterial(int nIndex)
	{
		if (m_UIManager == null || m_UIManager.mPanelMaterial == null)
		{
			return;
		}
		iDataCenter dataCenter = m_GameData.GetDataCenter();
		if (dataCenter == null)
		{
			return;
		}
		CMaterialInfo gainMaterial = m_GameState.GetGainMaterial(nIndex);
		if (gainMaterial == null || gainMaterial.nItemCount <= 0)
		{
			return;
		}
		int num = dataCenter.CheckStashVolume(1);
		if (num < 1)
		{
			Debug.Log("stash full, please purchase more stash pos!");
			return;
		}
		dataCenter.AddMaterialNum(gainMaterial.nItemID, num);
		gainMaterial.nItemCount -= num;
		m_UIManager.mPanelMaterial.SetIconAnimate(nIndex, num);
		if (gainMaterial.nItemCount <= 0)
		{
			m_UIManager.mPanelMaterial.ShowItem(nIndex, false);
		}
		else
		{
			m_UIManager.mPanelMaterial.SetCount(nIndex, gainMaterial.nItemCount);
		}
		bool flag = true;
		int gainMaterialCount = m_GameState.GetGainMaterialCount();
		for (int i = 0; i < gainMaterialCount; i++)
		{
			CMaterialInfo gainMaterial2 = m_GameState.GetGainMaterial(i);
			if (gainMaterial2 != null && gainMaterial2.nItemCount > 0)
			{
				flag = false;
				break;
			}
		}
		if (flag)
		{
			dataCenter.Save();
			FadeOut(2f);
			m_GameScene.LeaveGame(2f);
		}
	}

	protected void Event_ClickTakeAllMaterial()
	{
		if (m_UIManager == null || m_UIManager.mPanelMaterial == null)
		{
			return;
		}
		iDataCenter dataCenter = m_GameData.GetDataCenter();
		if (dataCenter == null)
		{
			return;
		}
		int gainMaterialCount = m_GameState.GetGainMaterialCount();
		if (gainMaterialCount < 1)
		{
			return;
		}
		int num = 0;
		for (int i = 0; i < gainMaterialCount; i++)
		{
			CMaterialInfo gainMaterial = m_GameState.GetGainMaterial(i);
			if (gainMaterial != null && gainMaterial.nItemCount > 0)
			{
				num = dataCenter.CheckStashVolume(gainMaterial.nItemCount);
				if (num < 1)
				{
					Debug.Log("stash full, please purchase more stash pos!");
					break;
				}
				dataCenter.AddMaterialNum(gainMaterial.nItemID, num);
				gainMaterial.nItemCount -= num;
				m_UIManager.mPanelMaterial.SetIconAnimate(i, num);
				if (gainMaterial.nItemCount <= 0)
				{
					m_UIManager.mPanelMaterial.ShowItem(i, false);
				}
				else
				{
					m_UIManager.mPanelMaterial.SetCount(i, gainMaterial.nItemCount);
				}
			}
		}
		dataCenter.Save();
		FadeOut(2f);
		m_GameScene.LeaveGame(2f);
	}

	protected void Event_ClickBackToHome()
	{
		FadeOut(0.5f);
		m_GameScene.LeaveGame(0.5f);
	}

	protected gyUIEventRegister GetEventRegister(GameObject o)
	{
		gyUIEventRegister gyUIEventRegister2 = o.GetComponent<gyUIEventRegister>();
		if (gyUIEventRegister2 == null)
		{
			gyUIEventRegister2 = o.AddComponent<gyUIEventRegister>();
		}
		return gyUIEventRegister2;
	}
}
