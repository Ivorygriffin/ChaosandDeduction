using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QualityManager : MonoBehaviour
{
    private void Awake()
    {
        QualitySettings.SetQualityLevel(PlayerPrefs.GetInt("Quality", 1), true);
    }
    public void SetQuality(int index)
    {
        QualitySettings.SetQualityLevel(index, true);
        PlayerPrefs.SetInt("Quality", index);
    }
}
