using System.Collections.Generic;

public class GameLevelInfo
{
	public int nID;

	public string sSceneName;

	public bool bIsSkyScene;

	public string sLevelName;

	public string sLevelDesc;

	public float fNavPlane;

	public int nBirthPos;

	public List<int> ltGameWave;

	public int nDefaultSPGround;

	public int nDefaultSPSky;

	public List<StartPointTrigger> ltSPTrigger;

	public int nDefaultHoverPoint;

	public List<StartPointTrigger> ltHPTrigger;

	public List<MonsterNumLimitInfo> ltMonsterNumLimit;

	public int nTPBeginCfg;

	public int nTPEndCfg;

	public int nTaskID;

	public int nRewardExp;

	public int nRewardGold;

	public List<CRewardMaterial> ltRewardMaterial;

	public string sCutScene;

	public string sCutSceneContent;

	public string sCutSceneAmbience;

	public string sBGM;

	public string sBGMAmbience;

	public GameLevelInfo()
	{
		ltGameWave = new List<int>();
		ltSPTrigger = new List<StartPointTrigger>();
		ltHPTrigger = new List<StartPointTrigger>();
		ltMonsterNumLimit = new List<MonsterNumLimitInfo>();
		sCutScene = string.Empty;
		sCutSceneContent = string.Empty;
		sCutSceneAmbience = string.Empty;
		sBGM = string.Empty;
		sBGMAmbience = string.Empty;
		ltRewardMaterial = new List<CRewardMaterial>();
	}
}
