using System;
using System.Collections.Generic;
using UnityEngine;

public class CWeaponHoldy : CWeaponBase
{
    protected float m_fRadius;
    protected float m_fAngle;
    protected float m_fEffectTime;
    protected float m_fEffectTimeCount;

    protected GameObject m_FireEffect;

    protected ParticleSystem[] m_arrParticleSystem;

    protected override void OnEquip(CCharPlayer player)
    {
        if (m_FireEffect != null || m_pWeaponLvlInfo == null || player == null)
            return;

        RefreshBulletUI();

        GameObject prefab = PrefabManager.Get(m_pWeaponLvlInfo.nFire);
        if (prefab == null)
            return;

        m_FireEffect = UnityEngine.Object.Instantiate(prefab);
        if (m_FireEffect == null)
            return;

        m_FireEffect.transform.SetParent(player.GetShootMouseTf());
        m_FireEffect.transform.localPosition = Vector3.zero;
        m_FireEffect.transform.localEulerAngles = new Vector3(90f, 0f, 0f);

        // ONLY use ParticleSystem
        m_arrParticleSystem = m_FireEffect.GetComponentsInChildren<ParticleSystem>(true);

        if (m_arrParticleSystem != null)
        {
            foreach (ParticleSystem ps in m_arrParticleSystem)
            {
                var emission = ps.emission;
                emission.enabled = false;
                ps.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            }
        }
    }

    protected override void OnDestroy()
    {
        if (m_FireEffect != null)
        {
            UnityEngine.Object.Destroy(m_FireEffect);
            m_FireEffect = null;
        }
    }

    protected override void OnFire(CCharPlayer player)
    {
        if (!player.IsCanAttack())
            return;

        m_fFireLightTime = 0f;
        ShowFireLight(true);

        player.PlayAnimMix(kAnimEnum.Attack, WrapMode.Loop, 1f);
        player.PlayAudio(m_pWeaponLvlInfo.sAudioFire);

        // Enable particle emission
        if (m_arrParticleSystem != null)
        {
            foreach (ParticleSystem ps in m_arrParticleSystem)
            {
                var emission = ps.emission;
                emission.enabled = true;

                ps.Play(true);
            }
        }

        m_fRadius = 0f;
        m_fAngle = 0f;
        m_fEffectTime = 0.5f;

        m_pWeaponLvlInfo.GetAtkModeValue(0, ref m_fRadius);
        m_pWeaponLvlInfo.GetAtkModeValue(1, ref m_fAngle);
        m_pWeaponLvlInfo.GetAtkModeValue(2, ref m_fEffectTime);

        m_fEffectTimeCount = m_fEffectTime;
    }

    protected override void OnStop(CCharPlayer player)
    {
        m_fFireLightTime = 1.5f;

        player.StopAction(kAnimEnum.Attack);
        player.StopAudio(m_pWeaponLvlInfo.sAudioFire);

        // Disable particle emission
        if (m_arrParticleSystem != null)
        {
            foreach (ParticleSystem ps in m_arrParticleSystem)
            {
                var emission = ps.emission;
                emission.enabled = false;

                ps.Stop(true, ParticleSystemStopBehavior.StopEmitting);
            }
        }
    }

    protected override void OnUpdate(CCharPlayer player, float deltaTime)
    {
        if (m_fFireIntervalCount < m_fFireInterval)
            m_fFireIntervalCount += deltaTime;

        if (!m_bFire)
            return;

        m_fEffectTimeCount += deltaTime;

        if (m_fEffectTimeCount < m_fEffectTime)
            return;

        m_fEffectTimeCount = 0f;

        if (base.IsBulletEmpty)
        {
            Stop(player);
            return;
        }

        ConsumeBullet();

        iGameUIBase gameUI = m_GameScene.GetGameUI();
        if (gameUI != null)
            gameUI.ExpandAimCross();

        Dictionary<int, CCharMob> mobData = m_GameScene.GetMobData();

        foreach (CCharMob mob in mobData.Values)
        {
            if (mob.isDead)
                continue;

            Vector3 toMob = mob.Pos - player.Pos;

            if (toMob.sqrMagnitude > m_fRadius * m_fRadius)
                continue;

            if (m_fAngle > 0f)
            {
                toMob.y = 0f;

                if (Vector3.Dot(player.Dir2D, toMob.normalized) <
                    Mathf.Cos(m_fAngle * Mathf.Deg2Rad / 2f))
                    continue;
            }

            Vector3 hitDir = mob.Pos - player.Pos;
            Vector3 bloodPos = mob.GetBloodPos(
                player.GetUpBodyPos() + new Vector3(0f, 0.7f, 0f),
                hitDir
            );

            m_GameScene.AddHitEffect(bloodPos, hitDir, m_pWeaponLvlInfo.nHit);

            if (!base.isNetPlayerShoot)
                OnHitMob(player, mob, bloodPos, hitDir, string.Empty);

            mob.PlayAudio(kAudioEnum.HitBody);
        }
    }

    protected override void OnHitMob(CCharPlayer player, CCharMob mob,
        Vector3 hitpos, Vector3 hitdir, string sBodyPart = "")
    {
        mob.SetLifeBarParam(1f);

        float damage = player.CalcWeaponDamage(m_pWeaponLvlInfo);
        float critChance = player.CalcCritical(m_pWeaponLvlInfo);
        float critDmg = player.CalcCriticalDmg(m_pWeaponLvlInfo);

        bool isCritical = false;

        if (critChance > UnityEngine.Random.Range(1f, 100f))
        {
            damage *= 1f + critDmg / 100f;
            isCritical = true;
        }

        float protect = mob.CalcProtect();
        damage *= 1f - protect / 100f;

        mob.OnHit(-damage, m_pWeaponLvlInfo, string.Empty);

        m_GameScene.AddDamageText(damage, hitpos, isCritical);

        iGameLogic.HitInfo hitinfo = new iGameLogic.HitInfo();
        hitinfo.v3HitDir = hitdir;
        hitinfo.v3HitPos = hitpos;

        m_GameLogic = m_GameScene.GetGameLogic();

        if (m_GameLogic != null)
        {
            m_GameLogic.CaculateFunc(
                player,
                mob,
                m_pWeaponLvlInfo.arrFunc,
                m_pWeaponLvlInfo.arrValueX,
                m_pWeaponLvlInfo.arrValueY,
                ref hitinfo
            );

            m_GameLogic.ltDamageInfo.Add(damage);

            CGameNetSender.GetInstance()
                .BattleDamageMob(mob.UID, m_GameLogic.ltDamageInfo);
        }

        if (mob.isDead)
        {
            CMobInfoLevel mobInfo = mob.GetMobInfo();

            if (mobInfo != null)
            {
                player.AddExp(mobInfo.nExp);
                m_GameScene.AddExpText(mobInfo.nExp, hitinfo.v3HitPos);
            }
        }
    }
}