using System.Collections.Generic;
using UnityEngine;

public class DebugGUI : MonoBehaviour
{
	public int m_nMaxCount = 30;

	private List<string> m_DebugList = new List<string>();

	private int m_nLineHeight = 22;

	private void Awake()
	{
		Object.DontDestroyOnLoad(base.gameObject);
	}

	private void OnGUI()
	{
		GUI.color = Color.green;
		for (int i = 0; i < m_DebugList.Count; i++)
		{
			GUI.Label(new Rect(0f, Screen.height - m_nLineHeight * (m_DebugList.Count - i + 1), Screen.width, m_nLineHeight), m_DebugList[i]);
		}
	}

	public void Debug(string str)
	{
		m_DebugList.Add(str);
		if (m_DebugList.Count > m_nMaxCount)
		{
			m_DebugList.RemoveAt(0);
		}
	}
}
