using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class MapManager : NetworkBehaviour
{
    public Canvas mapCanvas;
    public Rect mapRect;

    public AreaScriptableObject[] playerLocations;
    public RectTransform[] playerIcons;
    public Vector2[] offsets;

    public AreaScriptableObject initialArea;
    public AreaScriptableObject nullArea;

    public static MapManager instance;


    bool initialised = false;
    GameObject localPlayer;
    int localIndex = -1;
    // Start is called before the first frame update
    void Start()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);


        for (int i = 0; i < playerIcons.Length; i++)
        {
            CmdSetPlayerLocation(initialArea, i);
        }
    }

    private void Update()
    {
        if (!initialised && PlayerManager.Instance)
        {
            localIndex = PlayerManager.Instance.localPlayerData.modelIndex;
            localPlayer = PlayerManager.Instance.localPlayer;

            if (localPlayer && localIndex != -1)
            {
                playerIcons[localIndex].gameObject.SetActive(true);
                initialised = true;
            }
        }

        if (localIndex != -1 && localPlayer != null)
        {
            Vector2 mapPos = new Vector2(
                localPlayer.transform.position.x * mapRect.width,
                localPlayer.transform.position.z * mapRect.height);
            mapPos += mapRect.position;
            playerIcons[localIndex].anchoredPosition = mapPos;
            playerIcons[localIndex].rotation = Quaternion.Euler(0, 0, Random.Range(-7.0f, 7.0f));
        }
    }

    private void OnDestroy()
    {
        if (instance == this)
            instance = null;
    }
    [Command(requiresAuthority = false)]
    public void CmdSetPlayerLocation(AreaScriptableObject area, int index)
    {
        RpcSetPlayerLocations(area, index);
    }
    [ClientRpc]
    void RpcSetPlayerLocations(AreaScriptableObject area, int index)
    {
        if (index == PlayerManager.Instance.localPlayerData.modelIndex)
            return; //Do not update local position (will be set live in update)

        //scriptable objects are not the same when sent over network (can not be compared)
        playerIcons[index].gameObject.SetActive(area.mapPos != nullArea.mapPos);

        playerLocations[index] = area;
        playerIcons[index].anchoredPosition = area.mapPos + offsets[index];
        playerIcons[index].rotation = Quaternion.Euler(0, 0, Random.Range(-7.0f, 7.0f));
    }
}
