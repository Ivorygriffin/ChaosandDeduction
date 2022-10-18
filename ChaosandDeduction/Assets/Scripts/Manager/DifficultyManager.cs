using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DifficultyManager : MonoBehaviour
{
    public static DifficultyManager instance;
    public DifficultyObject currentDifficulty;

    // Start is called before the first frame update
    void Start()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(this);
    }

    private void OnDestroy()
    {
        if (instance == this)
            instance = null;
    }

    // Update is called once per frame
    public void SetDifficulty(DifficultyObject difficulty)
    {
        currentDifficulty = difficulty;
    }
    public float GetTime()
    {
        return currentDifficulty.numDays;
    }
    public int GetVTasks()
    {
        return currentDifficulty.villagerTasks;
    }
    public int GetTTasks()
    {
        return currentDifficulty.traitorTasks;
    }
}
