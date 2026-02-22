using UnityEngine;

public class iBulletTrack : MonoBehaviour
{
	protected bool m_bActive;

	protected Transform m_Transform;

	protected Vector3 m_v3Src;

	protected Vector3 m_v3Dst;

	protected float m_fSpeed;

	protected ParticleSystem[] m_arrParicleSystem;

	protected bool m_bEmission;

	private void Awake()
	{
		m_bActive = false;
		m_Transform = base.transform;
		m_arrParicleSystem = GetComponentsInChildren<ParticleSystem>();
		m_bEmission = false;
		Emit(false);
	}

	private void Update()
	{
		if (!m_bActive)
		{
			return;
		}
		if (!m_bEmission)
		{
			m_bEmission = true;
			Emit(true);
		}
		float num = m_fSpeed * Time.deltaTime;
		if (num > (m_v3Dst - m_Transform.position).magnitude)
		{
			m_Transform.position = m_v3Dst;
			Emit(false);
			gyUIPoolObject component = GetComponent<gyUIPoolObject>();
			if (component != null)
			{
				component.TakeBack(2f);
			}
		}
		else
		{
			m_Transform.position += m_Transform.forward * num;
		}
	}

	public void Initialize(Vector3 v3Src, Vector3 v3Dst, float speed)
	{
		m_v3Src = v3Src;
		m_v3Dst = v3Dst;
		m_fSpeed = speed;
		m_Transform.forward = m_v3Dst - m_v3Src;
		m_Transform.position = m_v3Src + m_Transform.forward * 0.5f;
		TrailRenderer component = GetComponent<TrailRenderer>();
		if (component != null)
		{
			float num = Vector3.Distance(v3Src, v3Dst);
			component.time = MyUtils.Lerp(0.01f, 0.2f, num / 20f);
		}
		m_bActive = true;
		m_bEmission = false;
		Emit(false);
		if (m_arrParicleSystem != null)
		{
			ParticleSystem[] arrParicleSystem = m_arrParicleSystem;
			foreach (ParticleSystem particleSystem in arrParicleSystem)
			{
				particleSystem.Clear(true);
			}
		}
	}

	protected void Emit(bool bEmit)
	{
		if (m_arrParicleSystem != null)
		{
			ParticleSystem[] arrParicleSystem = m_arrParicleSystem;
			foreach (ParticleSystem particleSystem in arrParicleSystem)
			{
				particleSystem.enableEmission = bEmit;
			}
		}
	}
}
