using System;
using System.Collections.Generic;
using UnityEngine;

public class iGameLogic
{
	public class HitInfo
	{
		public Vector3 v3HitDir = Vector3.zero;

		public Vector3 v3HitPos = Vector3.zero;

		public CWeaponInfoLevel weaponinfolevel;

		public bool isPlayerSkill;

		public int nFromSkill = -1;

		public bool isHurt;
	}

	public List<float> ltDamageInfo;

	protected iGameSceneBase m_GameScene;

	protected iGameUIBase m_GameUI;

	protected iGameState m_GameState;

	protected iGameData m_GameData;

	protected List<int> m_ltFunc;

	protected List<int> m_ltValueX;

	protected List<int> m_ltValueY;

	public void Initialize()
	{
		m_GameScene = iGameApp.GetInstance().m_GameScene;
		m_GameUI = m_GameScene.GetGameUI();
		m_GameState = iGameApp.GetInstance().m_GameState;
		m_GameData = iGameApp.GetInstance().m_GameData;
		ltDamageInfo = new List<float>();
		m_ltFunc = new List<int>();
		m_ltValueX = new List<int>();
		m_ltValueY = new List<int>();
	}

	public void CaculateFunc(CCharBase actor, CCharBase target, int[] arrFunc, int[] arrValueX, int[] arrValueY, ref HitInfo hitinfo)
	{
		ltDamageInfo.Clear();
		for (int i = 0; i < arrFunc.Length; i++)
		{
			int num = arrFunc[i];
			int num2 = arrValueX[i];
			int num3 = arrValueY[i];
			if (num > 0)
			{
			}
			switch (num)
			{
			case 2:
				if (target != null)
				{
					float num14 = num2;
					float num15 = actor.CalcCritical(hitinfo.weaponinfolevel);
					float num16 = actor.CalcCriticalDmg(hitinfo.weaponinfolevel);
					bool bCritical3 = false;
					if (num15 > UnityEngine.Random.Range(1f, 100f))
					{
						num14 *= 1f + num16 / 100f;
						bCritical3 = true;
					}
					float num17 = target.CalcProtect();
					num14 *= 1f - num17 / 100f;
					target.OnHit(0f - num14, hitinfo.weaponinfolevel, string.Empty);
					hitinfo.isHurt = true;
					if (m_GameScene.IsMyself(actor) || m_GameScene.IsMyself(target))
					{
						m_GameScene.AddDamageText(num14, hitinfo.v3HitPos, bCritical3);
					}
					ltDamageInfo.Add(num14);
				}
				break;
			case 5:
				if (target != null)
				{
					float num19 = actor.Property.GetValue(kProEnum.Damage) * (float)MyUtils.Low32(num2) / 100f + (float)MyUtils.High32(num2);
					float num20 = actor.CalcCritical(hitinfo.weaponinfolevel);
					float num21 = actor.CalcCriticalDmg(hitinfo.weaponinfolevel);
					bool bCritical4 = false;
					if (num20 > UnityEngine.Random.Range(1f, 100f))
					{
						num19 *= 1f + num21 / 100f;
						bCritical4 = true;
					}
					float num22 = target.CalcProtect();
					num19 *= 1f - num22 / 100f;
					target.OnHit(0f - num19, hitinfo.weaponinfolevel, string.Empty);
					hitinfo.isHurt = true;
					if (m_GameScene.IsMyself(actor) || m_GameScene.IsMyself(target))
					{
						m_GameScene.AddDamageText(num19, hitinfo.v3HitPos, bCritical4);
					}
					ltDamageInfo.Add(num19);
				}
				break;
			case 7:
			{
				float num4 = num2;
				float num5 = actor.CalcCritical(hitinfo.weaponinfolevel);
				float num6 = actor.CalcCriticalDmg(hitinfo.weaponinfolevel);
				foreach (CCharMob item in m_GameScene.GetMobEnumerator())
				{
					if (!(item == actor) && !item.isDead && !(Vector3.Distance(item.Pos, hitinfo.v3HitPos) > (float)num3))
					{
						float num7 = num4;
						bool bCritical = false;
						if (num5 > UnityEngine.Random.Range(1f, 100f))
						{
							num7 *= 1f + num6 / 100f;
							bCritical = true;
						}
						float num8 = item.CalcProtect();
						num7 *= 1f - num8 / 100f;
						target.OnHit(0f - num7, hitinfo.weaponinfolevel, string.Empty);
						hitinfo.isHurt = true;
						m_GameScene.AddDamageText(num7, item.GetBone(1).position, bCritical);
					}
				}
				break;
			}
			case 8:
			{
				float num9 = actor.Property.GetValue(kProEnum.Damage) * (float)MyUtils.Low32(num2) / 100f + (float)MyUtils.High32(num2);
				float num10 = actor.CalcCritical(hitinfo.weaponinfolevel);
				float num11 = actor.CalcCriticalDmg(hitinfo.weaponinfolevel);
				foreach (CCharMob item2 in m_GameScene.GetMobEnumerator())
				{
					if (!(item2 == actor) && !item2.isDead && !(Vector3.Distance(item2.Pos, hitinfo.v3HitPos) > (float)num3))
					{
						float num12 = num9;
						bool bCritical2 = false;
						if (num10 > UnityEngine.Random.Range(1f, 100f))
						{
							num12 *= 1f + num11 / 100f;
							bCritical2 = true;
						}
						float num13 = item2.CalcProtect();
						num12 *= 1f - num13 / 100f;
						target.OnHit(0f - num12, hitinfo.weaponinfolevel, string.Empty);
						hitinfo.isHurt = true;
						m_GameScene.AddDamageText(num12, item2.GetBone(1).position, bCritical2);
					}
				}
				break;
			}
			case 3:
				if (target == null)
				{
					return;
				}
				target.AddBuff(num2, num3, hitinfo.nFromSkill);
				break;
			case 4:
			{
				if (!(target != null))
				{
					break;
				}
				float value = target.Property.GetValue(kProEnum.ResistBeatBack);
				if (value <= 0f)
				{
					target.BeatBack(hitinfo.v3HitDir, num2);
					break;
				}
				float num18 = num3;
				if (actor.IsPlayer() && hitinfo.weaponinfolevel != null)
				{
					num18 = (float)num3 + ((CCharPlayer)actor).CalcWeaponBeatBack(hitinfo.weaponinfolevel);
				}
				if (value < num18)
				{
					target.BeatBack(hitinfo.v3HitDir, (float)num2 * ((num18 - value) / num18));
				}
				break;
			}
			case 9:
				if (target != null && (m_GameScene.IsMyself(actor) || m_GameScene.IsMyself(target)))
				{
					float num23 = target.Property.GetValue(kProEnum.HPMax) * (float)num2 / 100f + (float)num3;
					target.AddHP(num23);
					m_GameScene.AddHealText(num23, target.GetBone(0).position);
				}
				break;
			case 101:
			{
				CCharUser cCharUser3 = target as CCharUser;
				if (cCharUser3 != null)
				{
					cCharUser3.AddExp(num2);
					m_GameScene.AddExpText(num2, cCharUser3.GetBone(0).position);
				}
				break;
			}
			case 10:
				target.SetStealth(true, num2);
				break;
			case 11:
				target.SetStun(true, num2);
				break;
			case 100:
			{
				CCharUser cCharUser2 = target as CCharUser;
				if (cCharUser2 != null)
				{
					m_GameScene.AddGoldText(num2, cCharUser2.GetBone(1).position);
				}
				m_GameState.AddGold(num2);
				break;
			}
			case 102:
			{
				CCharUser cCharUser = target as CCharUser;
				if (cCharUser != null)
				{
					CItemInfoLevel itemInfo = m_GameData.GetItemInfo(num2, 1);
					if (itemInfo != null)
					{
						m_GameScene.AddMaterial(cCharUser.GetBone(1).position, itemInfo.sIcon, num3);
					}
				}
				m_GameState.AddMaterial(num2, num3);
				break;
			}
			}
		}
	}

