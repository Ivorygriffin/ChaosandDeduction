using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DifficultyButton : MonoBehaviour
{
    public DifficultyObject buttonDifficulty;
    // Update is called once per frame
    public void OnPress()
    {
        DifficultyManager.instance.SetDifficulty(buttonDifficulty);
    }
}
