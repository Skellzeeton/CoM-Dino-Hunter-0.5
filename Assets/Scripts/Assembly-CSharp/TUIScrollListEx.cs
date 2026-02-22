using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("TUI/Control/TUIScrollListEx")]
public class TUIScrollListEx : TUIControlImpl
{
	protected enum CommandType
	{
		Command_Begin = 0,
		Command_Move = 1,
		Command_End = 2,
		Command_Free = 3
	}

	public List<GameObject> items_list;

	public bool open_child_event;

	public float min_x;

	public float min_y;

	public float size_scale = 0.6f;

	public float move_z;

	public float sensivity = 100f;

	public float offset_y;

	public bool open_fade;

	protected int finger_id = -1;

	protected Vector2 finger_position = Vector2.zero;

	protected bool move;

	protected CommandType command_type = CommandType.Command_Free;

	[SerializeField]
	private int now_index;

	public void Add(GameObject go)
	{
		items_list.Add(go);
	}

	public override bool HandleInput(TUIInput input)
	{
		if (open_child_event)
		{
			base.HandleInput(input);
		}
		switch (input.inputType)
		{
		case TUIInputType.Began:
			if (PtInControl(input.position))
			{
				if (move)
				{
					command_type = CommandType.Command_Free;
				}
				finger_id = input.fingerId;
				finger_position = input.position;
				move = false;
			}
			return false;
		case TUIInputType.Moved:
			if (input.fingerId != finger_id)
			{
				return false;
			}
			if (PtInControl(input.position))
			{
				float num = input.position.x - finger_position.x;
				float num2 = input.position.y - finger_position.y;
				if (move)
				{
					finger_position = input.position;
					command_type = CommandType.Command_Move;
					UpdateItems(num, num2);
				}
				else
				{
					float num3 = Mathf.Abs(num);
					float num4 = Mathf.Abs(num2);
					if (num3 > min_x || num4 > min_y)
					{
						move = true;
						finger_position = input.position;
						command_type = CommandType.Command_Move;
					}
				}
				return true;
			}
			return false;
		case TUIInputType.Ended:
			if (input.fingerId != finger_id)
			{
				return false;
			}
			if (move)
			{
				move = false;
				finger_id = -1;
				finger_position = Vector2.zero;
				command_type = CommandType.Command_Free;
				return true;
			}
			finger_id = -1;
			finger_position = Vector2.zero;
			return false;
		default:
			return false;
		}
	}

	private void Update()
	{
		UpdateItems(Time.deltaTime);
	}

	private void UpdateItems(float wparam, float lparam)
	{
		if (command_type != CommandType.Command_Move || items_list == null || items_list.Count <= 0)
		{
			return;
		}
		if (wparam > 0f)
		{
			float x = items_list[0].transform.localPosition.x;
			if (x > 0f)
			{
				float num = Mathf.Abs(x) * 0.1f;
				if (num < 1f)
				{
					num = 1f;
				}
				wparam /= num;
			}
		}
		else if (wparam < 0f)
		{
			float x2 = items_list[items_list.Count - 1].transform.localPosition.x;
			if (x2 < 0f)
			{
				float num2 = Mathf.Abs(x2) * 0.1f;
				if (num2 < 1f)
				{
					num2 = 1f;
				}
				wparam /= num2;
			}
		}
		for (int i = 0; i < items_list.Count; i++)
		{
			GameObject gameObject = items_list[i];
			Vector2 vector = new Vector2(gameObject.transform.localPosition.x, gameObject.transform.localPosition.y);
			float num3 = 0f;
			if (sensivity != 0f)
			{
				num3 = Mathf.Abs(vector.x + wparam) / sensivity * (0f - size_scale) + 1f;
			}
			else
			{
				Debug.Log("error!");
			}
			if (num3 < size_scale)
			{
				num3 = size_scale;
			}
			else if (num3 > 1f)
			{
				num3 = 1f;
			}
			float num4 = offset_y * (1f - (1f - num3) / (1f - size_scale));
			float z = move_z * ((num3 - size_scale) / (1f - size_scale));
			Vector3 localPosition = new Vector3(vector.x + wparam, num4 / 2f, z);
			if (open_fade)
			{
				TUIMeshSprite[] componentsInChildren = gameObject.GetComponentsInChildren<TUIMeshSprite>();
				for (int j = 0; j < componentsInChildren.Length; j++)
				{
					if (num3 >= 0.7f)
					{
						componentsInChildren[j].color = new Color(1f, 1f, 1f, 1f);
					}
					else
					{
						componentsInChildren[j].color = new Color(1f, 1f, 1f, -18.33f * num3 * num3 + 31.83f * num3 - 12.5f);
					}
				}
			}
			gameObject.transform.localScale = new Vector3(num3, num3, 1f);
			gameObject.transform.localPosition = localPosition;
		}
	}

