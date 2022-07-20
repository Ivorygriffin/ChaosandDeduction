using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using UnityEngine.UI;

public class Joystick : MonoBehaviour
{
    public RectTransform knob;
    RectTransform backPlate;
    public float magnitudeMultiplier = 1;

    public Vector2 inputVector
    {
        get { return knob.anchoredPosition / (backPlate.sizeDelta.x * magnitudeMultiplier); }
    }
    // Start is called before the first frame update
    void Start()
    {
        EnhancedTouchSupport.Enable();
        UnityEngine.InputSystem.EnhancedTouch.Touch.onFingerMove += OnMoveFinger;
        UnityEngine.InputSystem.EnhancedTouch.Touch.onFingerUp += OnFingerUp;

        backPlate = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnMoveFinger(Finger finger)
    {
        knob.anchoredPosition = finger.screenPosition - backPlate.anchoredPosition - (backPlate.sizeDelta / 2);

        if (knob.anchoredPosition.magnitude > backPlate.sizeDelta.x * magnitudeMultiplier)
            knob.anchoredPosition = knob.anchoredPosition.normalized * backPlate.sizeDelta.x * magnitudeMultiplier;
    }

    public void OnFingerUp(Finger finger)
    {
        knob.anchoredPosition = new Vector2(0, 0);
    }
}
