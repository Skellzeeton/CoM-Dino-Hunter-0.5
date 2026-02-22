using System.Collections.Generic;
using UnityEngine;

public class Popup_Show : MonoBehaviour
{
	public enum PopupType
	{
		Roles = 0,
		Props = 1,
		Skills01 = 2,
		Skills = 3,
		Weapons01 = 4,
		Weapons02 = 5,
		Weapons03 = 6
	}

	public BtnSelect_Item prefab_items;

	public TUIScrollList go_scroll_list;

	public TUIRect rect_show;

	public TUILabel label_title;

	public TUILabel label_introduce;

	public TUILabel label_damage_value;

	public TUILabel label_fire_rate_value;

	public TUILabel label_blast_radius_value;

	public TUILabel label_knockback_value;

	public TUILabel label_ammo_value;

	public TUILabel label_damage;

	public TUILabel label_fire_rate;

	public TUILabel label_blast_radius;

	public TUILabel label_knockback;

	public TUILabel label_ammo;

	public Popup_BtnEquip btn_equip;

	private List<TUIPopupInfo> popup_list;

	private List<BtnSelect_Item> select_list;

	private BtnItem_Item item_now;

	private TUIPopupInfo item_select;

	private PopupType popup_type;

	private BtnItem_Item item_01;

	private BtnItem_Item item_02;

	private BtnItem_Item item_03;

	private BtnItem_Item item_04;

	private BtnItem_Item exchange_item01;

	private BtnItem_Item exchange_item02;

	private void Awake()
	{
		popup_type = PopupType.Roles;
		select_list = new List<BtnSelect_Item>();
	}

	private void Update()
	{
	}

	public void Show()
	{
		ClearInfo();
		base.transform.localPosition = new Vector3(0f, 0f, base.transform.localPosition.z);
		base.GetComponent<Animation>().Play();
	}

	public void Hide()
	{
		base.transform.localPosition = new Vector3(0f, -1000f, base.transform.localPosition.z);
		ClearInfo();
	}

	public void SetInfo(List<TUIPopupInfo> m_popup_list, GameObject invoke_object, PopupType m_popup_type)
	{
		if (m_popup_list == null)
		{
			Debug.Log("error!");
			return;
		}
		popup_list = m_popup_list;
		popup_type = m_popup_type;
		for (int i = 0; i < popup_list.Count; i++)
		{
			BtnSelect_Item btnSelect_Item = (BtnSelect_Item)Object.Instantiate(prefab_items);
			btnSelect_Item.DoCreate(popup_list[i], rect_show, i + 1, popup_type);
			TUIButtonSelect component = btnSelect_Item.transform.GetComponent<TUIButtonSelect>();
			component.invokeOnEvent = true;
			component.invokeObject = invoke_object;
			component.componentName = "Scene_Equip";
			if (popup_type == PopupType.Roles)
			{
				component.invokeFunctionName = "TUIEvent_PopupRoleSelect";
			}
			else if (popup_type == PopupType.Skills)
			{
				component.invokeFunctionName = "TUIEvent_PopupSkillSelect";
			}
			else if (popup_type == PopupType.Weapons01 || popup_type == PopupType.Weapons02 || popup_type == PopupType.Weapons03)
			{
				component.invokeFunctionName = "TUIEvent_PopupWeaponSelect";
			}
			else
			{
				Debug.Log("error!");
			}
			go_scroll_list.Add(component);
			select_list.Add(btnSelect_Item);
		}
	}

	public void SetSimpleInfo(TUIPopupInfo m_popup_info, PopupType m_popup_type = PopupType.Props)
	{
		label_title.Text = m_popup_info.name;
		label_introduce.Text = m_popup_info.introduce;
	}

	public void SetBtnInfo(int m_id, BtnItem_Item m_item)
	{
		switch (m_id)
		{
		case 1:
			item_01 = m_item;
			break;
		case 2:
			item_02 = m_item;
			break;
		case 3:
			item_03 = m_item;
			break;
		case 4:
			item_04 = m_item;
			break;
		default:
			Debug.Log("error!");
			break;
		}
	}

	public void BeforeItemSelect(TUIPopupInfo m_item)
	{
		item_select = m_item;
		if (item_select != null)
		{
			for (int i = 0; i < select_list.Count; i++)
			{
				BtnSelect_Item btnSelect_Item = select_list[i];
				TUIButtonSelect component = btnSelect_Item.GetComponent<TUIButtonSelect>();
				if (btnSelect_Item.GetInfo().texture_id == item_select.texture_id)
				{
					component.SetSelected(true);
					go_scroll_list.Remove(component, false);
					go_scroll_list.Insert(0, component);
				}
				else
				{
					component.SetSelected(false);
				}
			}
		}
		else
		{
			for (int j = 0; j < select_list.Count; j++)
			{
				TUIButtonSelect component2 = select_list[j].GetComponent<TUIButtonSelect>();
				component2.SetSelected(false);
			}
		}
		SetItemSelectInfo(item_select);
		go_scroll_list.ScrollListTo(0f);
	}

