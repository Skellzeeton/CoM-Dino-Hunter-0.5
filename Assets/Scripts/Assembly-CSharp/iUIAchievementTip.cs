using UnityEngine;

public class iUIAchievementTip : MonoBehaviour
{
	protected enum kState
	{
		None = 0,
		MoveIn = 1,
		Hold = 2,
		MoveOut = 3
	}

	public iUIAchievementStar mUIAchievementStar;

	public UILabel mLabel;

	protected kState m_State;

	protected float m_fTime;

	protected float m_fTimeCount;

	protected bool m_bActive;

	protected UIAnchor mAnchor;

	public bool isActive
	{
		get
		{
			return m_bActive;
		}
	}

	private void Awake()
	{
		m_bActive = false;
		mAnchor = NGUITools.FindInParents<UIAnchor>(base.gameObject);
	}

	private void Update()
	{
		if (!m_bActive)
		{
			return;
		}
		switch (m_State)
		{
		case kState.MoveIn:
			m_fTimeCount += Time.deltaTime;
			if (!(m_fTimeCount < m_fTime))
			{
				m_State = kState.Hold;
				m_fTime = 1.5f;
				m_fTimeCount = 0f;
			}
			break;
		case kState.Hold:
			m_fTimeCount += Time.deltaTime;
			if (!(m_fTimeCount < m_fTime))
			{
				TweenPosition tweenPosition = TweenPosition.Begin(base.gameObject, 0.5f, Vector3.zero);
				tweenPosition.to = new Vector3(0f, 30f, 0f);
				tweenPosition.method = UITweener.Method.EaseOut;
				m_State = kState.MoveOut;
				m_fTime = 0.5f;
				m_fTimeCount = 0f;
			}
			break;
		case kState.MoveOut:
			m_fTimeCount += Time.deltaTime;
			if (!(m_fTimeCount < m_fTime))
			{
				m_bActive = false;
			}
			break;
		}
	}

	public void ShowTip(string sTip, int nStar)
	{
		m_bActive = true;
		mUIAchievementStar.SetStar(nStar);
		mLabel.text = sTip;
		TweenPosition tweenPosition = TweenPosition.Begin(base.gameObject, 0.5f, Vector3.zero);
		tweenPosition.from = new Vector3(0f, 30f, 0f);
		tweenPosition.to = new Vector3(0f, (float)(-Screen.height) * 0.25f / mAnchor.transform.localScale.x, 0f);
		tweenPosition.method = UITweener.Method.EaseIn;
		m_State = kState.MoveIn;
		m_fTime = 0.5f;
		m_fTimeCount = 0f;
	}
}
