using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Cosmetic")]
public class Cosmetic : ScriptableObject
{
    public GameObject prefab;
    public Vector3 positionOffset;
    public Quaternion rotationOffset;
    public Vector3 scaleOffset;
    public Slot slot;

    public enum Slot
    {
        head,
        chest
    }

    public GameObject Spawn(Transform transform)
    {
        GameObject temp = Instantiate(prefab, transform, false);
        temp.transform.localPosition = positionOffset;
        temp.transform.localRotation = rotationOffset;
        temp.transform.localScale = scaleOffset;

        return temp;
    }
}
