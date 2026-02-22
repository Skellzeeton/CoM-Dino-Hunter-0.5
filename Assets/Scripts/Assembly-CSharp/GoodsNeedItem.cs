using UnityEngine;

public class GoodsNeedItem : MonoBehaviour
{
	public TUILabel label_goods_need;

	public GoodsNeedItemBuy btn_buy;

	public GoodsNeedItemImg img_goods;

	private Vector3 m_local_position;

	private int index;

	private int goods_id;

	private GoodsQualityType goods_need_quality;

	private int goods_now_count;

	private int goods_need_count;

	private int goods_price;

	private UnitType goods_gold_type;

	private void Start()
	{
		m_local_position = base.gameObject.transform.localPosition;
	}

	private void Update()
	{
	}

	public void ShowGoodsNeedItem(int m_goods_now_count, GoodsQualityType m_goods_need_quality, int m_goods_need_count, int m_price, int m_id, UnitType m_gold_type)
	{
		base.transform.localPosition = m_local_position;
		goods_id = m_id;
		goods_need_quality = m_goods_need_quality;
		goods_now_count = m_goods_now_count;
		goods_need_count = m_goods_need_count;
		goods_price = m_price;
		goods_gold_type = m_gold_type;
		string empty = string.Empty;
		empty = ((goods_now_count >= goods_need_count) ? "{color}{0}{color}/{1}" : "{color:FF0000FF}{0}{color}/{1}");
		string text = TUITool.StringFormat(empty, goods_now_count, goods_need_count);
		label_goods_need.Text = text;
		if (goods_now_count < goods_need_count)
		{
			btn_buy.SetInfo(goods_price, goods_id, m_goods_need_quality, goods_need_count - goods_now_count, m_gold_type);
		}
		else
		{
			btn_buy.HideInfo();
		}
		img_goods.SetInfo(goods_id, m_goods_need_quality);
	}

	public void HideGoodsNeedItem()
	{
		base.transform.localPosition = m_local_position + new Vector3(0f, -1000f, 0f);
	}

	public void SetDefaultParam()
	{
		goods_id = 0;
		goods_now_count = 0;
		goods_need_count = 0;
		goods_price = 0;
		label_goods_need.Text = string.Empty;
	}

	public void SetIndex(int m_index)
	{
		index = m_index;
		btn_buy.SetIndex(m_index);
	}

	public int GetIndex()
	{
		return index;
	}

	public void UpdateGoodsBuy()
	{
		ShowGoodsNeedItem(goods_need_count, goods_need_quality, goods_need_count, goods_price, goods_id, goods_gold_type);
	}

	public int GetGoodsID()
	{
		return goods_id;
	}

	public GoodsQualityType GetGoodsQuality()
	{
		return goods_need_quality;
	}

	public int GetGoodsNeedCount()
	{
		return goods_need_count;
	}
}
