using EventCenter;
using UnityEngine;

public class Scene_MainMenu : MonoBehaviour
{
	public TUIFade m_fade;

	public Camera_Village camera_village;

	public Popup_Option popup_option;

	public Top_Bar top_bar;

	public Transform go_forge;

	public Transform go_forge_name;

	public Transform go_tavern;

	public Transform go_tavern_name;

	public Transform go_skill;

	public Transform go_skill_name;

	public Transform go_stash;

	public Transform go_stash_name;

	public Transform go_camp;

	public Transform go_camp_name;

	public UnlockBlink unlock_blink;

	private Transform go_control;

	private bool is_click;

	private float m_fade_in_time;

	private float m_fade_out_time;

	private bool do_fade_in;

	private bool is_fade_out;

	private bool do_fade_out;

	private string next_scene = string.Empty;

	private void Awake()
	{
		TUIDataServer.Instance().Initialize();
		global::EventCenter.EventCenter.Instance.Register<TUIEvent.BackEvent_SceneMainMenu>(TUIEvent_SetUIInfo);
		camera_village.SetCurrentAngle(TUIMappingInfo.Instance().GetCurrentAngle());
	}

	private void Start()
	{
		global::EventCenter.EventCenter.Instance.Publish(this, new TUIEvent.SendEvent_SceneMainMenu("TUIEvent_TopBar"));
		global::EventCenter.EventCenter.Instance.Publish(this, new TUIEvent.SendEvent_SceneMainMenu("TUIEvent_OptionInfo"));
		global::EventCenter.EventCenter.Instance.Publish(this, new TUIEvent.SendEvent_SceneMainMenu("TUIEvent_EnterInfo"));
	}

	private void Update()
	{
		LookAtCamera();
		if (m_fade == null)
		{
			Debug.Log("error! no found m_fade!");
			return;
		}
		m_fade_in_time += Time.deltaTime;
		if (m_fade_in_time >= m_fade.fadeInTime && !do_fade_in)
		{
			do_fade_in = true;
		}
		if (!is_fade_out)
		{
			return;
		}
		m_fade_out_time += Time.deltaTime;
		if (m_fade_out_time >= m_fade.fadeOutTime && !do_fade_out)
		{
			TUIMappingInfo.Instance().SetCurrentAngle(camera_village.GetCurrentAngle());
			do_fade_out = true;
			TUIMappingInfo.SwitchScene switchScene = TUIMappingInfo.Instance().GetSwitchScene();
			if (switchScene != null)
			{
				switchScene(next_scene);
			}
		}
	}

	private void OnDestroy()
	{
		global::EventCenter.EventCenter.Instance.Unregister<TUIEvent.BackEvent_SceneMainMenu>(TUIEvent_SetUIInfo);
	}

	public void TUIEvent_SetUIInfo(object sender, TUIEvent.BackEvent_SceneMainMenu m_event)
	{
		if (m_event.GetEventName() == "TUIEvent_TopBar")
		{
			if (m_event.GetEventInfo().player_info != null)
			{
				int level = m_event.GetEventInfo().player_info.level;
				int exp = m_event.GetEventInfo().player_info.exp;
				int level_exp = m_event.GetEventInfo().player_info.level_exp;
				int gold = m_event.GetEventInfo().player_info.gold;
				int crystal = m_event.GetEventInfo().player_info.crystal;
				top_bar.SetAllValue(level, exp, level_exp, gold, crystal);
			}
			else
			{
				Debug.Log("error!");
			}
		}
		else if (m_event.GetEventName() == "TUIEvent_OptionInfo")
		{
			if (m_event.GetEventInfo() != null && m_event.GetEventInfo().option_info != null)
			{
				bool music_open = m_event.GetEventInfo().option_info.music_open;
				bool sfx_open = m_event.GetEventInfo().option_info.sfx_open;
				popup_option.SetOption(music_open, sfx_open);
			}
			else
			{
				Debug.Log("error!");
			}
		}
		else if (m_event.GetEventName() == "TUIEvent_ChangeOption")
		{
			if (m_event.GetControlSuccess())
			{
				popup_option.ChangeOption();
			}
			else
			{
				popup_option.RestoreOption();
			}
		}
		else if (m_event.GetEventName() == "TUIEvent_EnterInfo" && m_event.GetEventInfo() != null)
		{
			Debug.Log("!!!");
		}
	}

