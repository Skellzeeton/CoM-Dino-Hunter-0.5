using UnityEngine;

public class LabelInfo_Weapon : MonoBehaviour
{
	public TUILabel label_damage;

	public TUILabel label_damage_value;

	public TUILabel label_fire_rate;

	public TUILabel label_fire_rate_value;

	public TUILabel label_blast_radius;

	public TUILabel label_blast_radius_value;

	public TUILabel label_knockback;

	public TUILabel label_knockback_value;

	public TUILabel label_ammo;

	public TUILabel label_ammo_value;

	public TUILabel label_hp;

	public TUILabel label_hp_value;

	public TUILabel label_introduce;

	private void Start()
	{
	}

	private void Update()
	{
	}

	public void SetWeaponInfo(int damage, float fire_rate, int blast_radius, int knockback, int ammo)
	{
		label_damage.gameObject.SetActiveRecursively(true);
		label_damage_value.gameObject.SetActiveRecursively(true);
		label_fire_rate.gameObject.SetActiveRecursively(true);
		label_fire_rate_value.gameObject.SetActiveRecursively(true);
		label_blast_radius.gameObject.SetActiveRecursively(true);
		label_blast_radius_value.gameObject.SetActiveRecursively(true);
		label_knockback.gameObject.SetActiveRecursively(true);
		label_knockback_value.gameObject.SetActiveRecursively(true);
		label_ammo.gameObject.SetActiveRecursively(true);
		label_ammo_value.gameObject.SetActiveRecursively(true);
		label_hp.gameObject.SetActiveRecursively(false);
		label_hp_value.gameObject.SetActiveRecursively(false);
		label_introduce.gameObject.SetActiveRecursively(false);
		if (damage == 0)
		{
			label_damage_value.Text = "--";
		}
		else
		{
			label_damage_value.Text = "{color:73A206FF}" + damage;
		}
		if (fire_rate == 0f)
		{
			label_fire_rate_value.Text = "--";
		}
		else
		{
			label_fire_rate_value.Text = fire_rate.ToString();
		}
		if (blast_radius == 0)
		{
			label_blast_radius_value.Text = "--";
		}
		else
		{
			label_blast_radius_value.Text = blast_radius.ToString();
		}
		if (knockback == 0)
		{
			label_knockback_value.Text = "--";
		}
		else
		{
			label_knockback_value.Text = knockback.ToString();
		}
		if (ammo == 0)
		{
			label_ammo_value.Text = "--";
		}
		else
		{
			label_ammo_value.Text = ammo.ToString();
		}
	}

	public void SetStoneskinInfo(string m_introduce, int m_hp)
	{
		label_damage.gameObject.SetActiveRecursively(false);
		label_damage_value.gameObject.SetActiveRecursively(false);
		label_fire_rate.gameObject.SetActiveRecursively(false);
		label_fire_rate_value.gameObject.SetActiveRecursively(false);
		label_blast_radius.gameObject.SetActiveRecursively(false);
		label_blast_radius_value.gameObject.SetActiveRecursively(false);
		label_knockback.gameObject.SetActiveRecursively(false);
		label_knockback_value.gameObject.SetActiveRecursively(false);
		label_ammo.gameObject.SetActiveRecursively(false);
		label_ammo_value.gameObject.SetActiveRecursively(false);
		label_hp.gameObject.SetActiveRecursively(true);
		label_hp_value.gameObject.SetActiveRecursively(true);
		label_introduce.gameObject.SetActiveRecursively(true);
		if (m_hp == 0)
		{
			label_hp_value.Text = "--";
		}
		else
		{
			label_hp_value.Text = "{color:73A206FF}" + m_hp;
		}
		label_introduce.Text = m_introduce;
	}
}
