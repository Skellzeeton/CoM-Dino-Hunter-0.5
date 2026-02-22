using System.Collections.Generic;
using UnityEngine;

public class PopupWeapon : MonoBehaviour
{
	public Top_Bar top_bar;

	public Role_Control role_control;

	public WeaponKindItem weapon_kind_item;

	public LevelStars level_stars;

	public LabelInfo_Weapon label_info_weapon;

	public TUILabel label_title;

	public PopupWeaponUpdate popup_weapon_update;

	public PopupWeaponBuy popup_weapon_buy;

	public UnlockBlink unlock_blink;

	private ScrollList_WeaponItem item_choose;

	private int role_now_id;

	private GoodsNeedItemBuy btn_goods_buy;

	private TUIWeaponInfo weapon_info;

	private ScrollList_Weapon scroll_list_weapon_now;

	public ScrollList_Weapon prefab_scroll_list_weapon01;

	public ScrollList_Weapon prefab_scroll_list_weapon02;

	public ScrollList_Weapon prefab_scroll_list_weapon03;

	public ScrollList_Weapon prefab_scroll_list_weapon04;

	public ScrollList_Weapon prefab_scroll_list_weapon05;

	public ScrollList_Weapon prefab_scroll_list_weapon06;

	public ScrollList_Weapon prefab_scroll_list_weapon07;

	private ScrollList_Weapon scroll_list_weapon01;

	private ScrollList_Weapon scroll_list_weapon02;

	private ScrollList_Weapon scroll_list_weapon03;

	private ScrollList_Weapon scroll_list_weapon04;

	private ScrollList_Weapon scroll_list_weapon05;

	private ScrollList_Weapon scroll_list_weapon06;

	private ScrollList_Weapon scroll_list_weapon07;

	private void Start()
	{
	}

	private void Update()
	{
		CheckScrollChoose();
	}

	public void SetWeaponInfo(TUIWeaponInfo m_weapon_info)
	{
		if (weapon_info == null)
		{
			weapon_info = new TUIWeaponInfo();
		}
		weapon_info = m_weapon_info;
	}

	public void CheckScrollChoose()
	{
		if (scroll_list_weapon_now == null)
		{
			return;
		}
		ScrollList_WeaponItem itemChoose = scroll_list_weapon_now.GetItemChoose();
		if (item_choose != itemChoose)
		{
			item_choose = itemChoose;
			if (item_choose != null)
			{
				SetInfo(item_choose.GetWeaponAttributeInfo());
				SetRoleWeapon(item_choose.GetWeaponAttributeInfo().id);
			}
		}
	}

	public void SetInfo(TUIWeaponAttributeInfo m_info)
	{
		if (m_info == null)
		{
			Debug.Log("error! no info!");
			return;
		}
		label_title.Text = m_info.name;
		float x = label_title.CalculateBounds(label_title.Text).size.x;
		Vector3 position = new Vector3(label_title.transform.localPosition.x + x + 10f, label_title.transform.localPosition.y, label_title.transform.localPosition.z);
		int level = m_info.level;
		TUIWeaponUpdateInfo weapon_update_info = m_info.weapon_update_info;
		if (weapon_update_info == null)
		{
			Debug.Log("error! no update info");
			return;
		}
		if (m_info.kind == WeaponType.Stoneskin)
		{
			string introduce = string.Empty;
			int hp = 0;
			if (weapon_update_info.level_hp != null && weapon_update_info.level_hp.ContainsKey((level == 0) ? 1 : level))
			{
				hp = weapon_update_info.level_hp[(level == 0) ? 1 : level];
				introduce = weapon_update_info.level_introduce_ex[(level == 0) ? 1 : level];
			}
			label_info_weapon.SetStoneskinInfo(introduce, hp);
		}
		else
		{
			int damage = 0;
			float fire_rate = 0f;
			int blast_radius = 0;
			int knockback = 0;
			int ammo = 0;
			if (weapon_update_info.level_damage != null && weapon_update_info.level_damage.ContainsKey((level == 0) ? 1 : level))
			{
				damage = weapon_update_info.level_damage[(level == 0) ? 1 : level];
			}
			if (weapon_update_info.level_fire_rate != null && weapon_update_info.level_fire_rate.ContainsKey((level == 0) ? 1 : level))
			{
				fire_rate = weapon_update_info.level_fire_rate[(level == 0) ? 1 : level];
			}
			if (weapon_update_info.level_blast_radius != null && weapon_update_info.level_blast_radius.ContainsKey((level == 0) ? 1 : level))
			{
				blast_radius = weapon_update_info.level_blast_radius[(level == 0) ? 1 : level];
			}
			if (weapon_update_info.level_knockback != null && weapon_update_info.level_knockback.ContainsKey((level == 0) ? 1 : level))
			{
				knockback = weapon_update_info.level_knockback[(level == 0) ? 1 : level];
			}
			if (weapon_update_info.level_ammo != null && weapon_update_info.level_ammo.ContainsKey((level == 0) ? 1 : level))
			{
				ammo = weapon_update_info.level_ammo[(level == 0) ? 1 : level];
			}
			label_info_weapon.SetWeaponInfo(damage, fire_rate, blast_radius, knockback, ammo);
		}
		level_stars.SetStars(level, position);
		if (level >= 5)
		{
			popup_weapon_buy.SetStateMax();
		}
		else if (level <= 0)
		{
			popup_weapon_buy.SetStateCraft();
		}
		else
		{
			popup_weapon_buy.SetStateUpdate();
		}
	}

