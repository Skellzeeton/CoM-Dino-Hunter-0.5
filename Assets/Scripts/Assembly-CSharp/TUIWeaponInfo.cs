using System.Collections.Generic;

public class TUIWeaponInfo
{
	public List<TUIWeaponAttributeInfo> weapon_list01;

	public List<TUIWeaponAttributeInfo> weapon_list02;

	public List<TUIWeaponAttributeInfo> weapon_list03;

	public List<TUIWeaponAttributeInfo> weapon_list04;

	public List<TUIWeaponAttributeInfo> weapon_list05;

	public List<TUIWeaponAttributeInfo> weapon_list06;

	public List<TUIWeaponAttributeInfo> weapon_list07;

	public void AddItem(TUIWeaponAttributeInfo m_info)
	{
		if (m_info.kind == WeaponType.CloseWeapon)
		{
			if (weapon_list01 == null)
			{
				weapon_list01 = new List<TUIWeaponAttributeInfo>();
			}
			weapon_list01.Add(m_info);
		}
		else if (m_info.kind == WeaponType.Crossbow)
		{
			if (weapon_list02 == null)
			{
				weapon_list02 = new List<TUIWeaponAttributeInfo>();
			}
			weapon_list02.Add(m_info);
		}
		else if (m_info.kind == WeaponType.MachineGun)
		{
			if (weapon_list03 == null)
			{
				weapon_list03 = new List<TUIWeaponAttributeInfo>();
			}
			weapon_list03.Add(m_info);
		}
		else if (m_info.kind == WeaponType.ViolenceGun)
		{
			if (weapon_list04 == null)
			{
				weapon_list04 = new List<TUIWeaponAttributeInfo>();
			}
			weapon_list04.Add(m_info);
		}
		else if (m_info.kind == WeaponType.LiquidFireGun)
		{
			if (weapon_list05 == null)
			{
				weapon_list05 = new List<TUIWeaponAttributeInfo>();
			}
			weapon_list05.Add(m_info);
		}
		else if (m_info.kind == WeaponType.RPG)
		{
			if (weapon_list06 == null)
			{
				weapon_list06 = new List<TUIWeaponAttributeInfo>();
			}
			weapon_list06.Add(m_info);
		}
		else if (m_info.kind == WeaponType.Stoneskin)
		{
			if (weapon_list07 == null)
			{
				weapon_list07 = new List<TUIWeaponAttributeInfo>();
			}
			weapon_list07.Add(m_info);
		}
	}
}
