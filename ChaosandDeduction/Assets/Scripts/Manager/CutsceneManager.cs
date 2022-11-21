using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Mirror;

public class CutsceneManager : MonoBehaviour
{
    public Animator[] characters;


    [Header("Stages")]
    bool firstStage = true;
    bool secondStage = false;
    bool thirdStage = false;
    bool fourthStage = false;
    bool fifthStage = false;


    [Header("Cages")]
    public GameObject[] cages;
    float cageTime = 0;
    public float cageSpeed = 3;

    [Header("UI")]
    public RectTransform villagerUI;
    public TMP_Text vTask;

    public RectTransform traitorUI;
    public TMP_Text tTask;

    [Header("Light")]
    public Light spotLight;
    public Vector3[] spotLightPositions;

    float spotLightTimer = 0;
    public float spotLightSpeed = 4;
    int spotLightIndex = 0;
    bool spotLightDirection = false;

    int spotLightPasses = 0;
    public int spotLightNumPasses = 3;

    public Vector3 sneakPoint;

    const float transitionTime = 0.15f;

    short voted = -1;
    short traitor = -1;
    short villagerTasksDone = -1;
    short villagerTasksTemp = 0;
    short villagerTasks = -1;
    short traitorTasksDone = -1;
    short traitorTasksTemp = 0;
    short traitorTasks = -1;
    // Start is called before the first frame update
    void Start()
    {
        //voted = PlayerPrefs.GetInt("VotedWitch", -1); //TODO: determine if we should use playerPrefs for this
        //PlayerPrefs.DeleteKey("VotedWitch");
        //traitor = PlayerPrefs.GetInt("TraitorWitch", -1);
        //PlayerPrefs.DeleteKey("TraitorWitch");

        //villagerTasksDone = PlayerPrefs.GetInt("VillagerTasksDone", -1);
        //PlayerPrefs.DeleteKey("VillagerTasksDone");
        //villagerTasks = PlayerPrefs.GetInt("VillagerTasks", -1);
        //PlayerPrefs.DeleteKey("VillagerTasks");

        //traitorTasksDone = PlayerPrefs.GetInt("TraitorTasksDone", -1);
        //PlayerPrefs.DeleteKey("TraitorTasksDone");
        //traitorTasks = PlayerPrefs.GetInt("TraitorTasks", -1);
        //PlayerPrefs.DeleteKey("TraitorTasks");

        CustomNetworkManager customNetworkManager = (CustomNetworkManager)NetworkManager.singleton;
        customNetworkManager.GetResults(out voted, out traitor, out villagerTasks, out villagerTasksDone, out traitorTasks, out traitorTasksDone);

        UpdateVillagerText();
        UpdateTraitorText();


        for (int i = 0; i < characters.Length; i++)
        {
            characters[i].CrossFade("Anxious", transitionTime);
        }
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
            cageTime = 0;
            if (voted == traitor)
                StartCoroutine(DisplayVillagerNumber());
            else
                StartCoroutine(DisplayTraitorNumber());
        }