	public void OpenWeaponUpdate()
	{
		if (item_choose == null)
		{
			Debug.Log("error! no item_choose");
			return;
		}
		popup_weapon_update.ShowWeaponUpdate();
		popup_weapon_update.SetInfo(item_choose);
	}

	public void CloseWeaponUpdate()
	{
		popup_weapon_update.HideWeaponUpdate();
	}

	public void SetRoleID(int id)
	{
		role_now_id = id;
		role_control.ChangeRole(id);
	}

	public void SetRoleWeapon(int id)
	{
		role_control.ChangeWeapon(id);
	}

	public void UpdateGoodsBuy()
	{
		int index = btn_goods_buy.GetIndex();
		int goodsID = btn_goods_buy.GetGoodsID();
		int goodsLackCount = btn_goods_buy.GetGoodsLackCount();
		int goodsPrice = btn_goods_buy.GetGoodsPrice();
		switch (btn_goods_buy.GetGoodsUnitType())
		{
		case UnitType.Gold:
		{
			int goldValue = top_bar.GetGoldValue();
			goldValue -= goodsPrice * goodsLackCount;
			if (goldValue < 0)
			{
				Debug.Log("error!you have no gold enough!");
				return;
			}
			top_bar.SetGoldValue(goldValue);
			break;
		}
		case UnitType.Crystal:
		{
			int crystalValue = top_bar.GetCrystalValue();
			crystalValue -= goodsPrice * goodsLackCount;
			if (crystalValue < 0)
			{
				Debug.Log("error!you have no crystal enough!");
				return;
			}
			top_bar.SetCrystalValue(crystalValue);
			break;
		}
		default:
			Debug.Log("error!");
			return;
		}
		int count = item_choose.GetWeaponAttributeInfo().goods_list[goodsID].count;
		count += goodsLackCount;
		item_choose.GetWeaponAttributeInfo().goods_list[goodsID].SetCount(count);
		popup_weapon_update.UpdateGoodsBuy(index);
		Debug.Log("add goods:" + goodsID + " count:" + goodsLackCount + " remain:" + count);
	}

	public int GetWeaponID()
	{
		if (item_choose == null || item_choose.GetWeaponAttributeInfo() == null)
		{
			Debug.Log("error! no info!");
			return 0;
		}
		return item_choose.GetWeaponAttributeInfo().id;
	}

	public WeaponType GetWeaponType()
	{
		if (item_choose == null || item_choose.GetWeaponAttributeInfo() == null)
		{
			Debug.Log("error! no info!");
			return WeaponType.None;
		}
		return item_choose.GetWeaponAttributeInfo().kind;
	}

	public int GetWeaponLevel()
	{
		return item_choose.GetWeaponAttributeInfo().level;
	}

