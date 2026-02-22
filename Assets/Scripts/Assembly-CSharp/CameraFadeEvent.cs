using UnityEngine;

public class CameraFadeEvent : MonoBehaviour, IRoamEvent
{
	public bool isFadeIn;

	public float fadeInTime;

	public bool isFadeOut;

	public float fadeOutTime;

	public void OnRoamTrigger()
	{
		if (isFadeIn)
		{
			CameraFade.CameraFadeIn(fadeInTime);
		}
	}

	public void OnRoamStop()
	{
		if (isFadeOut)
		{
			CameraFade.CameraFadeOut(fadeOutTime);
		}
	}
}
