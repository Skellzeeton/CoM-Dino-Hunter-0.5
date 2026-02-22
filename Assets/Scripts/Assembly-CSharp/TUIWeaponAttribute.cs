public class TUIWeaponAttribute
{
	public int damage;

	public int fire_rate;

	public int blast_radius;

	public int knockback;

	public int ammo;

	public TUIWeaponAttribute()
	{
	}

	public TUIWeaponAttribute(int m_damage = 0, int m_fire_rate = 0, int m_blast_radius = 0, int m_knockback = 0, int m_ammo = 0)
	{
		damage = m_damage;
		fire_rate = m_fire_rate;
		blast_radius = m_blast_radius;
		knockback = m_knockback;
		ammo = m_ammo;
	}
}
