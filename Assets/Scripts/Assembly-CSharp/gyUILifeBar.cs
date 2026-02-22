using UnityEngine;

public class gyUILifeBar : MonoBehaviour
{
	public float fProcessSpeed = 10f;

	public bool isMinusOffProc;

	public UIFilledSprite LifeBarDeep;

	public UIFilledSprite LifeBarShallow;

	protected bool m_bInProcess;

	protected float m_fSrcValue;

	protected float m_fDstValue;

	protected float m_fCurValue;

	private void Awake()
	{
		m_bInProcess = false;
		m_fSrcValue = 0f;
		m_fDstValue = LifeBarDeep.fillAmount;
		m_fCurValue = LifeBarDeep.fillAmount;
		LifeBarShallow.gameObject.active = false;
	}

	private void Update()
	{
		if (!m_bInProcess)
		{
			return;
		}
		if (m_fSrcValue > m_fDstValue)
		{
			m_fCurValue -= fProcessSpeed * Time.deltaTime;
			if (m_fCurValue <= m_fDstValue)
			{
				m_fCurValue = m_fDstValue;
				m_bInProcess = false;
				LifeBarShallow.gameObject.active = false;
			}
			else
			{
				LifeBarShallow.fillAmount = m_fCurValue;
			}
		}
		else
		{
			m_fCurValue += fProcessSpeed * Time.deltaTime;
			if (m_fCurValue >= m_fDstValue)
			{
				m_fCurValue = m_fDstValue;
				m_bInProcess = false;
				LifeBarShallow.gameObject.active = false;
			}
			else
			{
				LifeBarDeep.fillAmount = m_fCurValue;
			}
		}
	}

	public void InitValue(float fRate)
	{
		if (base.gameObject.active)
		{
			m_fCurValue = fRate;
			m_fDstValue = m_fCurValue;
			LifeBarDeep.fillAmount = m_fCurValue;
		}
	}

	public float GetValue()
	{
		return m_fCurValue;
	}

	public void SetValue(float fRate)
	{
		if (!base.gameObject.active || m_fDstValue == fRate)
		{
			return;
		}
		m_fSrcValue = m_fCurValue;
		m_fDstValue = fRate;
		if (isMinusOffProc && m_fSrcValue > m_fDstValue)
		{
			m_fCurValue = m_fDstValue;
			LifeBarDeep.fillAmount = m_fCurValue;
			LifeBarShallow.gameObject.active = false;
			return;
		}
		m_bInProcess = true;
		LifeBarShallow.gameObject.active = true;
		fProcessSpeed = Mathf.Abs(m_fDstValue - m_fSrcValue);
		if (m_fSrcValue > m_fDstValue)
		{
			LifeBarShallow.fillAmount = m_fCurValue;
			LifeBarDeep.fillAmount = m_fDstValue;
		}
		else
		{
			LifeBarShallow.fillAmount = m_fDstValue;
			LifeBarDeep.fillAmount = m_fCurValue;
		}
	}

	public void SetColor(Color color)
	{
		if (!(LifeBarDeep == null))
		{
			LifeBarDeep.color = color;
		}
	}
}
