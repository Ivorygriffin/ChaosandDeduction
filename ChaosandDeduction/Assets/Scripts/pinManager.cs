using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class pinManager : MonoBehaviour
{
  
    public bool tracking, nothingpinned;
    public string taskName;


    public GameObject frog, eye, leaf, water, ash, blood, firefly, moss, crystal, tooth, carrot, tears, boxstomp, cavescare, wellpush, laughgrave, breakstatue, carvetree, destroytent, bookfire;

   
    public void Pinned()
    {
        nothingpinned = false;
    }
    public void UnPinned()
    {
        nothingpinned=true;
    }

    public void Stickers()
    {
        if (tracking && nothingpinned)
        {
            Pin();
        }
        if (!tracking && !nothingpinned)
        {
            UnPin();
        }
    }


    public void Pin()
    {
        if (nothingpinned)
        {
            switch (taskName)
            {
                case "frog":
                    frog.SetActive(true);

                    break;
                case "eye":
                    eye.SetActive(true);

                    break;
                case "leaf":
                    leaf.SetActive(true);

                    break;
                case "water":
                    water.SetActive(true);

                    break;
                case "ash":
                    ash.SetActive(true);

                    break;
                case "blood":
                    blood.SetActive(true);

                    break;
                case "firefly":
                    firefly.SetActive(true);

                    break;
                case "moss":
                    moss.SetActive(true);

                    break;
                case "crystal":
                    crystal.SetActive(true);

                    break;
                case "tooth":
                    tooth.SetActive(true);

                    break;
                case "carrot":
                    carrot.SetActive(true);

                    break;
                case "tears":
                    tears.SetActive(true);

                    break;
                case "boxstomp":
                    boxstomp.SetActive(true);

                    break;
                case "cavescare":
                    cavescare.SetActive(true);

                    break;
                case "wellpush":
                    wellpush.SetActive(true);

                    break;
                case "laughgrave":
                    laughgrave.SetActive(true);

                    break;
                case  "breakstatue":
                    breakstatue.SetActive(true);

                    break;
                case "carvetree":
                    carvetree.SetActive(true);

                    break;
                case "destroytent":
                    destroytent.SetActive(true);

                    break;
                case "bookfire":
                    bookfire.SetActive(true);

                    break;
                default:
                    
                    break;
            }

        }

    }
    public void UnPin()
    {

        switch (taskName)
        {
            case "frog":
                frog.SetActive(false);
                break;
            case "eye":
                eye.SetActive(false);
                break;
            case "leaf":
                leaf.SetActive(false);
                break;
            case "water":
                water.SetActive(false);
                break;
            case "ash":
                ash.SetActive(false);
                break;
            case "blood":
                blood.SetActive(false);
                break;
            case "firefly":
                firefly.SetActive(false);
                break;
            case "moss":
                moss.SetActive(false);
                break;
            case "crystal":
                crystal.SetActive(false);
                break;
            case "tooth":
                tooth.SetActive(false);
                break;
            case "carrot":
                carrot.SetActive(false);
                break;
            case "tears":
                tears.SetActive(false);
                break;
            case "boxstomp":
                boxstomp.SetActive(false);
                break;
            case "cavescare":
                cavescare.SetActive(false);
                break;
            case "wellpush":
                wellpush.SetActive(false);
                break;
            case "laughgrave":
                laughgrave.SetActive(false);
                break;
            case "breakstatue":
                breakstatue.SetActive(false);
                break;
            case "carvetree":
                carvetree.SetActive(false);
                break;
            case "destroytent":
                destroytent.SetActive(false);
                break;
            case "bookfire":
                bookfire.SetActive(false);
                break;
            default:
                
                break;
        }

    }


}
