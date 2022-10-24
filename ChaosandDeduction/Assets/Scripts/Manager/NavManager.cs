using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavManager : MonoBehaviour
{
    public Vector3 target
    {
        get { return targetPos; }
        set
        {
            NavMeshHit hit = new NavMeshHit();
            NavMesh.SamplePosition(value, out hit, Mathf.Infinity, NavMesh.AllAreas);
            targetPos = hit.position;

            if (!NavMesh.CalculatePath(PlayerManager.Instance.localPlayer.transform.position, targetPos, NavMesh.AllAreas, currentPath))
                Debug.LogWarning("Path could not be made!");
        }
    }
    Vector3 targetPos;
    NavMeshPath currentPath;
    public LineRenderer lineRenderer;

    static public NavManager instance;
    // Start is called before the first frame update
    void Start()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        currentPath = new NavMeshPath();
    }

    private void OnDestroy()
    {
        if (instance == this)
            instance = null;
    }

    // Update is called once per frame
    void Update()
    {
        if (currentPath.status == NavMeshPathStatus.PathComplete && currentPath.corners.Length > 0)
        {
            lineRenderer.positionCount = currentPath.corners.Length;

            for (int i = 0; i < currentPath.corners.Length; i++)
                lineRenderer.SetPosition(i, currentPath.corners[i] + Vector3.up * 0.2f);

            currentPath.ClearCorners();
        }
    }
}
