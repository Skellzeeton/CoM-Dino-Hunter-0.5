using EventCenter;
using UnityEngine;

public class Scene_Skill : MonoBehaviour
{
	public TUIFade m_fade;

	private float m_fade_in_time;

	private float m_fade_out_time;

	private bool do_fade_in;

	private bool is_fade_out;

	private bool do_fade_out;

	private string next_scene = string.Empty;

	public PopupSkill popup_skill;

	private void Awake()
	{
		TUIDataServer.Instance().Initialize();
		global::EventCenter.EventCenter.Instance.Register<TUIEvent.BackEvent_SceneSkill>(TUIEvent_SetUIInfo);
	}

	private void Start()
	{
		global::EventCenter.EventCenter.Instance.Publish(this, new TUIEvent.SendEvent_SceneSkill("TUIEvent_TopBar"));
		global::EventCenter.EventCenter.Instance.Publish(this, new TUIEvent.SendEvent_SceneSkill("TUIEvent_SkillInfo"));
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
		global::EventCenter.EventCenter.Instance.Unregister<TUIEvent.BackEvent_SceneSkill>(TUIEvent_SetUIInfo);
	}

	public void TUIEvent_SetUIInfo(object sender, TUIEvent.BackEvent_SceneSkill m_event)
	{
		if (m_event.GetEventName() == "TUIEvent_TopBar")
		{
			if (m_event.GetEventInfo().GetPlayerInfo() != null)
			{
				popup_skill.SetTopBarInfo(m_event.GetEventInfo().GetPlayerInfo());
			}
			else
			{
				Debug.Log("error!");
			}
		}
		else if (m_event.GetEventName() == "TUIEvent_SkillInfo")
		{
			if (m_event.GetEventInfo() != null)
			{
				popup_skill.AddScrollList(m_event.GetEventInfo().all_skill_info, base.gameObject);
			}
			else
			{
				Debug.Log("!!!");
			}
		}
		else if (m_event.GetEventName() == "TUIEvent_SkillUnlcok")
		{
			if (m_event.GetControlSuccess())
			{
				popup_skill.SkillUnlock();
			}
		}
		else if (m_event.GetEventName() == "TUIEvent_SkillBuy")
		{
			if (m_event.GetControlSuccess())
			{
				popup_skill.SkillBuy();
			}
		}
		else if (m_event.GetEventName() == "TUIEvent_SkillUpdate")
		{
			if (m_event.GetControlSuccess())
			{
				popup_skill.SkillUpdate();
			}
		}
		else if (m_event.GetEventName() == "TUIEvent_Back" && !is_fade_out)
		{
			next_scene = "Scene_MainMenu";
			is_fade_out = true;
			m_fade.FadeOut();
		}
	}

	public void TUIEvent_BtnRole(TUIControl control, int event_type, float wparam, float lparam, object data)
	{
		if (event_type == 1)
		{
			int index = control.GetComponent<PopupSkillBtnRole>().GetIndex();
			popup_skill.ScrollListChoose(index);
		}
	}

	public void TUIEvent_OpenSkillUpdate(TUIControl control, int event_type, float wparam, float lparam, object data)
	{
		if (event_type == 3)
		{
			if (popup_skill.GetStateBtnSkill() == Btn_BuySkill.StateButtonSkill.State_Unlock)
			{
				int scrollListIndex = popup_skill.GetScrollListIndex();
				int skillID = popup_skill.GetSkillID();
				global::EventCenter.EventCenter.Instance.Publish(this, new TUIEvent.SendEvent_SceneSkill("TUIEvent_SkillUnlcok", scrollListIndex, skillID));
			}
			else if (popup_skill.GetStateBtnSkill() == Btn_BuySkill.StateButtonSkill.State_Buy)
			{
				int scrollListIndex2 = popup_skill.GetScrollListIndex();
				int skillID2 = popup_skill.GetSkillID();
				global::EventCenter.EventCenter.Instance.Publish(this, new TUIEvent.SendEvent_SceneSkill("TUIEvent_SkillBuy", scrollListIndex2, skillID2));
			}
			else if (popup_skill.GetStateBtnSkill() == Btn_BuySkill.StateButtonSkill.State_Update)
			{
				popup_skill.ShowSkillUpdate();
			}
		}
	}

	public void TUIEvent_SkillUpdate(TUIControl control, int event_type, float wparam, float lparam, object data)
	{
		if (event_type == 3)
		{
			int scrollListIndex = popup_skill.GetScrollListIndex();
			int skillID = popup_skill.GetSkillID();
			global::EventCenter.EventCenter.Instance.Publish(this, new TUIEvent.SendEvent_SceneSkill("TUIEvent_SkillUpdate", scrollListIndex, skillID));
			popup_skill.CloseSkillUpdate();
		}
	}

	public void TUIEvent_CloseSkillUpdate(TUIControl control, int event_type, float wparam, float lparam, object data)
	{
		if (event_type == 3)
		{
			popup_skill.CloseSkillUpdate();
		}
	}

	public void TUIEvent_HideUnlockBlink(TUIControl control, int event_type, float wparam, float lparam, object data)
	{
		if (event_type == 3)
		{
			popup_skill.CloseBlink();
		}
	}

	public void TUIEvent_Back(TUIControl control, int event_type, float wparam, float lparam, object data)
	{
		if (event_type == 3)
		{
			global::EventCenter.EventCenter.Instance.Publish(this, new TUIEvent.SendEvent_SceneSkill("TUIEvent_Back"));
		}
	}

	public void TUIEvent_IAP(TUIControl control, int event_type, float wparam, float lparam, object data)
	{
		if (event_type == 3 && !is_fade_out)
		{
			next_scene = "Scene_IAP";
			is_fade_out = true;
			m_fade.FadeOut();
		}
	}
}
