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
            targetPos = value;
            NavMesh.CalculatePath(PlayerManager.Instance.localPlayer.transform.position, value, NavMesh.AllAreas, currentPath);
            lineRenderer.positionCount = currentPath.corners.Length;
            lineRenderer.SetPositions(currentPath.corners);
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
    }
}
