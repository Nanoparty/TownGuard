using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TransitionIn : MonoBehaviour
{
    public GameObject wipeInImage;
    public float speed;
    public float endInPos;
    public bool transInDone;

    public GameObject wipeOutImage;
    public float endOutPos;
    public bool transOutDone;
    public bool transOutStart;
    public string nextScene;

    private void Start()
    {
        wipeInImage.GetComponent<Image>().enabled = true;
        wipeOutImage.GetComponent<Image>().enabled = false;
    }

    private void Update()
    {
        if (!transInDone)
        {
            Vector3 newPos = wipeInImage.transform.localPosition;
            newPos.x += speed * Time.deltaTime;
            wipeInImage.transform.localPosition = newPos;

            if (newPos.x > endInPos)
            {
                transInDone = true;
                wipeInImage.GetComponent<Image>().enabled = false;
            }
        }

        if (transOutStart)
        {
            wipeOutImage.GetComponent<Image>().enabled = true;

            Vector3 newPos = wipeOutImage.transform.localPosition;
            newPos.x += speed * Time.deltaTime;
            wipeOutImage.transform.localPosition = newPos;

            if (newPos.x > endOutPos)
            {
                SceneManager.LoadScene(nextScene, LoadSceneMode.Single);
            }
        }

        
    }
}
