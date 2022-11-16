using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavManager : MonoBehaviour
{
    public TaskScriptableObject currentTask;
    public int currentTaskIndex;
    public NavMeshPath currentPath;

    bool taskNotMade = false;

    public GameObject particlePrefab;
    List<Particle> spawnedParticles = new List<Particle>();
    public class Particle
    {
        public Particle(GameObject particlePrefab, int index)
        {
            gameObject = Instantiate(particlePrefab, Vector3.down * 100, Quaternion.identity);
            transform = gameObject.transform;
            targetPos = Vector3.down * 100;

            offset = index / amplitude;
        }

        public void Update()
        {
            transform.position = targetPos + (Vector3.up * instance.curve.Evaluate((Time.time - offset) * speed)) + (Vector3.down * 0.5f);
        }

        public bool active
        {
            set
            {
                gameObject.SetActive(value);
                m_active = value;
            }
            get { return m_active; }
        }
        bool m_active = false;

        GameObject gameObject;
        Transform transform;
        public Vector3 targetPos;

        float offset;
        const float speed = 1f;
        const float amplitude = 10;
    }
    public float particleGap = 5;

    public AnimationCurve curve;


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
        //TODO: check if needs to recalculate better
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

        for (int i = 0; i < spawnedParticles.Count; i++)
        {
            if (!spawnedParticles[i].active)
                break; //dont update inactive particles
            spawnedParticles[i].Update();
        }
    }

    [ContextMenu("Recalculate")]
    void CalculatePath(int index)
    {
        taskNotMade = true; //used to repeat calculation until path found



        Vector3 fromPoint = currentTask.paths[index].startPosition;
        if (fromPoint == Vector3.zero)
            fromPoint = PlayerManager.Instance.localPlayer.transform.position;

        Vector3 toPoint = currentTask.paths[index].endPosition;
        //TODO: check any edge cases?

        NavMeshHit hit1;//= new NavMeshHit();
        NavMesh.SamplePosition(fromPoint, out hit1, Mathf.Infinity, NavMesh.AllAreas);

        NavMeshHit hit2;//= new NavMeshHit();
        NavMesh.SamplePosition(toPoint, out hit2, Mathf.Infinity, NavMesh.AllAreas);

        NavMesh.CalculatePath(hit1.position, hit2.position, NavMesh.AllAreas, currentPath);

        if (currentPath.status == NavMeshPathStatus.PathComplete || currentPath.status == NavMeshPathStatus.PathPartial)
        {
            taskNotMade = false;
            spawnPoints();
        }
#if UNITY_EDITOR
        else
            Debug.LogWarning("Path could not be made!");
#endif
    }

    void spawnPoints()
    {
        DestroyPoints();

        int particleCounter = 0;

        for (int i = 1; i < currentPath.corners.Length; i++)
        {
            Vector3 direction = (currentPath.corners[i] - currentPath.corners[i - 1]).normalized;
            float distanceMade = 0;
            while (distanceMade < Vector3.Distance(currentPath.corners[i], currentPath.corners[i - 1]))
            {
                if (spawnedParticles.Count <= particleCounter) //allow essentially lazy initialisation for particles
                    AddPoint();
                spawnedParticles[particleCounter].targetPos = currentPath.corners[i - 1] + (direction * distanceMade);
                spawnedParticles[particleCounter].active = true;

                particleCounter++;

                distanceMade += particleGap;
            }
        }

        currentPath.ClearCorners();
    }
    void AddPoint()
    {
        spawnedParticles.Add(new Particle(particlePrefab, spawnedParticles.Count));
        //spawnedParticles[spawnedParticles.Count - 1].SetActive(false); //shouldnt be needed?
    }

    void DestroyPoints()
    {
        for (int i = 0; i < spawnedParticles.Count; i++)
        {
            spawnedParticles[i].targetPos = Vector3.down * 100;
            spawnedParticles[i].active = false;
        }
        //spawnedParticles.Clear();
    }
}