	private void UpdateItems(float delta_time)
	{
		if (command_type != CommandType.Command_Free || items_list.Count <= 0)
		{
			return;
		}
		GameObject gameObject = null;
		int index = 0;
		float num = 0f;
		float num2 = 0f;
		for (int i = 0; i < items_list.Count; i++)
		{
			gameObject = items_list[index];
			GameObject gameObject2 = items_list[i];
			num = Mathf.Abs(gameObject.transform.localPosition.x);
			num2 = Mathf.Abs(gameObject2.transform.localPosition.x);
			if (num2 < num)
			{
				index = i;
				now_index = i;
				num = num2;
			}
		}
		now_index = index;
		gameObject = items_list[index];
		num = gameObject.transform.localPosition.x;
		for (int j = 0; j < items_list.Count; j++)
		{
			GameObject gameObject3 = items_list[j];
			Vector2 vector = new Vector2(gameObject3.transform.localPosition.x, gameObject3.transform.localPosition.y);
			float num3 = 0f;
			if (sensivity != 0f)
			{
				num3 = Mathf.Abs(vector.x - num * 5f * delta_time) / sensivity * (0f - size_scale) + 1f;
			}
			else
			{
				Debug.Log("error!");
			}
			if (num3 < size_scale)
			{
				num3 = size_scale;
			}
			else if (num3 > 1f)
			{
				num3 = 1f;
			}
			float num4 = 0f;
			float z = 0f;
			if (1f - size_scale != 0f)
			{
				num4 = offset_y * (1f - (1f - num3) / (1f - size_scale));
				z = move_z * ((num3 - size_scale) / (1f - size_scale));
			}
			else
			{
				Debug.Log("error!");
			}
			Vector3 localPosition = new Vector3(vector.x - num * 5f * delta_time, num4 / 2f, z);
			if (open_fade)
			{
				TUIMeshSprite[] componentsInChildren = gameObject3.GetComponentsInChildren<TUIMeshSprite>();
				for (int k = 0; k < componentsInChildren.Length; k++)
				{
					if (num3 >= 0.7f)
					{
						componentsInChildren[k].color = new Color(1f, 1f, 1f, 1f);
					}
					else
					{
						componentsInChildren[k].color = new Color(1f, 1f, 1f, -18.33f * num3 * num3 + 31.83f * num3 - 12.5f);
					}
				}
			}
			gameObject3.transform.localScale = new Vector3(num3, num3, 1f);
			gameObject3.transform.localPosition = localPosition;
		}
	}

	public int GetNowIndex()
	{
		if (items_list.Count <= 0)
		{
			return -1;
		}
		return now_index;
	}

	public GameObject GetNowItem()
	{
		if (items_list.Count <= 0)
		{
			return null;
		}
		return items_list[now_index];
	}

	public List<GameObject> GetItemsList()
	{
		if (items_list.Count <= 0)
		{
			return null;
		}
		return items_list;
	}
}
