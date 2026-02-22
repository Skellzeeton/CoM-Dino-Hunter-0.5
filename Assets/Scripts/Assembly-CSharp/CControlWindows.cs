using TNetSdk;
using UnityEngine;

public class CControlWindows : CControlBase
{
    protected int m_nCurWeaponIndex;

    // New field: whether mouse is locked (camera-control mode)
    private bool m_mouseLocked = false;

    // small deadzone for mouse movement considered "no movement"
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

        // Debug / admin keys (unchanged)
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

        // Toggle mouse lock with right mouse button (on press) or F1
        if (Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.F1))
        {
            ToggleMouseLock();
        }

        // If no user or game not in play states, early out (unchanged)
        if (m_User == null || (m_GameScene.GameStatus != iGameSceneBase.kGameStatus.Gameing && m_GameScene.GameStatus != iGameSceneBase.kGameStatus.GameOver_ShowTime))
        {
            return;
        }

        // Movement (WASD) - unchanged
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

        // Camera control: if mouse is locked, use mouse delta to rotate camera
        // (previously you only rotated while RMB was held; now we rotate while locked)
        if (m_mouseLocked)
        {
            // lock cursor for compatibility, also set modern API
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

            // Assist-aim: if the player moves the mouse significantly, disable assist aim
            if (Mathf.Abs(axisX) > 0.1f || Mathf.Abs(axisY) > 0.1f)
            {
                m_GameScene.AssistAim_Stop();
            }
            else if (m_User.IsFire() && !m_GameScene.IsAssistAim())
            {
                m_GameScene.AssistAim_Start();
            }
        }
        else
        {
            // unlock cursor mode
            ApplyCursorLock(false);

            // If assist-aim was active while unlocked and the user is not firing, stop it
            if (!m_User.IsFire() && m_GameScene.IsAssistAim())
            {
                m_GameScene.AssistAim_Stop();
            }
        }

        // --- Shooting: left mouse only ---
        // Start shooting on mouse down, keep shooting while held, stop on mouse up
        if (m_User.IsCanAttack())
        {
            // left button pressed this frame -> start fire
            if (Input.GetMouseButtonDown(0))
            {
                m_User.SetFire(true);
            }

            // left button released this frame -> stop fire
            if (Input.GetMouseButtonUp(0))
            {
                m_User.SetFire(false);
            }

            // also, if left button is being held, ensure firing continues (useful if game logic checks hold)
            if (Input.GetMouseButton(0))
            {
                if (!m_User.IsFire())
                {
                    m_User.SetFire(true);
                }
            }
            else
            {
                // If not holding left mouse, ensure not firing (defensive)
                if (m_User.IsFire())
                {
                    m_User.SetFire(false);
                }
            }

            // Skill keys (unchanged)
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                m_User.UseSkill(1);
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                m_User.UseSkill(2);
            }
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                m_User.UseSkill(3);
            }
            if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                m_User.UseSkill(4);
            }
            if (Input.GetKeyDown(KeyCode.Alpha5))
            {
                m_User.UseSkill(5);
            }
        }

        // Weapon quick switch (Q/E) - unchanged
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

        // Dev shortcuts - unchanged
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

    /// <summary>
    /// Toggle mouse lock state.
    /// </summary>
    private void ToggleMouseLock()
    {
        m_mouseLocked = !m_mouseLocked;

        // When toggling, if locking we might want to stop assist-aim immediately
        if (m_mouseLocked)
        {
            m_GameScene.AssistAim_Stop();
        }
        else
        {
            // when unlocking, ensure we stop firing assist aim if not firing
            if (!m_User.IsFire() && m_GameScene.IsAssistAim())
            {
                m_GameScene.AssistAim_Stop();
            }
        }

        ApplyCursorLock(m_mouseLocked);
    }

    /// <summary>
    /// Apply cursor lock state via modern and legacy APIs for widest compatibility.
    /// </summary>
    private void ApplyCursorLock(bool locked)
    {
        // Modern API (recommended)
        Cursor.visible = !locked;
#if UNITY_5_2_OR_NEWER
        Cursor.lockState = locked ? CursorLockMode.Locked : CursorLockMode.None;
#endif
        // Legacy API for compatibility with older code paths
#pragma warning disable 618
        Screen.lockCursor = locked;
#pragma warning restore 618
    }
}