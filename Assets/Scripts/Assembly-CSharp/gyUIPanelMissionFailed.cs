using UnityEngine;

public class gyUIPanelMissionFailed : MonoBehaviour
{
	protected bool m_bShow;

	private void Awake()
	{
		m_bShow = false;
		base.gameObject.SetActiveRecursively(false);
	}

	private void Start()
	{
	}

	private void Update()
	{
	}

	public void Show(bool bShow)
	{
		m_bShow = bShow;
		base.gameObject.SetActiveRecursively(bShow);
		if (bShow)
		{
			base.transform.localPosition = Vector3.zero;
		}
		else
		{
			base.transform.localPosition = new Vector3(10000f, 10000f, 10000f);
		}
	}
}
