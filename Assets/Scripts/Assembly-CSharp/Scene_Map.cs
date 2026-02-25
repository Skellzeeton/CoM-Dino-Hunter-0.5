using EventCenter;
using UnityEngine;

public class Scene_Map : MonoBehaviour
{
	public TUIFade m_fade;

	private float m_fade_in_time;

	private float m_fade_out_time;

	private bool do_fade_in;

	private bool is_fade_out;

	private bool do_fade_out;

	private string next_scene = string.Empty;

	public Top_Bar top_bar;

	public LevelMap level_map;

	public PopupLevel popup_level_map;

	public Camera tui_camera;

	private Transform level_point;

	private void Awake()
	{
		TUIDataServer.Instance().Initialize();
		global::EventCenter.EventCenter.Instance.Register<TUIEvent.BackEvent_SceneMap>(TUIEvent_SetUIInfo);
	}

	private void Start()
	{
		level_map.SetBorder(tui_camera);
		global::EventCenter.EventCenter.Instance.Publish(this, new TUIEvent.SendEvent_SceneMap("TUIEvent_TopBar"));
		global::EventCenter.EventCenter.Instance.Publish(this, new TUIEvent.SendEvent_SceneMap("TUIEvent_MapEnterInfo"));
	}

	private void Update()
	{
		m_fade_in_time += Time.deltaTime;
		if (m_fade_in_time >= m_fade.fadeInTime && !do_fade_in)
		{
			do_fade_in = true;
		}
		if (!is_fade_out)
		{
			return;
		}
		m_fade_out_time += Time.deltaTime;
		if (m_fade_out_time >= m_fade.fadeOutTime && !do_fade_out)
		{
			do_fade_out = true;
			TUIMappingInfo.SwitchScene switchScene = TUIMappingInfo.Instance().GetSwitchScene();
			if (switchScene != null)
			{
				switchScene(next_scene);
			}
		}
	}

	private void OnDestroy()
	{
		global::EventCenter.EventCenter.Instance.Unregister<TUIEvent.BackEvent_SceneMap>(TUIEvent_SetUIInfo);
	}

	public void TUIEvent_SetUIInfo(object sender, TUIEvent.BackEvent_SceneMap m_event)
	{
		if (m_event.GetEventName() == "TUIEvent_TopBar")
		{
			if (m_event.GetEventInfo() != null && m_event.GetEventInfo().GetPlayerInfo() != null)
			{
				int avatar_id = m_event.GetEventInfo().player_info.avatar_id;
				int level = m_event.GetEventInfo().player_info.level;
				int exp = m_event.GetEventInfo().player_info.exp;
				int level_exp = m_event.GetEventInfo().player_info.level_exp;
				int gold = m_event.GetEventInfo().player_info.gold;
				int crystal = m_event.GetEventInfo().player_info.crystal;
				top_bar.SetAllValue(level, exp, level_exp, gold, crystal);
			}
			else
			{
				Debug.Log("error!");
			}
		}
		else if (m_event.GetEventName() == "TUIEvent_MapEnterInfo")
		{
			if (m_event.GetEventInfo() != null)
			{
				level_map.SetMapEnterInfo(m_event.GetEventInfo().map_info);
			}
		}
		else if (m_event.GetEventName() == "TUIEvent_LevelInfo")
		{
			if (m_event.GetEventInfo() == null)
			{
				return;
			}
			TUIMapInfo map_info = m_event.GetEventInfo().map_info;
			if (map_info != null)
			{
				TUILevelInfo level_info = map_info.level_info;
				if (map_info != null)
				{
					level_map.SetLevelInfo(level_info);
					popup_level_map.Show(level_info);
				}
				else
				{
					Debug.Log("error! no map info!");
				}
			}
			else
			{
				Debug.Log("error! no map info!");
			}
		}
		else if (m_event.GetEventName() == "TUIEvent_EnterLevel")
		{
			string str = m_event.GetStr();
			if (str != string.Empty)
			{
				if (!is_fade_out)
				{
					next_scene = str;
					is_fade_out = true;
					m_fade.FadeOut();
				}
			}
			else
			{
				Debug.Log("error!");
			}
		}
		else if (m_event.GetEventName() == "TUIEvent_Back" && !is_fade_out)
		{
			next_scene = "Scene_MainMenu";
			is_fade_out = true;
			m_fade.FadeOut();
		}
	}

