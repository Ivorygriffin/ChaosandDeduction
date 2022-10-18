using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class spriteSwap : MonoBehaviour
{

    public string taskName;
    public Button button;
    public Sprite added, add;
    public bool T, NP;
    public pinManager pinManager;

    public void Start()
    {
        T = pinManager.tracking;
        NP = pinManager.nothingpinned;
    }
    public void setTaskName()
    {
        pinManager.taskName = taskName;
    }

    public void SpriteSwap()
    {
        if (T && NP)
        {
            button.GetComponent<Image>().sprite = added;
            T = false;
        }
        else if (!T)
        {
            button.GetComponent<Image>().sprite = add;
            T = true;
        }
    }
}
