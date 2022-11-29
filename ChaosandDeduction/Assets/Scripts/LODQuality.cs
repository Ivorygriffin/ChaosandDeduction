using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LODQuality : MonoBehaviour
{
    public MeshRenderer[] lods;
    
    void Start()
    {
        QualityManager.instance.onQualityChange.AddListener(UpdateQuality);
        UpdateQuality(PlayerPrefs.GetInt("Quality", 1));
    }
    private void Awake()
    {
        UpdateQuality(PlayerPrefs.GetInt("Quality", 1));
    }


    private void UpdateQuality(int quality)
    {
        //get 1 for low, get 0 for anything else
        quality = Mathf.Clamp((quality - 1) * -1, 0, 1);
        for (int i = 0; i < lods.Length; i++)
            lods[i].enabled = false;

        lods[quality].enabled = true;
        //Debug.Log((i < quality) ? 1 : 0);
    }
#if UNITY_EDITOR
    private void OnValidate()
    {
        lods = gameObject.GetComponentsInChildren<MeshRenderer>();
    }
#endif
}
