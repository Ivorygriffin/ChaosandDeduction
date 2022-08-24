using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class barricade : MonoBehaviour
{
    public Material newMaterial;
    public Material oldMaterial;
    public bool barricade1;
    public bool barricade2;
    public List<GameObject> set1;
    public List<GameObject> set2;

    public void Barricade()
    {
        var set = GameObject.FindGameObjectsWithTag("barricade");
        var setCount = set.Length;
        foreach (var barricade in set)
        {
            if (barricade.GetComponent<Renderer>().material != newMaterial)
            {
                barricade.GetComponent<Renderer>().material = newMaterial;
                Debug.Log("newmat");
            }
            else
            {
                barricade.GetComponent<Renderer>().material = oldMaterial;
                Debug.Log("oldmat");
            }
            
        }
        //not working
    }
    public void SwitchB()
    {
        if (barricade1 == true)
        {
            foreach (GameObject go in set1)
            {
                go.SetActive(false);
            }
            foreach (GameObject obj in set2)
            {
                obj.SetActive(true);
            }
            barricade1 = false;

        }
        if (barricade2 == true)
        {
            foreach (GameObject go in set2)
            {
                go.SetActive(false);
            }
            foreach (GameObject go in set1)
            {
                go.SetActive(true);
            }
            barricade2 = false;
        }
    }
    public void B1True()
    {
        barricade1 = true;
    }
    public void B2True()
    {
        barricade2 = true;
    }

}