	public void UpdateWeapon()
	{
		TUIWeaponAttributeInfo weaponAttributeInfo = item_choose.GetWeaponAttributeInfo();
		int level = weaponAttributeInfo.level;
		int count = weaponAttributeInfo.weapon_update_info.level_price.Count;
		if (level >= count)
		{
			return;
		}
		int price = weaponAttributeInfo.weapon_update_info.level_price[level + 1].price;
		UnitType unit_type = weaponAttributeInfo.weapon_update_info.level_price[level + 1].unit_type;
		int num = 0;
		switch (unit_type)
		{
		case UnitType.Gold:
			num = top_bar.GetGoldValue();
			num -= price;
			if (num < 0)
			{
				Debug.Log("you have no gold enough!");
				return;
			}
			break;
		case UnitType.Crystal:
			num = top_bar.GetCrystalValue();
			num -= price;
			if (num < 0)
			{
				Debug.Log("you have no crystal enough!");
				return;
			}
			break;
		}
		List<TUIGoodsNeedInfo> goodsNeedInfo = weaponAttributeInfo.level_goods_need_info.GetGoodsNeedInfo(weaponAttributeInfo.level + 1);
		if (goodsNeedInfo != null)
		{
			for (int i = 0; i < goodsNeedInfo.Count; i++)
			{
				int goods_id = goodsNeedInfo[i].goods_id;
				int need_count = goodsNeedInfo[i].need_count;
				GoodsQualityType goods_quality = goodsNeedInfo[i].goods_quality;
				int num2 = 0;
				if (goods_quality == weaponAttributeInfo.goods_list[goods_id].quality)
				{
					num2 = weaponAttributeInfo.goods_list[goods_id].count;
				}
				num2 -= need_count;
				if (num2 < 0)
				{
					Debug.Log("you have no goods enough!");
					return;
				}
			}
			Debug.Log("!!!!");
			for (int j = 0; j < goodsNeedInfo.Count; j++)
			{
				int goods_id2 = goodsNeedInfo[j].goods_id;
				int need_count2 = goodsNeedInfo[j].need_count;
				GoodsQualityType goods_quality2 = goodsNeedInfo[j].goods_quality;
				int num3 = 0;
				if (goods_quality2 == weaponAttributeInfo.goods_list[goods_id2].quality)
				{
					num3 = weaponAttributeInfo.goods_list[goods_id2].count;
				}
				num3 -= need_count2;
				Debug.Log("cost goods:" + goods_id2 + " count:" + need_count2 + " remain:" + num3);
				weaponAttributeInfo.goods_list[goods_id2].SetCount(num3);
			}
		}
		weaponAttributeInfo.level++;
		float x = label_title.CalculateBounds(label_title.Text).size.x;
		Vector3 position = new Vector3(label_title.transform.localPosition.x + x + 10f, label_title.transform.localPosition.y, label_title.transform.localPosition.z);
		level_stars.SetStars(weaponAttributeInfo.level, position, weaponAttributeInfo.level);
		if (weaponAttributeInfo == null)
		{
			Debug.Log("error! no attribute info");
			return;
		}
		int level2 = weaponAttributeInfo.level;
		TUIWeaponUpdateInfo weapon_update_info = weaponAttributeInfo.weapon_update_info;
		if (weaponAttributeInfo.kind == WeaponType.Stoneskin)
		{
			string introduce = string.Empty;
			int hp = 0;
			if (weapon_update_info.level_hp != null && weapon_update_info.level_hp.ContainsKey((level2 == 0) ? 1 : level2))
			{
				hp = weapon_update_info.level_hp[(level2 == 0) ? 1 : level2];
				introduce = weapon_update_info.level_introduce_ex[(level2 == 0) ? 1 : level2];
			}
			label_info_weapon.SetStoneskinInfo(introduce, hp);
		}
		else
		{
			int damage = 0;
			float fire_rate = 0f;
			int blast_radius = 0;
			int knockback = 0;
			int ammo = 0;
			if (weapon_update_info.level_damage != null && weapon_update_info.level_damage.ContainsKey((level2 == 0) ? 1 : level2))
			{
				damage = weapon_update_info.level_damage[(level2 == 0) ? 1 : level2];
			}
			if (weapon_update_info.level_fire_rate != null && weapon_update_info.level_fire_rate.ContainsKey((level2 == 0) ? 1 : level2))
			{
				fire_rate = weapon_update_info.level_fire_rate[(level2 == 0) ? 1 : level2];
			}
			if (weapon_update_info.level_blast_radius != null && weapon_update_info.level_blast_radius.ContainsKey((level2 == 0) ? 1 : level2))
			{
				blast_radius = weapon_update_info.level_blast_radius[(level2 == 0) ? 1 : level2];
			}
			if (weapon_update_info.level_knockback != null && weapon_update_info.level_knockback.ContainsKey((level2 == 0) ? 1 : level2))
			{
				knockback = weapon_update_info.level_knockback[(level2 == 0) ? 1 : level2];
			}
			if (weapon_update_info.level_ammo != null && weapon_update_info.level_ammo.ContainsKey((level2 == 0) ? 1 : level2))
			{
				ammo = weapon_update_info.level_ammo[(level2 == 0) ? 1 : level2];
			}
			label_info_weapon.SetWeaponInfo(damage, fire_rate, blast_radius, knockback, ammo);
		}
		if (level2 >= count)
		{
			popup_weapon_buy.SetStateMax();
		}
		if (level2 == 1)
		{
			unlock_blink.OpenBlink(item_choose.GetCustomizeTexture());
		}
		switch (unit_type)
		{
		case UnitType.Gold:
			top_bar.SetGoldValue(num);
			break;
		case UnitType.Crystal:
			top_bar.SetCrystalValue(num);
			break;
		}
	}