	public void TUIEvent_MoveScreen(TUIControl control, int event_type, float wparam, float lparam, object obj)
	{
		level_map.MoveScreen(wparam, 0f);
	}

	public void TUIEvent_ShowPopup(TUIControl control, int event_type, float wparam, float lparam, object obj)
	{
		if (event_type != 3)
		{
			return;
		}
		CUISound.GetInstance().Play("UI_Button");
		level_point = control.transform.parent.transform;
		LevelPoint component = level_point.GetComponent<LevelPoint>();
		LevelPointEx component2 = level_point.GetComponent<LevelPointEx>();
		if (component != null)
		{
			if (component.GetLevelInfo() == null)
			{
				int levelID = component.GetLevelID();
				global::EventCenter.EventCenter.Instance.Publish(this, new TUIEvent.SendEvent_SceneMap("TUIEvent_LevelInfo", levelID));
			}
			else
			{
				popup_level_map.Show(component.GetLevelInfo());
			}
		}
		else if (component2 != null)
		{
			if (component2.GetLevelInfo() == null)
			{
				int levelID2 = component2.GetLevelID();
				global::EventCenter.EventCenter.Instance.Publish(this, new TUIEvent.SendEvent_SceneMap("TUIEvent_LevelInfo", levelID2));
			}
			else
			{
				popup_level_map.Show(component2.GetLevelInfo());
			}
		}
		else
		{
			Debug.Log("error!");
		}
	}

	public void TUIEvent_EnterLevel(TUIControl control, int event_type, float wparam, float lparam, object obj)
	{
		if (event_type != 3)
		{
			return;
		}
		CUISound.GetInstance().Play("UI_Button");
		popup_level_map.Hide();
		if (level_point == null)
		{
			Debug.Log("error!");
			return;
		}
		LevelPoint component = level_point.GetComponent<LevelPoint>();
		LevelPointEx component2 = level_point.GetComponent<LevelPointEx>();
		if (component != null)
		{
			int levelID = component.GetLevelID();
			global::EventCenter.EventCenter.Instance.Publish(this, new TUIEvent.SendEvent_SceneMap("TUIEvent_EnterLevel", levelID));
		}
		else if (component2 != null)
		{
			int levelID2 = component2.GetLevelID();
			global::EventCenter.EventCenter.Instance.Publish(this, new TUIEvent.SendEvent_SceneMap("TUIEvent_EnterLevel", levelID2));
		}
		else
		{
			Debug.Log("error!");
		}
	}

	public void TUIEvent_ClickRecommend(TUIControl control, int event_type, float wparam, float lparam, object obj)
	{
		if (event_type == 3)
		{
			PopupLevel_Recommend component = control.transform.parent.GetComponent<PopupLevel_Recommend>();
			PopupLevel_Recommend.RecommendBtnState recommendBtnState = component.GetRecommendBtnState();
			Debug.Log("m_btn_state:" + recommendBtnState);
			switch (recommendBtnState)
			{
			case PopupLevel_Recommend.RecommendBtnState.RoleBuy:
				next_scene = "Scene_Tavern";
				break;
			case PopupLevel_Recommend.RecommendBtnState.WeaponBuy:
				next_scene = "Scene_Forge";
				break;
			case PopupLevel_Recommend.RecommendBtnState.RoleEquip:
			case PopupLevel_Recommend.RecommendBtnState.WeaponEquip:
				next_scene = "Scene_Equip";
				break;
			}
			if (!is_fade_out)
			{
				is_fade_out = true;
				m_fade.FadeOut();
			}
		}
	}

	public void TUIEvent_ClosePopup(TUIControl control, int event_type, float wparam, float lparam, object obj)
	{
		if (event_type == 3)
		{
			CUISound.GetInstance().Play("UI_Cancle");
			popup_level_map.Hide();
		}
	}

	public void TUIEvent_Back(TUIControl control, int event_type, float wparam, float lparam, object data)
	{
		if (event_type == 3)
		{
			CUISound.GetInstance().Play("UI_Button");
			global::EventCenter.EventCenter.Instance.Publish(this, new TUIEvent.SendEvent_SceneMap("TUIEvent_Back"));
		}
	}
}
