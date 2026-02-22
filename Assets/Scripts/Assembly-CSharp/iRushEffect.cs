using UnityEngine;

public class iRushEffect : _iAnimEventFollowBase
{
	public int nPrefabID;

	public float fDisappearTime = 2f;

	public void iRushEffect_PlayEffect()
	{
		PlayEffect(nPrefabID);
	}

	public void iRushEffect_StopEffect()
	{
		if (!(m_Effect != null))
		{
			return;
		}
		m_Effect.transform.parent = null;
		ParticleSystem[] componentsInChildren = m_Effect.GetComponentsInChildren<ParticleSystem>();
		if (componentsInChildren != null)
		{
			ParticleSystem[] array = componentsInChildren;
			foreach (ParticleSystem particleSystem in array)
			{
				particleSystem.enableEmission = false;
			}
			Object.Destroy(m_Effect, fDisappearTime);
		}
		else
		{
			Object.Destroy(m_Effect);
		}
	}
}
