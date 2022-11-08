using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public Animator animator;
    public AudioPlayer footsteps;
    const float requiredFootstepSpeed = 0.2f;

    Vector3 lastFramePos = Vector3.zero;

    public Bone[] cosmeticBones;
    [System.Serializable]
    public struct Bone
    {
        public string name;
        public Transform transform;

        public Vector3 positionOffset;
        public Quaternion rotationOffset;
        public Vector3 scaleOffset;
    }

    private void Start()
    {
        if (CosmeticManager.Instance)
            EquipAll();
    }

    // Update is called once per frame
    public void RemoteUpdate()
    {
        float speed = Vector3.Distance(lastFramePos, transform.position) / (Time.deltaTime);
        animator.SetFloat("Blend", speed);

        if (speed > requiredFootstepSpeed && footsteps)
            footsteps.TryPlay();

        lastFramePos = transform.position;
    }
    public void LocalUpdate(Vector3 velocity)
    {
        if (animator)
            animator.SetFloat("Blend", velocity.magnitude);

        if (velocity.magnitude > requiredFootstepSpeed && footsteps)
            footsteps.TryPlay();
    }

    public void EquipAll()
    {

        int length1 = CosmeticManager.Instance.slots.Length;
        for (int i = 0; i < length1; i++)
        {
            int length2 = CosmeticManager.Instance.slots[i].Count;
            for (int j = 0; j < length2; j++)
            {
                EquipCosmetic(CosmeticManager.Instance.slots[i][j]);
            }
        }
    }
    public GameObject EquipCosmetic(Cosmetic cosmetic)
    {
        Bone bone = cosmeticBones[(int)cosmetic.slot];
        GameObject temp = cosmetic.Spawn(cosmeticBones[(int)cosmetic.slot].transform);

        temp.transform.localPosition += bone.positionOffset;
        temp.transform.localRotation *= bone.rotationOffset;
        temp.transform.localScale += bone.scaleOffset;

        return temp;
    }
#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        for (int i = 0; i < cosmeticBones.Length; i++)
        {
            if (cosmeticBones[i].transform != null)
            {
                Vector3 point = cosmeticBones[i].transform.position + cosmeticBones[i].positionOffset;
                Gizmos.DrawCube(point, Vector3.one * 0.1f);
            }
        }
    }

    private void OnValidate()
    {
        int numBones = System.Enum.GetValues(typeof(Cosmetic.Slot)).Length;
        if (cosmeticBones.Length != numBones)
            cosmeticBones = new Bone[numBones];
        else return;

        for (int i = 0; i < numBones; i++)
        {
            cosmeticBones[i].name = ((Cosmetic.Slot)i).ToString();
        }
    }
#endif
}