        if (fourthStage && FourthStage())
        {
            fourthStage = false;
            cageTime = 0;
            fifthStage = true;
        }
        if (fifthStage && FifthStage())
        {
            fifthStage = false;

            //TODO: End game after delay?
        }
    }

    void LowerCage(int index, float time)
    {
        Vector3 cagePoint = new Vector3(cages[index].transform.position.x, Mathf.Lerp(4.119f, 0, time), cages[index].transform.position.z);
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

    bool FirstStage() //Move spotlight over witches, land on voted player
    {
        int nextIndex = spotLightIndex + (spotLightDirection ? -1 : 1);
        Vector3 lightPoint = Vector3.Slerp(spotLightPositions[spotLightIndex], spotLightPositions[nextIndex], spotLightTimer);
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
    bool SecondStage() //Lower cage on voted player
    {
        cageTime += Time.deltaTime * cageSpeed;
        LowerCage(voted, cageTime);

        if (cageTime > 1)
        {
            for (int i = 0; i < characters.Length; i++)
            {
                if (i == voted)
                    characters[i].CrossFade("TraitorFound", transitionTime);
                else
                    characters[i].CrossFade("Cheer", transitionTime);
            }
            return true;
        }
        return false;
    }
    bool ThirdStage() //if not traitor, lower cages on villagers
    {
        cageTime += Time.deltaTime * cageSpeed;
        if (voted == traitor)
        {
            villagerUI.anchoredPosition = new Vector2(0, Mathf.Lerp(-100, 100, cageTime));
            if (cageTime > 1)
                return true;
        }
        else
        {
            traitorUI.anchoredPosition = new Vector2(0, Mathf.Lerp(-100, 100, cageTime));
            for (int i = 0; i < characters.Length; i++)
            {
                if (i != voted && i != traitor)
                    LowerCage(i, cageTime);
            }

            if (cageTime > 1)
            {
                for (int i = 0; i < characters.Length; i++)
                {
                    if (i != traitor)
                        characters[i].CrossFade("TraitorFound", transitionTime);
                    else
                        characters[i].CrossFade("Cheer", transitionTime);
                }
                return true;
            }
        }

        return false;
    }
    bool FourthStage() //if villagers not done tasks, lower cages on them, if traitor not done tasks, lower cage on them
    {
        cageTime += Time.deltaTime * cageSpeed;
        if (voted == traitor)
        {
            if (villagerTasksDone < villagerTasks)
            {
                for (int i = 0; i < characters.Length; i++)
                {
                    if (i != voted)
                        LowerCage(i, cageTime);
                }
            }
            else
            {
                for (int i = 0; i < characters.Length; i++)
                {
                    if (i != voted)
                        characters[i].CrossFade("Cheer", transitionTime);
                }
                characters[voted].CrossFade("TraitorFound", transitionTime);
                return true;
            }
        }
        else
        {
            if (traitorTasksDone < traitorTasks)
            {
                LowerCage(traitor, cageTime);
            }
            else
            {
                for (int i = 0; i < characters.Length; i++)
                {
                    if (i != traitor)
                        characters[i].CrossFade("TraitorFound", transitionTime);
                }
                characters[traitor].CrossFade("TraitorSneak", transitionTime);
                return true;
            }
        }
        if (cageTime > 1)
        {
            for (int i = 0; i < characters.Length; i++)
                characters[i].CrossFade("TraitorFound", transitionTime);
            return true;
        }

        return false;
    }
    bool FifthStage() //traitor sneaks off if they win
    {
        if (voted != traitor && traitorTasksDone == traitorTasks)
        {
            Transform traitorTransform = characters[traitor].transform;
            traitorTransform.position = Vector3.MoveTowards(traitorTransform.position, sneakPoint, 3 * Time.deltaTime);
            traitorTransform.LookAt(sneakPoint);
        }
        else
            return true;
        return false;
    }

    IEnumerator DelayThirdStage()
    {
        yield return new WaitForSeconds(3);
        thirdStage = true;

        spotLight.color = Color.red;
        spotLight.transform.position = spotLightPositions[traitor];
    }
    IEnumerator DelayFourthStage()
    {
        yield return new WaitForSeconds(1);
        fourthStage = true;
    }

    IEnumerator DisplayVillagerNumber() //TODO: determine if this a terrible way to run a delayed incremeting of a variable
    {
        yield return new WaitForSeconds(0.4f);

        if (villagerTasksTemp != villagerTasksDone)
        {
            villagerTasksTemp++;
            UpdateVillagerText();
        }
        if (villagerTasksTemp < villagerTasksDone)
            StartCoroutine(DisplayVillagerNumber());
        else
            StartCoroutine(DelayFourthStage());
    }
    IEnumerator DisplayTraitorNumber() //TODO: determine if this a terrible way to run a delayed incremeting of a variable
    {
        yield return new WaitForSeconds(0.75f);

        if (traitorTasksTemp != traitorTasksDone)
        {
            traitorTasksTemp++;
            UpdateTraitorText();
        }
        if (traitorTasksTemp < traitorTasksDone)
            StartCoroutine(DisplayTraitorNumber());
        else
            StartCoroutine(DelayFourthStage());
    }



#if UNITY_EDITOR
    private void OnValidate()
    {
        spotLightPositions = new Vector3[4];
        for (int i = 0; i < spotLightPositions.Length; i++)
            spotLightPositions[i] = characters[i].transform.position + Vector3.up * 5;
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(sneakPoint, 0.5f);
    }
#endif
}
