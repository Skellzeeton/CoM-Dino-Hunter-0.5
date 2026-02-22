using UnityEngine;

public class Btn_BuySkill : MonoBehaviour
{
	public enum StateButtonSkill
	{
		State_Unlock = 0,
		State_Buy = 1,
		State_Update = 2,
		State_Disable = 3
	}

	public TUIMeshSprite img_crystal_normal;

	public TUIMeshSprite img_crystal_press;

	public TUILabel label_price_normal;

	public TUILabel label_price_press;

	private string gold_texture = "title_jingbi";

	private string crystal_texture = "title_shuijing";

	private StateButtonSkill state_btnskill;

	private void Start()
	{
	}

	private void Update()
	{
	}

	public void SetStateUnlock(int m_value, UnitType m_type)
	{
		state_btnskill = StateButtonSkill.State_Unlock;
		base.gameObject.SetActiveRecursively(true);
		base.gameObject.GetComponent<TUIButtonClick>().Show();
		label_price_normal.Text = m_value.ToString();
		label_price_press.Text = m_value.ToString();
		switch (m_type)
		{
		case UnitType.Gold:
			img_crystal_normal.texture = gold_texture;
			img_crystal_press.texture = gold_texture;
			break;
		case UnitType.Crystal:
			img_crystal_normal.texture = crystal_texture;
			img_crystal_press.texture = crystal_texture;
			break;
		}
	}

	public void SetStateBuy(int m_value, UnitType m_type)
	{
		state_btnskill = StateButtonSkill.State_Buy;
		base.gameObject.SetActiveRecursively(true);
		base.gameObject.GetComponent<TUIButtonClick>().Show();
		label_price_normal.Text = m_value.ToString();
		label_price_press.Text = m_value.ToString();
		switch (m_type)
		{
		case UnitType.Gold:
			img_crystal_normal.texture = gold_texture;
			img_crystal_press.texture = gold_texture;
			break;
		case UnitType.Crystal:
			img_crystal_normal.texture = crystal_texture;
			img_crystal_press.texture = crystal_texture;
			break;
		}
	}

	public void SetStateUpdate()
	{
		state_btnskill = StateButtonSkill.State_Update;
		base.gameObject.SetActiveRecursively(true);
		base.gameObject.GetComponent<TUIButtonClick>().Show();
		img_crystal_normal.texture = string.Empty;
		img_crystal_press.texture = string.Empty;
		label_price_normal.Text = "UPDATE";
		label_price_press.Text = "UPDATE";
	}

	public void SetStateDisable()
	{
		state_btnskill = StateButtonSkill.State_Disable;
		base.gameObject.SetActiveRecursively(false);
		img_crystal_normal.texture = string.Empty;
		img_crystal_press.texture = string.Empty;
		label_price_normal.Text = string.Empty;
		label_price_press.Text = string.Empty;
	}

	public StateButtonSkill GetStateBtnSkill()
	{
		return state_btnskill;
	}
}
