using UnityEngine;

public class ScrollList_SkillItem : MonoBehaviour
{
	public TUIMeshSprite img_bg;

	public TUIMeshSprite img_frame;

	public TUIMeshSprite img_frame_choose;

	public TUIMeshSprite img_lock;

	private bool be_choose;

	private TUISkillInfo skill_info;

	private void Start()
	{
	}

	private void Update()
	{
	}

	public void DoChoose()
	{
		if (!be_choose)
		{
			be_choose = true;
			img_frame.gameObject.SetActiveRecursively(false);
			img_frame_choose.gameObject.SetActiveRecursively(true);
		}
	}

	public void DoUnChoose()
	{
		if (be_choose)
		{
			be_choose = false;
			img_frame.gameObject.SetActiveRecursively(true);
			img_frame_choose.gameObject.SetActiveRecursively(false);
		}
	}

	public void DoCreate(TUISkillInfo m_skill_info)
	{
		be_choose = true;
		skill_info = m_skill_info;
		img_bg.texture = TUIMappingInfo.Instance().GetSkillTexture(m_skill_info.id);
		if (m_skill_info.unlock || m_skill_info.active_skill)
		{
			img_lock.gameObject.SetActiveRecursively(false);
		}
		DoUnChoose();
	}

	public int GetSkillID()
	{
		return skill_info.id;
	}

	public string GetSkillName()
	{
		return skill_info.name;
	}

	public int GetSkillLevel()
	{
		return skill_info.level;
	}

	public bool GetSkillActive()
	{
		return skill_info.active_skill;
	}

	public string GetSkillIntroduce()
	{
		if (skill_info.level_introduce.ContainsKey(skill_info.level + 1))
		{
			return skill_info.level_introduce[skill_info.level + 1];
		}
		Debug.Log("warning! no introduce!");
		return string.Empty;
	}

	public string GetSkillIntroduceEx()
	{
		int key = ((skill_info.level <= 1) ? 1 : skill_info.level);
		if (skill_info.level_introduce_ex.ContainsKey(key))
		{
			return skill_info.level_introduce_ex[key];
		}
		Debug.Log("warning! no introduce_ex!");
		return string.Empty;
	}

	public string GetSkillActiveIntroduce()
	{
		return skill_info.active_skill_introduce;
	}

	public bool GetSkillUnlock()
	{
		return skill_info.unlock;
	}

	public TUIPriceInfo GetSkillUnlockPrice()
	{
		return skill_info.unlock_price;
	}

	public TUIPriceInfo GetSkillBuyPrice()
	{
		if (skill_info.level_price.ContainsKey(1))
		{
			return skill_info.level_price[1];
		}
		return null;
	}

	public TUIPriceInfo GetSkillUpdatePrice()
	{
		TUIPriceInfo result = null;
		if (skill_info == null || skill_info.level_price == null)
		{
			Debug.Log("error!");
			return result;
		}
		if (skill_info.level_price.ContainsKey(skill_info.level + 1))
		{
			result = skill_info.level_price[skill_info.level + 1];
		}
		else
		{
			Debug.Log("error!");
		}
		return result;
	}

	public void SkillUnlock()
	{
		img_lock.gameObject.SetActiveRecursively(false);
		skill_info.unlock = true;
	}

	public void SkillBuy()
	{
		skill_info.level = 1;
	}

	public void SkillUpdate()
	{
		skill_info.level++;
	}

	public string GetCustomizeTexture()
	{
		return img_bg.m_texture;
	}

	public bool ReachLevelMax()
	{
		if (skill_info.level >= skill_info.level_price.Count)
		{
			return true;
		}
		return false;
	}
}
