using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class spriteSwap : MonoBehaviour
{

    public string taskName;
    public Button button;
    public Sprite added, add;
    public int clickTimes;
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
            clickTimes -=1;
            pin = true;
            button.GetComponent<Image>().sprite = added;
            
        }
        else if (pin)
        {
            clickTimes += 1;
            pin = false;
            button.GetComponent<Image>().sprite = add;     
        }
        if (clickTimes == -1)
        {
            Debug.Log("-1");
            foreach (GameObject go in pinManager.everymap)
            {
                Debug.Log("turnoffallmap");
                go.SetActive(false);
            }
            pinManager.initialMap.SetActive(true);

        }
    }

   
}
