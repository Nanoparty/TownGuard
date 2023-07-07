using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    public float horizontal;
    public float vertical;
    public float moveAmount;
    public float mouseX;
    public float mouseY;

    public bool b_Input;
    public bool rb_Input;
    public bool rt_Input;
    public bool a_Input;
    public bool jump_Input;

    public bool d_Up;
    public bool d_Down;
    public bool d_Left;
    public bool d_Right;

    public bool key_1;
    public bool key_2;

    public bool l_Click;
    public bool r_Click;

    public bool potion_Input;
    public bool pause_Input;
    public bool lockOn_Input;

    public bool rollFlag;
    public bool sprintFlag;
    public bool comboFlag;
    public bool lockOnFlag;
    public float rollInputTimer;
    

    public bool dancingFlag;

    PlayerControls inputActions;
    CameraHandler cameraHandler;
    PlayerAttacker playerAttacker;
    PlayerInventory playerInventory;
    PlayerManager playerManager;
    PlayerStats playerStats;

    Vector2 movementInput;
    Vector2 cameraInput;

    private void Awake()
    {
        cameraHandler = CameraHandler.singleton;
        playerAttacker = GetComponent<PlayerAttacker>();
        playerInventory = GetComponent<PlayerInventory>();
        playerManager = GetComponent<PlayerManager>();
        playerStats = GetComponent<PlayerStats>();
    }

    public void OnEnable()
    {
        if (inputActions == null)
        {
            inputActions = new PlayerControls();
            inputActions.PlayerMovement.Movement.performed += inputActions => movementInput = inputActions.ReadValue<Vector2>();
            inputActions.PlayerMovement.Camera.performed += i => cameraInput = i.ReadValue<Vector2>();
            inputActions.PlayerActions.LockOn.performed += i => lockOn_Input = true;
        }

        inputActions.Enable();
    }

    private void OnDisable()
    {
        inputActions.Disable();
    }

    public void TickInput(float delta)
    {
        MoveInput(delta);
        MouseInput();
        HandleRollInput(delta);
        HandleDanceInput(delta);
        HandleAttackInput(delta);
        HandleQuickSlotsInput();
        HandleInteractionInput();
        HandleJumpInput();
        HandlePotionInput();
        HandlePauseInput();
        HandleLockOnInput();
    }

    private void MouseInput()
    {
        l_Click = inputActions.PlayerActions.Left_Mouse.phase == UnityEngine.InputSystem.InputActionPhase.Performed;
        r_Click = inputActions.PlayerActions.Right_Mouse.phase == UnityEngine.InputSystem.InputActionPhase.Performed;
    }

    private void MoveInput(float delta)
    {
        horizontal = movementInput.x;
        vertical = movementInput.y;
        moveAmount = Mathf.Clamp01(Mathf.Abs(horizontal) + Mathf.Abs(vertical));
        mouseX = cameraInput.x;
        mouseY = cameraInput.y;
    }

    private void HandleRollInput(float delta)
    {
        b_Input = inputActions.PlayerActions.Roll.phase == UnityEngine.InputSystem.InputActionPhase.Performed;

        if (b_Input)
        {
            rollInputTimer += delta;
            sprintFlag = true;
        }
        else
        {
            if (rollInputTimer > 0 && rollInputTimer < 0.5f)
            {
                sprintFlag = false;
                rollFlag = true;
            }

            rollInputTimer = 0;
        }
    }

    private void HandleDanceInput(float delta)
    {
        bool d_Input = inputActions.PlayerActions.Dance1.phase == UnityEngine.InputSystem.InputActionPhase.Performed;

        if (d_Input)
        {
            dancingFlag = true;
        }
    }

    private void HandleAttackInput(float delta)
    {
        inputActions.PlayerActions.RB.performed += i => rb_Input = true;
        inputActions.PlayerActions.RT.performed += i => rt_Input = true;

        if (playerManager.paused) return;

        if (rb_Input && playerStats.currentStamina > playerInventory.rightWeapon.baseStamina * playerInventory.rightWeapon.lightAttackMultiplier)
        {
            if (playerManager.canDoCombo)
            {
                comboFlag = true;
                playerAttacker.HandleWeaponCombo(playerInventory.rightWeapon);
                comboFlag = false;
            }
            else
            {
                if (playerManager.isInteracting)
                    return;
                if (playerManager.canDoCombo)
                    return;
                playerAttacker.HandleLightAttack(playerInventory.rightWeapon);
            }
        }

        if (rt_Input && playerStats.currentStamina > playerInventory.rightWeapon.baseStamina * playerInventory.rightWeapon.lightAttackMultiplier)
        {
            if (playerManager.canDoCombo)
            {
                comboFlag = true;
                playerAttacker.HandleWeaponCombo(playerInventory.rightWeapon);
                comboFlag = false;
            }
            else
            {
                if (playerManager.isInteracting)
                    return;
                if (playerManager.canDoCombo)
                    return;
                playerAttacker.HandleHeavyAttack(playerInventory.rightWeapon);
            }
        }
    }

    private void HandleQuickSlotsInput()
    {
        //inputActions.PlayerActions.D_Right.performed += i => d_Right = true;
        //inputActions.PlayerActions.D_Left.performed += i => d_Left = true;

        inputActions.PlayerActions.Key_1.performed += i => key_1 = true;
        inputActions.PlayerActions.Key_2.performed += i => key_2 = true;

        if (key_1)
        {
            playerInventory.ChangeRightWeapon(new string[1] { "Enemy" });
        }
        else if (key_2)
        {
            //playerInventory.ChangeLeftWeapon(new string[1] { "Enemy" });
            //playerInventory.UsePotion();
        }
    }

    private void HandlePotionInput()
    {
        inputActions.PlayerActions.Potion.performed += inputActions => potion_Input = true;

        //if (potion_Input)
        //{
        //    Debug.Log("G");
        //    //playerInventory.UsePotion();
        //    PlayerLocomotion.HandlePotion();
        //}
    }

    private void HandleInteractionInput()
    {
        inputActions.PlayerActions.A.performed += inputActions => a_Input = true;
    }

    private void HandleJumpInput()
    {
        inputActions.PlayerActions.Jump.performed += inputActions => jump_Input = true;
    }

    private void HandlePauseInput()
    {
        inputActions.PlayerActions.Pause.performed += inputActions => pause_Input = true;
    }

    private void HandleLockOnInput()
    {
        if (lockOn_Input && !lockOnFlag)
        {
            Debug.Log("Setting Lockon");
            cameraHandler.ClearLockOnTargets();
            lockOn_Input = false;
            cameraHandler.HandleLockOn();
            if (cameraHandler.nearestLockOnTarget != null)
            {
                cameraHandler.currentLockOnTarget = cameraHandler.nearestLockOnTarget;
                lockOnFlag = true;
            }
            
        }
        else if (lockOn_Input && lockOnFlag)
        {
            Debug.Log("Clearing Lockon");
            cameraHandler.ClearLockOnTargets();
            lockOn_Input = false;
            lockOnFlag = false;
        }
    }
}
