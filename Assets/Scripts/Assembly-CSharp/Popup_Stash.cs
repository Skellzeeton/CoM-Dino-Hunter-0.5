using System.Collections.Generic;
using UnityEngine;

public class Popup_Stash : MonoBehaviour
{
	public Top_Bar top_bar;

	public Popup_Capacity_Stash go_capacity_add;

	public TUILabel label_capacity;

	public PageFrame_Stash page_frame;

	public TUILabel label_name;

	public TUILabel label_price;

	public TUIMeshSprite img_price;

	public Sell_Stash go_sell;

	public Btn_Search_Sell btn_search;

	public Btn_Search_Sell btn_sell;

	private Btn_Select_Stash goods_control;

	private bool btn_sell_enable = true;

	private TUIStashInfo stash_info;

	private string gold_texture = "title_jingbi";

	private string crystal_texture = "title_shuijing";

	private void Awake()
	{
	}

	private void Start()
	{
		SetPriceTextNull();
		SetSellBtnEnableEx(false);
	}

	private void Update()
	{
	}

	public void ShowCapacityAdd()
	{
		if (stash_info == null || stash_info.stash_update_info == null)
		{
			Debug.Log("error!");
			return;
		}
		int level = stash_info.level;
		int num = stash_info.stash_update_info.Length;
		int nowCapacity = stash_info.GetNowCapacity();
		if (level >= num)
		{
			Debug.Log("You reach max level!");
			return;
		}
		Debug.Log("You open capacity add!");
		TUIStashUpdateInfo stashLevelInfo = stash_info.GetStashLevelInfo();
		int max_capacity = stashLevelInfo.max_capacity;
		TUIPriceInfo price_info = stashLevelInfo.price_info;
		if (price_info == null)
		{
			Debug.Log("error!");
			return;
		}
		string introduce = stashLevelInfo.introduce;
		go_capacity_add.SetInfo(nowCapacity, max_capacity, price_info, introduce);
		go_capacity_add.transform.localPosition = new Vector3(0f, 0f, go_capacity_add.transform.localPosition.z);
		go_capacity_add.GetComponent<Animation>().Play();
	}

	public void CloseCapacityAdd()
	{
		Debug.Log("You close capacity add");
		go_capacity_add.transform.localPosition = new Vector3(0f, -1000f, go_capacity_add.transform.localPosition.z);
	}

	public void ShowSell()
	{
		Debug.Log("You open sell");
		if (btn_sell_enable)
		{
			go_sell.transform.localPosition = new Vector3(0f, 0f, go_sell.transform.localPosition.z);
			go_sell.GetComponent<Animation>().Play();
		}
	}

	public void HideSell()
	{
		Debug.Log("You close sell");
		go_sell.transform.localPosition = new Vector3(0f, -2000f, go_sell.transform.localPosition.z);
	}

	public void SetInfo(TUIStashInfo m_stash_info, GameObject go_invoke)
	{
		if (m_stash_info == null)
		{
			Debug.Log("error!");
			return;
		}
		stash_info = m_stash_info;
		int level = m_stash_info.level;
		int nowCapacity = m_stash_info.GetNowCapacity();
		TUIStashUpdateInfo stashLevelInfo = m_stash_info.GetStashLevelInfo();
		if (stashLevelInfo == null)
		{
			Debug.Log("error!");
			return;
		}
		int max_capacity = stashLevelInfo.max_capacity;
		label_capacity.Text = nowCapacity + "/" + max_capacity;
		AddPage(m_stash_info.goods_info_list, go_invoke);
	}

