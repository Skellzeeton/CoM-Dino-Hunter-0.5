using UnityEngine;

public class PopupLevel_Recommend : MonoBehaviour
{
	public enum RecommendType
	{
		None = 0,
		Role = 1,
		Weapon = 2
	}

	public enum RequiredType
	{
		None = 0,
		Role = 1,
		Weapon = 2
	}

	public enum RecommendBtnState
	{
		Disable = 0,
		RoleBuy = 1,
		RoleEquip = 2,
		WeaponBuy = 3,
		WeaponEquip = 4
	}

	public TUIMeshSprite img_role;

	public TUIMeshSprite img_weapon;

	public LevelStars level_stars;

	public TUILabel label_recommend_title;

	public TUIButtonClick btn_buy;

	public TUILabel label_btn_buy_normal;

	public TUILabel label_btn_buy_press;

	private string weapon_texture_path = "TUI/Weapon/";

	private RecommendType recommend_type;

	private RequiredType required_type;

	private RecommendBtnState recommend_btn_state;

	private void Start()
	{
	}

	private void Update()
	{
	}

	public RecommendType GetRecommendType()
	{
		return recommend_type;
	}

	public RequiredType GetRequiredType()
	{
		return required_type;
	}

	public void SetRecommendNone()
	{
		recommend_type = RecommendType.None;
		required_type = RequiredType.None;
		label_recommend_title.Text = string.Empty;
		img_role.texture = string.Empty;
		img_weapon.UseCustomize = false;
		img_weapon.CustomizeTexture = null;
		img_role.gameObject.SetActiveRecursively(false);
		img_weapon.gameObject.SetActiveRecursively(false);
		level_stars.gameObject.SetActiveRecursively(false);
		btn_buy.gameObject.SetActiveRecursively(false);
	}

	public void SetRecommendWeapon(int m_weapon_id, int m_weapon_level, int m_weapon_recommend_level, bool m_have_equip, bool m_required)
	{
		recommend_type = RecommendType.Weapon;
		if (m_required)
		{
			required_type = RequiredType.Weapon;
			label_recommend_title.Text = "Required";
		}
		else
		{
			required_type = RequiredType.None;
			label_recommend_title.Text = "Recommended";
		}
		img_role.texture = string.Empty;
		img_role.gameObject.SetActiveRecursively(false);
		string weaponTexture = TUIMappingInfo.Instance().GetWeaponTexture(m_weapon_id);
		SetCustomizeTexture(img_weapon, weapon_texture_path + weaponTexture);
		img_weapon.gameObject.SetActiveRecursively(true);
		level_stars.SetStars(m_weapon_level);
		level_stars.gameObject.SetActiveRecursively(true);
		if (m_weapon_level < m_weapon_recommend_level)
		{
			label_btn_buy_normal.Text = "Buy";
			label_btn_buy_press.Text = "Buy";
			btn_buy.gameObject.SetActiveRecursively(true);
			btn_buy.Show();
			recommend_btn_state = RecommendBtnState.WeaponBuy;
			return;
		}
		if (m_weapon_recommend_level == 0 && m_required)
		{
			level_stars.gameObject.SetActiveRecursively(false);
		}
		if (!m_have_equip)
		{
			label_btn_buy_normal.Text = "Equip";
			label_btn_buy_press.Text = "Equip";
			btn_buy.gameObject.SetActiveRecursively(true);
			btn_buy.Show();
			recommend_btn_state = RecommendBtnState.WeaponEquip;
		}
		else
		{
			btn_buy.gameObject.SetActiveRecursively(false);
			recommend_btn_state = RecommendBtnState.Disable;
		}
	}

	public void SetRecommendRole(int m_role_id, bool m_have_buy, bool m_have_equip, bool m_required)
	{
		recommend_type = RecommendType.Role;
		if (m_required)
		{
			required_type = RequiredType.Role;
			label_recommend_title.Text = "Required";
		}
		else
		{
			required_type = RequiredType.None;
			label_recommend_title.Text = "Recommended";
		}
		img_weapon.UseCustomize = false;
		img_weapon.CustomizeTexture = null;
		img_weapon.gameObject.SetActiveRecursively(false);
		level_stars.gameObject.SetActiveRecursively(false);
		img_role.texture = TUIMappingInfo.Instance().GetRoleTexture(m_role_id);
		img_role.gameObject.SetActiveRecursively(true);
		if (!m_have_buy)
		{
			label_btn_buy_normal.Text = "Buy";
			label_btn_buy_press.Text = "Buy";
			btn_buy.gameObject.SetActiveRecursively(true);
			btn_buy.Show();
			recommend_btn_state = RecommendBtnState.RoleBuy;
		}
		else if (!m_have_equip)
		{
			label_btn_buy_normal.Text = "Equip";
			label_btn_buy_press.Text = "Equip";
			btn_buy.gameObject.SetActiveRecursively(true);
			btn_buy.Show();
			recommend_btn_state = RecommendBtnState.RoleEquip;
		}
		else
		{
			btn_buy.gameObject.SetActiveRecursively(false);
			recommend_btn_state = RecommendBtnState.Disable;
		}
	}

	private void SetCustomizeTexture(TUIMeshSprite m_sprite, string m_path)
	{
		m_sprite.texture = string.Empty;
		m_sprite.UseCustomize = true;
		m_sprite.CustomizeTexture = Resources.Load(m_path) as Texture;
		if (m_sprite.CustomizeTexture == null)
		{
			Debug.Log("lose texture!");
		}
		else
		{
			m_sprite.CustomizeRect = new Rect(0f, 0f, m_sprite.CustomizeTexture.width, m_sprite.CustomizeTexture.height);
		}
	}

	public RecommendBtnState GetRecommendBtnState()
	{
		return recommend_btn_state;
	}
}