	public void Skill(int nSkillID, CCharBase attacker, CCharBase defender, ref HitInfo hitinfo)
	{
		CSkillInfoLevel skillInfo = m_GameData.GetSkillInfo(nSkillID, 1);
		if (skillInfo != null)
		{
			Skill(skillInfo, attacker, defender, ref hitinfo);
		}
	}

	public void Skill(CSkillInfoLevel skillinfolevel, CCharBase attacker, CCharBase defender, ref HitInfo hitinfo)
	{
		if (skillinfolevel != null && !(attacker == null) && !(defender == null))
		{
			hitinfo.nFromSkill = skillinfolevel.nID;
			Vector3 normalized = (defender.Pos - attacker.Pos).normalized;
			CCharPlayer cCharPlayer = attacker as CCharPlayer;
			if (cCharPlayer != null && cCharPlayer.Property != null && cCharPlayer.Property.GetSkillPro(skillinfolevel.nID) != null)
			{
				m_ltFunc.Clear();
				m_ltValueX.Clear();
				m_ltValueY.Clear();
				cCharPlayer.Property.CaculateSkillFuncBySkillPro(skillinfolevel, ref m_ltFunc, ref m_ltValueX, ref m_ltValueY);
				CaculateFunc(attacker, defender, m_ltFunc.ToArray(), m_ltValueX.ToArray(), m_ltValueY.ToArray(), ref hitinfo);
			}
			else
			{
				CaculateFunc(attacker, defender, skillinfolevel.arrFunc, skillinfolevel.arrValueX, skillinfolevel.arrValueY, ref hitinfo);
			}
		}
	}

