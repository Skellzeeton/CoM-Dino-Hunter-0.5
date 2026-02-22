using UnityEngine;

public class WeaponKindItem : MonoBehaviour
{
	public TUIButtonSelect btn_select01;

	public TUIButtonSelect btn_select02;

	public TUIButtonSelect btn_select03;

	public TUIButtonSelect btn_select04;

	public TUIButtonSelect btn_select05;

	public TUIButtonSelect btn_select06;

	public TUIButtonSelect btn_select07;

	private TUIButtonSelect[] btn_select_list;

	private void Awake()
	{
		btn_select_list = new TUIButtonSelect[7];
		btn_select_list[0] = btn_select01;
		btn_select_list[1] = btn_select02;
		btn_select_list[2] = btn_select03;
		btn_select_list[3] = btn_select04;
		btn_select_list[4] = btn_select05;
		btn_select_list[5] = btn_select06;
		btn_select_list[6] = btn_select07;
	}

	private void Start()
	{
	}

	private void Update()
	{
	}

	public void SetSelectBtn(WeaponType m_type)
	{
		ResetSelectBtn();
		switch (m_type)
		{
		case WeaponType.CloseWeapon:
			btn_select_list[0].SetSelected(true);
			break;
		case WeaponType.Crossbow:
			btn_select_list[1].SetSelected(true);
			break;
		case WeaponType.MachineGun:
			btn_select_list[2].SetSelected(true);
			break;
		case WeaponType.ViolenceGun:
			btn_select_list[3].SetSelected(true);
			break;
		case WeaponType.LiquidFireGun:
			btn_select_list[4].SetSelected(true);
			break;
		case WeaponType.RPG:
			btn_select_list[5].SetSelected(true);
			break;
		case WeaponType.Stoneskin:
			btn_select_list[6].SetSelected(true);
			break;
		}
	}

	public void ResetSelectBtn()
	{
		for (int i = 0; i < btn_select_list.Length; i++)
		{
			TUIButtonSelect tUIButtonSelect = btn_select_list[i];
			if (tUIButtonSelect != null)
			{
				tUIButtonSelect.Reset();
			}
		}
	}
}
