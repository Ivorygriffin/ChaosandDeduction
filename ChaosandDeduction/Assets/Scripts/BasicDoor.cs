using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicDoor : MonoBehaviour
{
    public Quaternion closedAngle = new Quaternion();
    public Quaternion openAngle = new Quaternion();
    public float maxTime = 1;
    float timer = 0;
    bool state = true;

    private void Start()
    {
        if(closedAngle == null)
            closedAngle = Quaternion.identity;
        if(openAngle == null)
            openAngle = Quaternion.identity;

        transform.rotation = state ? openAngle : closedAngle;
    }
    // Update is called once per frame
    void Update()
    {
        if (state)
        {
            if (timer > 0)
                timer -= Time.deltaTime;
        }
        else
        {
            if (timer < maxTime)
                timer += Time.deltaTime;
        }
        transform.rotation = Quaternion.Lerp(openAngle, closedAngle, timer);
    }
    public void SetState(bool open)
    {
        state = open;
        timer = state ? maxTime : 0;
    }
}
