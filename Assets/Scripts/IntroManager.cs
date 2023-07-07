using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class IntroManager : MonoBehaviour
{
    PlayerControls inputActions;

    public TransitionIn transition;

    public List<string> lines;
    public List<Sprite> images;

    public int index;

    public Image background;
    public TextScroller textScroll;

    public bool next;
    public bool l_Click;
    public bool exit = false;

    private void Awake()
    {
        if (inputActions == null)
        {
            inputActions = new PlayerControls();
        }

        inputActions.Enable();
        textScroll = GetComponentInChildren<TextScroller>();
    }

    private void Start()
    {
        index = 0;
        background.sprite = images[index];
        textScroll.text = lines[index];
        textScroll.begin = true;
        AudioManager.am.StartIntro();
    }

    private void Update()
    {
        l_Click = inputActions.PlayerActions.Left_Mouse.phase == UnityEngine.InputSystem.InputActionPhase.Performed;

        if (textScroll.done && l_Click)
        {
            AudioManager.am.Click();
            if (index < lines.Count - 1)
            {
                index++;
                background.sprite = images[index];
                textScroll.curLength = 0;
                textScroll.text = lines[index];
                textScroll.begin = true;
                textScroll.done = false;
            }
            else if (!exit)
            {
                exit = true;
                AudioManager.am.EndIntro();
                AudioManager.am.Whoosh();
                transition.transOutStart = true;
                //SceneManager.LoadScene("Town", LoadSceneMode.Single);
                textScroll.done = false;
            }
        }
    }

    private void LateUpdate()
    {
        l_Click = false;
    }
}
