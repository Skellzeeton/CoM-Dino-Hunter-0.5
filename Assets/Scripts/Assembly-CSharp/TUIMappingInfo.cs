using System.Collections.Generic;
using UnityEngine;

public class TUIMappingInfo
{
	public delegate void SwitchScene(string m_next_scene);

	private static TUIMappingInfo instance = null;

	private static Vector3 current_angle = Vector3.zero;

	public Dictionary<int, string> stash_dictionary;

	public Dictionary<int, string> skill_dictionary;

	public Dictionary<int, string> weapon_dictionary;

	public Dictionary<int, string> prop_dictionary;

	public Dictionary<int, string> role_dictionary;

	public SwitchScene switch_scene_function;

	public TUIMappingInfo()
	{
		stash_dictionary = new Dictionary<int, string>();
		stash_dictionary[30001] = "bawanglong_BOSS_ya1";
		stash_dictionary[30002] = "shuangguanlong_BOSS_duzhi1";
		stash_dictionary[30003] = "shuangguanlong_gongyou_longpi1";
		stash_dictionary[30004] = "shuangguanlong_putong_duzhi1";
		stash_dictionary[30005] = "shuangguanlong_bianyi_duzhi1";
		stash_dictionary[30006] = "yilong_gongyou_longzhua1";
		stash_dictionary[30007] = "yilong_BOSS_yizhua1";
		stash_dictionary[30008] = "yilong_putong_yizhua1";
		stash_dictionary[30009] = "yilong_bianyi_yizhua1";
		stash_dictionary[30010] = "sanjiaolong_gongyou_linke1";
		stash_dictionary[30011] = "sanjiaolong_putong_touke1";
		stash_dictionary[30012] = "sanjiaolong_putong_touke1";
		stash_dictionary[30013] = "sanjiaolong_BOSS_touke1";
		stash_dictionary[30014] = "xunmenglong_BOSS_duanwei1";
		stash_dictionary[30015] = "xunmenglong_gongyou_longgu1";
		stash_dictionary[30016] = "xunmenglong_putong_duanwei1";
		stash_dictionary[30017] = "xunmenglong_bianyi_duanwei1";
		stash_dictionary[30018] = "jibeilong_BOSS_weici1";
		stash_dictionary[30019] = "bawanglong_BOSS_ya3";
		stash_dictionary[30020] = "shuangguanlong_BOSS_duzhi3";
		stash_dictionary[30021] = "shuangguanlong_putong_duzhi3";
		stash_dictionary[30022] = "shuangguanlong_bianyi_duzhi3";
		stash_dictionary[30023] = "yilong_BOSS_yizhua";
		stash_dictionary[30024] = "yilong_putong_yizhua3";
		stash_dictionary[30025] = "yilong_bianyi_yizhua3";
		stash_dictionary[30026] = "sanjiaolong_BOSS_touke3";
		stash_dictionary[30027] = "sanjiaolong_putong_touke3";
		stash_dictionary[30028] = "sanjiaolong_bianyi_touke3";
		stash_dictionary[30029] = "xunmenglong_BOSS_duanwei3";
		stash_dictionary[30030] = "xunmenglong_putong_duanwei3";
		stash_dictionary[30031] = "xunmenglong_bianyi_duanwei3";
		stash_dictionary[30032] = "jibeilong_BOSS_weici3";
		stash_dictionary[30033] = "shangu_gongyou_tiekuangshi1";
		stash_dictionary[30034] = "yulin_gongyou_caishuijing1";
		stash_dictionary[30035] = "yanjiang_gongyou_rongyankuai1";
		stash_dictionary[30036] = "bawanglong_BOSS_beijia1";
		stash_dictionary[30037] = "shuangguanlong_gongyou_longpi3";
		stash_dictionary[30038] = "shuangguanlong_BOSS_guan1";
		stash_dictionary[30039] = "shuangguanlong_putong_guan1";
		stash_dictionary[30040] = "shuangguanlong_bianyi_guan1";
		stash_dictionary[30041] = "yilong_gongyou_longzhua3";
		stash_dictionary[30042] = "yilong_BOSS_yimo1";
		stash_dictionary[30043] = "yilong_putong_yimo1";
		stash_dictionary[30044] = "yilong_bianyi_yimo1";
		stash_dictionary[30045] = "sanjiaolong_gongyou_linke3";
		stash_dictionary[30046] = "sanjiaolong_BOSS_jiao1";
		stash_dictionary[30047] = "sanjiaolong_putong_jiao1";
		stash_dictionary[30048] = "sanjiaolong_bianyi_jiao1";
		stash_dictionary[30049] = "xunmenglong_putong_tougu1";
		stash_dictionary[30050] = "xunmenglong_bianyi_tougu1";
		stash_dictionary[30051] = "xunmenglong_BOSS_tougu1";
		stash_dictionary[30052] = "jibeilong_BOSS_xiongmo1";
		stash_dictionary[30053] = "bawanglong_BOSS_beijia3";
		stash_dictionary[30054] = "shuangguanlong_BOSS_guan3";
		stash_dictionary[30055] = "shuangguanlong_putong_guan3";
		stash_dictionary[30056] = "sanjiaolong_BOSS_touke3";
		stash_dictionary[30057] = "yilong_BOSS_yimo3";
		stash_dictionary[30058] = "yilong_putong_yimo3";
		stash_dictionary[30059] = "yilong_bianyi_yimo3";
		stash_dictionary[30060] = "sanjiaolong_BOSS_jiao3";
		stash_dictionary[30061] = "sanjiaolong_putong_jiao3";
		stash_dictionary[30062] = "sanjiaolong_bianyi_jiao3";
		stash_dictionary[30063] = "xunmenglong_gongyou_longgu3";
		stash_dictionary[30064] = "xunmenglong_BOSS_tougu3";
		stash_dictionary[30065] = "xunmenglong_putong_tougu3";
		stash_dictionary[30066] = "xunmenglong_bianyi_tougu3";
		stash_dictionary[30067] = "jibeilong_BOSS_xiongmo3";
		stash_dictionary[30068] = "shangu_gongyou_tiekuangshi3";
		stash_dictionary[30069] = "yulin_gongyou_caishuijing3";
		stash_dictionary[30070] = "yanjiang_gongyou_rongyankuai3";
		stash_dictionary[30071] = "bawanglong_BOSS_touke1";
		stash_dictionary[30072] = "jibeilong_BOSS_gusui1";
		stash_dictionary[30073] = "bawanglong_BOSS_touke3";
		stash_dictionary[30074] = "jibeilong_BOSS_gusui3";
		skill_dictionary = new Dictionary<int, string>();
		skill_dictionary[2] = "chongfeng";
		skill_dictionary[4] = "dunxing";
		skill_dictionary[5] = "huti";
		skill_dictionary[1] = "kuangbao";
		skill_dictionary[3] = "zhiliao";
		skill_dictionary[1001] = "passiveskill_1001";
		skill_dictionary[1002] = "passiveskill_1002";
		skill_dictionary[1003] = "passiveskill_1003";
		skill_dictionary[1004] = "passiveskill_1004";
		skill_dictionary[1005] = "passiveskill_1005";
		skill_dictionary[1006] = "passiveskill_1006";
		skill_dictionary[1007] = "passiveskill_1007";
		skill_dictionary[2001] = "passiveskill_2001";
		skill_dictionary[2002] = "passiveskill_2002";
		skill_dictionary[2003] = "passiveskill_2003";
		skill_dictionary[2004] = "passiveskill_2004";
		skill_dictionary[2005] = "passiveskill_2005";
		skill_dictionary[2006] = "passiveskill_2006";
		skill_dictionary[2007] = "passiveskill_2007";
		skill_dictionary[2008] = "passiveskill_2008";
		skill_dictionary[2009] = "passiveskill_2009";
		skill_dictionary[3001] = "passiveskill_3001";
		skill_dictionary[3002] = "passiveskill_3002";
		skill_dictionary[3003] = "passiveskill_3003";
		skill_dictionary[3004] = "passiveskill_3004";
		skill_dictionary[3005] = "passiveskill_3005";
		skill_dictionary[3006] = "passiveskill_3006";
		skill_dictionary[3007] = "passiveskill_3007";
		skill_dictionary[3008] = "passiveskill_3008";
		skill_dictionary[3009] = "passiveskill_3009";
		skill_dictionary[4001] = "passiveskill_4001";
		skill_dictionary[4002] = "passiveskill_4002";
		skill_dictionary[4003] = "passiveskill_4003";
		skill_dictionary[4004] = "passiveskill_4004";
		skill_dictionary[4005] = "passiveskill_4005";
		skill_dictionary[4006] = "passiveskill_4006";
		skill_dictionary[4007] = "passiveskill_4007";
		skill_dictionary[4008] = "passiveskill_4008";
		skill_dictionary[5001] = "passiveskill_5001";
		skill_dictionary[5002] = "passiveskill_5002";
		skill_dictionary[5003] = "passiveskill_5003";
		skill_dictionary[5004] = "passiveskill_5004";
		skill_dictionary[5005] = "passiveskill_5005";
		skill_dictionary[5006] = "passiveskill_5006";
		skill_dictionary[5007] = "passiveskill_5007";
		skill_dictionary[5008] = "passiveskill_5008";
		skill_dictionary[5009] = "passiveskill_5009";
		weapon_dictionary = new Dictionary<int, string>();
		weapon_dictionary[1] = "Weapon_001";
		weapon_dictionary[2] = "Weapon_002";
		weapon_dictionary[3] = "Weapon_003";
		weapon_dictionary[4] = "Weapon_004";
		weapon_dictionary[5] = "Weapon_005";
		weapon_dictionary[6] = "Weapon_006";
		weapon_dictionary[7] = "Weapon_007";
		weapon_dictionary[8] = "Weapon_008";
		weapon_dictionary[9] = "Weapon_009";
		weapon_dictionary[10] = "Weapon_010";
		weapon_dictionary[11] = "Weapon_011";
		weapon_dictionary[12] = "Weapon_012";
		weapon_dictionary[13] = "Weapon_013";
		weapon_dictionary[14] = "Weapon_014";
		weapon_dictionary[15] = "Weapon_015";
		weapon_dictionary[16] = "Weapon_016";
		weapon_dictionary[17] = "Weapon_017";
		weapon_dictionary[18] = "Weapon_018";
		weapon_dictionary[19] = "Weapon_019";
		weapon_dictionary[21] = "Weapon_021";
		weapon_dictionary[23] = "Weapon_023";
		weapon_dictionary[10001] = "Stoneskin_001";
		weapon_dictionary[10002] = "Stoneskin_002";
		weapon_dictionary[10003] = "Stoneskin_003";
		weapon_dictionary[10004] = "Stoneskin_004";
		weapon_dictionary[10005] = "Stoneskin_005";
		weapon_dictionary[10006] = "Stoneskin_006";
		weapon_dictionary[10007] = "Stoneskin_007";
		prop_dictionary = new Dictionary<int, string>();
		prop_dictionary[1] = "Abundance";
		prop_dictionary[2] = "Fury";
		role_dictionary = new Dictionary<int, string>();
		role_dictionary[1] = "jineng_tx1";
		role_dictionary[2] = "jineng_tx5";
		role_dictionary[3] = "jineng_tx4";
		role_dictionary[4] = "jineng_tx3";
		role_dictionary[5] = "jineng_tx2";
		current_angle = new Vector3(354.6f, 189.9f, 0f);
		SetSwitchScene(iGameApp.GetInstance().EnterScene);
	}

