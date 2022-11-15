using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class pinManager : MonoBehaviour
{

    public bool on;
    public string taskName;


    public GameObject[] stickers;
    public GameObject[] maps;

    public TaskScriptableObject[] tasks;
    public TaskScriptableObject pinnedTask;

    public Image[] everyPin;
    public GameObject[] everymap;
    public GameObject initialMap;
    public spriteSwap ss;

    public static pinManager instance;
    private void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
            Destroy(gameObject);
    }

    private void OnDestroy()
    {
        if (instance == this)
            instance = null;
    }

    public void Pin(int index)
    {
        initialMap.SetActive(false);
        foreach (GameObject map in maps)
            map.SetActive(false);
        foreach (GameObject sticker in stickers)
            sticker.SetActive(false);

        foreach (Image btn in everyPin)
            btn.sprite = ss.added;

        //Debug.Log("unpin run");

        if (index < 0)
        {
            initialMap.SetActive(true);

            pinnedTask = null;
        }
        else
        {
            stickers[index].SetActive(true);
            maps[index].SetActive(true);

            pinnedTask = tasks[index];
        }
    }
}
