using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BindInteract : MonoBehaviour
{
    GameObject player;
    public Button button;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerManager.Instance)
        {
            GameObject temp = PlayerManager.Instance.localPlayer;
            if (temp != null && (temp != player || player == null))
            {
                player = PlayerManager.Instance.localPlayer;
                button.onClick.RemoveAllListeners();
                button.onClick.AddListener(player.GetComponent<CharacterInteraction>().Interact);
            }
        }
    }
}
