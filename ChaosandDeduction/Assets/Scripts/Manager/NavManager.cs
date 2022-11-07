using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavManager : MonoBehaviour
{
    Vector3 targetPos;
    TaskScriptableObject currentTask;
    int currentTaskIndex;
    NavMeshPath currentPath;
    public LineRenderer lineRenderer;

    bool taskNotMade = false;

    public GameObject particlePrefab;
    List<GameObject> spawnedParticles = new List<GameObject>();
    public float particleGap = 5;


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
        if ((pinManager.instance.pinnedTask && currentTask != pinManager.instance.pinnedTask) || (currentTask != null && currentTaskIndex != currentTask.currentIndex))
        {
            if (pinManager.instance.pinnedTask)
            {
                currentTask = pinManager.instance.pinnedTask;
                currentTaskIndex = currentTask.currentIndex;
            }

            if (!pinManager.instance.pinnedTask || pinManager.instance.pinnedTask.currentIndex == -1)
            {
                if (pinManager.instance.pinnedTask)
                    pinManager.instance.Pin(-1);
                lineRenderer.positionCount = 0;

                DestroyPoints();
            }
            else
            {
                CalculatePath(currentTaskIndex);
            }
        }

        if (taskNotMade)
            CalculatePath(currentTaskIndex);

        if (currentPath.status == NavMeshPathStatus.PathComplete && currentPath.corners.Length > 0)
        {
            spawnPoints();
            //lineRenderer.positionCount = currentPath.corners.Length;

            //for (int i = 0; i < currentPath.corners.Length; i++)
            //    lineRenderer.SetPosition(i, currentPath.corners[i] + Vector3.up * 0.2f);

            currentPath.ClearCorners();
        }
    }

    void CalculatePath(int index)
    {
        taskNotMade = true; //used to repeat calculation until path found



        Vector3 fromPoint = PlayerManager.Instance.localPlayer.transform.position;
        if (index > 0)
            fromPoint = currentTask.points[index - 1];
        NavMeshHit hit1;//= new NavMeshHit();
        NavMesh.SamplePosition(fromPoint, out hit1, Mathf.Infinity, NavMesh.AllAreas);

        NavMeshHit hit2;//= new NavMeshHit();
        NavMesh.SamplePosition(currentTask.points[index], out hit2, Mathf.Infinity, NavMesh.AllAreas);

        if (!NavMesh.CalculatePath(hit1.position, hit2.position, NavMesh.AllAreas, currentPath))
            Debug.LogWarning("Path could not be made!");
        else
            taskNotMade = false;
    }

    void spawnPoints()
    {
        DestroyPoints();

        for (int i = 1; i < currentPath.corners.Length; i++)
        {
            Vector3 direction = (currentPath.corners[i] - currentPath.corners[i - 1]).normalized;
            float distanceMade = 0;
            while (distanceMade < Vector3.Distance(currentPath.corners[i], currentPath.corners[i - 1]))
            {
                spawnedParticles.Add(Instantiate(particlePrefab, currentPath.corners[i - 1] + (direction * distanceMade), Quaternion.identity));
                distanceMade += particleGap;
            }
        }
    }

    void DestroyPoints()
    {
        foreach (GameObject particle in spawnedParticles)
            Destroy(particle);
        spawnedParticles.Clear();
    }
}
