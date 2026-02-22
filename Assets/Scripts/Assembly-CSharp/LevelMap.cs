using System.Collections.Generic;
using UnityEngine;

public class LevelMap : MonoBehaviour
{
	public LevelPoint[] level_point_list;

	public Transform[] mask_list;

	private float camera_top_border;

	private float camera_bottom_border;

	private float camera_left_border;

	private float camera_right_border;

	private float map_height = 384f;

	private float map_width = 480f;

	protected float map_width_total = 1700f;

	protected Camera m_Camera;

	private void Awake()
	{
		GameObject gameObject = GameObject.Find("TUICamera");
		if (gameObject != null)
		{
			m_Camera = gameObject.GetComponent<Camera>();
			if (m_Camera != null)
			{
				map_width = m_Camera.orthographicSize * m_Camera.aspect;
				Debug.Log(Screen.height + " " + Screen.width);
				Debug.Log(m_Camera.orthographicSize + " " + map_width);
			}
		}
	}

	private void Start()
	{
	}

	private void Update()
	{
	}

	public void MoveScreen(float wparam, float lparam)
	{
		Vector3 localPosition = base.transform.localPosition + new Vector3(wparam, lparam);
		float num = map_width - 240f;
		if (localPosition.x > 0f - num)
		{
			localPosition.x = 0f - num;
		}
		if (localPosition.x < 0f - map_width_total + map_width * 2f - num)
		{
			localPosition.x = 0f - map_width_total + map_width * 2f - num;
		}
		base.transform.localPosition = localPosition;
	}

	public void SetScreenPos(Vector3 m_pos)
	{
		Vector3 localPosition = base.transform.localPosition - m_pos;
		float num = map_width - 240f;
		if (localPosition.x > 0f - num)
		{
			localPosition.x = 0f - num;
		}
		if (localPosition.x < 0f - map_width_total + map_width * 2f - num)
		{
			localPosition.x = 0f - map_width_total + map_width * 2f - num;
		}
		base.transform.localPosition = localPosition;
	}

	public void SetBorder(Camera m_camera)
	{
		camera_top_border = Screen.height / 4f;
		camera_bottom_border = (0f - Screen.height) / 4f;
		camera_left_border = (0f - Screen.width) / 4f;
		camera_right_border = Screen.width / 4f;
	}

