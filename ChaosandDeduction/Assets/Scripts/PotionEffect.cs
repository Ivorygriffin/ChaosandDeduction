using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotionEffect : MonoBehaviour
{
    public Material materialSpiral;
    public Gradient gradient1;
    public Gradient gradient2;

    public Material materialBubble;
    public Gradient gradient3;

    public float time;

    // Update is called once per frame
    void Update()
    {
        materialSpiral.SetColor("_Color", gradient1.Evaluate(time));
        materialSpiral.SetColor("_Color2", gradient2.Evaluate(time));

        materialBubble.SetColor("_Color", gradient3.Evaluate(time));

        time += Time.deltaTime * 0.25f;
        if (time > 1)
            time = 0;
    }
}
