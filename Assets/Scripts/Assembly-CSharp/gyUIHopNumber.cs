using UnityEngine;

public class gyUIHopNumber : MonoBehaviour
{
	protected UILabel mLabel;

	protected float m_fFrom;

	protected float m_fTo;

	protected float m_fTime;

	protected GameObject m_Temp;

	protected TweenScale m_Scale;

	protected bool m_bHop;

	protected float m_fTimeCount;

	public bool isHop
	{
		get
		{
			return m_bHop;
		}
	}

	private void Awake()
	{
		mLabel = GetComponentInChildren<UILabel>();
		if (mLabel != null)
		{
			mLabel.text = "0";
		}
	}

	private void Start()
	{
	}

	private void Update()
	{
		if (!(mLabel == null) && m_bHop)
		{
			mLabel.text = ((int)m_Temp.transform.localScale.x).ToString();
			m_fTimeCount += Time.deltaTime;
			if (m_fTimeCount >= m_fTime)
			{
				m_bHop = false;
			}
		}
	}

	public void Go(float from, float to, float time)
	{
		m_fFrom = from;
		m_fTo = to;
		m_fTime = time;
		m_bHop = true;
		m_fTimeCount = 0f;
		if (m_Temp == null)
		{
			m_Temp = new GameObject("temp");
			m_Temp.transform.parent = base.transform;
		}
		m_Scale = TweenScale.Begin(m_Temp, time, Vector3.zero);
		m_Scale.from = new Vector3(from, 0f, 0f);
		m_Scale.to = new Vector3(to, 0f, 0f);
	}

	public void Stop()
	{
		m_bHop = false;
		if (mLabel != null)
		{
			mLabel.text = m_fTo.ToString();
		}
	}
}
