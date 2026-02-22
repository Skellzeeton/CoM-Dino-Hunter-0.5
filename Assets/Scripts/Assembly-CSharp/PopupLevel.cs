using UnityEngine;

public class PopupLevel : MonoBehaviour
{
	public GameObject go_popup;

	public PopupLevel_Frame01 popuplevel_frame01;

	public PopupLevel_Frame02 popuplevel_frame02;

	public PopupLevel_Frame03 popuplevel_frame03;

	public TUILabel label_title;

	private TUILevelInfo level_info;

	private void Start()
	{
	}

	private void Update()
	{
	}

	public void Show(TUILevelInfo m_level_info)
	{
		if (popuplevel_frame01 == null || popuplevel_frame02 == null || popuplevel_frame03 == null)
		{
			Debug.Log("error!");
			return;
		}
		level_info = m_level_info;
		if (m_level_info == null)
		{
			Debug.Log("error! no info");
		}
		else
		{
			popuplevel_frame01.SetInfo(level_info.introduce01);
			popuplevel_frame02.SetInfo(level_info.introduce02);
			popuplevel_frame03.SetGoodsInfo(level_info.goods_drop_list);
			popuplevel_frame03.SetRecommend(level_info.recommend_role_info, level_info.recommend_weapon_info);
			label_title.Text = m_level_info.title;
		}
		base.transform.localPosition = new Vector3(0f, 0f, base.transform.localPosition.z);
		go_popup.GetComponent<Animation>().Play();
	}

	public void Hide()
	{
		base.transform.localPosition = new Vector3(0f, -1000f, base.transform.localPosition.z);
	}
}