	public void SetItemSelectInfo(TUIPopupInfo m_popup_info)
	{
		item_select = m_popup_info;
		if (item_select != null)
		{
			if (popup_type == PopupType.Weapons01 || popup_type == PopupType.Weapons02)
			{
				label_title.Text = item_select.name;
				label_introduce.Text = item_select.introduce;
				label_damage_value.Text = item_select.weapon_attribute.damage.ToString();
				label_fire_rate_value.Text = item_select.weapon_attribute.fire_rate.ToString();
				label_blast_radius_value.Text = item_select.weapon_attribute.blast_radius.ToString();
				label_knockback_value.Text = item_select.weapon_attribute.knockback.ToString();
				label_ammo_value.Text = item_select.weapon_attribute.ammo.ToString();
				if (label_damage != null)
				{
					label_damage.Text = "DAMAGE";
				}
				if (label_fire_rate != null)
				{
					label_fire_rate.Text = "FIRE RATE";
				}
				if (label_blast_radius != null)
				{
					label_blast_radius.Text = "BLAST RADIUS";
				}
				if (label_knockback != null)
				{
					label_knockback.Text = "KNOCKBACK";
				}
				if (label_ammo != null)
				{
					label_ammo.Text = "AMMO";
				}
			}
			else
			{
				label_title.Text = item_select.name;
				label_introduce.Text = item_select.introduce;
			}
			if (item_now.GetInfo() != null && item_now.GetInfo().texture_id == item_select.texture_id)
			{
				SetBtnEquip(false);
			}
			else
			{
				SetBtnEquip(true);
			}
		}
		else
		{
			SetBtnEquip(true);
			ClearInfo();
		}
	}

	public void SetItemNowInfo(BtnItem_Item m_info)
	{
		item_now = m_info;
	}

	public TUIPopupInfo GetItemSelectInfo()
	{
		return item_select;
	}

	public BtnItem_Item GetItemNowInfo()
	{
		return item_now;
	}

	public void EquipItem()
	{
		if (item_select != null)
		{
			if (popup_type == PopupType.Roles)
			{
				item_now.SetInfo(item_select, popup_type);
			}
			else
			{
				item_now.SetInfo(item_select, popup_type, true);
			}
		}
	}

	public bool IsExchangeWeapon()
	{
		if (item_select == null)
		{
			return false;
		}
		if (popup_type != PopupType.Weapons02)
		{
			return false;
		}
		if (item_now == item_02)
		{
			if (item_03.GetInfo() != null && item_select.texture_id == item_03.GetInfo().texture_id)
			{
				exchange_item01 = item_02;
				exchange_item02 = item_03;
				return true;
			}
		}
		else if (item_now == item_03 && item_02.GetInfo() != null && item_select.texture_id == item_02.GetInfo().texture_id)
		{
			exchange_item01 = item_03;
			exchange_item02 = item_02;
			return true;
		}
		return false;
	}

	public bool IsExchangeSkill()
	{
		if (item_select == null)
		{
			return false;
		}
		if (popup_type != PopupType.Skills && popup_type != PopupType.Skills01)
		{
			return false;
		}
		if (item_now == item_01)
		{
			if (item_02.GetInfo() != null && item_select.texture_id == item_02.GetInfo().texture_id)
			{
				exchange_item01 = item_01;
				exchange_item02 = item_02;
				return true;
			}
			if (item_03.GetInfo() != null && item_select.texture_id == item_03.GetInfo().texture_id)
			{
				exchange_item01 = item_01;
				exchange_item02 = item_03;
				return true;
			}
			if (item_04.GetInfo() != null && item_select.texture_id == item_04.GetInfo().texture_id)
			{
				exchange_item01 = item_01;
				exchange_item02 = item_04;
				return true;
			}
		}
		else if (item_now == item_02)
		{
			if (item_01.GetInfo() != null && item_select.texture_id == item_01.GetInfo().texture_id)
			{
				exchange_item01 = item_02;
				exchange_item02 = item_01;
				return true;
			}
			if (item_03.GetInfo() != null && item_select.texture_id == item_03.GetInfo().texture_id)
			{
				exchange_item01 = item_02;
				exchange_item02 = item_03;
				return true;
			}
			if (item_04.GetInfo() != null && item_select.texture_id == item_04.GetInfo().texture_id)
			{
				exchange_item01 = item_02;
				exchange_item02 = item_04;
				return true;
			}
		}
		else if (item_now == item_03)
		{
			if (item_01.GetInfo() != null && item_select.texture_id == item_01.GetInfo().texture_id)
			{
				exchange_item01 = item_03;
				exchange_item02 = item_01;
				return true;
			}
			if (item_02.GetInfo() != null && item_select.texture_id == item_02.GetInfo().texture_id)
			{
				exchange_item01 = item_03;
				exchange_item02 = item_02;
				return true;
			}
			if (item_04.GetInfo() != null && item_select.texture_id == item_04.GetInfo().texture_id)
			{
				exchange_item01 = item_03;
				exchange_item02 = item_04;
				return true;
			}
		}
		else if (item_now == item_04)
		{
			if (item_01.GetInfo() != null && item_select.texture_id == item_01.GetInfo().texture_id)
			{
				exchange_item01 = item_04;
				exchange_item02 = item_01;
				return true;
			}
			if (item_02.GetInfo() != null && item_select.texture_id == item_02.GetInfo().texture_id)
			{
				exchange_item01 = item_04;
				exchange_item02 = item_02;
				return true;
			}
			if (item_03.GetInfo() != null && item_select.texture_id == item_03.GetInfo().texture_id)
			{
				exchange_item01 = item_04;
				exchange_item02 = item_03;
				return true;
			}
		}
		return false;
	}

