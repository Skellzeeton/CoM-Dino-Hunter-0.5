using System.Collections.Generic;
using UnityEngine;

public class ScrollList_Weapon : MonoBehaviour
{
	public ScrollList_WeaponItem item_prefab;

	public TUIGrid grid;

	private TUIScrollListEx scroll_list_ex;

	private ScrollList_WeaponItem item_choose;

	private Vector3 normal_position = Vector3.zero;

	private void Awak()
	{
	}

	private void Start()
	{
		scroll_list_ex = base.gameObject.GetComponent<TUIScrollListEx>();
	}

	private void Update()
	{
		CheckItemChoose();
	}

	private void CheckItemChoose()
	{
		GameObject nowItem = scroll_list_ex.GetNowItem();
		if (nowItem == null)
		{
			return;
		}
		ScrollList_WeaponItem component = nowItem.GetComponent<ScrollList_WeaponItem>();
		if (item_choose == null)
		{
			if (component != null)
			{
				item_choose = component;
				item_choose.DoChoose();
			}
		}
		else if (item_choose != component)
		{
			item_choose.DoUnChoose();
			item_choose = component;
			item_choose.DoChoose();
		}
	}

	public void AddScrollListItem(List<TUIWeaponAttributeInfo> m_attribute_info)
	{
		if (m_attribute_info == null)
		{
			Debug.Log("no find m_attribute_info!");
			return;
		}
		if (scroll_list_ex == null)
		{
			scroll_list_ex = base.gameObject.GetComponent<TUIScrollListEx>();
		}
		for (int i = 0; i < m_attribute_info.Count; i++)
		{
			ScrollList_WeaponItem scrollList_WeaponItem = (ScrollList_WeaponItem)Object.Instantiate(item_prefab);
			scrollList_WeaponItem.transform.parent = grid.transform;
			scrollList_WeaponItem.DoCreate(m_attribute_info[i]);
			scroll_list_ex.Add(scrollList_WeaponItem.gameObject);
		}
		ResetPosition();
		normal_position = base.transform.localPosition;
	}

	public void ResetPosition()
	{
		grid.repositionNow = true;
	}

	public ScrollList_WeaponItem GetItemChoose()
	{
		return item_choose;
	}

	public void Show()
	{
		base.transform.localPosition = normal_position;
	}

	public void Hide()
	{
		ResetPosition();
		base.transform.localPosition = normal_position + new Vector3(0f, 1000f, 0f);
	}
}
