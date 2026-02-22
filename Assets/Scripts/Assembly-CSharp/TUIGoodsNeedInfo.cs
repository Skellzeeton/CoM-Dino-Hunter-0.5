public class TUIGoodsNeedInfo
{
	public int goods_id;

	public GoodsQualityType goods_quality;

	public int need_count;

	public TUIGoodsNeedInfo(int id, GoodsQualityType m_goods_quality, int count)
	{
		goods_id = id;
		need_count = count;
		goods_quality = m_goods_quality;
	}
}
