using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class spriteSwap : MonoBehaviour
{

    public string taskName;
    public Button button;
    public Sprite added, add;

    public pinManager pinManager;
    public bool pin;
    
    
    public void setTaskName()
    {
        pinManager.taskName = string.Empty;
        pinManager.taskName = taskName;
        Debug.Log(taskName);
    }

    public void SpriteSwap()
    {
        if (!pin)
        {
            pin = true;
            button.GetComponent<Image>().sprite = added;
            
        }
        else if (pin)
        {
            pin = false;
            button.GetComponent<Image>().sprite = add;     
        }
       
    }

   
}
