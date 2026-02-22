using UnityEngine;

public class PopupSkill : MonoBehaviour
{
	public TUIButtonSelect btn_role_prefab;

	public Transform roles;

	public TUILabel label_skill_introduce;

	public TUILabel label_skill_name;

	public LevelStars level_stars;

	public Btn_BuySkill btn_click;

	public UnlockBlink unlock_blink;

	private TUIButtonSelect[] btn_role_list;

	public Top_Bar top_bar;

	public PopupSkillUpdate popup_skill_update;

	public ScrollSkill scroll_skill;

	private TUIScrollListEx scroll_list_ex_now;

	private ScrollList_SkillItem item_choose;

	private void Start()
	{
	}

	private void Update()
	{
		CheckScrollChoose();
	}

	public void SetInfo(ScrollList_SkillItem m_item)
	{
		label_skill_name.Text = m_item.GetSkillName();
		bool skillUnlock = m_item.GetSkillUnlock();
		int skillLevel = m_item.GetSkillLevel();
		if (m_item.GetSkillActive())
		{
			label_skill_introduce.Text = m_item.GetSkillActiveIntroduce();
			btn_click.SetStateDisable();
			level_stars.SetStarsDisable();
			return;
		}
		label_skill_introduce.Text = m_item.GetSkillIntroduceEx();
		float x = label_skill_name.CalculateBounds(label_skill_name.Text).size.x;
		Vector3 position = new Vector3(label_skill_name.transform.localPosition.x + x + 10f, label_skill_name.transform.localPosition.y, label_skill_name.transform.localPosition.z);
		level_stars.SetStars(m_item.GetSkillLevel(), position);
		if (!skillUnlock)
		{
			if (m_item.GetSkillUnlockPrice() == null)
			{
				Debug.Log("error!");
			}
			else
			{
				btn_click.SetStateUnlock(m_item.GetSkillUnlockPrice().price, m_item.GetSkillUnlockPrice().unit_type);
			}
		}
		else if (skillLevel == 0)
		{
			if (m_item.GetSkillBuyPrice() == null)
			{
				Debug.Log("error!");
			}
			else
			{
				btn_click.SetStateBuy(m_item.GetSkillBuyPrice().price, m_item.GetSkillBuyPrice().unit_type);
			}
		}
		else if (skillLevel < 5)
		{
			btn_click.SetStateUpdate();
		}
		else
		{
			btn_click.SetStateDisable();
		}
	}

	public Btn_BuySkill.StateButtonSkill GetStateBtnSkill()
	{
		return btn_click.GetStateBtnSkill();
	}

	public void CheckScrollChoose()
	{
		TUIScrollListEx scrollListChoose = scroll_skill.GetScrollListChoose();
		if (scroll_list_ex_now != scrollListChoose)
		{
			if (scroll_list_ex_now != null)
			{
				if (item_choose != null)
				{
					item_choose.DoUnChoose();
				}
				scroll_list_ex_now.GetComponent<ScrollList_Skill>().ResetPosition();
			}
			scroll_list_ex_now = scrollListChoose;
		}
		ScrollList_SkillItem itemChoose = scroll_skill.GetItemChoose();
		if (item_choose != itemChoose)
		{
			item_choose = itemChoose;
			if (item_choose != null)
			{
				SetInfo(item_choose);
			}
		}
	}

	public void ShowSkillUpdate()
	{
		popup_skill_update.ShowSkillUpdate();
		popup_skill_update.SetInfo(item_choose);
	}

	public void CloseSkillUpdate()
	{
		popup_skill_update.HideSkillUpdate();
	}

	public void ScrollListChoose(int index)
	{
		scroll_skill.ScrollListChoose(index);
	}

	public int GetScrollListIndex()
	{
		return scroll_list_ex_now.GetComponent<ScrollList_Skill>().GetIndex();
	}

	public int GetSkillID()
	{
		return item_choose.GetSkillID();
	}

	public void AddScrollList(TUIAllSkillInfo role_skill_info, GameObject go_invoke)
	{
		if (role_skill_info == null || role_skill_info.all_role_skill_list == null)
		{
			Debug.Log("no skill!");
			return;
		}
		scroll_skill.AddScrollList(role_skill_info);
		btn_role_list = new TUIButtonSelect[role_skill_info.all_role_skill_list.Length];
		for (int i = 0; i < role_skill_info.all_role_skill_list.Length; i++)
		{
			TUIButtonSelect tUIButtonSelect = (TUIButtonSelect)Object.Instantiate(btn_role_prefab);
			tUIButtonSelect.transform.parent = roles;
			tUIButtonSelect.transform.localPosition = new Vector3(0 + i * 48, 0f, 0f);
			tUIButtonSelect.GetComponent<PopupSkillBtnRole>().SetIndex(i + 1);
			tUIButtonSelect.GetComponent<PopupSkillBtnRole>().SetTexture(role_skill_info.all_role_skill_list[i].id);
			tUIButtonSelect.invokeObject = go_invoke;
			btn_role_list[i] = tUIButtonSelect;
		}
		btn_role_list[0].SetSelected(true);
	}

	public int GetSkillLevel()
	{
		return item_choose.GetSkillLevel();
	}

