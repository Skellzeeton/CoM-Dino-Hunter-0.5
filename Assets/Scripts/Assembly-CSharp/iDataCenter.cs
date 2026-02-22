using System.Collections.Generic;
using System;
using System.IO;
using System.Xml;
using UnityEngine;

public class iDataCenter
{
	protected float m_fVersion;

	protected int m_nGold;

	protected int m_nCrystal;

	protected int m_nStashLevel;

	protected int m_nStashCount;

	protected Dictionary<int, int> m_dictMaterials;

	protected Dictionary<int, int> m_dictWeapon;

	protected Dictionary<int, int> m_dictEquipStone;

	protected Dictionary<int, int> m_dictPassiveSkill;

	protected Dictionary<int, CCharSaveInfo> m_dictCharSaveInfo;

	protected List<int> m_ltLevelList;

	protected int m_nCurCharID;

	protected int[] m_arrSelectWeapon;

	protected int[] m_arrSelectPassiveSkill;

	protected int m_nCurEquipStone;

	protected int m_nLatestLevel;

	protected bool m_bUnLockLevel;
	
	private string GetSaveFilePath()
	{
		return Application.persistentDataPath + "/gamedata.xml";
	}

	public int Gold
	{
		get
		{
			return m_nGold;
		}
	}

	public int Crystal
	{
		get
		{
			return m_nCrystal;
		}
	}

	public int CurCharID
	{
		get
		{
			return m_nCurCharID;
		}
		set
		{
			m_nCurCharID = value;
		}
	}

	public int CurEquipStone
	{
		get
		{
			return m_nCurEquipStone;
		}
		set
		{
			m_nCurEquipStone = value;
		}
	}

	public int LatestLevel
	{
		get
		{
			return m_nLatestLevel;
		}
		set
		{
			m_nLatestLevel = value;
		}
	}

	public int StashLevel
	{
		get
		{
			return m_nStashLevel;
		}
		set
		{
			m_nStashLevel = value;
		}
	}

	public int StashCount
	{
		get
		{
			return m_nStashCount;
		}
	}

	public int StashCountMax
	{
		get
		{
			iGameData gameData = iGameApp.GetInstance().m_GameData;
			if (gameData == null)
			{
				return 0;
			}
			return gameData.GetStashCapacityCount(m_nStashLevel);
		}
	}

