public class CLevelUpWeapon
{
	protected iGameData m_GameData;

	protected iDataCenter m_DataCenter;

	protected CWeaponInfo m_pWeaponInfo;

	protected int m_nLevel;

	protected int m_nLevelNext;

	protected bool m_bCrystalTrade;

	public bool isCrystalTrade
	{
		get
		{
			return m_bCrystalTrade;
		}
	}

	public CLevelUpWeapon()
	{
		m_GameData = iGameApp.GetInstance().m_GameData;
		m_DataCenter = m_GameData.GetDataCenter();
		m_bCrystalTrade = false;
	}

	public bool Initialize(int nWeaponID)
	{
		m_pWeaponInfo = m_GameData.GetWeaponInfo(nWeaponID);
		if (m_pWeaponInfo == null)
		{
			return false;
		}
		m_nLevel = m_DataCenter.GetWeaponLevel(nWeaponID);
		m_nLevelNext = ((m_nLevel == -1) ? 1 : (m_nLevel + 1));
		return true;
	}

	public bool IsConditionMatch()
	{
		return true;
	}

	public bool IsMaterialsMatch()
	{
		if (m_pWeaponInfo == null)
		{
			return false;
		}
		CWeaponInfoLevel cWeaponInfoLevel = m_pWeaponInfo.Get(m_nLevelNext);
		if (cWeaponInfoLevel == null)
		{
			return false;
		}
		for (int i = 0; i < cWeaponInfoLevel.ltMaterials.Count && i < cWeaponInfoLevel.ltMaterialsCount.Count; i++)
		{
			if (m_DataCenter.GetMaterialNum(cWeaponInfoLevel.ltMaterials[i]) < cWeaponInfoLevel.ltMaterialsCount[i])
			{
				return false;
			}
		}
		return true;
	}

	public bool IsPriceMatch()
	{
		if (m_pWeaponInfo == null)
		{
			return false;
		}
		CWeaponInfoLevel cWeaponInfoLevel = m_pWeaponInfo.Get(m_nLevelNext);
		if (cWeaponInfoLevel == null)
		{
			return false;
		}
		if (cWeaponInfoLevel.isCrystalPurchase)
		{
			if (m_DataCenter.Crystal < cWeaponInfoLevel.nPurchasePrice)
			{
				return false;
			}
		}
		else if (m_DataCenter.Gold < cWeaponInfoLevel.nPurchasePrice)
		{
			return false;
		}
		return true;
	}

	public bool LevelUp()
	{
		if (!IsMaterialsMatch() || !IsPriceMatch())
		{
			return false;
		}
		if (m_pWeaponInfo == null)
		{
			return false;
		}
		CWeaponInfoLevel cWeaponInfoLevel = m_pWeaponInfo.Get(m_nLevelNext);
		if (cWeaponInfoLevel == null)
		{
			return false;
		}
		for (int i = 0; i < cWeaponInfoLevel.ltMaterials.Count && i < cWeaponInfoLevel.ltMaterialsCount.Count; i++)
		{
			m_DataCenter.AddMaterialNum(cWeaponInfoLevel.ltMaterials[i], -cWeaponInfoLevel.ltMaterialsCount[i]);
		}
		if (cWeaponInfoLevel.isCrystalPurchase)
		{
			m_DataCenter.AddCrystal(-cWeaponInfoLevel.nPurchasePrice);
			m_bCrystalTrade = true;
		}
		else
		{
			m_DataCenter.AddGold(-cWeaponInfoLevel.nPurchasePrice);
			m_bCrystalTrade = false;
		}
		m_DataCenter.SetWeaponLevel(m_pWeaponInfo.nID, m_nLevelNext);
		m_DataCenter.Save();
		return true;
	}
}