	public void AddCapacity()
	{
		if (stash_info == null)
		{
			Debug.Log("error!");
			return;
		}
		TUIStashUpdateInfo stashLevelInfo = stash_info.GetStashLevelInfo();
		if (stashLevelInfo == null)
		{
			Debug.Log("error!");
			return;
		}
		TUIPriceInfo price_info = stashLevelInfo.price_info;
		if (price_info == null)
		{
			Debug.Log("error!");
			return;
		}
		int level = stashLevelInfo.level;
		int price = price_info.price;
		UnitType unit_type = price_info.unit_type;
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
			top_bar.SetGoldValue(num);
			break;
		case UnitType.Crystal:
			num = top_bar.GetCrystalValue();
			num -= price;
			if (num < 0)
			{
				Debug.Log("you have no crystal enough!");
				return;
			}
			top_bar.SetCrystalValue(num);
			break;
		}
		stash_info.level++;
		int nowCapacity = stash_info.GetNowCapacity();
		int max_capacity = stash_info.GetStashLevelInfo().max_capacity;
		label_capacity.Text = nowCapacity + "/" + max_capacity;
	}

	public int GetAddCapacityPrice()
	{
		return 0;
	}

	public void AddPage(List<TUIGoodsInfo> goods_info_list, GameObject go_invoke)
	{
		page_frame.AddPage(goods_info_list, go_invoke);
	}

	public void SetPriceText(string m_name, TUIPriceInfo m_price)
	{
		if (m_price == null)
		{
			Debug.Log("error!");
			return;
		}
		label_name.Text = m_name;
		label_price.Text = m_price.price.ToString();
		if (m_price.unit_type == UnitType.Gold)
		{
			img_price.texture = gold_texture;
		}
		else if (m_price.unit_type == UnitType.Crystal)
		{
			img_price.texture = crystal_texture;
		}
	}

	public void SetPriceTextNull()
	{
		label_name.Text = string.Empty;
		label_price.Text = string.Empty;
		img_price.texture = string.Empty;
	}

	public void SetSellParam(int m_count, TUIPriceInfo m_price)
	{
		go_sell.SetParam(m_count, m_price);
	}

	public void SetSellParamNull()
	{
		go_sell.SetParamNull();
	}

	public void SetSellParamPlus(int m_plus)
	{
		go_sell.SetCountPlus(m_plus);
	}

	public void SetSellParamSubstract(int m_substract)
	{
		go_sell.SetCountSubstract(m_substract);
	}

	public Btn_Select_Stash GetGoodsControl()
	{
		return goods_control;
	}

	public int GetSellCount()
	{
		return go_sell.GetCountNow();
	}

	public void SetGoodsControl(Btn_Select_Stash control)
	{
		if (control != null)
		{
			if (goods_control != control)
			{
				goods_control = control;
				TUIGoodsInfo goodsInfo = goods_control.GetGoodsInfo();
				if (goodsInfo != null && goodsInfo.count != 0)
				{
					SetPriceText(goodsInfo.name, goodsInfo.price_info);
					SetSellParam(goodsInfo.count, goodsInfo.price_info);
					SetSellBtnEnableEx(true);
				}
				else
				{
					SetPriceTextNull();
					SetSellParamNull();
					SetSellBtnEnableEx(false);
				}
			}
		}
		else
		{
			goods_control = null;
			SetPriceTextNull();
			SetSellParamNull();
			SetSellBtnEnableEx(false);
		}
	}

	public void UpdateSellGoods()
	{
		if (go_sell == null)
		{
			Debug.Log("error!");
			return;
		}
		int totalPrice = go_sell.GetTotalPrice();
		UnitType unitType = go_sell.GetUnitType();
		int num = 0;
		switch (unitType)
		{
		case UnitType.Gold:
			num = top_bar.GetGoldValue();
			num += totalPrice;
			top_bar.SetGoldValue(num);
			break;
		case UnitType.Crystal:
			num = top_bar.GetCrystalValue();
			num += totalPrice;
			top_bar.SetCrystalValue(num);
			break;
		}
		TUIGoodsInfo goodsInfo = goods_control.GetGoodsInfo();
		int num2 = goodsInfo.count - GetSellCount();
		if (num2 > 0)
		{
			goods_control.SetSellInfo(num2);
			SetPriceText(goodsInfo.name, goodsInfo.price_info);
			SetSellParam(goodsInfo.count, goodsInfo.price_info);
		}
		else if (num2 == 0)
		{
			goods_control.SetSellInfo(num2);
			SetPriceTextNull();
			SetSellParamNull();
			SetSellBtnEnableEx(false);
		}
		else
		{
			Debug.Log("error!!!");
		}
	}

	public void SetSellBtnEnableEx(bool m_bool)
	{
		if (btn_sell_enable != m_bool)
		{
			btn_sell_enable = m_bool;
		}
		if (!m_bool)
		{
			btn_sell.SetStateDisable();
			btn_search.SetStateDisable();
		}
		else
		{
			btn_sell.SetStateNormal();
			btn_search.SetStateNormal();
		}
	}

	public void SetSearchBtnEnableEx(bool m_bool)
	{
		if (btn_sell_enable != m_bool)
		{
			btn_sell_enable = m_bool;
		}
		if (!m_bool)
		{
			btn_sell.SetStateDisable();
		}
		else
		{
			btn_sell.SetStateNormal();
		}
	}

	public void SetTopBarInfo(TUIPlayerInfo m_info)
	{
		if (m_info == null)
		{
			Debug.Log("error! no found info");
			return;
		}
		int level = m_info.level;
		int exp = m_info.exp;
		int level_exp = m_info.level_exp;
		int gold = m_info.gold;
		int crystal = m_info.crystal;
		top_bar.SetAllValue(level, exp, level_exp, gold, crystal);
	}
}
