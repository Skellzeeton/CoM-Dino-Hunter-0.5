using UnityEngine;

public class ScrollList_Skill : MonoBehaviour
{
	public ScrollList_SkillItem item_prefab;

	public TUIScrollListEx scroll_list_ex;

	public TUIGrid grid;

	private int index;

	private void Start()
	{
	}

	private void Update()
	{
	}

	public void AddItem(TUISkillInfo[] m_role_skill_info, int m_index)
	{
		if (m_role_skill_info == null || m_role_skill_info.Length == 0)
		{
			Debug.Log("no skill!");
			return;
		}
		SetIndex(m_index);
		for (int i = 0; i < m_role_skill_info.Length; i++)
		{
			ScrollList_SkillItem scrollList_SkillItem = (ScrollList_SkillItem)Object.Instantiate(item_prefab);
			scrollList_SkillItem.transform.parent = grid.transform;
			scrollList_SkillItem.DoCreate(m_role_skill_info[i]);
			scroll_list_ex.Add(scrollList_SkillItem.gameObject);
		}
		ResetPosition();
	}

	public void ResetPosition()
	{
		grid.repositionNow = true;
	}

	public void SetIndex(int m_index)
	{
		index = m_index;
	}

	public int GetIndex()
	{
		return index;
	}
}
