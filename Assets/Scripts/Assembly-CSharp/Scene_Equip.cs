using EventCenter;
using UnityEngine;

public class Scene_Equip : MonoBehaviour
{
	public TUIFade m_fade;

	private float m_fade_in_time;

	private float m_fade_out_time;

	private bool do_fade_in;

	private bool is_fade_out;

	private bool do_fade_out;

	private string next_scene = string.Empty;

	public BtnItem_Item btn_skill01;

	public BtnItem_Item btn_skill02;

	public BtnItem_Item btn_skill03;

	public BtnItem_Item btn_skill04;

	public BtnItem_Item btn_prop01;

	public BtnItem_Item btn_prop02;

	public BtnItem_Item btn_weapon01;

	public BtnItem_Item btn_weapon02;

	public BtnItem_Item btn_weapon03;

	public BtnItem_Item btn_weapon04;

	public BtnItem_Item btn_role;

	public Popup_Show popup_prop;

	public Popup_Show popup_skill01;

	public Popup_Show popup_skill;

	public Popup_Show popup_weapon01;

	public Popup_Show popup_weapon02;

	public Popup_Show popup_weapon03;

	public Popup_Show popup_role;

	public Role_Control go_role;

	public TUILabel label_role_name;

	public Top_Bar top_bar;

	private Popup_Show popup_weapon_now;

	private void Awake()
	{
		TUIDataServer.Instance().Initialize();
		global::EventCenter.EventCenter.Instance.Register<TUIEvent.BackEvent_SceneEquip>(TUIEvent_SetUIInfo);
	}

	private void Start()
	{
		global::EventCenter.EventCenter.Instance.Publish(this, new TUIEvent.SendEvent_SceneEquip("TUIEvent_TopBar"));
		global::EventCenter.EventCenter.Instance.Publish(this, new TUIEvent.SendEvent_SceneEquip("TUIEvent_RoleSign"));
		global::EventCenter.EventCenter.Instance.Publish(this, new TUIEvent.SendEvent_SceneEquip("TUIEvent_PropSign"));
		global::EventCenter.EventCenter.Instance.Publish(this, new TUIEvent.SendEvent_SceneEquip("TUIEvent_SkillSign"));
		global::EventCenter.EventCenter.Instance.Publish(this, new TUIEvent.SendEvent_SceneEquip("TUIEvent_WeaponSign"));
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
		global::EventCenter.EventCenter.Instance.Unregister<TUIEvent.BackEvent_SceneEquip>(TUIEvent_SetUIInfo);
	}

