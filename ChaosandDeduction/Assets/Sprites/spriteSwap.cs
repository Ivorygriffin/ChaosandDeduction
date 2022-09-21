using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class spriteSwap : MonoBehaviour
{
    public Button button;
    public Sprite added, add;
    public bool tracking;
    public task taskName;
    
    public GameObject frog, eye, leaf, water,ash,blood,firefly,moss,crystal,tooth,carrot,tears,boxstomp,cavescare,wellpush,laughgrave,breakstatue,carvetree,destroytent,bookfire;
    public enum task
    {
        frog,
        eye,
        leaf,
        water,
        ash,
        blood,
        firefly,
        moss,
        crystal,
        tooth,
        carrot,
        tears,
        boxstomp,
        cavescare,
        wellpush,
        laughgrave,
        breakstatue,
        carvetree,
        destroytent,
        bookfire


    }

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

    public void Stickers()
    {
        if (tracking)
        {
            Pin();
        }
        if (!tracking)
        {
            UnPin();
        }
    }


    public void Pin()
    {
        switch (taskName) 
        {
            case task.frog:
                frog.SetActive(true);
                break;
            case task.eye:
                eye.SetActive(true);
                break;
            case task.leaf:
                leaf.SetActive(true);
                break;
            case task.water:
                water.SetActive(true);
                break;
            case task.ash:
                ash.SetActive(true);
                break;
            case task.blood:
                blood.SetActive(true);
                break;
            case task.firefly:
                firefly.SetActive(true);
                break;
            case task.moss:
                moss.SetActive(true);
                break;
            case task.crystal:
                crystal.SetActive(true);
                break;
            case task.tooth:
                tooth.SetActive(true);
                break;
            case task.carrot:
                carrot.SetActive(true);
                break;
            case task.tears:
                tears.SetActive(true);
                break;
            case task.boxstomp:
                boxstomp.SetActive(true);
                break;
            case task.cavescare:
                cavescare.SetActive(true);
                break;
            case task.wellpush:
                wellpush.SetActive(true);
                break;
            case task.laughgrave:
                laughgrave.SetActive(true);
                break;
            case task.breakstatue:
                breakstatue.SetActive(true);
                break;
            case task.carvetree:
                carvetree.SetActive(true);
                break;
            case task.destroytent:
                destroytent.SetActive(true);
                break;
            case task.bookfire:
                bookfire.SetActive(true);
                break;
            default:
                Debug.Log("label not selected");
                break;
        }

    }
    public void UnPin()
    {
        switch (taskName) 
        {
            case task.frog:
                frog.SetActive(false);
                break;
            case task.eye:
                eye.SetActive(false);
                break;
            case task.leaf:
                leaf.SetActive(false);
                break;
            case task.water:
                water.SetActive(false);
                break;
            case task.ash:
                ash.SetActive(false);
                break;
            case task.blood:
                blood.SetActive(false);
                break;
            case task.firefly:
                firefly.SetActive(false);
                break;
            case task.moss:
                moss.SetActive(false);
                break;
            case task.crystal:
                crystal.SetActive(false);
                break;
            case task.tooth:
                tooth.SetActive(false);
                break;
            case task.carrot:
                carrot.SetActive(false);
                break;
            case task.tears:
                tears.SetActive(false);
                break;
            case task.boxstomp:
                boxstomp.SetActive(false);
                break;
            case task.cavescare:
                cavescare.SetActive(false);
                break;
            case task.wellpush:
                wellpush.SetActive(false);
                break;
            case task.laughgrave:
                laughgrave.SetActive(false);
                break;
            case task.breakstatue:
                breakstatue.SetActive(false);
                break;
            case task.carvetree:
                carvetree.SetActive(false);
                break;
            case task.destroytent:
                destroytent.SetActive(false);
                break;
            case task.bookfire:
                bookfire.SetActive(false);
                break;
            default:
                Debug.Log("label not selected");
                break;
        }

    }

}
