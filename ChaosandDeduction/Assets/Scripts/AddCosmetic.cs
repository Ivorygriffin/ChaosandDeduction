using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEngine.UI;
using UnityEngine.Events;

using UnityEditor.Events;
#endif

[RequireComponent(typeof(Button))]
public class AddCosmetic : MonoBehaviour
{
    public Cosmetic cosmetic;
    public void Add()
    {
        CosmeticManager.Instance.EquipCosmetic(cosmetic);
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        Button temp = GetComponent<Button>();
        if (temp.onClick.GetPersistentEventCount() == 0)
        {
            UnityAction action = new UnityAction(this.Add);
            UnityEventTools.AddPersistentListener(temp.onClick, action);
        }
    }

#endif
}
