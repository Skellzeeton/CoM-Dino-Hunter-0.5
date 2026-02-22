using UnityEngine;

public class ScrollSkill : MonoBehaviour
{
	public TUIScrollListEx scroll_list_ex_prefab;

	private TUIScrollListEx[] scroll_list_ex_list;

	private TUIScrollListEx scroll_list_ex_now;

	private ScrollList_SkillItem item_choose;

	private void Start()
	{
		ScrollListChoose(1);
	}

	private void Update()
	{
		CheckItemChoose();
	}

	public void AddScrollList(TUIAllSkillInfo m_all_role_skill_info)
	{
		if (m_all_role_skill_info == null || m_all_role_skill_info.all_role_skill_list == null)
		{
			Debug.Log("no role skill!");
			return;
		}
		scroll_list_ex_list = new TUIScrollListEx[m_all_role_skill_info.all_role_skill_list.Length];
		for (int i = 0; i < scroll_list_ex_list.Length; i++)
		{
			scroll_list_ex_list[i] = (TUIScrollListEx)Object.Instantiate(scroll_list_ex_prefab);
			scroll_list_ex_list[i].transform.parent = base.gameObject.transform;
			scroll_list_ex_list[i].transform.localPosition = new Vector3(0f, 1000f, 0f);
			scroll_list_ex_list[i].GetComponent<ScrollList_Skill>().AddItem(m_all_role_skill_info.all_role_skill_list[i].skill_list_info, i + 1);
		}
	}

	public void ScrollListChoose(int m_id)
	{
		if (scroll_list_ex_now != null)
		{
			scroll_list_ex_now.transform.localPosition = new Vector3(0f, 1000f, 0f);
		}
		scroll_list_ex_now = scroll_list_ex_list[m_id - 1];
		scroll_list_ex_now.transform.localPosition = new Vector3(0f, 0f, 0f);
		item_choose = null;
	}

	private void CheckItemChoose()
	{
		GameObject nowItem = scroll_list_ex_now.GetNowItem();
		if (nowItem == null)
		{
			return;
		}
		ScrollList_SkillItem component = nowItem.GetComponent<ScrollList_SkillItem>();
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

	public TUIScrollListEx GetScrollListChoose()
	{
		return scroll_list_ex_now;
	}

	public ScrollList_SkillItem GetItemChoose()
	{
		return item_choose;
	}
}
