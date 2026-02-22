using System;
using System.Collections.Generic;
using UnityEngine;

public class gyUICellPanel : MonoBehaviour
{
	public delegate void OnClickCellFunc(int nIndex);

	protected OnClickCellFunc m_OnClickCellFunc;

	protected gyUICell[] m_arrCell;

	public void Awake()
	{
		List<gyUICell> list = new List<gyUICell>();
		foreach (Transform item in base.transform)
		{
			gyUICell component = item.GetComponent<gyUICell>();
			if (component != null)
			{
				list.Add(component);
			}
		}
		m_arrCell = list.ToArray();
	}

	private void Start()
	{
	}

	private void Update()
	{
	}

	public void OnClickCell(int nIndex)
	{
		if (m_OnClickCellFunc != null)
		{
			m_OnClickCellFunc(nIndex);
		}
	}

	public void RegisterOnClickCell(OnClickCellFunc func)
	{
		m_OnClickCellFunc = (OnClickCellFunc)Delegate.Combine(m_OnClickCellFunc, func);
	}

	public void UnregisterOnClickCell(OnClickCellFunc func)
	{
		m_OnClickCellFunc = (OnClickCellFunc)Delegate.Remove(m_OnClickCellFunc, func);
	}
}
