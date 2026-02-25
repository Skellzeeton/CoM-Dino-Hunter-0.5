using EventCenter;
using UnityEngine;

public class Scene_Tavern : MonoBehaviour
{
	public TUIFade m_fade;

	private float m_fade_in_time;

	private float m_fade_out_time;

	private bool do_fade_in;

	private bool is_fade_out;

	private bool do_fade_out;

	private string next_scene = string.Empty;

	public PopupRole popup_role;

	private void Awake()
	{
		TUIDataServer.Instance().Initialize();
		global::EventCenter.EventCenter.Instance.Register<TUIEvent.BackEvent_SceneTavern>(TUIEvent_SetUIInfo);
	}

	private void Start()
	{
		global::EventCenter.EventCenter.Instance.Publish(this, new TUIEvent.SendEvent_SceneTavern("TUIEvent_TopBar"));
		global::EventCenter.EventCenter.Instance.Publish(this, new TUIEvent.SendEvent_SceneTavern("TUIEvent_AllRoleInfo"));
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
		global::EventCenter.EventCenter.Instance.Unregister<TUIEvent.BackEvent_SceneTavern>(TUIEvent_SetUIInfo);
	}

	public void TUIEvent_SetUIInfo(object sender, TUIEvent.BackEvent_SceneTavern m_event)
	{
		if (m_event.GetEventName() == "TUIEvent_TopBar")
		{
			if (m_event.GetEventInfo().GetPlayerInfo() != null)
			{
				popup_role.SetTopBarInfo(m_event.GetEventInfo().player_info);
			}
			else
			{
				Debug.Log("error!");
			}
		}
		else if (m_event.GetEventName() == "TUIEvent_AllRoleInfo")
		{
			if (m_event.GetEventInfo() != null)
			{
				popup_role.AddScrollListItem(m_event.GetEventInfo().all_role_info);
			}
			else
			{
				Debug.Log("!!!");
			}
		}
		else if (m_event.GetEventName() == "TUIEvent_RoleUnlock")
		{
			if (m_event.GetControlSuccess())
			{
				popup_role.SetRoleUnlock();
			}
		}
		else if (m_event.GetEventName() == "TUIEvent_RoleBuy")
		{
			if (m_event.GetControlSuccess())
			{
				popup_role.SetRoleBuy();
			}
		}
		else if (m_event.GetEventName() == "TUIEvent_RoleChange")
		{
			if (m_event.GetControlSuccess())
			{
				if (m_event.GetEventInfo() != null && m_event.GetEventInfo().GetPlayerInfo() != null)
				{
					popup_role.SetTopBarInfo(m_event.GetEventInfo().player_info);
				}
				else
				{
					Debug.Log("error!");
				}
			}
		}
		else if (m_event.GetEventName() == "TUIEvent_Back" && !is_fade_out)
		{
			next_scene = "Scene_MainMenu";
			is_fade_out = true;
			m_fade.FadeOut();
		}
	}

	public void TUIEvent_MoveScreen(TUIControl control, int event_type, float wparam, float lparam, object data)
	{
		if (event_type == 2)
		{
			popup_role.SetRoleRotation(wparam, lparam);
		}
	}

	public void TUIEvent_BtnBuy(TUIControl control, int event_type, float wparam, float lparam, object data)
	{
		if (event_type == 3)
		{
			CUISound.GetInstance().Play("UI_Button");
			switch (popup_role.GetRoleBuyState())
			{
			case PopupRoleBuy.PopupRoleBuyState.State_Unlock:
			{
				int roleChooseID3 = popup_role.GetRoleChooseID();
				global::EventCenter.EventCenter.Instance.Publish(this, new TUIEvent.SendEvent_SceneTavern("TUIEvent_RoleUnlock", roleChooseID3));
				break;
			}
			case PopupRoleBuy.PopupRoleBuyState.State_Buy:
			{
				int roleChooseID2 = popup_role.GetRoleChooseID();
				global::EventCenter.EventCenter.Instance.Publish(this, new TUIEvent.SendEvent_SceneTavern("TUIEvent_RoleBuy", roleChooseID2));
				break;
			}
			case PopupRoleBuy.PopupRoleBuyState.State_Use:
			{
				int roleChooseID = popup_role.GetRoleChooseID();
				global::EventCenter.EventCenter.Instance.Publish(this, new TUIEvent.SendEvent_SceneTavern("TUIEvent_RoleChange", roleChooseID));
				break;
			}
			default:
				Debug.Log("error!");
				break;
			}
		}
	}

	public void TUIEvent_Back(TUIControl control, int event_type, float wparam, float lparam, object data)
	{
		if (event_type == 3)
		{
			CUISound.GetInstance().Play("UI_Button");
			global::EventCenter.EventCenter.Instance.Publish(this, new TUIEvent.SendEvent_SceneTavern("TUIEvent_Back"));
		}
	}
}
