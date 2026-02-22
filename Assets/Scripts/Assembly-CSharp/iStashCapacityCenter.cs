using System.Collections.Generic;

public class iStashCapacityCenter
{
	protected Dictionary<int, CStashCapacity> m_dictStashCapacity;

	public iStashCapacityCenter()
	{
		m_dictStashCapacity = new Dictionary<int, CStashCapacity>();
	}

	public Dictionary<int, CStashCapacity> GetData()
	{
		return m_dictStashCapacity;
	}

	public CStashCapacity Get(int nLevel)
	{
		if (!m_dictStashCapacity.ContainsKey(nLevel))
		{
			return null;
		}
		return m_dictStashCapacity[nLevel];
	}

	public int GetCapacity(int nLevel)
	{
		CStashCapacity cStashCapacity = Get(nLevel);
		if (cStashCapacity == null)
		{
			return 0;
		}
		return cStashCapacity.nCapacity;
	}

	public bool Load()
	{
		m_dictStashCapacity.Add(1, new CStashCapacity(1, true, 100, 100, "Add capacity to 200"));
		m_dictStashCapacity.Add(2, new CStashCapacity(2, true, 200, 200, "Add capacity to 300"));
		m_dictStashCapacity.Add(3, new CStashCapacity(3, true, 300, 300, "Max"));
		return true;
	}
}
