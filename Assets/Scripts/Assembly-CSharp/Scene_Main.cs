using UnityEngine;

public class Scene_Main : MonoBehaviour
{
	public TUIFade m_fade;

	private float m_fade_in_time;

	private float m_fade_out_time;

	private bool do_fade_in;

	private bool is_fade_out;

	private bool do_fade_out;

	private string next_scene = string.Empty;

	private void Awake()
	{
		TUIDataServer.Instance().Initialize();
	}

	private void Start()
	{
		Application.targetFrameRate = 60;
		CUISound.GetInstance().Play("BGM_theme");
	}

	private void Update()
	{
		if (m_fade == null)
		{
			Debug.Log("error!no found m_fade!");
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
			do_fade_out = true;
			TUIMappingInfo.SwitchScene switchScene = TUIMappingInfo.Instance().GetSwitchScene();
			if (switchScene != null)
			{
				switchScene(next_scene);
			}
		}
	}

	public void TUIEvent_Enter(TUIControl control, int event_type, float wparam, float lparam, object data)
	{
		if (event_type == 3 && !is_fade_out)
		{
			is_fade_out = true;
			next_scene = "Scene_MainMenu";
			m_fade.FadeOut();
		}
	}
}
