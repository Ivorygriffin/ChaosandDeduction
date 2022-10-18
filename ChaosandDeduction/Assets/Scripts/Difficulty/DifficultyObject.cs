using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Difficulty")]
public class DifficultyObject : ScriptableObject
{
    public float numDays = 1;
    public int villagerTasks = 8;
    public int traitorTasks = 4;

    void Clone(DifficultyObject difficulty)
    {
        numDays = difficulty.numDays;
        villagerTasks = difficulty.villagerTasks;
        traitorTasks = difficulty.traitorTasks;
    }
}