	public void SetTopBarInfo(TUIPlayerInfo m_player_info)
	{
		if (m_player_info == null)
		{
			Debug.Log("error! no found info");
			return;
		}
		int avatar_id = m_player_info.avatar_id;
		int level = m_player_info.level;
		int exp = m_player_info.exp;
		int level_exp = m_player_info.level_exp;
		int gold = m_player_info.gold;
		int crystal = m_player_info.crystal;
		top_bar.SetAllValue(level, exp, level_exp, gold, crystal);
		SetRoleID(avatar_id);
	}

	public void SetGoodsNeedItemBuy(GoodsNeedItemBuy m_item)
	{
		btn_goods_buy = m_item;
	}

	public GoodsNeedItemBuy GetGoodsNeedItemBuy()
	{
		return btn_goods_buy;
	}

	public void CloseBlink()
	{
		unlock_blink.CloseBlink();
	}

	public void SetRoleRotation(float wparam, float lparam)
	{
		role_control.SetRotation(wparam, lparam);
	}

	public void SetWeaponKindItem(WeaponType m_type)
	{
		weapon_kind_item.SetSelectBtn(m_type);
		if (scroll_list_weapon_now != null)
		{
			scroll_list_weapon_now.Hide();
			item_choose = null;
		}
		if (weapon_info == null)
		{
			Debug.Log("no weapon_info!");
			return;
		}
		Vector3 normal_scroll_pos = new Vector3(84f, 15f, -3f);
		switch (m_type)
		{
		case WeaponType.CloseWeapon:
			SetWeaponKindItemEx(ref scroll_list_weapon01, prefab_scroll_list_weapon01, weapon_info.weapon_list01, normal_scroll_pos);
			break;
		case WeaponType.Crossbow:
			SetWeaponKindItemEx(ref scroll_list_weapon02, prefab_scroll_list_weapon02, weapon_info.weapon_list02, normal_scroll_pos);
			break;
		case WeaponType.MachineGun:
			SetWeaponKindItemEx(ref scroll_list_weapon03, prefab_scroll_list_weapon03, weapon_info.weapon_list03, normal_scroll_pos);
			break;
		case WeaponType.ViolenceGun:
			SetWeaponKindItemEx(ref scroll_list_weapon04, prefab_scroll_list_weapon04, weapon_info.weapon_list04, normal_scroll_pos);
			break;
		case WeaponType.LiquidFireGun:
			SetWeaponKindItemEx(ref scroll_list_weapon05, prefab_scroll_list_weapon05, weapon_info.weapon_list05, normal_scroll_pos);
			break;
		case WeaponType.RPG:
			SetWeaponKindItemEx(ref scroll_list_weapon06, prefab_scroll_list_weapon06, weapon_info.weapon_list06, normal_scroll_pos);
			break;
		case WeaponType.Stoneskin:
			SetWeaponKindItemEx(ref scroll_list_weapon07, prefab_scroll_list_weapon07, weapon_info.weapon_list07, normal_scroll_pos);
			break;
		}
	}

	private void SetWeaponKindItemEx(ref ScrollList_Weapon m_scrolllist_weapon, ScrollList_Weapon m_prefab_scrolllist_weapon, List<TUIWeaponAttributeInfo> m_weapon_list, Vector3 m_normal_scroll_pos)
	{
		if (m_scrolllist_weapon == null && m_weapon_list != null)
		{
			if (m_prefab_scrolllist_weapon == null)
			{
				Debug.Log("error!");
				return;
			}
			GameObject gameObject = (GameObject)Object.Instantiate(m_prefab_scrolllist_weapon.gameObject);
			m_scrolllist_weapon = gameObject.GetComponent<ScrollList_Weapon>();
			m_scrolllist_weapon.transform.parent = base.transform.parent;
			m_scrolllist_weapon.transform.localPosition = m_normal_scroll_pos;
			m_scrolllist_weapon.AddScrollListItem(m_weapon_list);
		}
		scroll_list_weapon_now = m_scrolllist_weapon;
		if (scroll_list_weapon_now != null)
		{
			scroll_list_weapon_now.Show();
		}
	}
}
