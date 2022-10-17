using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotionEffect : MonoBehaviour
{
    public Material material;
    public Gradient gradient1;
    public Gradient gradient2;

    public float time;
    // Start is called before the first frame update
    void Start()
    {
        material.EnableKeyword("_Color");
        material.EnableKeyword("_Color 2");
    }

    // Update is called once per frame
    void Update()
    {
        material.SetColor("_Color", gradient1.Evaluate(time));
        material.SetColor("_Color 2", gradient2.Evaluate(time));
    }
}
