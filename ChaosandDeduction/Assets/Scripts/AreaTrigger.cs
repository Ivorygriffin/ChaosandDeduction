using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaTrigger : MonoBehaviour
{
    public AreaScriptableObject area;

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")// && PlayerManager.Instance.localPlayer == other.gameObject)
            MapManager.instance.SetPlayerLocation(area, PlayerManager.Instance.localPlayerData.modelIndex);
    }

    private void OnDrawGizmos()
    {
        if (area == null)
            Gizmos.color = Color.red;

        Gizmos.DrawCube(transform.position - transform.right, Vector3.one);
    }
}
