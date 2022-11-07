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
        for (int i = 0; i < levels[0].enabled.Length; i++)
            pp.profile.components[i].active = true;
    }

    // Update is called once per frame
    void Update()
    {

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
