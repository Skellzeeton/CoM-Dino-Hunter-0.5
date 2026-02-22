using UnityEngine;

public class PopupRoleBuy : MonoBehaviour
{
	public enum PopupRoleBuyState
	{
		State_Unlock = 0,
		State_Buy = 1,
		State_Use = 2,
		State_Disable = 3
	}

	public TUILabel label_normal;

	public TUILabel label_press;

	public TUIMeshSprite img_normal;

	public TUIMeshSprite img_press;

	private string gold_texture = "title_jingbi";

	private string crystal_texture = "title_shuijing";

	private PopupRoleBuyState btn_state;

	private void Start()
	{
	}

	private void Update()
	{
	}

	public PopupRoleBuyState GetState()
	{
		return btn_state;
	}

	public void SetStateUnlock(int value, UnitType gold_type)
	{
		if (btn_state == PopupRoleBuyState.State_Disable)
		{
			base.gameObject.SetActiveRecursively(true);
			base.gameObject.GetComponent<TUIButtonClick>().Show();
		}
		btn_state = PopupRoleBuyState.State_Unlock;
		label_normal.Text = value.ToString();
		label_press.Text = value.ToString();
		switch (gold_type)
		{
		case UnitType.Gold:
			img_normal.texture = gold_texture;
			img_press.texture = gold_texture;
			break;
		case UnitType.Crystal:
			img_normal.texture = crystal_texture;
			img_press.texture = crystal_texture;
			break;
		}
	}

	public void SetStateBuy(int value, UnitType gold_type)
	{
		if (btn_state == PopupRoleBuyState.State_Disable)
		{
			base.gameObject.SetActiveRecursively(true);
			base.gameObject.GetComponent<TUIButtonClick>().Show();
		}
		btn_state = PopupRoleBuyState.State_Buy;
		label_normal.Text = value.ToString();
		label_press.Text = value.ToString();
		switch (gold_type)
		{
		case UnitType.Gold:
			img_normal.texture = gold_texture;
			img_press.texture = gold_texture;
			break;
		case UnitType.Crystal:
			img_normal.texture = crystal_texture;
			img_press.texture = crystal_texture;
			break;
		}
	}

	public void SetStateUse()
	{
		if (btn_state != PopupRoleBuyState.State_Use)
		{
			if (btn_state == PopupRoleBuyState.State_Disable)
			{
				base.gameObject.SetActiveRecursively(true);
				base.gameObject.GetComponent<TUIButtonClick>().Show();
			}
			btn_state = PopupRoleBuyState.State_Use;
			label_normal.Text = "USE";
			label_press.Text = "USE";
			img_normal.texture = string.Empty;
			img_press.texture = string.Empty;
		}
	}

	public void SetStateDisable()
	{
		if (btn_state != PopupRoleBuyState.State_Disable)
		{
			btn_state = PopupRoleBuyState.State_Disable;
			base.gameObject.SetActiveRecursively(false);
		}
	}
}
