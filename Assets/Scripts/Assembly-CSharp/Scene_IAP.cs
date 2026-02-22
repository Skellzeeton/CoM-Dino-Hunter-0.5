using EventCenter;
using UnityEngine;

public class Scene_IAP : MonoBehaviour
{
	public TUIFade m_fade;

	private float m_fade_in_time;

	private float m_fade_out_time;

	private bool do_fade_in;

	private bool is_fade_out;

	private bool do_fade_out;

	private string next_scene = string.Empty;

	public Top_Bar top_bar;

	public PopupIAP popup_iap;

	private void Awake()
	{
		TUIDataServer.Instance().Initialize();
		global::EventCenter.EventCenter.Instance.Register<TUIEvent.BackEvent_SceneIAP>(TUIEvent_SetUIInfo);
	}

	private void Start()
	{
		global::EventCenter.EventCenter.Instance.Publish(this, new TUIEvent.SendEvent_SceneIAP("TUIEvent_TopBar"));
	}

	private void Update()
	{
		if (m_fade == null)
		{
			Debug.Log("error!no found m_fade!");
		}
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
		global::EventCenter.EventCenter.Instance.Unregister<TUIEvent.BackEvent_SceneIAP>(TUIEvent_SetUIInfo);
	}

	public void TUIEvent_SetUIInfo(object sender, TUIEvent.BackEvent_SceneIAP m_event)
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
		else if (m_event.GetEventName() == "TUIEvent_IAPResult")
		{
			if (m_event.GetControlSuccess())
			{
				popup_iap.Hide();
				if (m_event.GetEventInfo() != null && m_event.GetEventInfo().GetPlayerInfo() != null)
				{
					int gold2 = m_event.GetEventInfo().player_info.gold;
					int crystal2 = m_event.GetEventInfo().player_info.crystal;
					top_bar.SetGoldValue(gold2);
					top_bar.SetCrystalValue(crystal2);
				}
				else
				{
					Debug.Log("error! no info!");
				}
			}
			else
			{
				popup_iap.ShowPopupYes();
			}
		}
		else if (m_event.GetEventName() == "TUIEvent_Back" && !is_fade_out)
		{
			next_scene = "Scene_MainMenu";
			is_fade_out = true;
			m_fade.GetComponent<TUIFade>().FadeOut();
		}
	}

	public void TUIEvent_IAPBuy(TUIControl control, int event_type, float wparam, float lparam, object data)
	{
		if (event_type == 3)
		{
			IAPItem component = control.transform.parent.GetComponent<IAPItem>();
			if (component != null)
			{
				int iD = component.GetID();
				popup_iap.ShowPopupWaitting();
				global::EventCenter.EventCenter.Instance.Publish(this, new TUIEvent.SendEvent_SceneIAP("TUIEvent_IAPBuy", iD));
			}
		}
	}

	public void TUIEvent_HidePopup(TUIControl control, int event_type, float wparam, float lparam, object data)
	{
		if (event_type == 3)
		{
			popup_iap.Hide();
		}
	}

	public void TUIEvent_Back(TUIControl control, int event_type, float wparam, float lparam, object data)
	{
		if (event_type == 3)
		{
			global::EventCenter.EventCenter.Instance.Publish(this, new TUIEvent.SendEvent_SceneIAP("TUIEvent_Back"));
		}
	}
}
