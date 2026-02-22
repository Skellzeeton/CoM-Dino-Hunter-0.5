using UnityEngine;

public class ScrollList_Role : MonoBehaviour
{
	public ScrollList_RoleItem item_prefab;

	public TUIGrid grid;

	private TUIScrollListEx scroll_list_ex;

	private ScrollList_RoleItem item_choose;

	private void Awake()
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

	public void AddScrollListItem(TUIAllRoleInfo m_info)
	{
		if (m_info == null || m_info.role_list == null)
		{
			Debug.Log("no roll_list!");
			return;
		}
		if (scroll_list_ex == null)
		{
			scroll_list_ex = base.gameObject.GetComponent<TUIScrollListEx>();
		}
		for (int i = 0; i < m_info.role_list.Length; i++)
		{
			ScrollList_RoleItem scrollList_RoleItem = (ScrollList_RoleItem)Object.Instantiate(item_prefab);
			scrollList_RoleItem.transform.parent = grid.transform;
			scrollList_RoleItem.DoCreate(m_info.role_list[i]);
			scroll_list_ex.Add(scrollList_RoleItem.gameObject);
		}
		ResetPosition();
	}

	public void CheckItemChoose()
	{
		GameObject nowItem = scroll_list_ex.GetNowItem();
		if (nowItem == null)
		{
			return;
		}
		ScrollList_RoleItem component = nowItem.GetComponent<ScrollList_RoleItem>();
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

	public void ResetPosition()
	{
		grid.repositionNow = true;
	}

	public ScrollList_RoleItem GetItemChoose()
	{
		return item_choose;
	}
}