	public iDataCenter()
	{
		m_nGold = 0;
		m_nCrystal = 0;
		m_nStashLevel = 1;
		m_nStashCount = 0;
		m_dictMaterials = new Dictionary<int, int>();
		m_dictWeapon = new Dictionary<int, int>();
		m_dictEquipStone = new Dictionary<int, int>();
		m_dictPassiveSkill = new Dictionary<int, int>();
		m_dictCharSaveInfo = new Dictionary<int, CCharSaveInfo>();
		m_nCurCharID = 1;
		m_arrSelectWeapon = new int[3] { 2, 1, -1 };
		m_arrSelectPassiveSkill = new int[3] { -1, -1, -1 };
		m_nCurEquipStone = 0;
		m_ltLevelList = new List<int>();
		for (int i = 1001; i <= 1024; i++)
		{
			m_ltLevelList.Add(i);
		}
		m_nLatestLevel = 1001;
		m_bUnLockLevel = false;
	}

public bool Load()
{
    string content = string.Empty;
    string savePath = GetSaveFilePath();

    try
    {
        if (File.Exists(savePath))
        {
            content = File.ReadAllText(savePath);
        }
        else
        {
            string[] legacyCandidates = new string[]
            {
	            Application.dataPath + "/gamedata.xml",
	            Application.streamingAssetsPath + "/gamedata.xml",
            };

            foreach (var candidate in legacyCandidates)
            {
                if (File.Exists(candidate))
                {
                    try
                    {
                        content = File.ReadAllText(candidate);
                        try
                        {
	                        string folder = Application.persistentDataPath;

	                        if (!Directory.Exists(folder))
	                        {
		                        Directory.CreateDirectory(folder);
	                        }
                            string tmp = savePath + ".tmp";
                            File.WriteAllText(tmp, content);
                            if (File.Exists(savePath)) File.Delete(savePath);
                            File.Move(tmp, savePath);
                            Debug.Log("[iDataCenter] Migrated save from " + candidate + " -> " + savePath);
                        }
                        catch (Exception exM)
                        {
                            Debug.LogWarning("[iDataCenter] Migration to persistent path failed: " + exM);
                        }
                        break;
                    }
                    catch (Exception exRead)
                    {
                        Debug.LogWarning("[iDataCenter] Failed reading legacy save " + candidate + " : " + exRead);
                    }
                }
            }
        }

        if (string.IsNullOrEmpty(content))
        {
            SetCharacter(1, 1, 0);
            SetWeaponLevel(1, 1);
            SetWeaponLevel(2, 1);
            SetEquipStone(10001, 1);
            Save();
            return false;
        }
        content = XXTEAUtils.Decrypt(content, iGameApp.GetInstance().GetKey());
        XmlDocument xmlDocument = new XmlDocument();
        xmlDocument.LoadXml(content);
        string value = string.Empty;
        XmlNode documentElement = xmlDocument.DocumentElement;
        if (documentElement == null)
        {
            Debug.LogWarning("[iDataCenter] gamedata.xml missing root element.");
            return false;
        }
        if (MyUtils.GetAttribute(documentElement, "gold", ref value))
        {
            m_nGold = int.Parse(value);
        }
        if (MyUtils.GetAttribute(documentElement, "crystal", ref value))
        {
            m_nCrystal = int.Parse(value);
        }
        if (MyUtils.GetAttribute(documentElement, "stashlevel", ref value))
        {
            m_nStashLevel = int.Parse(value);
        }
        if (MyUtils.GetAttribute(documentElement, "latestlevel", ref value))
        {
            m_nLatestLevel = int.Parse(value);
        }
        if (MyUtils.GetAttribute(documentElement, "isunlocklevel", ref value))
        {
            m_bUnLockLevel = bool.Parse(value);
        }

        foreach (XmlNode item in documentElement)
        {
            if (item.Name == "character")
            {
                if (MyUtils.GetAttribute(item, "select", ref value))
                {
                    m_nCurCharID = int.Parse(value);
                }
                foreach (XmlNode item2 in item)
                {
                    if (item2.Name == "node" && MyUtils.GetAttribute(item2, "id", ref value))
                    {
                        int nCharID = int.Parse(value);
                        int nLevel = 1;
                        int nExp = 0;
                        if (MyUtils.GetAttribute(item2, "level", ref value))
                        {
                            nLevel = int.Parse(value);
                        }
                        if (MyUtils.GetAttribute(item2, "exp", ref value))
                        {
                            nExp = int.Parse(value);
                        }
                        SetCharacter(nCharID, nLevel, nExp);
                    }
                }
            }
            else if (item.Name == "weapon")
            {
                if (MyUtils.GetAttribute(item, "select", ref value))
                {
                    string[] array = value.Split(',');
                    for (int i = 0; i < array.Length && i < m_arrSelectWeapon.Length; i++)
                    {
                        int.TryParse(array[i], out m_arrSelectWeapon[i]);
                    }
                }
                foreach (XmlNode item3 in item)
                {
                    if (item3.Name == "node" && MyUtils.GetAttribute(item3, "id", ref value))
                    {
                        int nWeaponID = int.Parse(value);
                        int nWeaponLevel = 0;
                        if (MyUtils.GetAttribute(item3, "level", ref value))
                        {
                            nWeaponLevel = int.Parse(value);
                        }
                        SetWeaponLevel(nWeaponID, nWeaponLevel);
                    }
                }
            }
            else if (item.Name == "skill")
            {
                if (MyUtils.GetAttribute(item, "select", ref value))
                {
                    string[] array = value.Split(',');
                    for (int j = 0; j < array.Length && j < m_arrSelectPassiveSkill.Length; j++)
                    {
                        int.TryParse(array[j], out m_arrSelectPassiveSkill[j]);
                    }
                }
                foreach (XmlNode item4 in item)
                {
                    if (item4.Name == "node" && MyUtils.GetAttribute(item4, "id", ref value))
                    {
                        int nSkillID = int.Parse(value);
                        int nLevel2 = 0;
                        if (MyUtils.GetAttribute(item4, "level", ref value))
                        {
                            nLevel2 = int.Parse(value);
                        }
                        SetPassiveSkill(nSkillID, nLevel2);
                    }
                }
            }
            else if (item.Name == "equipstone")
            {
                if (MyUtils.GetAttribute(item, "select", ref value))
                {
                    m_nCurEquipStone = int.Parse(value);
                }
                foreach (XmlNode item5 in item)
                {
                    if (item5.Name == "node" && MyUtils.GetAttribute(item5, "id", ref value))
                    {
                        int nItemID = int.Parse(value);
                        int nLevel3 = 0;
                        if (MyUtils.GetAttribute(item5, "level", ref value))
                        {
                            nLevel3 = int.Parse(value);
                        }
                        SetEquipStone(nItemID, nLevel3);
                    }
                }
            }
            else if (item.Name == "materials")
            {
                foreach (XmlNode item6 in item)
                {
                    if (item6.Name == "node" && MyUtils.GetAttribute(item6, "id", ref value))
                    {
                        int nItemID2 = int.Parse(value);
                        int nCount = 0;
                        if (MyUtils.GetAttribute(item6, "count", ref value))
                        {
                            nCount = int.Parse(value);
                        }
                        SetMaterialNum(nItemID2, nCount);
                    }
                }
            }
        }

        return true;
    }
    catch (Exception ex)
    {
        Debug.LogError("[iDataCenter] Failed to load gamedata: " + ex.Message + "\n" + ex.StackTrace);
        try
        {
            SetCharacter(1, 1, 0);
            SetWeaponLevel(1, 1);
            SetWeaponLevel(2, 1);
            SetEquipStone(10001, 1);
            Save();
        }
        catch (Exception ex2)
        {
            Debug.LogError("[iDataCenter] Failed to create default save: " + ex2);
        }
        return false;
    }
}

public void Save()
{
	XmlDocument xmlDocument = new XmlDocument();

	try
	{
        XmlNode newChild = xmlDocument.CreateXmlDeclaration("1.0", "UTF-8", "no");
        xmlDocument.AppendChild(newChild);
        string empty = string.Empty;
        XmlElement xmlElement = xmlDocument.CreateElement("gamedata");
        xmlDocument.AppendChild(xmlElement);
        xmlElement.SetAttribute("gold", m_nGold.ToString());
        xmlElement.SetAttribute("crystal", m_nCrystal.ToString());
        xmlElement.SetAttribute("stashlevel", m_nStashLevel.ToString());
        xmlElement.SetAttribute("latestlevel", m_nLatestLevel.ToString());
        xmlElement.SetAttribute("isunlocklevel", m_bUnLockLevel.ToString());
        XmlElement xmlElement2 = xmlDocument.CreateElement("character");
        xmlElement.AppendChild(xmlElement2);
        xmlElement2.SetAttribute("select", m_nCurCharID.ToString());
        foreach (CCharSaveInfo value in m_dictCharSaveInfo.Values)
        {
            XmlElement xmlElement3 = xmlDocument.CreateElement("node");
            xmlElement2.AppendChild(xmlElement3);
            xmlElement3.SetAttribute("id", value.nID.ToString());
            xmlElement3.SetAttribute("level", value.nLevel.ToString());
            xmlElement3.SetAttribute("exp", value.nExp.ToString());
        }
        XmlElement xmlElement4 = xmlDocument.CreateElement("weapon");
        xmlElement.AppendChild(xmlElement4);
        empty = string.Empty;
        int[] arrSelectWeapon = m_arrSelectWeapon;
        for (int i = 0; i < arrSelectWeapon.Length; i++)
        {
            int num = arrSelectWeapon[i];
            empty = ((empty.Length >= 1) ? (empty + "," + num) : num.ToString());
        }
        xmlElement4.SetAttribute("select", empty);
        foreach (KeyValuePair<int, int> item in m_dictWeapon)
        {
            XmlElement xmlElement5 = xmlDocument.CreateElement("node");
            xmlElement4.AppendChild(xmlElement5);
            xmlElement5.SetAttribute("id", item.Key.ToString());
            xmlElement5.SetAttribute("level", item.Value.ToString());
        }
        XmlElement xmlElement6 = xmlDocument.CreateElement("skill");
        xmlElement.AppendChild(xmlElement6);
        empty = string.Empty;
        int[] arrSelectPassiveSkill = m_arrSelectPassiveSkill;
        for (int j = 0; j < arrSelectPassiveSkill.Length; j++)
        {
            int num2 = arrSelectPassiveSkill[j];
            empty = ((empty.Length >= 1) ? (empty + "," + num2) : num2.ToString());
        }
        xmlElement6.SetAttribute("select", empty);
        foreach (KeyValuePair<int, int> item2 in m_dictPassiveSkill)
        {
            XmlElement xmlElement7 = xmlDocument.CreateElement("node");
            xmlElement6.AppendChild(xmlElement7);
            xmlElement7.SetAttribute("id", item2.Key.ToString());
            xmlElement7.SetAttribute("level", item2.Value.ToString());
        }
        XmlElement xmlElement8 = xmlDocument.CreateElement("equipstone");
        xmlElement.AppendChild(xmlElement8);
        xmlElement8.SetAttribute("select", m_nCurEquipStone.ToString());
        foreach (KeyValuePair<int, int> item3 in m_dictEquipStone)
        {
            XmlElement xmlElement9 = xmlDocument.CreateElement("node");
            xmlElement8.AppendChild(xmlElement9);
            xmlElement9.SetAttribute("id", item3.Key.ToString());
            xmlElement9.SetAttribute("level", item3.Value.ToString());
        }
        XmlElement xmlElement10 = xmlDocument.CreateElement("materials");
        xmlElement.AppendChild(xmlElement10);
        foreach (KeyValuePair<int, int> dictMaterial in m_dictMaterials)
        {
            if (dictMaterial.Value != 0)
            {
                XmlElement xmlElement11 = xmlDocument.CreateElement("node");
                xmlElement10.AppendChild(xmlElement11);
                xmlElement11.SetAttribute("id", dictMaterial.Key.ToString());
                xmlElement11.SetAttribute("count", dictMaterial.Value.ToString());
            }
        }
        StringWriter stringWriter = new StringWriter();
        xmlDocument.Save(stringWriter);
        string content = XXTEAUtils.Encrypt(stringWriter.ToString(), iGameApp.GetInstance().GetKey());

        string savePath = GetSaveFilePath();
        string saveDir = Application.persistentDataPath;

        if (!Directory.Exists(saveDir))
        {
	        Directory.CreateDirectory(saveDir);
        }
        string tmpPath = savePath + ".tmp";
        File.WriteAllText(tmpPath, content);
        if (File.Exists(savePath))
        {
            File.Delete(savePath);
        }
        File.Move(tmpPath, savePath);

        Debug.Log("[iDataCenter] Save successful: " + savePath);
    }
    catch (Exception ex)
    {
        Debug.LogError("[iDataCenter] Failed to save gamedata: " + ex.Message + "\n" + ex.StackTrace);
        try
        {
            string fallbackPath = Application.dataPath + "/gamedata.xml";
            File.WriteAllText(fallbackPath, XXTEAUtils.Encrypt(xmlDocument.OuterXml, iGameApp.GetInstance().GetKey()));
            Debug.LogWarning("[iDataCenter] Wrote fallback save to " + fallbackPath);
        }
        catch (Exception ex2)
        {
            Debug.LogError("[iDataCenter] Fallback save also failed: " + ex2);
        }
    }
}

