using UnityEngine;

public class LevelPointEx : MonoBehaviour
{
	public enum LevelPointExState
	{
		Open = 0,
		Disable = 1,
		Hide = 2
	}

	public enum LevelPointExType
	{
		None = 0,
		Killing = 1,
		Defended = 2,
		Steal = 3,
		Survival = 4
	}

	public int level_id;

	public TUIMeshSprite img_bottom;

	public TUIMeshSprite img_way;

	public TUIButtonClick btn_level;

	public bool open_btn_animation;

	private LevelPointExState level_point_state = LevelPointExState.Hide;

	private TUILevelInfo level_info;

	private bool open_time_gap;

	private float time_gap;

	private float time_total;

	public LevelPointExType level_point_ex_type;

	public TUIMeshSprite img_icon_normal;

	public TUIMeshSprite img_icon_press;

	public TUIMeshSprite img_icon_disable;

	private string texture_killing01 = "furenwu_1";

	private string texture_killing02 = "furenwu_1hui";

	private string texture_survival01 = "furenwu_2";

	private string texture_survival02 = "furenwu_2hui";

	private string texture_defended01 = "furenwu_3";

	private string texture_defended02 = "furenwu_3hui";

	private string texture_steal01 = "furenwu_4";

	private string texture_steal02 = "furenwu_4hui";

	private void Awake()
	{
		if (open_btn_animation && btn_level != null && btn_level.GetComponent<Animation>() != null)
		{
			open_btn_animation = false;
			btn_level.GetComponent<Animation>().wrapMode = WrapMode.Loop;
			btn_level.GetComponent<Animation>().Play();
		}
		if (img_icon_normal != null && img_icon_press != null && img_icon_disable != null)
		{
			if (level_point_ex_type == LevelPointExType.Killing)
			{
				img_icon_normal.texture = texture_killing01;
				img_icon_press.texture = texture_killing01;
				img_icon_disable.texture = texture_killing02;
			}
			else if (level_point_ex_type == LevelPointExType.Defended)
			{
				img_icon_normal.texture = texture_defended01;
				img_icon_press.texture = texture_defended01;
				img_icon_disable.texture = texture_defended02;
			}
			else if (level_point_ex_type == LevelPointExType.Steal)
			{
				img_icon_normal.texture = texture_steal01;
				img_icon_press.texture = texture_steal01;
				img_icon_disable.texture = texture_steal02;
			}
			else if (level_point_ex_type == LevelPointExType.Survival)
			{
				img_icon_normal.texture = texture_survival01;
				img_icon_press.texture = texture_survival01;
				img_icon_disable.texture = texture_survival02;
			}
			else
			{
				Debug.Log("warning! level type no set!");
			}
		}
	}

	private void Start()
	{
	}

	private void Update()
	{
		UpdateShowWayAffterTime(Time.deltaTime);
	}

	public void SetLevelPointState(LevelPointExState m_level_point_state)
	{
		if (btn_level == null || img_bottom == null)
		{
			Debug.Log("error! btn_level or img_bottom no found!");
			return;
		}
		level_point_state = m_level_point_state;
		if (level_point_state == LevelPointExState.Open)
		{
			btn_level.Disable(false);
			btn_level.gameObject.SetActiveRecursively(true);
			img_bottom.gameObject.SetActiveRecursively(true);
		}
		else if (level_point_state == LevelPointExState.Disable)
		{
			btn_level.Disable(true);
			btn_level.gameObject.SetActiveRecursively(true);
			img_bottom.gameObject.SetActiveRecursively(true);
		}
		else if (level_point_state == LevelPointExState.Hide)
		{
			btn_level.Disable(true);
			btn_level.gameObject.SetActiveRecursively(false);
			img_bottom.gameObject.SetActiveRecursively(false);
		}
	}

	public void ShowWay()
	{
		if (img_way == null)
		{
			Debug.Log("error! no found way");
		}
		else
		{
			img_way.gameObject.SetActiveRecursively(true);
		}
	}

	public void ShowWayAffterTime(float m_time_gap)
	{
		time_gap = m_time_gap;
		open_time_gap = true;
	}

	public void UpdateShowWayAffterTime(float delta_time)
	{
		if (open_time_gap)
		{
			time_total += delta_time;
			if (time_total > time_gap)
			{
				open_time_gap = false;
				time_gap = 0f;
				time_total = 0f;
				ShowWay();
				SetLevelPointState(LevelPointExState.Open);
			}
		}
	}

	public void HideWay()
	{
		if (img_way == null)
		{
			Debug.Log("error! no found way");
		}
		else
		{
			img_way.gameObject.SetActiveRecursively(false);
		}
	}

	public void OpenLevelAnimation()
	{
		btn_level.GetComponent<Animation>().wrapMode = WrapMode.Loop;
		btn_level.GetComponent<Animation>().Play();
	}

	public void CloseLevelAniamtion()
	{
		btn_level.GetComponent<Animation>().Stop();
	}

	public int GetLevelID()
	{
		return level_id;
	}

	public void SetLevelInfo(TUILevelInfo m_info)
	{
		level_info = m_info;
	}

	public TUILevelInfo GetLevelInfo()
	{
		return level_info;
	}
}
