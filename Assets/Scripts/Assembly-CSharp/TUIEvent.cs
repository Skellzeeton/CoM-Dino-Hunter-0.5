public class TUIEvent
{
	public class SendEvent_SceneMainMenu
	{
		private string name;

		private int wparam;

		private int lparam;

		public SendEvent_SceneMainMenu(string m_name, int m_wparam = 0, int m_lparam = 0)
		{
			name = m_name;
			wparam = m_wparam;
			lparam = m_lparam;
		}

		public string GetEventName()
		{
			return name;
		}

		public int GetWParam()
		{
			return wparam;
		}

		public int GetLparam()
		{
			return lparam;
		}
	}

	public class BackEvent_SceneMainMenu
	{
		private string name;

		private TUIGameInfo info;

		private bool control_success;

		public BackEvent_SceneMainMenu(string m_name, TUIGameInfo m_info, bool m_success = false)
		{
			name = m_name;
			info = m_info;
			control_success = m_success;
		}

		public BackEvent_SceneMainMenu(string m_name, bool m_success = false)
		{
			name = m_name;
			info = null;
			control_success = m_success;
		}

		public string GetEventName()
		{
			return name;
		}

		public TUIGameInfo GetEventInfo()
		{
			return info;
		}

		public bool GetControlSuccess()
		{
			return control_success;
		}
	}

	public class SendEvent_SceneEquip
	{
		private string name;

		private int wparam;

		private int lparam;

		public SendEvent_SceneEquip(string m_name, int m_wparam = 0, int m_lparam = 0)
		{
			name = m_name;
			wparam = m_wparam;
			lparam = m_lparam;
		}

		public string GetEventName()
		{
			return name;
		}

		public int GetWParam()
		{
			return wparam;
		}

		public int GetLparam()
		{
			return lparam;
		}
	}

	public class BackEvent_SceneEquip
	{
		private string name;

		private TUIGameInfo info;

		private bool control_success;

		public BackEvent_SceneEquip(string m_name, TUIGameInfo m_info, bool m_success = false)
		{
			name = m_name;
			info = m_info;
			control_success = m_success;
		}

		public BackEvent_SceneEquip(string m_name, bool m_success = false)
		{
			name = m_name;
			info = null;
			control_success = m_success;
		}

		public string GetEventName()
		{
			return name;
		}

		public TUIGameInfo GetEventInfo()
		{
			return info;
		}

		public bool GetControlSuccess()
		{
			return control_success;
		}
	}

	public class SendEvent_SceneStash
	{
		private string name;

		private int wparam;

		private int lparam;

		private int rparam;

		public SendEvent_SceneStash(string m_name, int m_wparam = 0, int m_lparam = 0, int m_rparam = 0)
		{
			name = m_name;
			wparam = m_wparam;
			lparam = m_lparam;
			rparam = m_rparam;
		}

		public string GetEventName()
		{
			return name;
		}

		public int GetWParam()
		{
			return wparam;
		}

		public int GetLparam()
		{
			return lparam;
		}

		public int GetRparam()
		{
			return rparam;
		}
	}

	public class BackEvent_SceneStash
	{
		private string name;

		private TUIGameInfo info;

		private bool control_success;

		public BackEvent_SceneStash(string m_name, TUIGameInfo m_info, bool m_success = false)
		{
			name = m_name;
			info = m_info;
			control_success = m_success;
		}

		public BackEvent_SceneStash(string m_name, bool m_success = false)
		{
			name = m_name;
			info = null;
			control_success = m_success;
		}

		public string GetEventName()
		{
			return name;
		}

		public TUIGameInfo GetEventInfo()
		{
			return info;
		}

		public bool GetControlSuccess()
		{
			return control_success;
		}
	}

	public class SendEvent_SceneSkill
	{
		private string name;

		private int wparam;

		private int lparam;

		public SendEvent_SceneSkill(string m_name, int m_wparam = 0, int m_lparam = 0)
		{
			name = m_name;
			wparam = m_wparam;
			lparam = m_lparam;
		}

		public string GetEventName()
		{
			return name;
		}

		public int GetWParam()
		{
			return wparam;
		}

		public int GetLparam()
		{
			return lparam;
		}
	}

	public class BackEvent_SceneSkill
	{
		private string name;

		private TUIGameInfo info;

		private bool control_success;

		public BackEvent_SceneSkill(string m_name, TUIGameInfo m_info, bool m_success = false)
		{
			name = m_name;
			info = m_info;
			control_success = m_success;
		}

		public BackEvent_SceneSkill(string m_name, bool m_success = false)
		{
			name = m_name;
			info = null;
			control_success = m_success;
		}

		public string GetEventName()
		{
			return name;
		}

		public TUIGameInfo GetEventInfo()
		{
			return info;
		}

		public bool GetControlSuccess()
		{
			return control_success;
		}
	}

	public class SendEvent_SceneForge
	{
		private string name;

		private int wparam;

		private int lparam;

		private int rparam;

		public SendEvent_SceneForge(string m_name, int m_wparam = 0, int m_lparam = 0, int m_rparam = 0)
		{
			name = m_name;
			wparam = m_wparam;
			lparam = m_lparam;
			rparam = m_rparam;
		}

		public string GetEventName()
		{
			return name;
		}

		public int GetWParam()
		{
			return wparam;
		}

		public int GetLparam()
		{
			return lparam;
		}

		public int GetRparam()
		{
			return rparam;
		}
	}

	public class BackEvent_SceneForge
	{
		private string name;

		private TUIGameInfo info;

		private bool control_success;

		private int wparam;

		private int lparam;

		public BackEvent_SceneForge(string m_name, TUIGameInfo m_info, bool m_success = false)
		{
			name = m_name;
			info = m_info;
			control_success = m_success;
			wparam = 0;
			lparam = 0;
		}

		public BackEvent_SceneForge(string m_name, bool m_success = false, int m_wparam = 0, int m_lparam = 0)
		{
			name = m_name;
			info = null;
			control_success = m_success;
			wparam = m_wparam;
			lparam = m_lparam;
		}

		public string GetEventName()
		{
			return name;
		}

		public TUIGameInfo GetEventInfo()
		{
			return info;
		}

		public bool GetControlSuccess()
		{
			return control_success;
		}

		public int GetWparam()
		{
			return wparam;
		}

		public int GetLparam()
		{
			return lparam;
		}
	}

	public class SendEvent_SceneTavern
	{
		private string name;

		private int wparam;

		private int lparam;

		public SendEvent_SceneTavern(string m_name, int m_wparam = 0, int m_lparam = 0)
		{
			name = m_name;
			wparam = m_wparam;
			lparam = m_lparam;
		}

		public string GetEventName()
		{
			return name;
		}

		public int GetWParam()
		{
			return wparam;
		}

		public int GetLparam()
		{
			return lparam;
		}
	}

	public class BackEvent_SceneTavern
	{
		private string name;

		private TUIGameInfo info;

		private bool control_success;

		public BackEvent_SceneTavern(string m_name, TUIGameInfo m_info, bool m_success = false)
		{
			name = m_name;
			info = m_info;
			control_success = m_success;
		}

		public BackEvent_SceneTavern(string m_name, bool m_success = false)
		{
			name = m_name;
			info = null;
			control_success = m_success;
		}

		public string GetEventName()
		{
			return name;
		}

		public TUIGameInfo GetEventInfo()
		{
			return info;
		}

		public bool GetControlSuccess()
		{
			return control_success;
		}
	}

	public class SendEvent_SceneMap
	{
		private string name;

		private int wparam;

		private int lparam;

		public SendEvent_SceneMap(string m_name, int m_wparam = 0, int m_lparam = 0)
		{
			name = m_name;
			wparam = m_wparam;
			lparam = m_lparam;
		}

		public string GetEventName()
		{
			return name;
		}

		public int GetWParam()
		{
			return wparam;
		}

		public int GetLparam()
		{
			return lparam;
		}
	}

	public class BackEvent_SceneMap
	{
		private string name;

		private TUIGameInfo info;

		private bool control_success;

		private string str;

		public BackEvent_SceneMap(string m_name, TUIGameInfo m_info, bool m_success = false)
		{
			name = m_name;
			info = m_info;
			control_success = m_success;
			str = string.Empty;
		}

		public BackEvent_SceneMap(string m_name, bool m_success = false)
		{
			name = m_name;
			info = null;
			control_success = m_success;
			str = string.Empty;
		}

		public BackEvent_SceneMap(string m_name, string m_str)
		{
			name = m_name;
			info = null;
			control_success = false;
			str = m_str;
		}

		public string GetEventName()
		{
			return name;
		}

		public TUIGameInfo GetEventInfo()
		{
			return info;
		}

		public bool GetControlSuccess()
		{
			return control_success;
		}

		public string GetStr()
		{
			return str;
		}
	}
}
