using TNetSdk;
using UnityEngine;

public class CControlWindows : CControlBase
{
    protected int m_nCurWeaponIndex;
    private bool m_mouseLocked = false;
    private const float MOUSE_MOVE_DEADZONE = 0.001f;

    public override void Initialize()
    {
        base.Initialize();
        m_GameUI.RegisterEvent_Windows();
    }

    public override void Update(float deltaTime)
    {
        if (m_GameScene == null)
        {
            return;
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            m_GameScene.ReviveGame();
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            m_GameState.Reset();
            m_GameScene.Reset();
            m_GameScene.StartGame();
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            TNetManager.GetInstance().DisConnect();
            iGameApp.GetInstance().EnterScene(kGameSceneEnum.Map);
        }
        if (Input.GetKeyDown(KeyCode.I))
        {
            TNetManager.GetInstance().Login("GGYY_" + Random.Range(0, 101), string.Empty);
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            TNetManager.GetInstance().CreateRoom("GGYY's room", string.Empty, 1, 2, RoomCreateCmd.RoomType.limit, RoomCreateCmd.RoomSwitchMasterType.Auto, string.Empty);
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            TNetManager.GetInstance().ApplyRoomList(-1, 0, 10, RoomDragListCmd.ListType.all);
        }
        if (Input.GetKeyDown(KeyCode.F1))
        {
            ToggleMouseLock();
        }
        if (m_User == null || (m_GameScene.GameStatus != iGameSceneBase.kGameStatus.Gameing && m_GameScene.GameStatus != iGameSceneBase.kGameStatus.GameOver_ShowTime))
        {
            return;
        }
        Vector2 zero = Vector2.zero;
        if (m_User.IsCanMove())
        {
            if (Input.GetKey(KeyCode.W))
            {
                zero.y += 1f;
            }
            if (Input.GetKey(KeyCode.S))
            {
                zero.y += -1f;
            }
            if (Input.GetKey(KeyCode.A))
            {
                zero.x += -1f;
            }
            if (Input.GetKey(KeyCode.D))
            {
                zero.x += 1f;
            }
        }

        if (zero == Vector2.zero)
        {
            m_User.MoveStop();
        }
        else
        {
            m_User.MoveByCompass(zero.x, zero.y);
            Ray ray = m_Camera.ScreenPointToRay(m_GameState.ScreenCenter, 0f);
            m_User.LookAt(ray.GetPoint(1000f));
        }
        if (m_mouseLocked)
        {
            ApplyCursorLock(true);
            float axisX = Input.GetAxis("Mouse X");
            float axisY = Input.GetAxis("Mouse Y");
            if (Mathf.Abs(axisX) > 0f)
            {
                m_Camera.Yaw(Mathf.Clamp(axisX, -1f, 1f) * 270f * Time.deltaTime);
                if (m_User.IsCanAim())
                {
                    m_User.SetYaw(m_Camera.GetYaw());
                }
            }
            if (Mathf.Abs(axisY) > 0f)
            {
                m_Camera.Pitch(Mathf.Clamp(axisY, -1f, 1f) * 270f * Time.deltaTime);
            }
            if (m_User.IsCanAim() && (Mathf.Abs(axisX) > 0f || Mathf.Abs(axisY) > 0f))
            {
                Ray r = m_Camera.ScreenPointToRay(m_GameState.ScreenCenter, 0f);
                m_User.LookAt(r.GetPoint(1000f));
            }
        }
        else
        {
            ApplyCursorLock(false);
        }
        if (m_User.IsCanAttack())
        {
            if (Input.GetMouseButtonDown(0))
            {
                m_User.SetFire(true);
            }
            if (Input.GetMouseButtonUp(0))
            {
                m_User.SetFire(false);
            }
            if (Input.GetMouseButton(0))
            {
                if (!m_User.IsFire())
                {
                    m_User.SetFire(true);
                }
            }
            else
            {
                if (m_User.IsFire())
                {
                    m_User.SetFire(false);
                }
            }
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                m_User.UseSkill(m_User.SkillID);
            }
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            int num = m_nCurWeaponIndex - 1;
            while (num != m_nCurWeaponIndex && m_GameState.GetWeapon(num) == null)
            {
                num--;
                if (num < 0)
                {
                    num = 2;
                }
            }
            m_nCurWeaponIndex = num;
            m_User.SwitchWeapon(m_nCurWeaponIndex);
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            int num2 = m_nCurWeaponIndex + 1;
            while (num2 != m_nCurWeaponIndex && m_GameState.GetWeapon(num2) == null)
            {
                num2++;
                if (num2 >= 3)
                {
                    num2 = 0;
                }
            }
            m_nCurWeaponIndex = num2;
            m_User.SwitchWeapon(m_nCurWeaponIndex);
        }
        if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            m_GameScene.GameOver(true);
        }
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            m_GameScene.GameOver(false);
        }
    }

    public override void LateUpdate(float deltaTime)
    {
    }
    
    private void ToggleMouseLock()
    {
        m_mouseLocked = !m_mouseLocked;
        ApplyCursorLock(m_mouseLocked);
    }
    
    private void ApplyCursorLock(bool locked)
    {
        Cursor.lockState = locked ? CursorLockMode.Locked : CursorLockMode.None;
    }
}