using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CutsceneManager : MonoBehaviour
{
    public Animator[] characters;


    [Header("Stages")]
    bool firstStage = true;
    bool secondStage = false;
    bool thirdStage = false;


    [Header("Cages")]
    public GameObject[] cages;
    float cageTime = 0;
    public float cageSpeed = 3;

    [Header("Numbers")]
    public TMP_Text vTask;

    public TMP_Text tTask;

    [Header("Light")]
    public GameObject spotLight;
    public Vector3[] spotLightPositions;

    float spotLightTimer = 0;
    public float spotLightSpeed = 4;
    int spotLightIndex = 0;
    bool spotLightDirection = false;

    int spotLightPasses = 0;
    public int spotLightNumPasses = 3;



    int voted = -1;
    int traitor = -1;
    int villagerTasksDone = -1;
    int villagerTasksTemp = 0;
    int villagerTasks = -1;
    int traitorTasksDone = -1;
    int traitorTasksTemp = 0;
    int traitorTasks = -1;
    // Start is called before the first frame update
    void Start()
    {
        voted = PlayerPrefs.GetInt("VotedWitch"); //TODO: determine if we should use playerPrefs for this
        PlayerPrefs.DeleteKey("VotedWitch");
        traitor = PlayerPrefs.GetInt("TraitorWitch", 1);
        PlayerPrefs.DeleteKey("TraitorWitch");

        villagerTasksDone = PlayerPrefs.GetInt("VillagerTasksDone", 12);
        PlayerPrefs.DeleteKey("VillagerTasksDone");
        villagerTasks = PlayerPrefs.GetInt("VillagerTasks", 12);
        PlayerPrefs.DeleteKey("VillagerTasks");

        traitorTasksDone = PlayerPrefs.GetInt("TraitorTasksDone", 4);
        PlayerPrefs.DeleteKey("TraitorTasksDone");
        traitorTasks = PlayerPrefs.GetInt("TraitorTasks", 4);
        PlayerPrefs.DeleteKey("TraitorTasks");

        UpdateVillagerText();
        UpdateTraitorText();
    }

    // Update is called once per frame
    void Update()
    {
        if (firstStage && FirstStage())
        {
            firstStage = false;
            secondStage = true;
        }

        if (secondStage && SecondStage())
        {
            secondStage = false;
            cageTime = 0;
            StartCoroutine(DelayThirdStage());
        }

        if (thirdStage && ThirdStage())
        {
            thirdStage = false;
            if (voted == traitor)
                StartCoroutine(DisplayVillagerNumber());
            else
                StartCoroutine(DisplayTraitorNumber());
        }
    }

    void LowerCage(int index, float time)
    {
        Vector3 cagePoint = new Vector3(cages[index].transform.position.x, Mathf.Lerp(4.119f, 0.011f, time), cages[index].transform.position.z);
        cages[index].transform.position = cagePoint;
    }

    void UpdateVillagerText()
    {
        vTask.text = villagerTasksTemp.ToString() + " " + villagerTasks.ToString();
    }
    void UpdateTraitorText()
    {
        tTask.text = traitorTasksTemp.ToString() + " " + traitorTasks.ToString();
    }

    bool FirstStage()
    {
        int nextIndex = spotLightIndex + (spotLightDirection ? -1 : 1);
        Vector3 lightPoint = Vector3.Lerp(spotLightPositions[spotLightIndex], spotLightPositions[nextIndex], spotLightTimer);
        spotLight.transform.position = lightPoint;
        spotLightTimer += Time.deltaTime * spotLightSpeed;
        if (spotLightTimer > 1)
        {
            spotLightTimer = 0;
            spotLightIndex += (spotLightDirection ? -1 : 1);
            if (spotLightPasses > spotLightNumPasses && spotLightIndex == voted)
                return true;

            nextIndex = spotLightIndex + (spotLightDirection ? -1 : 1);
            if (nextIndex >= characters.Length || nextIndex < 0)
            {
                spotLightDirection = !spotLightDirection;
                spotLightPasses++;
            }
        }
        return false;
    }

    bool SecondStage()
    {
        LowerCage(voted, cageTime);

        cageTime += Time.deltaTime * cageSpeed;
        if (cageTime > 1)
        {
            for (int i = 0; i < characters.Length; i++)
            {
                if (i == voted)
                    characters[i].Play("TraitorFound");
                else
                    characters[i].Play("Cheer");
            }
            return true;
        }
        return false;
    }

    bool ThirdStage()
    {
        if (voted == traitor)
        {
            return true;
        }
        else
        {
            for (int i = 0; i < characters.Length; i++)
            {
                if (i != voted && i != traitor)
                    LowerCage(i, cageTime);
            }

            cageTime += Time.deltaTime * cageSpeed;
            if (cageTime > 1)
            {
                for (int i = 0; i < characters.Length; i++)
                {
                    if (i != traitor)
                        characters[i].Play("TraitorFound");
                    else
                        characters[i].Play("Cheer");
                }
                return true;
            }
        }

        return false;
    }

    IEnumerator DelayThirdStage()
    {
        yield return new WaitForSeconds(2);
        thirdStage = true;
    }

    IEnumerator DisplayVillagerNumber() //TODO: determine if this a terrible way to run a delayed incremeting of a variable
    {
        yield return new WaitForSeconds(0.2f);

        villagerTasksTemp++;
        UpdateVillagerText();
        if (villagerTasksTemp < villagerTasksDone)
            StartCoroutine(DisplayVillagerNumber());
    }
    IEnumerator DisplayTraitorNumber() //TODO: determine if this a terrible way to run a delayed incremeting of a variable
    {
        yield return new WaitForSeconds(0.2f);

        traitorTasksTemp++;
        UpdateTraitorText();
        if (traitorTasksTemp < traitorTasksDone)
            StartCoroutine(DisplayTraitorNumber());
    }



#if UNITY_EDITOR
    private void OnValidate()
    {
        spotLightPositions = new Vector3[4];
        for (int i = 0; i < spotLightPositions.Length; i++)
            spotLightPositions[i] = characters[i].transform.position + Vector3.up * 5;
    }
#endif
}
