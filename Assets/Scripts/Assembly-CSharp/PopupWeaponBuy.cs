using UnityEngine;

public class PopupWeaponBuy : MonoBehaviour
{
	public enum PopupWeaponBuyState
	{
		State_None = 0,
		State_Craft = 1,
		State_Update = 2,
		State_Max = 3
	}

	public TUILabel label_normal;

	public TUILabel label_press;

	private PopupWeaponBuyState btn_state;

	private void Start()
	{
		SetStateCraft();
	}

	private void Update()
	{
	}

	public PopupWeaponBuyState GetState()
	{
		return btn_state;
	}

	public void SetStateCraft()
	{
		if (btn_state != PopupWeaponBuyState.State_Craft)
		{
			label_normal.Text = "CRAFT";
			label_press.Text = "CRAFT";
			btn_state = PopupWeaponBuyState.State_Craft;
		}
	}

	public void SetStateUpdate()
	{
		if (btn_state != PopupWeaponBuyState.State_Update)
		{
			label_normal.Text = "UPDATE";
			label_press.Text = "UPDATE";
			btn_state = PopupWeaponBuyState.State_Update;
		}
	}

	public void SetStateMax()
	{
		if (btn_state != PopupWeaponBuyState.State_Max)
		{
			label_normal.Text = "MAX";
			label_press.Text = "MAX";
			btn_state = PopupWeaponBuyState.State_Max;
		}
	}
}