	public void TUIEvent_SetUIInfo(object sender, TUIEvent.BackEvent_SceneEquip m_event)
	{
		if (m_event.GetEventName() == "TUIEvent_TopBar")
		{
			if (m_event.GetEventInfo() != null && m_event.GetEventInfo().player_info != null)
			{
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
		else if (m_event.GetEventName() == "TUIEvent_RoleSign")
		{
			if (m_event.GetEventInfo() != null && m_event.GetEventInfo().equip_info != null)
			{
				if (m_event.GetEventInfo().equip_info.role != null)
				{
					go_role.ChangeRole(m_event.GetEventInfo().equip_info.role.texture_id);
					label_role_name.Text = m_event.GetEventInfo().equip_info.role.name;
				}
				btn_role.SetInfo(m_event.GetEventInfo().equip_info.role);
				popup_role.SetInfo(m_event.GetEventInfo().equip_info.roles_list, base.gameObject, Popup_Show.PopupType.Roles);
				popup_role.SetBtnInfo(1, btn_role);
			}
			else
			{
				Debug.Log("error!");
			}
		}
		else if (m_event.GetEventName() == "TUIEvent_SkillSign")
		{
			if (m_event.GetEventInfo() != null && m_event.GetEventInfo().equip_info != null)
			{
				btn_skill01.SetInfo(m_event.GetEventInfo().equip_info.skill01, Popup_Show.PopupType.Skills);
				btn_skill02.SetInfo(m_event.GetEventInfo().equip_info.skill02, Popup_Show.PopupType.Skills);
				btn_skill03.SetInfo(m_event.GetEventInfo().equip_info.skill03, Popup_Show.PopupType.Skills);
				btn_skill04.SetInfo(m_event.GetEventInfo().equip_info.skill04, Popup_Show.PopupType.Skills);
				popup_skill.SetInfo(m_event.GetEventInfo().equip_info.skill_list, base.gameObject, Popup_Show.PopupType.Skills);
				popup_skill.SetBtnInfo(1, btn_skill01);
				popup_skill.SetBtnInfo(2, btn_skill02);
				popup_skill.SetBtnInfo(3, btn_skill03);
				popup_skill.SetBtnInfo(4, btn_skill04);
			}
			else
			{
				Debug.Log("error!");
			}
		}
		else if (m_event.GetEventName() == "TUIEvent_PropSign")
		{
			if (m_event.GetEventInfo() != null && m_event.GetEventInfo().equip_info != null)
			{
				btn_prop01.SetInfo(m_event.GetEventInfo().equip_info.prop01, Popup_Show.PopupType.Props);
				btn_prop02.SetInfo(m_event.GetEventInfo().equip_info.prop02, Popup_Show.PopupType.Props);
			}
			else
			{
				Debug.Log("error!");
			}
		}
		else if (m_event.GetEventName() == "TUIEvent_WeaponSign")
		{
			if (m_event.GetEventInfo() != null && m_event.GetEventInfo().equip_info != null)
			{
				if (m_event.GetEventInfo().equip_info.weapon01 != null)
				{
					go_role.ChangeWeapon(m_event.GetEventInfo().equip_info.weapon01.texture_id);
				}
				btn_weapon01.SetInfo(m_event.GetEventInfo().equip_info.weapon01, Popup_Show.PopupType.Weapons01);
				btn_weapon02.SetInfo(m_event.GetEventInfo().equip_info.weapon02, Popup_Show.PopupType.Weapons02);
				btn_weapon03.SetInfo(m_event.GetEventInfo().equip_info.weapon03, Popup_Show.PopupType.Weapons02);
				btn_weapon04.SetInfo(m_event.GetEventInfo().equip_info.weapon04, Popup_Show.PopupType.Weapons03);
				popup_weapon01.SetInfo(m_event.GetEventInfo().equip_info.weapon_list01, base.gameObject, Popup_Show.PopupType.Weapons01);
				popup_weapon02.SetInfo(m_event.GetEventInfo().equip_info.weapon_list02, base.gameObject, Popup_Show.PopupType.Weapons02);
				popup_weapon03.SetInfo(m_event.GetEventInfo().equip_info.weapon_list03, base.gameObject, Popup_Show.PopupType.Weapons03);
				popup_weapon01.SetBtnInfo(1, btn_weapon01);
				popup_weapon02.SetBtnInfo(2, btn_weapon02);
				popup_weapon02.SetBtnInfo(3, btn_weapon03);
				popup_weapon03.SetBtnInfo(4, btn_weapon04);
			}
			else
			{
				Debug.Log("error!");
			}
		}
		else if (m_event.GetEventName() == "TUIEvent_RoleEquip")
		{
			if (m_event.GetControlSuccess())
			{
				if (popup_role.GetItemSelectInfo() != null)
				{
					int texture_id = popup_role.GetItemSelectInfo().texture_id;
					string text = popup_role.GetItemSelectInfo().name;
					go_role.ChangeRole(texture_id);
					label_role_name.Text = text;
					if (m_event.GetEventInfo() != null && m_event.GetEventInfo().player_info != null)
					{
						int level2 = m_event.GetEventInfo().player_info.level;
						int exp2 = m_event.GetEventInfo().player_info.exp;
						int level_exp2 = m_event.GetEventInfo().player_info.level_exp;
						int gold2 = m_event.GetEventInfo().player_info.gold;
						int crystal2 = m_event.GetEventInfo().player_info.crystal;
						top_bar.SetAllValue(level2, exp2, level_exp2, gold2, crystal2);
						if (m_event.GetEventInfo().equip_info != null)
						{
							btn_skill01.SetInfo(m_event.GetEventInfo().equip_info.skill01, Popup_Show.PopupType.Skills);
							popup_skill.SetBtnInfo(1, btn_skill01);
						}
						else
						{
							Debug.Log("no skill01!");
						}
					}
					else
					{
						Debug.Log("error! no player info");
					}
				}
				else
				{
					Debug.Log("error!");
				}
				popup_role.EquipItem();
			}
			popup_role.SetItemSelectInfo(null);
			popup_role.SetItemNowInfo(null);
		}
		else if (m_event.GetEventName() == "TUIEvent_SkillEquip")
		{
			if (m_event.GetControlSuccess())
			{
				popup_skill.EquipItem();
			}
			popup_skill.SetItemSelectInfo(null);
			popup_skill.SetItemNowInfo(null);
		}
		else if (m_event.GetEventName() == "TUIEvent_SkillUnEquip")
		{
			if (m_event.GetControlSuccess())
			{
				popup_skill.UnEquipItem();
			}
			popup_skill.SetItemSelectInfo(null);
			popup_skill.SetItemNowInfo(null);
		}
		else if (m_event.GetEventName() == "TUIEvent_SkillExchange")
		{
			if (m_event.GetControlSuccess())
			{
				popup_skill.ExchangeSkill();
			}
			popup_skill.SetItemSelectInfo(null);
			popup_skill.SetItemNowInfo(null);
		}
		else if (m_event.GetEventName() == "TUIEvent_WeaponEquip")
		{
			if (m_event.GetControlSuccess())
			{
				int texture_id2 = popup_weapon_now.GetItemSelectInfo().texture_id;
				go_role.ChangeWeapon(texture_id2);
				popup_weapon_now.EquipItem();
			}
			popup_weapon_now.SetItemSelectInfo(null);
			popup_weapon_now.SetItemNowInfo(null);
		}
		else if (m_event.GetEventName() == "TUIEvent_WeaponExchange")
		{
			if (m_event.GetControlSuccess())
			{
				popup_weapon_now.ExchangeWeapon();
				int texture_id3 = popup_weapon_now.GetItemSelectInfo().texture_id;
				go_role.ChangeWeapon(texture_id3);
			}
			popup_weapon_now.SetItemSelectInfo(null);
			popup_weapon_now.SetItemNowInfo(null);
		}
		else if (m_event.GetEventName() == "TUIEvent_Back" && !is_fade_out)
		{
			next_scene = "Scene_MainMenu";
			is_fade_out = true;
			m_fade.GetComponent<TUIFade>().FadeOut();
		}
	}

	public void TUIEvent_PopupRole(TUIControl control, int event_type, float wparam, float lparam, object data)
	{
		if (event_type == 3)
		{
			Debug.Log("you click role");
			CUISound.GetInstance().Play("UI_Button");
			popup_role.Show();
			popup_role.SetItemNowInfo(control.GetComponent<BtnItem_Item>());
			popup_role.BeforeItemSelect(control.GetComponent<BtnItem_Item>().GetInfo());
		}
	}

	public void TUIEvent_PopupProp(TUIControl control, int event_type, float wparam, float lparam, object data)
	{
		if (event_type == 3)
		{
			CUISound.GetInstance().Play("UI_Button");
			popup_prop.Show();
			popup_prop.SetSimpleInfo(control.GetComponent<BtnItem_Item>().GetInfo());
		}
	}

	public void TUIEvent_PopupSkill01(TUIControl control, int event_type, float wparam, float lparam, object data)
	{
		if (event_type == 3)
		{
			CUISound.GetInstance().Play("UI_Button");
			popup_skill01.Show();
			popup_skill01.SetSimpleInfo(control.GetComponent<BtnItem_Item>().GetInfo(), Popup_Show.PopupType.Skills01);
		}
	}

	public void TUIEvent_PopupSkill(TUIControl control, int event_type, float wparam, float lparam, object data)
	{
		if (event_type == 3)
		{
			CUISound.GetInstance().Play("UI_Button");
			Debug.Log("You click skill");
			popup_skill.Show();
			popup_skill.SetItemNowInfo(control.GetComponent<BtnItem_Item>());
			popup_skill.BeforeItemSelect(control.GetComponent<BtnItem_Item>().GetInfo());
		}
	}

	public void TUIEvent_PopupWeapon(TUIControl control, int event_type, float wparam, float lparam, object data)
	{
		if (event_type == 3)
		{
			CUISound.GetInstance().Play("UI_Button");
			Debug.Log("You click weapon");
			BtnItem_Item component = control.GetComponent<BtnItem_Item>();
			switch (component.GetIndex())
			{
			case 1:
				popup_weapon01.Show();
				popup_weapon01.SetItemNowInfo(component);
				popup_weapon01.BeforeItemSelect(component.GetInfo());
				popup_weapon_now = popup_weapon01;
				break;
			case 2:
			case 3:
				popup_weapon02.Show();
				popup_weapon02.SetItemNowInfo(component);
				popup_weapon02.BeforeItemSelect(component.GetInfo());
				popup_weapon_now = popup_weapon02;
				break;
			case 4:
				popup_weapon03.Show();
				popup_weapon03.SetItemNowInfo(component);
				popup_weapon03.BeforeItemSelect(component.GetInfo());
				popup_weapon_now = popup_weapon03;
				break;
			}
		}
	}

	public void TUIEvent_PopupRoleSelect(TUIControl control, int evnet_type, float wparam, float lparam, object data)
	{
		BtnSelect_Item component = control.GetComponent<BtnSelect_Item>();
		popup_role.SetItemSelectInfo(component.GetInfo());
	}

	public void TUIEvent_PopupSkillSelect(TUIControl control, int evnet_type, float wparam, float lparam, object data)
	{
		BtnSelect_Item component = control.GetComponent<BtnSelect_Item>();
		popup_skill.SetItemSelectInfo(component.GetInfo());
	}

	public void TUIEvent_PopupWeaponSelect(TUIControl control, int event_type, float wparam, float lparam, object data)
	{
		if (event_type == 1)
		{
			BtnSelect_Item component = control.GetComponent<BtnSelect_Item>();
			popup_weapon_now.SetItemSelectInfo(component.GetInfo());
		}
	}

	public void TUIEvent_PopupRoleEquip(TUIControl control, int event_type, float wparam, float lparam, object data)
	{
		if (event_type == 3)
		{
			if (popup_role.GetItemSelectInfo() != null)
			{
				int texture_id = popup_role.GetItemSelectInfo().texture_id;
				global::EventCenter.EventCenter.Instance.Publish(this, new TUIEvent.SendEvent_SceneEquip("TUIEvent_RoleEquip", texture_id));
			}
			popup_role.Hide();
		}
	}

	public void TUIEvent_PopupSkillEquip(TUIControl control, int event_type, float wparam, float lparam, object data)
	{
		if (event_type != 3)
		{
			return;
		}
		if (popup_skill.GetItemSelectInfo() != null)
		{
			if (popup_skill.IsExchangeSkill())
			{
				int index = popup_skill.GetExchangeItem01().GetIndex();
				int index2 = popup_skill.GetExchangeItem02().GetIndex();
				global::EventCenter.EventCenter.Instance.Publish(this, new TUIEvent.SendEvent_SceneEquip("TUIEvent_SkillExchange", index, index2));
			}
			else
			{
				int index3 = popup_skill.GetItemNowInfo().GetIndex();
				int texture_id = popup_skill.GetItemSelectInfo().texture_id;
				if (popup_skill.GetItemNowInfo().GetInfo() != null && popup_skill.GetItemNowInfo().GetInfo().texture_id == popup_skill.GetItemSelectInfo().texture_id)
				{
					global::EventCenter.EventCenter.Instance.Publish(this, new TUIEvent.SendEvent_SceneEquip("TUIEvent_SkillUnEquip", index3, texture_id));
				}
				else
				{
					global::EventCenter.EventCenter.Instance.Publish(this, new TUIEvent.SendEvent_SceneEquip("TUIEvent_SkillEquip", index3, texture_id));
				}
			}
		}
		popup_skill.Hide();
	}

	public void TUIEvent_PopupWeaponEquip(TUIControl control, int event_type, float wapram, float lparam, object data)
	{
		if (event_type != 3)
		{
			return;
		}
		if (popup_weapon_now.GetItemSelectInfo() != null)
		{
			if (popup_weapon_now.IsExchangeWeapon())
			{
				int index = popup_weapon_now.GetExchangeItem01().GetIndex();
				int index2 = popup_weapon_now.GetExchangeItem02().GetIndex();
				global::EventCenter.EventCenter.Instance.Publish(this, new TUIEvent.SendEvent_SceneEquip("TUIEvent_WeaponExchange", index, index2));
			}
			else
			{
				int index3 = popup_weapon_now.GetItemNowInfo().GetIndex();
				int texture_id = popup_weapon_now.GetItemSelectInfo().texture_id;
				global::EventCenter.EventCenter.Instance.Publish(this, new TUIEvent.SendEvent_SceneEquip("TUIEvent_WeaponEquip", index3, texture_id));
			}
		}
		popup_weapon_now.Hide();
		popup_weapon_now = null;
	}

	public void TUIEvent_ClosePopupRole(TUIControl control, int event_type, float wparam, float lparam, object data)
	{
		if (event_type == 3)
		{
			CUISound.GetInstance().Play("UI_Cancle");
			popup_role.Hide();
			popup_role.SetItemSelectInfo(null);
			popup_role.SetItemNowInfo(null);
		}
	}

	public void TUIEvent_ClosePopupProp(TUIControl control, int event_type, float wparam, float lparam, object data)
	{
		if (event_type == 3)
		{
			CUISound.GetInstance().Play("UI_Cancle");
			popup_prop.Hide();
		}
	}

	public void TUIEvent_ClosePopupSkill01(TUIControl control, int event_type, float wparam, float lparam, object data)
	{
		if (event_type == 3)
		{
			CUISound.GetInstance().Play("UI_Cancle");
			popup_skill01.Hide();
		}
	}

	public void TUIEvent_ClosePopupSkill(TUIControl control, int event_type, float wparam, float lparam, object data)
	{
		if (event_type == 3)
		{
			CUISound.GetInstance().Play("UI_Cancle");
			popup_skill.Hide();
			popup_skill.SetItemSelectInfo(null);
			popup_skill.SetItemNowInfo(null);
		}
	}

	public void TUIEvent_ClosePopupWeapon(TUIControl control, int event_type, float wparam, float lparam, object data)
	{
		if (event_type == 3)
		{
			CUISound.GetInstance().Play("UI_Cancle");
			popup_weapon_now.Hide();
			popup_weapon_now.SetItemSelectInfo(null);
			popup_weapon_now.SetItemNowInfo(null);
			popup_weapon_now = null;
		}
	}

	public void TUIEvent_MoveScreen(TUIControl control, int event_type, float wparam, float lparam, object data)
	{
		if (event_type == 2)
		{
			go_role.SetRotation(wparam, lparam);
		}
	}

	public void TUIEvent_Back(TUIControl control, int event_type, float wparam, float lparam, object data)
	{
		if (event_type == 3)
		{
			CUISound.GetInstance().Play("UI_Button");
			global::EventCenter.EventCenter.Instance.Publish(this, new TUIEvent.SendEvent_SceneEquip("TUIEvent_Back"));
		}
	}

	public void TUIEvent_IAP(TUIControl control, int event_type, float wparam, float lparam, object data)
	{
		if (event_type == 3 && !is_fade_out)
		{
			next_scene = "Scene_IAP";
			is_fade_out = true;
			m_fade.GetComponent<TUIFade>().FadeOut();
		}
	}
}
