using System.Collections.Generic;
using System.Xml;
using UnityEngine;

namespace gyIAPSystem
{
	public class CIAPCenter
	{
		protected Dictionary<int, CIAPInfo> m_dictIAPInfo;

		public CIAPCenter()
		{
			m_dictIAPInfo = new Dictionary<int, CIAPInfo>();
		}

		public Dictionary<int, CIAPInfo> GetData()
		{
			return m_dictIAPInfo;
		}

		public CIAPInfo Get(int nID)
		{
			if (!m_dictIAPInfo.ContainsKey(nID))
			{
				return null;
			}
			return m_dictIAPInfo[nID];
		}

		public bool Load()
		{
			string content = string.Empty;
			if (MyUtils.isWindows)
			{
				if (!Utils.FileGetString("iap.xml", ref content))
				{
					return false;
				}
			}
			else if (MyUtils.isIOS || MyUtils.isAndroid)
			{
				TextAsset textAsset = (TextAsset)Resources.Load("_Config/iap", typeof(TextAsset));
				if (textAsset == null)
				{
					return false;
				}
				content = textAsset.ToString();
			}
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.LoadXml(content);
			string value = string.Empty;
			XmlNode documentElement = xmlDocument.DocumentElement;
			foreach (XmlNode childNode in documentElement.ChildNodes)
			{
				if (!(childNode.Name != "iap") && GetAttribute(childNode, "id", ref value))
				{
					CIAPInfo cIAPInfo = new CIAPInfo();
					cIAPInfo.nID = int.Parse(value);
					if (GetAttribute(childNode, "key", ref value))
					{
						cIAPInfo.sKey = value;
					}
					if (GetAttribute(childNode, "iscrystal", ref value))
					{
						cIAPInfo.isCrystal = bool.Parse(value);
					}
					if (GetAttribute(childNode, "value", ref value))
					{
						cIAPInfo.nValue = int.Parse(value);
					}
					m_dictIAPInfo.Add(cIAPInfo.nID, cIAPInfo);
				}
			}
			return true;
		}

		protected bool GetAttribute(XmlNode node, string name, ref string value)
		{
			if (node == null || node.Attributes[name] == null)
			{
				return false;
			}
			value = node.Attributes[name].Value.Trim();
			if (value.Length < 1)
			{
				return false;
			}
			return true;
		}
	}
}