	public void SkillUnlock()
	{
		if (item_choose.GetSkillUnlockPrice() == null)
		{
			Debug.Log("error!");
		}
		int price = item_choose.GetSkillUnlockPrice().price;
		UnitType unit_type = item_choose.GetSkillUnlockPrice().unit_type;
		int num = 0;
		switch (unit_type)
		{
		case UnitType.Gold:
			num = top_bar.GetGoldValue();
			num -= price;
			if (num >= 0)
			{
				top_bar.SetGoldValue(num);
				item_choose.SkillUnlock();
				if (item_choose.GetSkillBuyPrice() != null)
				{
					btn_click.SetStateBuy(item_choose.GetSkillBuyPrice().price, item_choose.GetSkillBuyPrice().unit_type);
				}
				else
				{
					Debug.Log("error!");
				}
				label_skill_introduce.Text = item_choose.GetSkillIntroduceEx();
			}
			else
			{
				Debug.Log("!!!you have no gold enough!!!");
			}
			break;
		case UnitType.Crystal:
			num = top_bar.GetCrystalValue();
			num -= price;
			if (num >= 0)
			{
				top_bar.SetCrystalValue(num);
				item_choose.SkillUnlock();
				if (item_choose.GetSkillBuyPrice() != null)
				{
					btn_click.SetStateBuy(item_choose.GetSkillBuyPrice().price, item_choose.GetSkillBuyPrice().unit_type);
				}
				else
				{
					Debug.Log("error!");
				}
				label_skill_introduce.Text = item_choose.GetSkillIntroduceEx();
			}
			else
			{
				Debug.Log("!!!you have no crystal enough!!!");
			}
			break;
		}
	}

	public void SkillBuy()
	{
		if (item_choose.GetSkillBuyPrice() == null)
		{
			Debug.Log("error!");
			return;
		}
		int price = item_choose.GetSkillBuyPrice().price;
		UnitType unit_type = item_choose.GetSkillBuyPrice().unit_type;
		int num = 0;
		switch (unit_type)
		{
		case UnitType.Gold:
			num = top_bar.GetGoldValue();
			num -= price;
			if (num >= 0)
			{
				top_bar.SetGoldValue(num);
				item_choose.SkillBuy();
				float x2 = label_skill_name.CalculateBounds(label_skill_name.Text).size.x;
				Vector3 position2 = new Vector3(label_skill_name.transform.localPosition.x + x2 + 10f, label_skill_name.transform.localPosition.y, label_skill_name.transform.localPosition.z);
				level_stars.SetStars(item_choose.GetSkillLevel(), position2, item_choose.GetSkillLevel());
				btn_click.SetStateUpdate();
				unlock_blink.OpenBlink(item_choose.GetCustomizeTexture());
				label_skill_introduce.Text = item_choose.GetSkillIntroduceEx();
			}
			else
			{
				Debug.Log("!!!you have no gold enough!!!");
			}
			break;
		case UnitType.Crystal:
			num = top_bar.GetCrystalValue();
			num -= price;
			if (num >= 0)
			{
				top_bar.SetCrystalValue(num);
				item_choose.SkillBuy();
				float x = label_skill_name.CalculateBounds(label_skill_name.Text).size.x;
				Vector3 position = new Vector3(label_skill_name.transform.localPosition.x + x + 10f, label_skill_name.transform.localPosition.y, label_skill_name.transform.localPosition.z);
				level_stars.SetStars(item_choose.GetSkillLevel(), position, item_choose.GetSkillLevel());
				btn_click.SetStateUpdate();
				unlock_blink.OpenBlink(item_choose.GetCustomizeTexture());
				label_skill_introduce.Text = item_choose.GetSkillIntroduceEx();
			}
			else
			{
				Debug.Log("!!!you have no crystal enough!!!");
			}
			break;
		}
	}

	public void SkillUpdate()
	{
		if (!item_choose.ReachLevelMax())
		{
			if (item_choose.GetSkillBuyPrice() == null)
			{
				Debug.Log("error!");
				return;
			}
			int price = item_choose.GetSkillUpdatePrice().price;
			UnitType unit_type = item_choose.GetSkillUpdatePrice().unit_type;
			int num = 0;
			switch (unit_type)
			{
			case UnitType.Gold:
				num = top_bar.GetGoldValue();
				num -= price;
				Debug.Log("coss:" + price + unit_type.ToString());
				if (num >= 0)
				{
					top_bar.SetGoldValue(num);
					item_choose.SkillUpdate();
					float x2 = label_skill_name.CalculateBounds(label_skill_name.Text).size.x;
					Vector3 position2 = new Vector3(label_skill_name.transform.localPosition.x + x2 + 10f, label_skill_name.transform.localPosition.y, label_skill_name.transform.localPosition.z);
					level_stars.SetStars(item_choose.GetSkillLevel(), position2, item_choose.GetSkillLevel());
					if (item_choose.ReachLevelMax())
					{
						btn_click.SetStateDisable();
					}
					label_skill_introduce.Text = item_choose.GetSkillIntroduceEx();
				}
				else
				{
					Debug.Log("!!!you have no gold enough!!!");
				}
				break;
			case UnitType.Crystal:
				num = top_bar.GetCrystalValue();
				num -= price;
				Debug.Log("coss:" + price + unit_type.ToString());
				if (num >= 0)
				{
					top_bar.SetCrystalValue(num);
					item_choose.SkillUpdate();
					float x = label_skill_name.CalculateBounds(label_skill_name.Text).size.x;
					Vector3 position = new Vector3(label_skill_name.transform.localPosition.x + x + 10f, label_skill_name.transform.localPosition.y, label_skill_name.transform.localPosition.z);
					level_stars.SetStars(item_choose.GetSkillLevel(), position, item_choose.GetSkillLevel());
					if (item_choose.ReachLevelMax())
					{
						btn_click.SetStateDisable();
					}
					label_skill_introduce.Text = item_choose.GetSkillIntroduceEx();
				}
				else
				{
					Debug.Log("!!!you have no crystal enough!!!");
				}
				break;
			}
		}
		else
		{
			Debug.Log("you reach max level!");
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
	}

	public void CloseBlink()
	{
		unlock_blink.CloseBlink();
	}
}
