using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(UISprite))]
public class gyUIScreenMask : MonoBehaviour
{
	protected Transform mTransform;

	protected BoxCollider mCollider;

	protected UISprite mSprite;

	protected bool m_bInProcess;

	protected bool m_bFinishHide;

	protected float m_fSrc;

	protected float m_fDst;

	protected float m_fSpeed;

	protected float m_fRate;

	protected UIAnchor mAnchor;

	private void Awake()
	{
		mAnchor = NGUITools.FindInParents<UIAnchor>(base.gameObject);
		mTransform = base.transform;
		mTransform.localScale = new Vector3(Screen.width, Screen.height, 1f);
		mCollider = GetComponent<BoxCollider>();
		if (mCollider != null)
		{
			mCollider.isTrigger = true;
			mCollider.center = Vector3.zero;
			mCollider.size = Vector3.one;
		}
		mSprite = GetComponent<UISprite>();
		if (mSprite != null)
		{
			mSprite.pivot = UIWidget.Pivot.Center;
		}
		base.gameObject.active = false;
	}

	private void Update()
	{
		if (!m_bInProcess || mSprite == null)
		{
			return;
		}
		m_fRate += m_fSpeed * Time.deltaTime;
		Color color = mSprite.color;
		color.a = Lerp(m_fSrc, m_fDst, m_fRate);
		mSprite.color = color;
		if (m_fRate >= 1f)
		{
			m_bInProcess = false;
			if (m_bFinishHide)
			{
				base.gameObject.active = false;
			}
		}
	}

	protected float Lerp(float src, float dst, float rate)
	{
		if (rate >= 1f)
		{
			return dst;
		}
		if (rate <= 0f)
		{
			return src;
		}
		return src + (dst - src) * rate;
	}

	public void FadeIn(float fTime)
	{
		m_bInProcess = true;
		m_bFinishHide = true;
		m_fSrc = 1f;
		m_fDst = 0f;
		m_fSpeed = 1f / fTime;
		m_fRate = 0f;
		mTransform.localScale = new Vector3(Screen.width, Screen.height, 1f) / mAnchor.transform.localScale.x;
		base.gameObject.active = true;
		Color color = mSprite.color;
		color.a = m_fSrc;
		mSprite.color = color;
	}

	public void FadeOut(float fTime)
	{
		m_bInProcess = true;
		m_bFinishHide = false;
		m_fSrc = 0f;
		m_fDst = 1f;
		m_fSpeed = 1f / fTime;
		m_fRate = 0f;
		mTransform.localScale = new Vector3(Screen.width, Screen.height, 1f) / mAnchor.transform.localScale.x;
		base.gameObject.active = true;
		Color color = mSprite.color;
		color.a = m_fSrc;
		mSprite.color = color;
	}

	public void ShowMask(bool bShow, float fAlpha = 0.8f)
	{
		mTransform.localScale = new Vector3(Screen.width, Screen.height, 1f) / mAnchor.transform.localScale.x;
		base.gameObject.active = bShow;
		if (bShow)
		{
			Color color = mSprite.color;
			color.a = fAlpha;
			mSprite.color = color;
		}
	}
}