	public List<int> GetLevelList()
	{
		return m_ltLevelList;
	}

	public Dictionary<int, int> GetMaterialData()
	{
		return m_dictMaterials;
	}

	public Dictionary<int, int> GetWeaponData()
	{
		return m_dictWeapon;
	}

	public CCharSaveInfo GetCharacter(int nCharID)
	{
		if (!m_dictCharSaveInfo.ContainsKey(nCharID))
		{
			return null;
		}
		return m_dictCharSaveInfo[nCharID];
	}

	public bool GetPassiveSkill(int nSkillID, ref int nSkillLevel)
	{
		if (!m_dictPassiveSkill.ContainsKey(nSkillID))
		{
			return false;
		}
		nSkillLevel = m_dictPassiveSkill[nSkillID];
		return true;
	}

	public bool GetEquipStone(int nItemID, ref int nItemLevel)
	{
		if (!m_dictEquipStone.ContainsKey(nItemID))
		{
			return false;
		}
		nItemLevel = m_dictEquipStone[nItemID];
		return true;
	}

	public int GetWeaponLevel(int nWeaponID)
	{
		if (!m_dictWeapon.ContainsKey(nWeaponID))
		{
			return -1;
		}
		return m_dictWeapon[nWeaponID];
	}

	public int GetSelectWeapon(int nIndex)
	{
		if (nIndex < 0 || nIndex >= m_arrSelectWeapon.Length)
		{
			return -1;
		}
		return m_arrSelectWeapon[nIndex];
	}

