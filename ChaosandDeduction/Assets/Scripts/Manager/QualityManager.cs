using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class QualityManager : MonoBehaviour
{
    public UnityEvent<int> onQualityChange;
    public static QualityManager instance;
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        QualitySettings.SetQualityLevel(PlayerPrefs.GetInt("Quality", 1), true);
    }
    private void OnDestroy()
    {
        if (instance == this)
            instance = null;
    }

    public void SetQuality(int index)
    {
        QualitySettings.SetQualityLevel(index, true);
        PlayerPrefs.SetInt("Quality", index);

        onQualityChange.Invoke(index);
    }
}
