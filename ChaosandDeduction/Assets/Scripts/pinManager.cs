using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class pinManager : MonoBehaviour
{

    public bool on;
    public string taskName;


    public GameObject frog, eye, leaf, water, ash, blood, firefly, moss, crystal, tooth, carrot, tears, boxstomp, cavescare, wellpush, laughgrave, breakstatue, carvetree, destroytent, bookfire;
    public GameObject frogm, eyem, leafm, waterm, ashm, bloodm, fireflym, mossm, crystalm, toothm, carrotm, tearsm, boxstompm, cavescarem, wellpushm, laughgravem, breakstatuem, carvetreem, destroytentm, bookfirem;

    public TaskScriptableObject[] tasks;

    public GameObject[] everysticker;
    public Button[] everyPin;
    public GameObject[] everymap;
    public GameObject initialMap;
    public spriteSwap ss;

    public void Pin()
    {



        foreach (GameObject map in everymap)
        {
            map.SetActive(false);
        }
        foreach (Button btn in everyPin)
        {
            btn.GetComponent<Image>().sprite = ss.added;
        }
        foreach (GameObject sticker in everysticker)
        {
            sticker.SetActive(false);
        }
        Debug.Log("unpin run");
        switch (taskName)
        {
            case "frog":
                frog.SetActive(true);
                frogm.SetActive(true);

                NavManager.instance.target = tasks[0].firstPoint;
                break;
            case "eye":
                eye.SetActive(true);
                eyem.SetActive(true);

                NavManager.instance.target = tasks[1].firstPoint;
                break;
            case "leaf":
                leaf.SetActive(true);
                leafm.SetActive(true);

                NavManager.instance.target = tasks[2].firstPoint;
                break;
            case "water":
                water.SetActive(true);
                waterm.SetActive(true);

                NavManager.instance.target = tasks[3].firstPoint;
                break;
            case "ash":
                ash.SetActive(true);
                ashm.SetActive(true);

                NavManager.instance.target = tasks[4].firstPoint;
                break;
            case "blood":
                blood.SetActive(true);
                bloodm.SetActive(true);

                NavManager.instance.target = tasks[5].firstPoint;
                break;
            case "firefly":
                firefly.SetActive(true);
                fireflym.SetActive(true);

                NavManager.instance.target = tasks[6].firstPoint;
                break;
            case "moss":
                moss.SetActive(true);
                mossm.SetActive(true);

                NavManager.instance.target = tasks[7].firstPoint;
                break;
            case "crystal":
                crystal.SetActive(true);
                crystalm.SetActive(true);

                NavManager.instance.target = tasks[8].firstPoint;
                break;
            case "tooth":
                tooth.SetActive(true);
                toothm.SetActive(true);

                NavManager.instance.target = tasks[9].firstPoint;
                break;
            case "carrot":
                carrot.SetActive(true);
                carrotm.SetActive(true);

                NavManager.instance.target = tasks[10].firstPoint;
                break;
            case "tears":
                tears.SetActive(true);
                tearsm.SetActive(true);

                NavManager.instance.target = tasks[11].firstPoint;
                break;
            case "boxstomp":
                boxstomp.SetActive(true);
                boxstompm.SetActive(true);

                NavManager.instance.target = tasks[12].firstPoint;
                break;
            case "cavescare":
                cavescare.SetActive(true);
                cavescarem.SetActive(true);

                NavManager.instance.target = tasks[13].firstPoint;
                break;
            case "wellpush":
                wellpush.SetActive(true);
                wellpushm.SetActive(true);

                NavManager.instance.target = tasks[14].firstPoint;
                break;
            case "laughgrave":
                laughgrave.SetActive(true);
                laughgravem.SetActive(true);

                NavManager.instance.target = tasks[15].firstPoint;
                break;
            case "breakstatue":
                breakstatue.SetActive(true);
                breakstatuem.SetActive(true);

                NavManager.instance.target = tasks[16].firstPoint;
                break;
            case "carvetree":
                carvetree.SetActive(true);
                carvetreem.SetActive(true);

                NavManager.instance.target = tasks[17].firstPoint;
                break;
            case "destroytent":
                destroytent.SetActive(true);
                destroytentm.SetActive(true);

                NavManager.instance.target = tasks[18].firstPoint;
                break;
            case "bookfire":
                bookfire.SetActive(true);
                bookfirem.SetActive(true);

                NavManager.instance.target = tasks[19].firstPoint;
                break;
            default:
                initialMap.SetActive(true);
                break;
        }




    }





}
