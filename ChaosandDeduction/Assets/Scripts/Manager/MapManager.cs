using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class MapManager : NetworkBehaviour
{
    public AreaScriptableObject[] playerLocations;
    public RectTransform[] playerIcons;
    public Vector2[] offsets;

    public AreaScriptableObject initialArea;
    public AreaScriptableObject nullArea;

    public static MapManager instance;

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
        //scriptable objects are not the same when sent over network (can not be compared)
        playerIcons[index].gameObject.SetActive(area.mapPos != nullArea.mapPos);

        playerLocations[index] = area;
        playerIcons[index].anchoredPosition = area.mapPos + offsets[index];
        playerIcons[index].rotation = Quaternion.Euler(0, 0, Random.Range(-7.0f, 7.0f));
    }
}
