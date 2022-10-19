using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem.EnhancedTouch;

#if UNITY_EDITOR
using UnityEngine.InputSystem;
#endif
public class FireButton : MonoBehaviour
{
    public static FireButton instance; //make this an instance, since its the only one, and needs to be referenced by the local player dynamically

    public RectTransform buttonTransform;
    public Canvas canvas;

    Finger legalFinger;

    public bool pressed { get; private set; }
    public bool pressedFrame { get; private set; }
    bool frameBuffer = false;

#if UNITY_EDITOR
    bool usingDebugControls = true;
#endif

    //public EventTrigger button;
    // Start is called before the first frame update
    void Start()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);

        EnhancedTouchSupport.Enable();
        UnityEngine.InputSystem.EnhancedTouch.Touch.onFingerDown += OnFingerDown;
        UnityEngine.InputSystem.EnhancedTouch.Touch.onFingerUp += OnFingerUp;
    }
    protected void OnDestroy()
    {
        UnityEngine.InputSystem.EnhancedTouch.Touch.onFingerDown -= OnFingerDown;
        UnityEngine.InputSystem.EnhancedTouch.Touch.onFingerUp -= OnFingerUp;

        if (instance == this)
            instance = null;
    }
    void Update()
    {
        if (!frameBuffer) //give pressedFrame an extra frame before being disabled?
            pressedFrame = false;
        else
            frameBuffer = false;

#if UNITY_EDITOR
        if (usingDebugControls)
        {
            if (Keyboard.current[Key.F].IsPressed() && !pressed)
            {
                pressedFrame = true;
                pressed = true;
            }
            else if (!Keyboard.current[Key.F].IsPressed())
            {
                pressedFrame = false;
                pressed = false;
            }
        }
#endif
    }

    public void OnFingerDown(Finger finger)
    {
        if (legalFinger != null || !canvas.enabled) //if joystick already engaged, ignore extra fingers
            return;


        Vector2 fingerPos = finger.screenPosition;

        fingerPos -= (Vector2)buttonTransform.position;
        fingerPos /= canvas.scaleFactor;


        //check if within button's bounds
        if (fingerPos.x > buttonTransform.sizeDelta.x / 2 || fingerPos.x < -buttonTransform.sizeDelta.x / 2 || fingerPos.y > buttonTransform.sizeDelta.y / 2 || fingerPos.x < -buttonTransform.sizeDelta.y / 2)
            return;

        legalFinger = finger;

        pressedFrame = true;
        frameBuffer = true;
        pressed = true;
#if UNITY_EDITOR
        usingDebugControls = false;
#endif
    }

    public void OnFingerUp(Finger finger)
    {
        if (!finger.Equals(legalFinger)) //only reset if its the correct finger going up
            return;

        pressedFrame = false;
        pressed = false;

        legalFinger = null;
#if UNITY_EDITOR
        usingDebugControls = true;
#endif
    }
}
