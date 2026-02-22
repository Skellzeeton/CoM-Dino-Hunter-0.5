using UnityEngine;

public class ScrollList_RoleItem : MonoBehaviour
{
	public TUIMeshSprite img_bg;

	public TUIMeshSprite img_frame;

	public TUIMeshSprite img_lock;

	private int index;

	private bool be_choose = true;

	private TUIRoleInfo role_info;

	private void Start()
	{
	}

	private void Update()
	{
	}

	public void DoCreate(TUIRoleInfo m_info)
	{
		role_info = m_info;
		img_bg.texture = TUIMappingInfo.Instance().GetRoleTexture(role_info.id);
		if (m_info.unlock)
		{
			img_lock.gameObject.SetActiveRecursively(false);
		}
		DoUnChoose();
	}

	public void DoChoose()
	{
		if (!be_choose)
		{
			be_choose = true;
			img_frame.gameObject.SetActiveRecursively(true);
		}
	}

	public void DoUnChoose()
	{
		if (be_choose)
		{
			be_choose = false;
			img_frame.gameObject.SetActiveRecursively(false);
		}
	}

	public void DoLock()
	{
		if (role_info.unlock)
		{
			img_lock.gameObject.SetActiveRecursively(true);
			role_info.unlock = false;
		}
	}

	public void DoUnlock()
	{
		if (!role_info.unlock)
		{
			img_lock.gameObject.SetActiveRecursively(false);
			role_info.unlock = true;
		}
	}

	public void DoBuy()
	{
		if (!role_info.do_buy)
		{
			role_info.do_buy = true;
		}
	}

	public int GetIndex()
	{
		return index;
	}

	public TUIRoleInfo GetRoleInfo()
	{
		return role_info;
	}
}