	public BtnItem_Item GetExchangeItem01()
	{
		return exchange_item01;
	}

	public BtnItem_Item GetExchangeItem02()
	{
		return exchange_item02;
	}

	public void ExchangeWeapon()
	{
		TUIPopupInfo info = exchange_item01.GetInfo();
		exchange_item01.SetInfo(exchange_item02.GetInfo(), popup_type, true);
		exchange_item02.SetInfo(info, popup_type, true);
	}

	public void ExchangeSkill()
	{
		TUIPopupInfo info = exchange_item01.GetInfo();
		exchange_item01.SetInfo(exchange_item02.GetInfo(), popup_type, true);
		exchange_item02.SetInfo(info, popup_type, true);
	}

	public void UnEquipItem()
	{
		if (popup_type != PopupType.Skills)
		{
			Debug.Log("error!");
		}
		else
		{
			item_now.SetInfo(null);
		}
	}

	public void SetBtnEquip(bool m_bool)
	{
		if (popup_type == PopupType.Skills)
		{
			if (btn_equip == null)
			{
				Debug.Log("error!");
			}
			else if (m_bool)
			{
				btn_equip.GetComponent<Popup_BtnEquip>().SetEquip();
			}
			else
			{
				btn_equip.GetComponent<Popup_BtnEquip>().SetUnEquip();
			}
		}
	}

	public void ClearInfo()
	{
		if (label_title != null)
		{
			label_title.Text = string.Empty;
		}
		if (label_introduce != null)
		{
			label_introduce.Text = string.Empty;
		}
		if (label_damage_value != null)
		{
			label_damage_value.Text = string.Empty;
		}
		if (label_fire_rate_value != null)
		{
			label_fire_rate_value.Text = string.Empty;
		}
		if (label_blast_radius_value != null)
		{
			label_blast_radius_value.Text = string.Empty;
		}
		if (label_knockback_value != null)
		{
			label_knockback_value.Text = string.Empty;
		}
		if (label_ammo_value != null)
		{
			label_ammo_value.Text = string.Empty;
		}
		if (label_damage != null)
		{
			label_damage.Text = string.Empty;
		}
		if (label_fire_rate != null)
		{
			label_fire_rate.Text = string.Empty;
		}
		if (label_blast_radius != null)
		{
			label_blast_radius.Text = string.Empty;
		}
		if (label_knockback != null)
		{
			label_knockback.Text = string.Empty;
		}
		if (label_ammo != null)
		{
			label_ammo.Text = string.Empty;
		}
	}

	public void ShowInfo()
	{
		if (label_title != null)
		{
			label_title.Text = string.Empty;
		}
		if (label_introduce != null)
		{
			label_introduce.Text = string.Empty;
		}
		if (label_damage_value != null)
		{
			label_damage_value.Text = string.Empty;
		}
		if (label_fire_rate_value != null)
		{
			label_fire_rate_value.Text = string.Empty;
		}
		if (label_blast_radius_value != null)
		{
			label_blast_radius_value.Text = string.Empty;
		}
		if (label_knockback_value != null)
		{
			label_knockback_value.Text = string.Empty;
		}
		if (label_ammo_value != null)
		{
			label_ammo_value.Text = string.Empty;
		}
		if (label_damage != null)
		{
			label_damage.Text = string.Empty;
		}
		if (label_fire_rate != null)
		{
			label_fire_rate.Text = string.Empty;
		}
		if (label_blast_radius != null)
		{
			label_blast_radius.Text = string.Empty;
		}
		if (label_knockback != null)
		{
			label_knockback.Text = string.Empty;
		}
		if (label_ammo != null)
		{
			label_ammo.Text = string.Empty;
		}
	}
}
