using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class CosmeticManager : NetworkBehaviour
{
    public static CosmeticManager Instance;

    public List<Cosmetic>[] slots = new List<Cosmetic>[System.Enum.GetValues(typeof(Cosmetic.Slot)).Length];
    //public Cosmetic tempDefault;
    // Start is called before the first frame update
    void Start()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);

        for (int i = 0; i < slots.Length; i++)
            slots[i] = new List<Cosmetic>();


        //EquipCosmetic(tempDefault); //Temp example cosmetic
    }
    public void EquipCosmetic(Cosmetic cosmetic)
    {
        slots[(int)cosmetic.slot].Add(cosmetic);
    }
    public void RemoveCosmetic(Cosmetic cosmetic)
    {
        int index = slots[(int)cosmetic.slot].IndexOf(cosmetic);

        if (index == -1)
            return;

        slots[(int)cosmetic.slot].RemoveAt(index);
    }
}