	public void SetMapEnterInfo(TUIMapInfo m_map_info)
	{
		if (m_map_info == null)
		{
			Debug.Log("error!no map info");
		}
		MapEnterType map_enter_type = m_map_info.map_enter_type;
		List<int> list = new List<int>();
		List<int> list2 = new List<int>();
		List<int> list3 = new List<int>();
		if (m_map_info.level_open_list != null)
		{
			for (int i = 0; i < m_map_info.level_open_list.Length; i++)
			{
				list.Add(m_map_info.level_open_list[i]);
			}
			if (m_map_info.level_no_open_list != null)
			{
				for (int j = 0; j < m_map_info.level_no_open_list.Length; j++)
				{
					list2.Add(m_map_info.level_no_open_list[j]);
				}
			}
			if (m_map_info.level_goods_drop_list != null)
			{
				for (int k = 0; k < m_map_info.level_goods_drop_list.Length; k++)
				{
					list3.Add(m_map_info.level_goods_drop_list[k]);
				}
			}
			int now_level = m_map_info.now_level;
			int next_level = m_map_info.next_level;
			int count = list.Count;
			Vector3 zero = Vector3.zero;
			int num = 0;
			if (count < 1)
			{
				Debug.Log("error! you have no open level!!");
				return;
			}
			if (now_level >= 1001 && now_level <= 1003)
			{
				num = 1;
				if (now_level == 1003 && map_enter_type == MapEnterType.OpenNewLevel)
				{
					num = 2;
				}
			}
			else if (now_level >= 1004 && now_level <= 1009)
			{
				num = 2;
				if (now_level == 1009 && map_enter_type == MapEnterType.OpenNewLevel)
				{
					num = 3;
				}
			}
			else if (now_level >= 1010 && now_level <= 1015)
			{
				num = 3;
				if (now_level == 1015 && map_enter_type == MapEnterType.OpenNewLevel)
				{
					num = 4;
				}
			}
			else if (now_level >= 1016 && now_level <= 1021)
			{
				num = 4;
				if (now_level == 1009 && map_enter_type == MapEnterType.OpenNewLevel)
				{
					num = 5;
				}
			}
			else if (now_level >= 1022)
			{
				num = 5;
			}
			switch (num)
			{
			case 1:
			{
				for (int num2 = 0; num2 < list2.Count; num2++)
				{
					if (list2[num2] >= 1004)
					{
						list2.RemoveRange(num2, list2.Count - num2);
						break;
					}
				}
				break;
			}
			case 2:
			{
				for (int m = 0; m < list2.Count; m++)
				{
					if (list2[m] >= 1010)
					{
						list2.RemoveRange(m, list2.Count - m);
						break;
					}
				}
				break;
			}
			case 3:
			{
				for (int n = 0; n < list2.Count; n++)
				{
					if (list2[n] >= 1016)
					{
						list2.RemoveRange(n, list2.Count - n);
						break;
					}
				}
				break;
			}
			case 4:
			{
				for (int l = 0; l < list2.Count; l++)
				{
					if (list2[l] >= 1022)
					{
						list2.RemoveRange(l, list2.Count - l);
						break;
					}
				}
				break;
			}
			case 5:
				list2.Clear();
				break;
			}
			switch (map_enter_type)
			{
			case MapEnterType.Normal:
				if (level_point_list != null)
				{
					for (int num10 = 0; num10 < level_point_list.Length; num10++)
					{
						if (list != null)
						{
							for (int num11 = 0; num11 < list.Count; num11++)
							{
								if (level_point_list[num10].GetLevelID() == list[num11])
								{
									level_point_list[num10].SetLevelOpen();
									level_point_list[num10].ShowWayPoint();
									level_point_list[num10].ShowWayEx();
								}
							}
							count = list.Count;
						}
						else
						{
							Debug.Log("warning! no level open list!");
						}
						if (list2 != null)
						{
							for (int num12 = 0; num12 < list2.Count; num12++)
							{
								if (level_point_list[num10].GetLevelID() == list2[num12])
								{
									level_point_list[num10].SetLevelDisable();
									level_point_list[num10].HideWayPoint();
									level_point_list[num10].HideWayEx();
								}
							}
						}
						if (level_point_list[num10].GetLevelID() == now_level)
						{
							level_point_list[num10].OpenLevelAnimation();
							level_point_list[num10].HideWayPoint();
							level_point_list[num10].ShowWayEx();
							zero.x = level_point_list[num10].transform.position.x - base.transform.position.x;
						}
					}
				}
				else
				{
					Debug.Log("no info found!");
				}
				break;
			case MapEnterType.OpenNewLevel:
			{
				LevelPoint levelPoint = null;
				LevelPoint newLevelOpen = null;
				if (level_point_list != null)
				{
					for (int num7 = 0; num7 < level_point_list.Length; num7++)
					{
						if (list != null)
						{
							for (int num8 = 0; num8 < list.Count; num8++)
							{
								if (level_point_list[num7].GetLevelID() == list[num8])
								{
									level_point_list[num7].SetLevelOpen();
									level_point_list[num7].ShowWayPoint();
									level_point_list[num7].ShowWayEx();
								}
							}
							count = list.Count + 1;
						}
						else
						{
							Debug.Log("warning! no level open list!");
						}
						if (list2 != null)
						{
							for (int num9 = 0; num9 < list2.Count; num9++)
							{
								if (level_point_list[num7].GetLevelID() == list2[num9])
								{
									level_point_list[num7].SetLevelDisable();
									level_point_list[num7].HideWayPoint();
									level_point_list[num7].HideWayEx();
								}
							}
						}
						else
						{
							Debug.Log("warning! no level no open list!");
						}
						if (level_point_list[num7].GetLevelID() == now_level)
						{
							levelPoint = level_point_list[num7];
							zero.x = level_point_list[num7].transform.position.x - base.transform.position.x;
						}
						if (level_point_list[num7].GetLevelID() == next_level)
						{
							newLevelOpen = level_point_list[num7];
						}
					}
					levelPoint.SetNewLevelOpen(newLevelOpen);
				}
				else
				{
					Debug.Log("no info found!");
				}
				break;
			}
			case MapEnterType.SearchGoods:
				if (level_point_list != null)
				{
					for (int num3 = 0; num3 < level_point_list.Length; num3++)
					{
						if (list != null)
						{
							for (int num4 = 0; num4 < list.Count; num4++)
							{
								if (level_point_list[num3].GetLevelID() == list[num4])
								{
									level_point_list[num3].SetLevelOpen();
									level_point_list[num3].ShowWayPoint();
									level_point_list[num3].ShowWayEx();
								}
							}
							count = list.Count;
						}
						else
						{
							Debug.Log("warning! no level open list!");
						}
						if (list2 != null)
						{
							for (int num5 = 0; num5 < list2.Count; num5++)
							{
								if (level_point_list[num3].GetLevelID() == list2[num5])
								{
									level_point_list[num3].SetLevelDisable();
									level_point_list[num3].HideWayPoint();
									level_point_list[num3].HideWayEx();
								}
							}
						}
						else
						{
							Debug.Log("warning! no level no open list!");
						}
						if (list3 != null)
						{
							for (int num6 = 0; num6 < list3.Count; num6++)
							{
								if (level_point_list[num3].GetLevelID() == list3[num6])
								{
									level_point_list[num3].OpenLevelAnimation();
									zero.x = level_point_list[num3].transform.position.x - base.transform.position.x;
								}
								else if (level_point_list[num3].FindGoodsDropLevelEx(list3[num6]))
								{
									zero.x = level_point_list[num3].transform.position.x - base.transform.position.x;
								}
							}
						}
						else
						{
							Debug.Log("warning! no level goods drop list!");
						}
						if (level_point_list[num3].GetLevelID() == now_level)
						{
							level_point_list[num3].HideWayPoint();
							level_point_list[num3].ShowWayEx();
						}
					}
				}
				else
				{
					Debug.Log("error! no info found!");
				}
				break;
			}
			ShowMask(num);
			SetScreenPos(zero);
		}
		else
		{
			Debug.Log("error! you have no open level!!");
		}
	}

	public void SetLevelInfo(TUILevelInfo m_level_info)
	{
		if (m_level_info == null)
		{
			Debug.Log("error! no level info!");
			return;
		}
		if (level_point_list == null)
		{
			Debug.Log("error! no level list!");
			return;
		}
		for (int i = 0; i < level_point_list.Length; i++)
		{
			if (level_point_list[i].GetLevelID() == m_level_info.id)
			{
				level_point_list[i].SetLevelInfo(m_level_info);
			}
		}
	}

	public void ShowMask(int m_id)
	{
		if (mask_list == null || mask_list.Length < m_id - 1 || m_id < 1)
		{
			Debug.Log("error!");
			return;
		}
		if (m_id == 5)
		{
			for (int i = 0; i < mask_list.Length; i++)
			{
				mask_list[i].gameObject.SetActiveRecursively(false);
			}
			return;
		}
		for (int j = 0; j < mask_list.Length; j++)
		{
			if (j == m_id - 1)
			{
				mask_list[j].gameObject.SetActiveRecursively(true);
			}
			else
			{
				mask_list[j].gameObject.SetActiveRecursively(false);
			}
		}
	}
}
