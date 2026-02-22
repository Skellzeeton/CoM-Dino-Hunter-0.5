using UnityEngine;

public class PopupRole : MonoBehaviour
{
	public Top_Bar top_bar;

	public Role_Control role_control;

	public ScrollList_Role scroll_list_role;

	public TUILabel label_title;

	public TUILabel label_introduce;

	public PopupRoleBuy btn_buy;

	private ScrollList_RoleItem item_choose;

	private int role_now_id;

	private void Start()
	{
	}

	private void Update()
	{
		CheckScrollChoose();
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
		SetRoleUse(avatar_id);
	}

	public void AddScrollListItem(TUIAllRoleInfo m_all_role_info)
	{
		scroll_list_role.AddScrollListItem(m_all_role_info);
	}

	public void CheckScrollChoose()
	{
		ScrollList_RoleItem itemChoose = scroll_list_role.GetItemChoose();
		if (item_choose != itemChoose)
		{
			item_choose = itemChoose;
			if (item_choose != null)
			{
				SetInfo(item_choose.GetRoleInfo());
			}
		}
	}

	public void SetInfo(TUIRoleInfo m_info)
	{
		if (m_info == null)
		{
			Debug.Log("error! no info!");
		}
		label_title.Text = m_info.name;
		label_introduce.Text = m_info.introduce;
		if (!m_info.unlock)
		{
			btn_buy.SetStateUnlock(m_info.unlock_price.price, m_info.unlock_price.unit_type);
		}
		else if (!m_info.do_buy)
		{
			btn_buy.SetStateBuy(m_info.do_buy_price.price, m_info.do_buy_price.unit_type);
		}
		else if (role_now_id != m_info.id)
		{
			btn_buy.SetStateUse();
		}
		else
		{
			btn_buy.SetStateDisable();
		}
		SetRoleID(m_info.id);
	}

	public void SetRoleID(int m_id)
	{
		role_control.ChangeRole(m_id);
	}

	public void SetRoleUse(int m_id)
	{
		btn_buy.SetStateDisable();
		role_now_id = m_id;
		SetRoleID(role_now_id);
	}

	public int GetRoleChooseID()
	{
		return item_choose.GetRoleInfo().id;
	}

	public PopupRoleBuy.PopupRoleBuyState GetRoleBuyState()
	{
		return btn_buy.GetState();
	}

	public void SetRoleUnlock()
	{
		int price = item_choose.GetRoleInfo().unlock_price.price;
		switch (item_choose.GetRoleInfo().unlock_price.unit_type)
		{
		case UnitType.Gold:
		{
			int goldValue = top_bar.GetGoldValue();
			goldValue -= price;
			if (goldValue >= 0)
			{
				top_bar.SetGoldValue(goldValue);
				break;
			}
			Debug.Log("you have no gold enough!");
			return;
		}
		case UnitType.Crystal:
		{
			int crystalValue = top_bar.GetCrystalValue();
			crystalValue -= price;
			if (crystalValue >= 0)
			{
				top_bar.SetCrystalValue(crystalValue);
				break;
			}
			Debug.Log("you have no crystal enough!");
			return;
		}
		}
		item_choose.DoUnlock();
		int price2 = item_choose.GetRoleInfo().do_buy_price.price;
		UnitType unit_type = item_choose.GetRoleInfo().do_buy_price.unit_type;
		btn_buy.SetStateBuy(price2, unit_type);
	}

	public void SetRoleBuy()
	{
		int price = item_choose.GetRoleInfo().do_buy_price.price;
		switch (item_choose.GetRoleInfo().do_buy_price.unit_type)
		{
		case UnitType.Gold:
		{
			int goldValue = top_bar.GetGoldValue();
			goldValue -= price;
			if (goldValue >= 0)
			{
				top_bar.SetGoldValue(goldValue);
				break;
			}
			Debug.Log("you have no gold enough!");
			return;
		}
		case UnitType.Crystal:
		{
			int crystalValue = top_bar.GetCrystalValue();
			crystalValue -= price;
			if (crystalValue >= 0)
			{
				top_bar.SetCrystalValue(crystalValue);
				break;
			}
			Debug.Log("you have no crystal enough!");
			return;
		}
		}
		item_choose.DoBuy();
		btn_buy.SetStateUse();
	}

	public void SetRoleRotation(float m_wparam, float m_lparam)
	{
		role_control.SetRotation(m_wparam, m_lparam);
	}
}
