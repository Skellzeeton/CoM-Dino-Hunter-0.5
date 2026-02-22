using EventCenter;
using UnityEngine;

public class Scene_Stash : MonoBehaviour
{
	public TUIFade m_fade;

	private float m_fade_in_time;

	private float m_fade_out_time;

	private bool do_fade_in;

	private bool is_fade_out;

	private bool do_fade_out;

	private string next_scene = string.Empty;

	public Popup_Stash popup_stash;

	private float m_click_time_gap;

	private bool m_plus_down;

	private bool m_substract_down;

	private void Awake()
	{
		m_plus_down = false;
		m_substract_down = false;
		m_click_time_gap = 0f;
		TUIDataServer.Instance().Initialize();
		global::EventCenter.EventCenter.Instance.Register<TUIEvent.BackEvent_SceneStash>(TUIEvent_SetUIInfo);
	}

	private void Start()
	{
		global::EventCenter.EventCenter.Instance.Publish(this, new TUIEvent.SendEvent_SceneStash("TUIEvent_TopBar"));
		global::EventCenter.EventCenter.Instance.Publish(this, new TUIEvent.SendEvent_SceneStash("TUIEvent_StashInfo"));
	}

	private void Update()
	{
		m_fade_in_time += Time.deltaTime;
		if (m_fade_in_time >= m_fade.fadeInTime && !do_fade_in)
		{
			do_fade_in = true;
		}
		if (is_fade_out)
		{
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
		UpdateSellButton();
	}

	private void OnDestroy()
	{
		global::EventCenter.EventCenter.Instance.Unregister<TUIEvent.BackEvent_SceneStash>(TUIEvent_SetUIInfo);
	}

	public void TUIEvent_SetUIInfo(object sender, TUIEvent.BackEvent_SceneStash m_event)
	{
		if (m_event.GetEventName() == "TUIEvent_TopBar")
		{
			if (m_event.GetEventInfo().GetPlayerInfo() != null)
			{
				popup_stash.SetTopBarInfo(m_event.GetEventInfo().GetPlayerInfo());
			}
			else
			{
				Debug.Log("error!");
			}
		}
		else if (m_event.GetEventName() == "TUIEvent_StashInfo")
		{
			if (m_event.GetEventInfo() != null)
			{
				popup_stash.SetInfo(m_event.GetEventInfo().stash_info, base.gameObject);
			}
			else
			{
				Debug.Log("error!");
			}
		}
		else if (m_event.GetEventName() == "TUIEvent_AddCapacity")
		{
			if (m_event.GetControlSuccess())
			{
				popup_stash.AddCapacity();
			}
		}
		else if (m_event.GetEventName() == "TUIEvent_SellGoods")
		{
			if (m_event.GetControlSuccess())
			{
				popup_stash.UpdateSellGoods();
			}
		}
		else if (m_event.GetEventName() == "TUIEvent_Back")
		{
			if (!is_fade_out)
			{
				next_scene = "Scene_MainMenu";
				is_fade_out = true;
				m_fade.FadeOut();
			}
		}
		else if (m_event.GetEventName() == "TUIEvent_SearchGoodsDrop" && !is_fade_out)
		{
			next_scene = "Scene_Map";
			is_fade_out = true;
			m_fade.FadeOut();
		}
	}

	public void TUIEvent_OpenCapacityAdd(TUIControl control, int event_type, float wparam, float lparam, object data)
	{
		if (event_type == 3)
		{
			popup_stash.ShowCapacityAdd();
		}
	}

	public void TUIEvent_CapacityAdd(TUIControl control, int event_type, float wparam, float lparam, object data)
	{
		if (event_type == 3)
		{
			global::EventCenter.EventCenter.Instance.Publish(this, new TUIEvent.SendEvent_SceneStash("TUIEvent_AddCapacity"));
			popup_stash.CloseCapacityAdd();
		}
	}

	public void TUIEvent_CloseCapacityAdd(TUIControl control, int event_type, float wparam, float lparam, object data)
	{
		if (event_type == 3)
		{
			popup_stash.CloseCapacityAdd();
		}
	}

	public void TUIEvent_ChooseGoods(TUIControl control, int event_type, float wparam, float lparam, object data)
	{
		if (event_type == 1)
		{
			Btn_Select_Stash component = control.GetComponent<Btn_Select_Stash>();
			if (component == null)
			{
				Debug.Log("no goods control!");
			}
			else
			{
				popup_stash.SetGoodsControl(component);
			}
		}
	}

	public void TUIEvent_SearchGoodsDrop(TUIControl control, int event_type, float wparam, float lparam, object data)
	{
		if (event_type == 3)
		{
			Btn_Select_Stash goodsControl = popup_stash.GetGoodsControl();
			if (goodsControl == null || goodsControl.GetGoodsInfo() == null)
			{
				Debug.Log("error! no goods control!");
				return;
			}
			int id = goodsControl.GetGoodsInfo().id;
			int quality = (int)goodsControl.GetGoodsInfo().quality;
			global::EventCenter.EventCenter.Instance.Publish(this, new TUIEvent.SendEvent_SceneStash("TUIEvent_SearchGoodsDrop", id, quality));
		}
	}

	public void TUIEvent_OpenSell(TUIControl control, int event_type, float wparam, float lparam, object data)
	{
		if (event_type == 3)
		{
			popup_stash.ShowSell();
		}
	}

	public void TUIEvent_SellPlus(TUIControl control, int event_type, float wparam, float lparam, object data)
	{
		switch (event_type)
		{
		case 3:
			m_plus_down = false;
			m_click_time_gap = 0.2f;
			popup_stash.SetSellParamPlus(1);
			break;
		case 1:
			m_plus_down = true;
			break;
		}
	}

	public void TUIEvent_SellSubstract(TUIControl control, int event_type, float wparam, float lparam, object data)
	{
		switch (event_type)
		{
		case 3:
			m_substract_down = false;
			m_click_time_gap = 0.2f;
			popup_stash.SetSellParamSubstract(1);
			break;
		case 1:
			m_substract_down = true;
			break;
		}
	}

	public void TUIEvent_Sell(TUIControl control, int event_type, float wparam, float lparam, object data)
	{
		if (event_type == 3)
		{
			Btn_Select_Stash goodsControl = popup_stash.GetGoodsControl();
			if (goodsControl == null || goodsControl.GetGoodsInfo() == null)
			{
				Debug.Log("error! no goods control!");
				return;
			}
			int id = goodsControl.GetGoodsInfo().id;
			int quality = (int)goodsControl.GetGoodsInfo().quality;
			int sellCount = popup_stash.GetSellCount();
			Debug.Log("goods_id:" + id + " goods_quality:" + quality.ToString() + " sell_count:" + sellCount);
			global::EventCenter.EventCenter.Instance.Publish(this, new TUIEvent.SendEvent_SceneStash("TUIEvent_SellGoods", id, quality, sellCount));
			popup_stash.HideSell();
		}
	}

	public void TUIEvent_CloseSell(TUIControl control, int event_type, float wparam, float lparam, object data)
	{
		if (event_type == 3)
		{
			popup_stash.HideSell();
		}
	}

	public void TUIEvent_PageFrame(TUIControl control, int event_type, float wparam, float lparam, object data)
	{
		if (event_type == 1)
		{
			popup_stash.SetGoodsControl(null);
		}
	}

	public void TUIEvent_Back(TUIControl control, int event_type, float wparam, float lparam, object data)
	{
		if (event_type == 3)
		{
			global::EventCenter.EventCenter.Instance.Publish(this, new TUIEvent.SendEvent_SceneStash("TUIEvent_Back"));
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

	public void UpdateSellButton()
	{
		if (m_plus_down)
		{
			m_click_time_gap -= Time.deltaTime;
			if (m_click_time_gap <= 0f)
			{
				m_click_time_gap = 0.2f;
				popup_stash.SetSellParamPlus(1);
			}
		}
		if (m_substract_down)
		{
			m_click_time_gap -= Time.deltaTime;
			if (m_click_time_gap <= 0f)
			{
				m_click_time_gap = 0.2f;
				popup_stash.SetSellParamSubstract(1);
			}
		}
	}
}
