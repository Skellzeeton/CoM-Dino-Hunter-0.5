using UnityEngine;

public class CUseSkillBump : CUseSkill
{
	protected float m_fBumpDis;

	protected float m_fBumpTime = 1f;

	protected float m_fBumpTimeCount;

	protected float m_fBumpFuncTime;

	protected float m_fBumpFuncTimeCount;

	protected Vector3 m_v3Src;

	protected Vector3 m_v3Dst;

	protected float m_fSpeed;

	protected iRushEffect m_RushEffect;

	public override kUseSkillStatus OnEnter(CCharBase charbase)
	{
		charbase.m_bBumping = true;
		m_pSkillInfoLevel.GetSkillModeValue(0, ref m_fBumpDis);
		m_pSkillInfoLevel.GetSkillModeValue(1, ref m_fBumpTime);
		m_pSkillInfoLevel.GetSkillModeValue(2, ref m_fBumpFuncTime);
		m_fBumpFuncTimeCount = 0f;
		m_v3Src = charbase.Pos;
		m_v3Dst = charbase.Pos + charbase.Dir2D * m_fBumpDis;
		Ray ray = new Ray(charbase.GetBone(1).position, charbase.Dir2D);
		RaycastHit hitInfo;
		if (Physics.Raycast(ray, out hitInfo, m_fBumpDis, -1879048192))
		{
			m_fBumpDis = Vector3.Distance(ray.origin, hitInfo.point) - 2f;
			m_v3Dst = charbase.Pos + charbase.Dir2D * m_fBumpDis;
		}
		m_fSpeed = m_fBumpDis / m_fBumpTime;
		float speed = 1f;
		switch (charbase.CharType)
		{
		case kCharType.Mob:
		case kCharType.Boss:
		{
			CCharMob cCharMob = charbase as CCharMob;
			if (!(cCharMob != null))
			{
				break;
			}
			CMobInfoLevel mobInfo = cCharMob.GetMobInfo();
			if (mobInfo != null)
			{
				if (m_pSkillInfoLevel.nAnim == 4)
				{
					speed = m_fSpeed * mobInfo.fMoveSpeedRate;
				}
				else if (m_pSkillInfoLevel.nAnim == 2504)
				{
					speed = m_fSpeed * mobInfo.fRushSpeedRate;
				}
			}
			break;
		}
		case kCharType.Player:
			if (m_pSkillInfoLevel.nAnim == 4)
			{
				speed = m_fSpeed * 0.3f;
			}
			break;
		case kCharType.User:
		{
			CCharUser cCharUser = charbase as CCharUser;
			if (!(cCharUser != null))
			{
				break;
			}
			if (m_pSkillInfoLevel.nAnim == 4)
			{
				speed = m_fSpeed * 0.3f;
			}
			cCharUser.MoveStop();
			cCharUser.SetFire(false);
			CWeaponInfoLevel curWeaponLvlInfo = cCharUser.GetCurWeaponLvlInfo();
			if (curWeaponLvlInfo != null && curWeaponLvlInfo.nType != 1)
			{
				iCameraTrail camera = m_GameScene.GetCamera();
				if (camera != null)
				{
					camera.SetViewMelee(true);
				}
			}
			break;
		}
		}
		charbase.PlayAnim((kAnimEnum)m_pSkillInfoLevel.nAnim, WrapMode.Loop, speed, 0f);
		if (charbase.Entity != null)
		{
			m_RushEffect = charbase.Entity.GetComponent<iRushEffect>();
			if (m_RushEffect != null)
			{
				m_RushEffect.iRushEffect_PlayEffect();
			}
		}
		if (m_pSkillInfoLevel.sUseAudio.Length > 0)
		{
			charbase.PlayAudio(m_pSkillInfoLevel.sUseAudio);
		}
		return kUseSkillStatus.Success;
	}

	public override void OnExit(CCharBase charbase)
	{
		charbase.m_bBumping = false;
		if (m_RushEffect != null)
		{
			m_RushEffect.iRushEffect_StopEffect();
		}
		if (!charbase.IsPlayer() && !charbase.IsUser())
		{
			return;
		}
		charbase.CrossAnim(kAnimEnum.Idle, WrapMode.Loop, 0.3f, 1f, 0f);
		CCharUser cCharUser = charbase as CCharUser;
		if (!(cCharUser != null))
		{
			return;
		}
		CWeaponInfoLevel curWeaponLvlInfo = cCharUser.GetCurWeaponLvlInfo();
		if (curWeaponLvlInfo != null && curWeaponLvlInfo.nType != 1)
		{
			iCameraTrail camera = m_GameScene.GetCamera();
			if (camera != null)
			{
				camera.SetViewMelee(false);
			}
		}
	}

	public override kUseSkillStatus OnUpdate(CCharBase charbase, float deltaTime)
	{
		m_fBumpFuncTimeCount += deltaTime;
		if (m_fBumpFuncTimeCount >= m_fBumpFuncTime)
		{
			m_fBumpFuncTimeCount = 0f;
			SkillEffect(charbase, m_Target);
		}
		if (m_fBumpTimeCount < m_fBumpTime)
		{
			m_fBumpTimeCount += deltaTime;
			charbase.Pos = Vector3.Lerp(m_v3Src, m_v3Dst, m_fBumpTimeCount / m_fBumpTime);
			if (m_fBumpTimeCount >= m_fBumpTime)
			{
				return kUseSkillStatus.Success;
			}
		}
		return kUseSkillStatus.Executing;
	}
}
