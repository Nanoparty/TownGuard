using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{

    InputHandler inputHandler;
    Animator anim;
    CameraHandler cameraHandler;
    PlayerLocomotion playerLocomotion;
    PlayerAudio playerAudio;
    InteractableUI interactableUI;
    WeaponSlotManager weaponSlotManager;
    DeathFadeIn deathFadeIn;
    PlayerStats playerStats;
    QuestManager questManager;
    SpawnManager spawnManager;

    public GameObject interactionPopupGameObject;
    public GameObject itemPopUpGameObject;
    public GameObject dialogPopUp;
    public GameObject VictoryBanner;
    public GameObject PauseObject;
    public GameObject ControlsObject;
    public GameObject DeathBanner;

    public Slider musicSlider;
    public Slider soundSlider;

    public Transform playerSpawn;

    public GameObject lastInteraction;
    public TransitionIn transitionOut;

    public List<string> questDialog;
    public int dialogIndex;

    [Header("Player Flags")]
    public bool isInteracting;
    public bool isSprinting;
    public bool isDancing;
    public bool isInAir;
    public bool isGrounded;
    public bool canDoCombo;
    public bool isJumping;
    public bool canRespawn;
    public bool victory;
    public bool paused;
    public bool controls;
    public bool speaking;
    public bool questSpeak;

    Interactable interaction;

    private void Awake()
    {
        cameraHandler = FindObjectOfType<CameraHandler>();

        Cursor.lockState = CursorLockMode.Locked;
    }

    void Start()
    {
        inputHandler = GetComponent<InputHandler>();
        anim = GetComponentInChildren<Animator>();
        playerLocomotion = GetComponent<PlayerLocomotion>();
        playerAudio = GetComponent<PlayerAudio>();
        interactableUI = FindObjectOfType<InteractableUI>();
        deathFadeIn = FindObjectOfType<DeathFadeIn>();
        playerStats = GetComponent<PlayerStats>();
        spawnManager = FindObjectOfType<SpawnManager>();
        questManager = FindObjectOfType<QuestManager>();

        PauseObject.SetActive(false);
        ControlsObject.SetActive(false);
        DeathBanner.SetActive(false);
    }

    void Update()
    {
        float delta = Time.deltaTime;
        isInteracting = anim.GetBool("isInteracting");
        canDoCombo = anim.GetBool("canDoCombo");
        isJumping = anim.GetBool("isJumping");
        anim.SetBool("isInAir", isInAir);

        inputHandler.rollFlag = false;
        inputHandler.sprintFlag = false;
        inputHandler.dancingFlag = false;

        inputHandler.TickInput(delta);

        CheckForDialog();
        CheckForQuest();
        HandlePause();
        if(victory)
        {
            HandleVictory();
        }
        if (paused || speaking || questSpeak)
        {
            playerLocomotion.HandlePaused();
            playerLocomotion.HandleFalling(delta, Vector3.zero);
            return;
        }

        playerLocomotion.HandleMovement(delta);
        playerLocomotion.HandleFalling(delta, playerLocomotion.moveDirection);
        playerLocomotion.HandleRollingAndSprinting(delta);
        playerLocomotion.HandleDancing(delta);
        playerLocomotion.HandleJumping();
        playerLocomotion.HandlePotion();
        playerStats.RegenStamina();

        CheckForInteractableObject();
        //HandleDeath();
        HandleRespawn();
        
        
    }

    private void FixedUpdate()
    {
        float delta = Time.fixedDeltaTime;

        if (cameraHandler != null)
        {
            cameraHandler.FollowTarget(delta);
            cameraHandler.HandleCameraRotation(delta, inputHandler.mouseX, inputHandler.mouseY);
        }
        else
        {
            cameraHandler = CameraHandler.singleton;
        }
    }

    private void LateUpdate()
    {
        inputHandler.rollFlag = false;
        inputHandler.sprintFlag = false;
        inputHandler.dancingFlag = false;
        inputHandler.rb_Input = false;
        inputHandler.rt_Input = false;
        inputHandler.d_Right = false;
        inputHandler.d_Left = false;
        inputHandler.d_Up = false;
        inputHandler.d_Down = false;
        inputHandler.a_Input = false;
        inputHandler.jump_Input = false;
        inputHandler.key_1 = false;
        inputHandler.key_2 = false;
        inputHandler.potion_Input = false;
        inputHandler.pause_Input = false;

        if (isInAir)
        {
            playerLocomotion.inAirTimer = playerLocomotion.inAirTimer + Time.deltaTime;
        }
    }

    public void CheckForInteractableObject()
    {
        RaycastHit hit;

        Debug.DrawRay(transform.position, Vector3.forward, Color.red);

        if (Physics.SphereCast(transform.position, 0.1f, transform.forward, out hit, 1f, cameraHandler.ignoreLayers))
        {
            if(hit.collider.tag == "Interactable")
            {
                Debug.Log("Interactable Detected");
                Interactable interactableObject = hit.collider.GetComponent<Interactable>();
                lastInteraction = hit.collider.gameObject;

                if (interactableObject != null)
                {
                    string interactableText = interactableObject.interactableText;
                    interactableUI.interactionText.text = interactableText + " (F)";

                    if (interactableObject.isInteracting)
                    {
                        interactionPopupGameObject.SetActive(false);
                    }
                    else
                    {
                        interactionPopupGameObject.SetActive(true);

                        if (inputHandler.a_Input)
                        {
                            hit.collider.GetComponent<Interactable>().Interact(this);
                            interactableObject.GetComponent<Interactable>().isInteracting = true;
                            Debug.Log("Start Interaction");
                            playerAudio.Interact1();
                            interactionPopupGameObject.SetActive(false);
                        }
                    }
                }
            }
        }
        else
        {
            if (interactionPopupGameObject != null)
            {
                interactionPopupGameObject.SetActive(false);
            }
            if (itemPopUpGameObject != null && itemPopUpGameObject.activeSelf && inputHandler.a_Input)
            {
                playerAudio.Interact2();
                itemPopUpGameObject.SetActive(false);
            }
        }
    }

    public void CheckForDialog()
    {
        if (speaking)
        {
            
            if (dialogPopUp.GetComponentInChildren<TextScroller>().done && inputHandler.a_Input)
            {
                speaking = false;
                TextScroller ts = dialogPopUp.GetComponentInChildren<TextScroller>();
                ts.begin = false;
                ts.done = false;
                ts.curLength = 0;
                ts.text = "";
                dialogPopUp.GetComponentInChildren<TMP_Text>().text = "";
                dialogPopUp.SetActive(false);
            }
            return;
        }

        RaycastHit hit;

        if (Physics.SphereCast(transform.position, 0.1f, transform.forward, out hit, 1f, cameraHandler.ignoreLayers))
        {
            if (hit.collider.tag == "Dialog")
            {
                Debug.Log("Interactable Detected");
                Interactable interactableObject = hit.collider.GetComponent<Interactable>();
                if (interactableObject != null)
                {
                    string interactableText = interactableObject.interactableText;
                    interactableUI.interactionText.text = interactableText + " (F)";
                    interactionPopupGameObject.SetActive(true);
                    if (inputHandler.a_Input)
                    {
                        speaking = true;
                        playerAudio.Interact1();
                        hit.collider.GetComponent<Interactable>().Interact(this);
                        interactionPopupGameObject.SetActive(false);
                    }
                }
            }
        }
        else
        {
            interactionPopupGameObject.SetActive(false);
        }
    }

    public void CheckForQuest()
    {
        if (questSpeak)
        {
            if (inputHandler.a_Input)
            {
                TextScroller ts = dialogPopUp.GetComponentInChildren<TextScroller>();
                if (questManager.hasNext() && ts.done)
                {
                    questManager.Next();
                    FindObjectOfType<QuestInteract>().Interact(this);

                }else if (ts.done)
                {
                    questManager.Next();
                    if (questManager.CurrentQuest == 4)
                    {
                        VictoryBanner.GetComponent<FadeOut>().Enable();
                        AudioManager.am.Victory();
                        paused = true;
                        victory = true;
                    }
                    questSpeak = false;
                    dialogPopUp.SetActive(false);

                }
            }
            return;
        }

        if (paused) return;

        RaycastHit hit;

        if (Physics.SphereCast(transform.position, 0.1f, transform.forward, out hit, 1f, cameraHandler.ignoreLayers))
        {
            if (hit.collider.tag == "Quest")
            {
                Debug.Log("Interactable Detected");
                Interactable interactableObject = hit.collider.GetComponent<Interactable>();
                if (interactableObject != null)
                {
                    string interactableText = interactableObject.interactableText;
                    interactableUI.interactionText.text = interactableText + " (F)";
                    interactionPopupGameObject.SetActive(true);
                    if (inputHandler.a_Input)
                    {
                        questSpeak = true;
                        playerAudio.Interact1();
                        hit.collider.GetComponent<Interactable>().Interact(this);
                        interactionPopupGameObject.SetActive(false);
                    }
                }
            }
        }
        else
        {
            interactionPopupGameObject.SetActive(false);
        }
    }

    public void HandleDeath()
    {
        Debug.Log("HandleDeath");
        DeathBanner.SetActive(true);
        //deathFadeIn.Begin();
        DeathBanner.GetComponentInChildren<DeathFadeIn>().Begin();
        AudioManager.am.EndBattle();
    }

    public void HandleRespawn()
    {
        if (canRespawn && inputHandler.a_Input)
        {
            canRespawn = false;
            DeathBanner.SetActive(false);

            transform.position = playerSpawn.position;

            deathFadeIn.Reset();
            playerStats.Reset();
            questManager.Reset();
            spawnManager.Reset();
        }
    }

    public void HandleVictory()
    {
        if (victory && inputHandler.jump_Input)
        {
            transitionOut.transOutStart = true;
        }
    }

    public void HandlePause()
    {
        if(!paused && inputHandler.pause_Input)
        {
            Debug.Log("Pause");
            paused = true;
            PauseObject.SetActive(true);
            Cursor.lockState = CursorLockMode.Confined;
            InitalizePauseMenu();
        }
    }

    public void InitalizePauseMenu()
    {
        GameObject sliders = PauseObject.transform.GetChild(0).transform.GetChild(1).gameObject;
        GameObject buttons = PauseObject.transform.GetChild(0).transform.GetChild(2).gameObject;

        musicSlider = sliders.transform.GetChild(0).GetComponentInChildren<Slider>();
        soundSlider = sliders.transform.GetChild(1).GetComponentInChildren<Slider>();

        musicSlider.onValueChanged.AddListener(delegate { MusicListener(); });
        soundSlider.onValueChanged.AddListener(delegate { SoundListener(); });

        buttons.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(MenuListener);
        buttons.transform.GetChild(1).GetComponent<Button>().onClick.AddListener(ControlsListener);
        buttons.transform.GetChild(2).GetComponent<Button>().onClick.AddListener(ResumeListener);
    }

    private void MusicListener()
    {
        Data.musicVolume = musicSlider.value;
    }

    private void SoundListener()
    {
        Data.soundVolume = soundSlider.value;
    }

    private void MenuListener()
    {
        if (controls) return;

        AudioManager.am.Click();
        AudioManager.am.Whoosh();
        transitionOut.transOutStart = true;
    }

    public void ControlsListener()
    {
        if (controls) return;

        if (!paused)
        {
            Debug.Log("Pause");
            paused = true;
            //PauseObject.SetActive(true);
            Cursor.lockState = CursorLockMode.Confined;
            //InitalizePauseMenu();
        }

        controls = true;
        AudioManager.am.Click();
        ControlsObject.SetActive(true);
        ControlsObject.GetComponentInChildren<Button>().onClick.AddListener(CloseControls);
    }

    private void ResumeListener()
    {
        if (controls) return;

        AudioManager.am.Click();
        paused = false;
        PauseObject.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void CloseControls()
    {
        AudioManager.am.Click();
        ControlsObject.SetActive(false);
        controls = false;

        if(paused && !PauseObject.activeSelf)
        {
            paused = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
}
