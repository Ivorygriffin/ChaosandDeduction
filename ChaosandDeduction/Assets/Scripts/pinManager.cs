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
  
    public GameObject[] everysticker;
    public Button[] everyPin;
    public GameObject[] everymap;
    public GameObject initialMap;
    public spriteSwap ss;

    public void Pin()
    {

        
      
            foreach(GameObject map in everymap)
            {
                map.SetActive(false);
            }
            foreach(Button btn in everyPin)
            {
                btn.GetComponent<Image>().sprite = ss.added;
            }
            foreach(GameObject sticker in everysticker)
            {
                sticker.SetActive(false);
            }
            Debug.Log("unpin run");
            switch (taskName)
            {
                case "frog":
                    frog.SetActive(true); 
                frogm.SetActive(true);
                
                    break;
                case "eye":
                    eye.SetActive(true); 
                eyem.SetActive(true);
                
                break;
                case "leaf":
                    leaf.SetActive(true); 
                leafm.SetActive(true);
               
                break;
                case "water":
                    water.SetActive(true);     
                waterm.SetActive(true);
                
                break;
                case "ash":
                    ash.SetActive(true);  
                ashm.SetActive(true);
                
                break;
                case "blood":
                    blood.SetActive(true);       
                bloodm.SetActive(true);
                
                break;
                case "firefly":
                    firefly.SetActive(true);           
                fireflym.SetActive(true);
                
                break;
                case "moss":
                    moss.SetActive(true);   
                mossm.SetActive(true);
               
                break;
                case "crystal":
                    crystal.SetActive(true);  
                crystalm.SetActive(true);
                
                break;
                case "tooth":
                    tooth.SetActive(true); 
                toothm.SetActive(true);
               
                break;
                case "carrot":
                    carrot.SetActive(true); 
                carrotm.SetActive(true);
               
                break;
                case "tears":
                    tears.SetActive(true);   
                tearsm.SetActive(true);
                
                break;
                case "boxstomp":
                    boxstomp.SetActive(true);   
                boxstompm.SetActive(true);
                
                break;
                case "cavescare":
                    cavescare.SetActive(true);   
                cavescarem.SetActive(true);
                
                break;
                case "wellpush":
                    wellpush.SetActive(true);     
                wellpushm.SetActive(true);
                
                break;
                case "laughgrave":
                    laughgrave.SetActive(true);  
                laughgravem.SetActive(true);
                
                break;
                case "breakstatue":
                    breakstatue.SetActive(true);   
                breakstatuem.SetActive(true);
                
                break;
                case "carvetree":
                    carvetree.SetActive(true); 
                carvetreem.SetActive(true);
                
                break;
                case "destroytent":
                    destroytent.SetActive(true);  
                destroytentm.SetActive(true);
                
                break;
                case "bookfire":
                    bookfire.SetActive(true);
                    bookfirem.SetActive(true);
                
                break;
                default:
                initialMap.SetActive(true);
                break;
            }
        
        


    }
    
   
       


}
