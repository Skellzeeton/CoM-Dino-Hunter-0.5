public class TUIPopupInfo
{
	public int texture_id;

	public string name = string.Empty;

	public string introduce = string.Empty;

	public int value;

	public TUIWeaponAttribute weapon_attribute;

	public TUIPopupInfo()
	{
	}

	public TUIPopupInfo(int m_id, string m_name, string m_introduce, int m_value = 0)
	{
		texture_id = m_id;
		name = m_name;
		introduce = m_introduce;
		value = m_value;
		weapon_attribute = null;
	}

	public TUIPopupInfo(int m_id, string m_name, string m_introduce, TUIWeaponAttribute m_weapon_attribute)
	{
		texture_id = m_id;
		name = m_name;
		introduce = m_introduce;
		value = 0;
		weapon_attribute = m_weapon_attribute;
	}
}
