using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapManager : MonoBehaviour
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
            SetPlayerLocation(initialArea, i);

        }
    }
    private void OnDestroy()
    {
        if (instance == this)
            instance = null;
    }

    public void SetPlayerLocation(AreaScriptableObject area, int index)
    {
        playerIcons[index].gameObject.SetActive(area != nullArea);

        playerLocations[index] = area;
        playerIcons[index].anchoredPosition = area.mapPos + offsets[index];
        playerIcons[index].rotation = Quaternion.Euler(0, 0, Random.Range(-7.0f, 7.0f));
    }
}
