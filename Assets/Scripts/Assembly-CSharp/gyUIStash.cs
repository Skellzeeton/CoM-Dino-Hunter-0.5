using UnityEngine;

public class gyUIStash : MonoBehaviour
{
	public UISprite mIcon;

	public UILabel mValueCur;

	public UILabel mValueMax;

	protected Vector3 m_v3ValueCurScale;

	protected int m_nCur;

	protected int m_nMax;

	private void Awake()
	{
		if (mValueCur != null)
		{
			m_v3ValueCurScale = mValueCur.transform.localScale;
		}
	}

	private void Start()
	{
	}

	private void Update()
	{
	}

	public void SetMax(int nCount)
	{
		if (!(mValueMax == null))
		{
			m_nMax = nCount;
			mValueMax.text = "/" + nCount;
		}
	}

	public void SetCur(int nCount)
	{
		if (!(mValueCur == null))
		{
			m_nCur = nCount;
			mValueCur.text = nCount.ToString();
			TweenScale tweenScale = TweenScale.Begin(mValueCur.gameObject, 0.5f, Vector3.zero);
			if (tweenScale != null)
			{
				tweenScale.from = m_v3ValueCurScale * 2f;
				tweenScale.to = m_v3ValueCurScale;
				tweenScale.method = UITweener.Method.BounceIn;
			}
			if (m_nCur < m_nMax)
			{
				mValueCur.color = Color.green;
				mValueMax.color = Color.green;
			}
			else
			{
				mValueCur.color = Color.red;
				mValueMax.color = Color.red;
			}
		}
	}
}