	public void TUIEvent_CameraMove(TUIControl control, int event_type, float wparam, float lparam, object data)
	{
		switch (event_type)
		{
		case 1:
		{
			if (go_control != null)
			{
				break;
			}
			camera_village.DoBegin();
			Vector2 clickPosition2 = control.GetComponent<TUIMoveEx>().GetClickPosition();
			Ray ray2 = camera_village.GetComponent<Camera>().ScreenPointToRay(new Vector3(clickPosition2.x, clickPosition2.y, 0f));
			Debug.DrawRay(ray2.origin, ray2.direction * 600f, Color.green);
			RaycastHit hitInfo2;
			if (Physics.Raycast(ray2, out hitInfo2))
			{
				Debug.Log("you hit: " + hitInfo2.transform.name);
				if (hitInfo2.transform == go_camp)
				{
					next_scene = "Scene_Equip";
					go_control = go_camp_name;
					PlayForwardAnimation(go_camp_name);
				}
				else if (hitInfo2.transform == go_forge)
				{
					next_scene = "Scene_Forge";
					go_control = go_forge_name;
					PlayForwardAnimation(go_forge_name);
				}
				else if (hitInfo2.transform == go_tavern)
				{
					next_scene = "Scene_Tavern";
					go_control = go_tavern_name;
					PlayForwardAnimation(go_tavern_name);
				}
				else if (hitInfo2.transform == go_skill)
				{
					next_scene = "Scene_Skill";
					go_control = go_skill_name;
					PlayForwardAnimation(go_skill_name);
				}
				else if (hitInfo2.transform == go_stash)
				{
					next_scene = "Scene_Stash";
					go_control = go_stash_name;
					PlayForwardAnimation(go_stash_name);
				}
			}
			break;
		}
		case 2:
			if (!is_click)
			{
				PlayBackwardAnimation(go_control);
				go_control = null;
				camera_village.DoMoveBegin();
			}
			break;
		case 3:
			if (!is_click)
			{
				camera_village.DoMove(wparam);
			}
			break;
		case 4:
			if (!is_click)
			{
				camera_village.DoMoveEnd();
			}
			break;
		case 5:
		{
			PlayBackwardAnimation(go_control);
			Vector2 clickPosition = control.GetComponent<TUIMoveEx>().GetClickPosition();
			Ray ray = camera_village.GetComponent<Camera>().ScreenPointToRay(new Vector3(clickPosition.x, clickPosition.y, 0f));
			Debug.DrawRay(ray.origin, ray.direction * 300f, Color.green);
			RaycastHit hitInfo;
			if (Physics.Raycast(ray, out hitInfo))
			{
				if (hitInfo.transform.name == "shop_camp")
				{
					next_scene = "Scene_Equip";
					camera_village.SetCloser(go_camp);
					is_click = true;
					SendMessage("TUIEvent_FadeOut");
				}
				else if (hitInfo.transform.name == "shop_forge")
				{
					next_scene = "Scene_Forge";
					camera_village.SetCloser(go_forge);
					is_click = true;
					SendMessage("TUIEvent_FadeOut");
				}
				else if (hitInfo.transform.name == "shop_tavern")
				{
					next_scene = "Scene_Tavern";
					camera_village.SetCloser(go_tavern);
					is_click = true;
					SendMessage("TUIEvent_FadeOut");
				}
				else if (hitInfo.transform.name == "shop_get skills")
				{
					next_scene = "Scene_Skill";
					camera_village.SetCloser(go_skill);
					is_click = true;
					SendMessage("TUIEvent_FadeOut");
				}
				else if (hitInfo.transform.name == "shop_stash")
				{
					next_scene = "Scene_Stash";
					camera_village.SetCloser(go_stash);
					is_click = true;
					SendMessage("TUIEvent_FadeOut");
				}
			}
			break;
		}
		}
	}

	public void TUIEvent_Acheviement(TUIControl control, int event_type, float wparam, float lparam, object data)
	{
		if (event_type == 3)
		{
			global::EventCenter.EventCenter.Instance.Publish(this, new TUIEvent.SendEvent_SceneMainMenu("TUIEvent_SetAcheviement"));
		}
	}