	public void Item(int nItemID, int nItemLevel, CCharBase actor, CCharBase target)
	{
		CItemInfoLevel itemInfo = m_GameData.GetItemInfo(nItemID, nItemLevel);
		if (itemInfo != null)
		{
			Item(itemInfo, actor, target);
		}
	}

	public void Item(CItemInfoLevel iteminfolevel, CCharBase actor, CCharBase target)
	{
		if (iteminfolevel != null && !(actor == null))
		{
			HitInfo hitinfo = new HitInfo();
			CaculateFunc(actor, target, iteminfolevel.arrFunc, iteminfolevel.arrValueX, iteminfolevel.arrValueY, ref hitinfo);
		}
	}

	public bool IsSkillCanUse(CCharBase actor, CCharBase target, int nSkillID)
	{
		CSkillInfoLevel skillInfo = m_GameData.GetSkillInfo(nSkillID, 1);
		if (skillInfo == null)
		{
			return false;
		}
		return IsSkillCanUse(actor, target, skillInfo);
	}

	public bool IsSkillCanUse(CCharBase actor, CCharBase target, CSkillInfoLevel skillinfolevel)
	{
		if (skillinfolevel == null || target == null)
		{
			return false;
		}
		switch (skillinfolevel.nRangeType)
		{
		case 0:
		{
			float fValue4 = 0f;
			float fValue5 = 0f;
			if (skillinfolevel.GetSkillRangeValue(0, ref fValue4) && skillinfolevel.GetSkillRangeValue(1, ref fValue5))
			{
				float num2 = Vector3.Distance(actor.Pos, target.Pos);
				if (num2 < fValue4 || num2 > fValue5)
				{
					return false;
				}
			}
			break;
		}
		case 1:
		{
			float fValue = 0f;
			float fValue2 = 0f;
			float fValue3 = 0f;
			skillinfolevel.GetSkillRangeValue(0, ref fValue);
			skillinfolevel.GetSkillRangeValue(1, ref fValue2);
			skillinfolevel.GetSkillRangeValue(2, ref fValue3);
			float num = Vector3.Distance(actor.Pos, target.Pos);
			if (num < fValue || num > fValue2)
			{
				return false;
			}
			if (fValue3 != 0f)
			{
				Vector3 vector = target.Pos - actor.Pos;
				vector.y = 0f;
				if (Vector3.Dot(actor.Dir2D, vector.normalized) < Mathf.Cos(fValue3 * ((float)Math.PI / 180f) / 2f))
				{
					return false;
				}
			}
			break;
		}
		}
		return true;
	}

	public bool IsComboCanUse(CCharBase actor, CCharBase target, int nComboID)
	{
		CSkillComboInfo skillComboInfo = m_GameData.GetSkillComboInfo(nComboID);
		if (skillComboInfo == null || skillComboInfo.ltSkill.Count < 1)
		{
			return false;
		}
		int num = skillComboInfo.ltSkill[0];
		if (num == -1)
		{
			return false;
		}
		return IsSkillCanUse(actor, target, num);
	}
}
