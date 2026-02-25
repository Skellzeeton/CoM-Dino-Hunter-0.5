using EventCenter;
using UnityEngine;

public class Scene_Forge : MonoBehaviour
{
	public TUIFade m_fade;

	private float m_fade_in_time;

	private float m_fade_out_time;

	private bool do_fade_in;

	private bool is_fade_out;

	private bool do_fade_out;

	private string next_scene = string.Empty;

	public PopupWeapon popup_weapon;

	private void Awake()
	{
		TUIDataServer.Instance().Initialize();
		global::EventCenter.EventCenter.Instance.Register<TUIEvent.BackEvent_SceneForge>(TUIEvent_SetUIInfo);
	}

	private void Start()
	{
		global::EventCenter.EventCenter.Instance.Publish(this, new TUIEvent.SendEvent_SceneForge("TUIEvent_TopBar"));
		global::EventCenter.EventCenter.Instance.Publish(this, new TUIEvent.SendEvent_SceneForge("TUIEvent_WeaponInfo"));
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
		global::EventCenter.EventCenter.Instance.Unregister<TUIEvent.BackEvent_SceneForge>(TUIEvent_SetUIInfo);
	}

	public void TUIEvent_SetUIInfo(object sender, TUIEvent.BackEvent_SceneForge m_event)
	{
		if (m_event.GetEventName() == "TUIEvent_TopBar")
		{
			if (m_event.GetEventInfo().GetPlayerInfo() != null)
			{
				popup_weapon.SetTopBarInfo(m_event.GetEventInfo().GetPlayerInfo());
			}
			else
			{
				Debug.Log("error!");
			}
		}
		else if (m_event.GetEventName() == "TUIEvent_WeaponInfo")
		{
			if (m_event.GetEventInfo() != null)
			{
				popup_weapon.SetWeaponInfo(m_event.GetEventInfo().weapon_info);
				popup_weapon.SetWeaponKindItem(WeaponType.CloseWeapon);
			}
			else
			{
				Debug.Log("error!");
			}
		}
		else if (m_event.GetEventName() == "TUIEvent_WeaponGoodsBuy")
		{
			if (m_event.GetControlSuccess())
			{
				popup_weapon.UpdateGoodsBuy();
			}
		}
		else if (m_event.GetEventName() == "TUIEvent_WeaponUpdate")
		{
			if (m_event.GetControlSuccess())
			{
				popup_weapon.UpdateWeapon();
				popup_weapon.CloseWeaponUpdate();
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

	public void TUIEvent_OpenWeaponItem01(TUIControl control, int event_type, float wparam, float lparam, object data)
	{
		if (event_type == 1)
		{
			if (popup_weapon == null)
			{
				Debug.Log("error!");
			}
			else
			{
				popup_weapon.SetWeaponKindItem(WeaponType.CloseWeapon);
			}
		}
	}

	public void TUIEvent_OpenWeaponItem02(TUIControl control, int event_type, float wparam, float lparam, object data)
	{
		if (event_type == 1)
		{
			if (popup_weapon == null)
			{
				Debug.Log("error!");
			}
			else
			{
				popup_weapon.SetWeaponKindItem(WeaponType.Crossbow);
			}
		}
	}

	public void TUIEvent_OpenWeaponItem03(TUIControl control, int event_type, float wparam, float lparam, object data)
	{
		if (event_type == 1)
		{
			if (popup_weapon == null)
			{
				Debug.Log("error!");
			}
			else
			{
				popup_weapon.SetWeaponKindItem(WeaponType.MachineGun);
			}
		}
	}

	public void TUIEvent_OpenWeaponItem04(TUIControl control, int event_type, float wparam, float lparam, object data)
	{
		if (event_type == 1)
		{
			if (popup_weapon == null)
			{
				Debug.Log("error!");
			}
			else
			{
				popup_weapon.SetWeaponKindItem(WeaponType.ViolenceGun);
			}
		}
	}

	public void TUIEvent_OpenWeaponItem05(TUIControl control, int event_type, float wparam, float lparam, object data)
	{
		if (event_type == 1)
		{
			if (popup_weapon == null)
			{
				Debug.Log("error!");
			}
			else
			{
				popup_weapon.SetWeaponKindItem(WeaponType.LiquidFireGun);
			}
		}
	}

	public void TUIEvent_OpenWeaponItem06(TUIControl control, int event_type, float wparam, float lparam, object data)
	{
		if (event_type == 1)
		{
			if (popup_weapon == null)
			{
				Debug.Log("error!");
			}
			else
			{
				popup_weapon.SetWeaponKindItem(WeaponType.RPG);
			}
		}
	}

	public void TUIEvent_OpenWeaponItem07(TUIControl control, int event_type, float wparam, float lparam, object data)
	{
		if (event_type == 1)
		{
			if (popup_weapon == null)
			{
				Debug.Log("error!");
			}
			else
			{
				popup_weapon.SetWeaponKindItem(WeaponType.Stoneskin);
			}
		}
	}

	public void TUIEvent_MoveScreen(TUIControl control, int event_type, float wparam, float lparam, object data)
	{
		if (event_type == 2)
		{
			popup_weapon.SetRoleRotation(wparam, lparam);
		}
	}

	public void TUIEvent_OpenWeaponUpdate(TUIControl control, int event_type, float wparam, float lparam, object data)
	{
		if (event_type == 3)
		{
			PopupWeaponBuy.PopupWeaponBuyState state = control.GetComponent<PopupWeaponBuy>().GetState();
			if (state == PopupWeaponBuy.PopupWeaponBuyState.State_Update || state == PopupWeaponBuy.PopupWeaponBuyState.State_Craft)
			{
				popup_weapon.OpenWeaponUpdate();
			}
		}
	}

	public void TUIEvent_CloseWeaponUpdate(TUIControl control, int event_type, float wparam, float lparam, object data)
	{
		if (event_type == 3)
		{
			popup_weapon.CloseWeaponUpdate();
		}
	}

	public void TUIEvent_WeaponGoodsBuy(TUIControl control, int event_type, float wparam, float lparam, object data)
	{
		if (event_type == 3)
		{
			GoodsNeedItemBuy component = control.GetComponent<GoodsNeedItemBuy>();
			int goodsID = component.GetGoodsID();
			int goodsQuality = (int)component.GetGoodsQuality();
			int goodsLackCount = component.GetGoodsLackCount();
			popup_weapon.SetGoodsNeedItemBuy(component);
			global::EventCenter.EventCenter.Instance.Publish(this, new TUIEvent.SendEvent_SceneForge("TUIEvent_WeaponGoodsBuy", goodsID, goodsQuality, goodsLackCount));
		}
	}

	public void TUIEvent_WeaponUpdate(TUIControl control, int event_type, float wparam, float lparam, object data)
	{
		if (event_type == 3)
		{
			int weaponID = popup_weapon.GetWeaponID();
			int weaponType = (int)popup_weapon.GetWeaponType();
			popup_weapon.CloseWeaponUpdate();
			global::EventCenter.EventCenter.Instance.Publish(this, new TUIEvent.SendEvent_SceneForge("TUIEvent_WeaponUpdate", weaponID, weaponType));
		}
	}

	public void TUIEvent_HideUnlockBlink(TUIControl control, int event_type, float wparam, float lparam, object data)
	{
		if (event_type == 3)
		{
			popup_weapon.CloseBlink();
		}
	}

	public void TUIEvent_Back(TUIControl control, int event_type, float wparam, float lparam, object data)
	{
		if (event_type == 3)
		{
			global::EventCenter.EventCenter.Instance.Publish(this, new TUIEvent.SendEvent_SceneForge("TUIEvent_Back"));
		}
	}

	public void TUIEvent_SearchGoodsDrop(TUIControl control, int event_type, float wparam, float lparam, object data)
	{
		if (event_type == 3)
		{
			GoodsNeedItem component = control.transform.parent.GetComponent<GoodsNeedItem>();
			if (component == null)
			{
				Debug.Log("error! no goods need item");
				return;
			}
			int goodsID = component.GetGoodsID();
			int goodsQuality = (int)component.GetGoodsQuality();
			global::EventCenter.EventCenter.Instance.Publish(this, new TUIEvent.SendEvent_SceneForge("TUIEvent_SearchGoodsDrop", goodsID, goodsQuality));
		}
	}
}
