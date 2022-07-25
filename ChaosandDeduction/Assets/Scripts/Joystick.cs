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

    public Vector2 inputVector = Vector2.zero;


#if UNITY_EDITOR
    bool usingDebugControls = true;
#endif
    //public Vector2 inputVector
    //{
    //    get { return knob.anchoredPosition / (backPlate.sizeDelta.x * rimMultiplier); }
    //}


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
#if UNITY_EDITOR

        Vector2 fingerPos = Vector2.zero;
        if (Keyboard.current[Key.W].isPressed)
        {
            fingerPos.y += 1;
        }
        if (Keyboard.current[Key.A].isPressed)
        {
            fingerPos.x -= 1;
        }
        if (Keyboard.current[Key.S].isPressed)
        {
            fingerPos.y -= 1;
        }
        if (Keyboard.current[Key.D].isPressed)
        {
            fingerPos.x += 1;
        }

        if (usingDebugControls)
            inputVector = fingerPos.normalized;
#endif
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
#if UNITY_EDITOR
        usingDebugControls = false;
#endif
        OnMoveFinger(finger); //run it once at the start incase no movement on finger
    }


    public void OnMoveFinger(Finger finger)
    {
        if (!finger.Equals(legalFinger)) //only move joystick using the finger that triggerd it
            return;

        Vector2 fingerPos = finger.screenPosition;

        if (canvas != null) //annoying screen scale fixing, god this took forever to find
            fingerPos /= canvas.scaleFactor;

        inputVector = fingerPos - backPlate.anchoredPosition - (backPlate.sizeDelta / 2);

        if (inputVector.magnitude > backPlate.sizeDelta.x * rimMultiplier) //cap the joystick to the rim
            inputVector = inputVector.normalized * backPlate.sizeDelta.x * rimMultiplier;

        knob.anchoredPosition = inputVector;
        inputVector /= (backPlate.sizeDelta.x * rimMultiplier); //adjust to be magnitude of 1
    }

    public void OnFingerUp(Finger finger)
    {
        if (!finger.Equals(legalFinger)) //only reset if its the correct finger going up
            return;

        inputVector = Vector2.zero;
        knob.anchoredPosition = Vector2.zero;

        legalFinger = null;
#if UNITY_EDITOR
        usingDebugControls = true;
#endif
    }
}
