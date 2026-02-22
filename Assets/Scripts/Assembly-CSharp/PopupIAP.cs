using UnityEngine;

public class PopupIAP : MonoBehaviour
{
	public Transform popup_yes;

	public Transform popup_waitting;

	private Vector3 popup_pos = Vector3.zero;

	private Vector3 popup_yes_pos = Vector3.zero;

	private Vector3 popup_waitting_pos = Vector3.zero;

	private void Awake()
	{
		popup_pos = base.transform.localPosition;
		popup_yes_pos = popup_yes.localPosition;
		popup_waitting_pos = popup_waitting.localPosition;
		Hide();
	}

	private void Start()
	{
	}

	private void Update()
	{
	}

	public void Hide()
	{
		base.transform.localPosition = popup_pos + new Vector3(0f, 1000f, 0f);
		popup_yes.localPosition = popup_yes_pos;
		popup_waitting.localPosition = popup_waitting_pos;
	}

	public void ShowPopupYes()
	{
		base.transform.localPosition = popup_pos;
		popup_yes.localPosition = popup_yes_pos;
		if (popup_yes.GetComponent<Animation>() != null)
		{
			popup_yes.GetComponent<Animation>().Play();
		}
		popup_waitting.localPosition = popup_waitting_pos + new Vector3(0f, 1000f, 0f);
	}

	public void ShowPopupWaitting()
	{
		base.transform.localPosition = popup_pos;
		popup_yes.localPosition = popup_yes_pos + new Vector3(0f, 1000f, 0f);
		popup_waitting.localPosition = popup_waitting_pos;
		if (popup_waitting.GetComponent<Animation>() != null)
		{
			popup_waitting.GetComponent<Animation>().Play();
		}
	}
}
