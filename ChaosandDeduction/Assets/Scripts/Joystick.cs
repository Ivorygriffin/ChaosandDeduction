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
    public float rimMultiplier = 1;
    public Canvas canvas;

    Finger legalFinger;

    public Vector2 inputVector
    {
        get { return knob.anchoredPosition / (backPlate.sizeDelta.x * rimMultiplier); }
    }
    // Start is called before the first frame update
    void Start()
    {
        EnhancedTouchSupport.Enable();
        UnityEngine.InputSystem.EnhancedTouch.Touch.onFingerDown += OnFingerDown;
        UnityEngine.InputSystem.EnhancedTouch.Touch.onFingerMove += OnMoveFinger;
        UnityEngine.InputSystem.EnhancedTouch.Touch.onFingerUp += OnFingerUp;

        backPlate = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnFingerDown(Finger finger)
    {
        if (legalFinger != null) //if joystick already engaged, ignore extra fingers
            return;


        Vector2 fingerPos = finger.screenPosition;

        if (canvas != null) //annoying screen scale fixing, god this took forever to find
            fingerPos /= canvas.scaleFactor;

        if ((fingerPos - backPlate.anchoredPosition - (backPlate.sizeDelta / 2)).magnitude > backPlate.sizeDelta.x * rimMultiplier * 2)
            return;

        legalFinger = finger;
        OnMoveFinger(finger); //run it once at the start incase no movement on finger
    }


    public void OnMoveFinger(Finger finger)
    {
        if (!finger.Equals(legalFinger)) //only move joystick using the finger that triggerd it
            return;

        Vector2 fingerPos = finger.screenPosition;

        if (canvas != null) //annoying screen scale fixing, god this took forever to find
            fingerPos /= canvas.scaleFactor;

        knob.anchoredPosition = fingerPos - backPlate.anchoredPosition - (backPlate.sizeDelta / 2);

        if (knob.anchoredPosition.magnitude > backPlate.sizeDelta.x * rimMultiplier) //cap the joystick to the rim
            knob.anchoredPosition = knob.anchoredPosition.normalized * backPlate.sizeDelta.x * rimMultiplier;
    }

    public void OnFingerUp(Finger finger)
    {
        if (!finger.Equals(legalFinger)) //only reset if its the correct finger going up
            return;

        knob.anchoredPosition = new Vector2(0, 0);

        legalFinger = null;
    }
}
