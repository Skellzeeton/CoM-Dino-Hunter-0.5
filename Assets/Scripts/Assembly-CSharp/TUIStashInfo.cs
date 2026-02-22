using System.Collections.Generic;

public class TUIStashInfo
{
	public int level;

	public TUIStashUpdateInfo[] stash_update_info;

	public List<TUIGoodsInfo> goods_info_list;

	public TUIStashInfo()
	{
	}

	public TUIStashInfo(int m_level, TUIStashUpdateInfo[] m_stash, List<TUIGoodsInfo> m_goods_info_list)
	{
		level = m_level;
		stash_update_info = m_stash;
		goods_info_list = m_goods_info_list;
	}

	public TUIStashUpdateInfo GetStashLevelInfo(int m_level)
	{
		for (int i = 0; i < stash_update_info.Length; i++)
		{
			if (stash_update_info[i].level == m_level)
			{
				return stash_update_info[i];
			}
		}
		return null;
	}

	public TUIStashUpdateInfo GetStashLevelInfo()
	{
		for (int i = 0; i < stash_update_info.Length; i++)
		{
			if (stash_update_info[i].level == level)
			{
				return stash_update_info[i];
			}
		}
		return null;
	}

	public int GetNowCapacity()
	{
		int num = 0;
		for (int i = 0; i < goods_info_list.Count; i++)
		{
			num += goods_info_list[i].count;
		}
		return num;
	}
}