	public int GetSelectPassiveSkill(int nIndex)
	{
		if (nIndex < 0 || nIndex >= m_arrSelectPassiveSkill.Length)
		{
			return -1;
		}
		return m_arrSelectPassiveSkill[nIndex];
	}

	public int GetMaterialNum(int nItemID)
	{
		if (!m_dictMaterials.ContainsKey(nItemID))
		{
			return -1;
		}
		return m_dictMaterials[nItemID];
	}

	public void AddMaterialNum(int nItemID, int nCount)
	{
		if (!m_dictMaterials.ContainsKey(nItemID))
		{
			m_dictMaterials.Add(nItemID, nCount);
		}
		else
		{
			Dictionary<int, int> dictMaterials;
			Dictionary<int, int> dictionary = (dictMaterials = m_dictMaterials);
			int key2;
			int key = (key2 = nItemID);
			key2 = dictMaterials[key2];
			dictionary[key] = key2 + nCount;
		}
		AddStashCount(nCount);
	}

	public void SetMaterialNum(int nItemID, int nCount)
	{
		if (!m_dictMaterials.ContainsKey(nItemID))
		{
			m_dictMaterials.Add(nItemID, nCount);
		}
		else
		{
			m_dictMaterials[nItemID] = nCount;
		}
		AddStashCount(nCount);
	}

