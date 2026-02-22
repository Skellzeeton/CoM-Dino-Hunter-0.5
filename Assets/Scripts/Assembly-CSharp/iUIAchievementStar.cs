using UnityEngine;

public class iUIAchievementStar : MonoBehaviour
{
	public UISprite mStar1;

	public UISprite mStar2;

	public UISprite mStar3;

	private void Start()
	{
	}

	private void Update()
	{
	}

	public void SetStar(int nStar)
	{
		switch (nStar)
		{
		case 0:
			mStar1.alpha = 0.5f;
			mStar2.alpha = 0.5f;
			mStar3.alpha = 0.5f;
			break;
		case 1:
			mStar1.alpha = 1f;
			mStar2.alpha = 0.5f;
			mStar3.alpha = 0.5f;
			break;
		case 2:
			mStar1.alpha = 1f;
			mStar2.alpha = 1f;
			mStar3.alpha = 0.5f;
			break;
		default:
			mStar1.alpha = 1f;
			mStar2.alpha = 1f;
			mStar3.alpha = 1f;
			break;
		}
	}
}
