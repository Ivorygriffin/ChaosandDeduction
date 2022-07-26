using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class PPQuality : MonoBehaviour
{
    public Volume pp;

    [System.Serializable]
    public struct QualityLevel
    {
        public bool[] enabled;
    }
    public QualityLevel[] levels;

    // Start is called before the first frame update
    void Start()
    {
        OnQualityChange(PlayerPrefs.GetInt("Quality", 1));
    }

    public void OnQualityChange(int qualityLevel)
    {
        for (int i = 0; i < levels[qualityLevel].enabled.Length; i++)
            pp.profile.components[i].active = levels[qualityLevel].enabled[i];
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        pp = GetComponent<Volume>();
        for (int i = 0; i < levels.Length; i++)
        {
            if (levels[i].enabled.Length != pp.profile.components.Count)
                levels[i].enabled = new bool[pp.profile.components.Count];
        }
    }
#endif
}
