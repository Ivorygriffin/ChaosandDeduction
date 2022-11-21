using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class QualityManager : MonoBehaviour
{
    public UnityEvent<int> onQualityChange;
    private void Awake()
    {
        QualitySettings.SetQualityLevel(PlayerPrefs.GetInt("Quality", 1), true);
    }
    public void SetQuality(int index)
    {
        QualitySettings.SetQualityLevel(index, true);
        PlayerPrefs.SetInt("Quality", index);

        onQualityChange.Invoke(index);
    }
}
