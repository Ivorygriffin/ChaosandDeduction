using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class spriteSwap : MonoBehaviour
{
    public Button button;
    public Sprite added, add;
    public bool tracking;


    public void SpriteSwap()
    {
        if (tracking == true)
        {
            button.GetComponent<Image>().sprite = added;
            tracking = false;
        }
        else if (tracking == false)
        {
            button.GetComponent<Image>().sprite = add;
            tracking = true;
        }
    }



}
