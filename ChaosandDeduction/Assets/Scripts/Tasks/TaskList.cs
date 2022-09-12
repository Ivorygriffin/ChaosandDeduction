using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class TaskList : NetworkBehaviour
{
    [System.Serializable]
    public struct Page
    {
        public GameObject page;
        public GameObject button;
        public bool active;
        public GameObject ribbon;
        public bool complete;
        public TaskScriptableObject task;
        //public bool initialised;
    }
    public Page[] pages;

    bool initialised = false;
    private void Start()
    {
        for (int i = 0; i < pages.Length; i++)
        {
            int x = i;
            pages[i].button.GetComponent<Button>().onClick.AddListener(() => Select(x));
        }
    }

    private void OnEnable()
    {
        if (initialised)
        {
            // SetActive();
        }
    }

    private void Update()
    {
        if (!initialised)//TODO: determine when this can be disabled after retreving information
        {
            if (isServer)
            {
                int count = 0;
                for (int i = 0; i < pages.Length; i++)
                {
                    if (TaskManager.instance)
                    {
                        if (!pages[i].task.isComplete)
                            count++;
                    }
                }
                if (count >= pages.Length)
                {
                    initialised = true;
                    //gameObject.SetActive(false);
                }
            }
            else
                CmdGetActive(); //Might be able to just use this instead?
        }
    }
    void SetActive()
    {
        for (int i = 0; i < pages.Length; i++)
        {
            pages[i].button.SetActive(pages[i].active);
            pages[i].ribbon.SetActive(pages[i].complete);
        }
    }

    void SetValues(int index, bool buttonActive, bool ribbonActive)
    {
        //Button temp = pages[index].button.GetComponent<Button>();
        pages[index].active = buttonActive;
        pages[index].complete = ribbonActive;
    }
    [ClientRpc]
    void RpcSetValues(int[] index, bool[] buttonActive, bool[] ribbonActive, bool finished)
    {
        // if (!initialised)
        //     gameObject.SetActive(false);
        for (int i = 0; i < index.Length && i < buttonActive.Length && i < ribbonActive.Length; i++)
            SetValues(index[i], buttonActive[i], ribbonActive[i]);

        initialised = finished;
        SetActive();

    }

    //[TargetRpc] //Most likely unable to use target as this is unowned
    //void TargetSetActive(int[] index, bool[] buttonActive, bool[] ribbonActive, bool finished)
    //{
    //    initialised = finished;
    //    for (int i = 0; i < index.Length && i < buttonActive.Length && i < ribbonActive.Length; i++)
    //        SetActive(index[i], buttonActive[i], ribbonActive[i]);
    //}


    [Command(requiresAuthority = false)]
    public void CmdGetActive()
    {
        int[] index = new int[pages.Length];
        bool[] buttonActive = new bool[pages.Length];
        bool[] ribbonActive = new bool[pages.Length];
        for (int i = 0; i < pages.Length; i++)
        {
            if (TaskManager.instance)
            {
                bool active = TaskManager.instance.RequiredTask(pages[i].task);
                index[i] = i;
                buttonActive[i] = active;
                ribbonActive[i] = pages[i].task.isComplete;
            }
        }

        RpcSetValues(index, buttonActive, ribbonActive, initialised);
    }


    public void Select(int index)
    {
        for (int i = 0; i < pages.Length; i++)
            pages[i].page.SetActive(i == index);

        // CmdGetActive(); //check current active, just incase?
    }
}
