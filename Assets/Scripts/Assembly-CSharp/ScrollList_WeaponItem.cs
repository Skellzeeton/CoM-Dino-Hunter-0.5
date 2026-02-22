using UnityEngine;

public class ScrollList_WeaponItem : MonoBehaviour
{
	public TUIMeshSprite img_bg;

	public TUIMeshSprite img_frame;

	public TUIMeshSprite img_frame_choose;

	private bool be_choose;

	private TUIWeaponAttributeInfo attribute_info;

	private string texture_path = "TUI/Weapon/";

	private void Start()
	{
	}

	private void Update()
	{
	}

	public void DoCreate(TUIWeaponAttributeInfo m_attribute_info)
	{
		if (m_attribute_info == null)
		{
			Debug.Log("error!");
			return;
		}
		attribute_info = m_attribute_info;
		be_choose = true;
		string weaponTexture = TUIMappingInfo.Instance().GetWeaponTexture(m_attribute_info.id);
		SetCustomizeTexture(img_bg, texture_path + weaponTexture);
		DoUnChoose();
	}

	public void SetCustomizeTexture(TUIMeshSprite m_sprite, string m_path)
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

	public TUIMeshSprite GetCustomizeTexture()
	{
		return img_bg;
	}

	public void DoChoose()
	{
		if (!be_choose)
		{
			be_choose = true;
			img_frame.gameObject.SetActiveRecursively(false);
			img_frame_choose.gameObject.SetActiveRecursively(true);
		}
	}

	public void DoUnChoose()
	{
		if (be_choose)
		{
			be_choose = false;
			img_frame.gameObject.SetActiveRecursively(true);
			img_frame_choose.gameObject.SetActiveRecursively(false);
		}
	}

	public TUIWeaponAttributeInfo GetWeaponAttributeInfo()
	{
		return attribute_info;
	}
}