	public static TUIMappingInfo Instance()
	{
		if (instance == null)
		{
			instance = new TUIMappingInfo();
		}
		return instance;
	}

	public string GetStashTexture(int id)
	{
		if (stash_dictionary.ContainsKey(id))
		{
			return stash_dictionary[id];
		}
		Debug.Log("error!" + id);
		return string.Empty;
	}

	public string GetSkillTexture(int id)
	{
		if (skill_dictionary.ContainsKey(id))
		{
			return skill_dictionary[id];
		}
		Debug.Log("error!");
		return string.Empty;
	}

	public string GetWeaponTexture(int id)
	{
		if (weapon_dictionary.ContainsKey(id))
		{
			return weapon_dictionary[id];
		}
		Debug.Log("error!" + id);
		return string.Empty;
	}

	public string GetPropTexture(int id)
	{
		if (prop_dictionary.ContainsKey(id))
		{
			return prop_dictionary[id];
		}
		Debug.Log("error!");
		return string.Empty;
	}

	public string GetRoleTexture(int id)
	{
		if (role_dictionary.ContainsKey(id))
		{
			return role_dictionary[id];
		}
		Debug.Log("error!" + id);
		return string.Empty;
	}

	public void SetStashTexture(int m_id, string m_name)
	{
		stash_dictionary[m_id] = m_name;
	}

	public void SetSkillTexture(int m_id, string m_name)
	{
		skill_dictionary[m_id] = m_name;
	}

	public void SetWeaponTexture(int m_id, string m_name)
	{
		weapon_dictionary[m_id] = m_name;
	}

	public void SetPropTexture(int m_id, string m_name)
	{
		prop_dictionary[m_id] = m_name;
	}

	public void SetRoleTexture(int m_id, string m_name)
	{
		role_dictionary[m_id] = m_name;
	}

	public Vector3 GetCurrentAngle()
	{
		return current_angle;
	}

	public void SetCurrentAngle(Vector3 m_angle)
	{
		current_angle = m_angle;
	}

	public void SetSwitchScene(SwitchScene m_function)
	{
		switch_scene_function = m_function;
	}

	public SwitchScene GetSwitchScene()
	{
		return switch_scene_function;
	}

	public void DoSwitchScene(string m_next_scene)
	{
		Application.LoadLevel(m_next_scene);
	}
}