	public int CheckStashVolume(int nCount)
	{
		int stashCountMax = StashCountMax;
		if (m_nStashCount + nCount > stashCountMax)
		{
			return stashCountMax - m_nStashCount;
		}
		return nCount;
	}

	public void AddStashCount(int nCount)
	{
		m_nStashCount += nCount;
	}

	public void SetWeaponLevel(int nWeaponID, int nWeaponLevel)
	{
		if (!m_dictWeapon.ContainsKey(nWeaponID))
		{
			m_dictWeapon.Add(nWeaponID, nWeaponLevel);
		}
		else
		{
			m_dictWeapon[nWeaponID] = nWeaponLevel;
		}
	}

	public void SetCharacter(int nCharID, int nLevel, int nExp)
	{
		if (!m_dictCharSaveInfo.ContainsKey(nCharID))
		{
			m_dictCharSaveInfo.Add(nCharID, new CCharSaveInfo(nCharID));
		}
		m_dictCharSaveInfo[nCharID].nLevel = nLevel;
		m_dictCharSaveInfo[nCharID].nExp = nExp;
	}

	public void UnlockCharacter(int nCharID)
	{
		if (!m_dictCharSaveInfo.ContainsKey(nCharID))
		{
			m_dictCharSaveInfo.Add(nCharID, new CCharSaveInfo(nCharID));
			m_dictCharSaveInfo[nCharID].nLevel = -1;
			m_dictCharSaveInfo[nCharID].nExp = 0;
		}
	}

	public void SetPassiveSkill(int nSkillID, int nLevel)
	{
		if (!m_dictPassiveSkill.ContainsKey(nSkillID))
		{
			m_dictPassiveSkill.Add(nSkillID, nLevel);
		}
		m_dictPassiveSkill[nSkillID] = nLevel;
	}

	public void UnlockPassiveSkill(int nSkillID)
	{
		if (!m_dictPassiveSkill.ContainsKey(nSkillID))
		{
			m_dictPassiveSkill.Add(nSkillID, -1);
		}
	}

	public void SetEquipStone(int nItemID, int nLevel)
	{
		if (!m_dictEquipStone.ContainsKey(nItemID))
		{
			m_dictEquipStone.Add(nItemID, nLevel);
		}
		else
		{
			m_dictEquipStone[nItemID] = nLevel;
		}
	}

	public void UnlockEquipStone(int nItemID)
	{
		if (!m_dictEquipStone.ContainsKey(nItemID))
		{
			m_dictEquipStone.Add(nItemID, -1);
		}
	}

	public void AddGold(int nGold)
	{
		m_nGold += nGold;
		if (m_nGold < 0)
		{
			m_nGold = 0;
		}
	}

	public void AddCrystal(int nCrystal)
	{
		m_nCrystal += nCrystal;
		if (m_nCrystal < 0)
		{
			m_nCrystal = 0;
		}
	}

	public void SetSelectWeapon(int nIndex, int nWeaponID)
	{
		if (nIndex >= 0 && nIndex < m_arrSelectWeapon.Length)
		{
			m_arrSelectWeapon[nIndex] = nWeaponID;
		}
	}

	public void SetSelectPassiveSkill(int nIndex, int nPassiveSkillID)
	{
		if (nIndex >= 0 && nIndex < m_arrSelectPassiveSkill.Length)
		{
			m_arrSelectPassiveSkill[nIndex] = nPassiveSkillID;
		}
	}

	public void UnlockNewLevelPrepare()
	{
		m_bUnLockLevel = true;
	}

	public void UnlockNewLevelConfirm(int nNewLevel)
	{
		m_bUnLockLevel = false;
		m_nLatestLevel = nNewLevel;
	}

	public bool GetNewLevel(ref int nNewLevel)
	{
		if (!m_bUnLockLevel)
		{
			return false;
		}
		for (int i = 0; i < m_ltLevelList.Count - 1; i++)
		{
			if (m_nLatestLevel == m_ltLevelList[i])
			{
				nNewLevel = m_ltLevelList[i + 1];
				return true;
			}
		}
		return false;
	}
}
