using System.Collections.Generic;

namespace gyAchievementSystem
{
	public class CAchievementManager
	{
		protected static CAchievementManager m_Instance;

		protected CAchievementCenter m_AchievementCenter;

		protected List<CAchievementTip> m_ltAchievementTip;

		public static CAchievementManager GetInstance()
		{
			if (m_Instance == null)
			{
				m_Instance = new CAchievementManager();
				m_Instance.Initialize();
			}
			return m_Instance;
		}

		public CAchievementTip PopTip()
		{
			CAchievementTip result = m_ltAchievementTip[0];
			m_ltAchievementTip.RemoveAt(0);
			return result;
		}

		public int GetTipCount()
		{
			return m_ltAchievementTip.Count;
		}

		public void Initialize()
		{
			m_AchievementCenter = new CAchievementCenter();
			m_AchievementCenter.LoadInfo();
			m_AchievementCenter.LoadData();
			m_ltAchievementTip = new List<CAchievementTip>();
		}

		public void Update(float deltaTime)
		{
		}

		protected void AddAchievementCount(int nID, int nCount)
		{
			CAchievementInfo info = m_AchievementCenter.GetInfo(nID);
			if (info == null)
			{
				return;
			}
			CAchievementData data = m_AchievementCenter.GetData(nID);
			if (data == null)
			{
				return;
			}
			int nCurValue = data.nCurValue;
			data.nCurValue += nCount;
			int stepCount = info.GetStepCount();
			for (int i = 0; i < stepCount; i++)
			{
				CAchievementStep step = info.GetStep(i);
				if (step != null && nCurValue < step.nStepPurpose && data.nCurValue >= step.nStepPurpose)
				{
					AddAchievementTip(info.nID, info.sName, i + 1);
					if (i == stepCount - 1)
					{
						data.nState = 2;
					}
					m_AchievementCenter.SaveData();
					break;
				}
			}
		}

		protected void AddAchievementTip(int nID, string sName, int nStep)
		{
			CAchievementTip cAchievementTip = new CAchievementTip();
			if (cAchievementTip != null)
			{
				cAchievementTip.nID = nID;
				cAchievementTip.sName = sName;
				cAchievementTip.nStep = nStep;
				m_ltAchievementTip.Add(cAchievementTip);
			}
		}

		public void KillMonster(int nID, int nType)
		{
			Dictionary<int, CAchievementInfo> dataInfo = m_AchievementCenter.GetDataInfo();
			if (dataInfo == null)
			{
				return;
			}
			int nValue = 0;
			foreach (CAchievementInfo value in dataInfo.Values)
			{
				CAchievementData cAchievementData = m_AchievementCenter.GetData(value.nID);
				if (cAchievementData == null)
				{
					cAchievementData = new CAchievementData();
					cAchievementData.nID = value.nID;
					cAchievementData.nState = 1;
					cAchievementData.nCurValue = 0;
					m_AchievementCenter.AddData(cAchievementData.nID, cAchievementData);
				}
				if (cAchievementData.nState == 1)
				{
					int nType2 = value.nType;
					if (nType2 == 1 || (nType2 == 2 && value.GetParam(0, ref nValue) && nValue == nID))
					{
						AddAchievementCount(value.nID, 1);
					}
				}
			}
		}
	}
}
