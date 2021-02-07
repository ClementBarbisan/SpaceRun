using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputLite : PlayerInput
{
    public enum InteractionType
    {
        PressOnly = 0,
        PressAndRelease = 1,
        ReleaseOnly = 2
    }

    public enum Any
    {
        
    }

    public enum Analog
    {
        
    }

    public enum Axis
    {
        
    }

    public enum Bone
    {
        
    }

    public enum Digital
    {
        
    }

    public enum Dpad
    {
        
    }

    public enum Eyes
    {
        
    }

    public enum Integer
    {
        
    }

    public enum Quaternion
    {
        
    }

    public enum Stick
    {
        
    }

    public enum Touch
    {
        
    }

    public enum Vector2
    {
        thumbstick,
        touchpad,
        joystick,
        trackpad
    }

    public enum Vector3
    {
        
    }
    
    public enum Button
    {
        triggerPressed,
        gripPressed,
        Space,
        leftButton,
        rightButton
    };

    public enum TypeHand
    {
        Any,
        LeftHand,
        RightHand
    }
    
    public enum TypeController
    {
        XRController,
        Keyboard,
        Mouse
    }

    private string[] interactionType = new string[3] {"Press", "Press(behavior=1)", "Press(behavior=2)"};
    private InputActionMap actionMap;
    public static PlayerInputLite Instance;
    
    public InputAction CreateAction<T>(string currentName, T bindType,
        TypeHand hand = TypeHand.Any, TypeController control = TypeController.XRController, 
        InputActionType typeAction = InputActionType.Value, InteractionType interactions = InteractionType.PressOnly)
    {
       actionMap.Disable();
       InputAction tmpAction;
       if (hand == TypeHand.Any)
            tmpAction = actionMap.AddAction(currentName, typeAction, "<" + control + ">/" + bindType,
            interactionType[(int)interactions], null, null, typeof(T).ToString());
       else
       {
           tmpAction = actionMap.AddAction(currentName, typeAction, "<" + control + ">{" + hand + "}/" + bindType,
               interactionType[(int)interactions], null, null, typeof(T).ToString());

       }
       actionMap.Enable();
       return (tmpAction);
    }
    
    // Start is called before the first frame update
    void Awake()
    {
        if (Instance)
        {
            Debug.LogWarning("A PlayerInputLite already exists, this one will be disabled.");
            enabled = false;
            return;
        }
        Instance = this;
        if (currentActionMap != null)
            actionMap = currentActionMap;
        else
        {
            actionMap = new InputActionMap("ActionMap");
            currentActionMap = actionMap;
        }
        notificationBehavior = PlayerNotifications.InvokeUnityEvents;
    }

    public void DisableActionMap()
    {
        actionMap.Disable();
    }

    public void EnableActionMap()
    {
        actionMap.Enable();
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }
}
