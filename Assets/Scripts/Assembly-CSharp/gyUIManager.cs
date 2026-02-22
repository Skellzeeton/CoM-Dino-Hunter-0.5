using UnityEngine;

public class gyUIManager : MonoBehaviour
{
	public Transform mParent;

	public Transform mAchorCenter;

	public GameObject mHeadPortrait;

	public UISprite mHeadIcon;

	public GameObject mPause;

	public GameObject mWeapon;

	public gyUISkillButton mSkill;

	public gyUIWheelButton mWheelMove;

	public gyUIWheelButton mWheelShoot;

	public gyUIScreenMask mScreenMask;

	public gyUIMovieMask mMovieMask;

	public GameObject mScreenTouch;

	public iUIAchievementTip mAchievementTip;

	public GameObject mTaskPlane;

	public gyUIScreenMask mScreenBloodMask;

	public gyUIPanelMissionSuccess mPanelMissionComplete;

	public gyUIPanelMissionFailed mPanelMissionFailed;

	public gyUIPanelRevive mPanelRevive;

	public gyUIPanelMissionSuccessLevelUp mPanelLevelUp;

	public gyUIPanelMaterial mPanelMaterial;

	public UILabel mBulletNum;

	public UISprite mBulletIcon;

	public gyUIPanelTool mToolPanel;
}