	public void TUIEvent_Option(TUIControl control, int event_type, float wparam, float lparam, object data)
	{
		if (event_type == 3)
		{
			popup_option.Show();
			global::EventCenter.EventCenter.Instance.Publish(this, new TUIEvent.SendEvent_SceneMainMenu("TUIEvent_SetOption"));
		}
	}

	public void TUIEvent_BtnMusic(TUIControl control, int event_type, float wparam, float lparam, object data)
	{
		popup_option.SetMusicNow();
	}

	public void TUIEvent_BtnSFX(TUIControl control, int event_type, float wparam, float lparam, object data)
	{
		popup_option.SetSFXNow();
	}

	public void TUIEvent_CloseOption(TUIControl control, int event_type, float wparam, float lparam, object data)
	{
		if (event_type == 3)
		{
			popup_option.Hide();
			if (popup_option.IsChange())
			{
				int wparam2 = (popup_option.GetMusicNow() ? 1 : 0);
				int lparam2 = (popup_option.GetSFXNow() ? 1 : 0);
				global::EventCenter.EventCenter.Instance.Publish(this, new TUIEvent.SendEvent_SceneMainMenu("TUIEvent_ChangeOption", wparam2, lparam2));
			}
		}
	}

	public void TUIEvent_Map(TUIControl control, int event_type, float wparam, float lparam, object data)
	{
		if (event_type == 3 && !is_fade_out)
		{
			is_fade_out = true;
			m_fade.FadeOut();
			next_scene = "Scene_Map";
		}
	}

	public void TUIEvent_FadeOut()
	{
		if (!is_fade_out)
		{
			is_fade_out = true;
			m_fade.FadeOut();
		}
	}

	private void PlayForwardAnimation(Transform go)
	{
		if (!(go == null))
		{
			AnimationState animationState = go.GetComponent<Animation>()[go.GetComponent<Animation>().clip.name];
			animationState.speed = 1f;
			animationState.normalizedTime = 0f;
			go.GetComponent<Animation>().Play(animationState.name, PlayMode.StopAll);
		}
	}

	private void PlayBackwardAnimation(Transform go)
	{
		if (!(go == null))
		{
			AnimationState animationState = go.GetComponent<Animation>()[go.GetComponent<Animation>().clip.name];
			animationState.speed = -1f;
			animationState.normalizedTime = 1f;
			go.GetComponent<Animation>().Play(animationState.name, PlayMode.StopAll);
		}
	}

	private void LookAtCamera()
	{
		float num = 0f;
		num = Quaternion.FromToRotation(go_forge_name.position, camera_village.transform.position - go_forge_name.position).eulerAngles.y;
		go_forge_name.transform.localEulerAngles = new Vector3(go_forge_name.localEulerAngles.x, -90f + num, go_forge_name.localEulerAngles.z);
		num = Quaternion.FromToRotation(go_tavern_name.position, camera_village.transform.position - go_tavern_name.position).eulerAngles.y;
		go_tavern_name.transform.localEulerAngles = new Vector3(go_tavern_name.localEulerAngles.x, -90f + num, go_tavern_name.localEulerAngles.z);
		num = Quaternion.FromToRotation(go_skill_name.position, camera_village.transform.position - go_skill_name.position).eulerAngles.y;
		go_skill_name.transform.localEulerAngles = new Vector3(go_skill_name.localEulerAngles.x, -90f + num, go_skill_name.localEulerAngles.z);
		num = Quaternion.FromToRotation(go_stash_name.position, camera_village.transform.position - go_stash_name.position).eulerAngles.y;
		go_stash_name.transform.localEulerAngles = new Vector3(go_stash_name.localEulerAngles.x, -90f + num, go_stash_name.localEulerAngles.z);
		num = Quaternion.FromToRotation(go_camp_name.position, camera_village.transform.position - go_camp_name.position).eulerAngles.y;
		go_camp_name.transform.localEulerAngles = new Vector3(go_camp_name.localEulerAngles.x, -90f + num, go_camp_name.localEulerAngles.z);
	}
}
