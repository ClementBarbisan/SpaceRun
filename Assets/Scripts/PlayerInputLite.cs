using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;
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
        Thumbstick,
        Touchpad,
        Joystick,
        Trackpad,
        LeftStick,
        RightStick
    }
    
    public enum Vector3
    {
        
    }
    
    public enum Button
    {
        TriggerPressed,
        GripPressed,
        Space,
        LeftButton,
        RightButton,
        LeftTrigger,
        RightTrigger
    }
    
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
        Mouse,
        Gamepad
    }
    
    private enum Gamepad
    {
        LeftStick,
        RightStick,
        LeftTrigger,
        RightTrigger
    }
    
    private enum Mouse
    {
        LeftButton,
        RightButton
    }
    
    private enum Keyboard
    {
        Space
    }
    
    private enum XRController
    {
        TriggerPressed,
        GripPressed,
        Thumbstick,
        Touchpad,
        Joystick
    }
    
    private readonly string[] _interactionType = {"Press", "Press(behavior=1)", "Press(behavior=2)"};
    public List<InputActionMap> listActionMap;
    public static PlayerInputLite Instance;
    private Dictionary<TypeController, Type> _controllerInputs;
    
    public InputAction CreateAction<T>(string currentName, T bindType,
        TypeHand hand = TypeHand.Any, TypeController control = TypeController.XRController, 
        InputActionType typeAction = InputActionType.Value, InteractionType interactions = InteractionType.PressOnly) where T : Enum
    {
        if (!Enum.IsDefined(_controllerInputs[control], bindType.ToString()))
        {
            Debug.LogError("Incompatible binding " + control + "/" + bindType + "! Action not created!");
            string logFix = "Try one of these input type : ";
            foreach (string item in Enum.GetNames(_controllerInputs[control]))
            {
                logFix += item + ", ";
            }

            string typeControl = "Unavailable";
            foreach (string item in Enum.GetNames(typeof(TypeController)))
            {
                TypeController controller = (TypeController) Enum.Parse(typeof(TypeController), item);
                if (Enum.IsDefined(_controllerInputs[controller], bindType.ToString()))
                {
                    typeControl = item.ToString();
                    break;
                }
            }
            logFix += "or change controller type to " + typeControl + " !";
            Debug.LogError(logFix);
            return (null);
        }

        currentActionMap.Disable();
        InputAction tmpAction;
        if (control == TypeController.XRController) 
            tmpAction = currentActionMap.AddAction(currentName, typeAction, "<" + control + ">{" + hand + "}/"  + bindType,
            _interactionType[(int)interactions], null, null, typeof(T).Name);
        else
        {
           tmpAction = currentActionMap.AddAction(currentName, typeAction, "<" + control + ">/" + bindType,
               _interactionType[(int)interactions], null, null, typeof(T).Name);

        }
        currentActionMap.Enable();
        return (tmpAction);
    }

    public void RemoveActionMap(string name)
    {
        foreach (InputActionMap map in listActionMap)
        {
            if (map.name == name)
            {
                map.Dispose();
                listActionMap.Remove(map);
                if (currentActionMap.name == name)
                    currentActionMap = listActionMap[0];
                currentActionMap.Enable();
                return;
            }
        }
        Debug.LogWarning("Action Map : " + name + " not found. List unchanged");
    }
    
    public InputActionMap SwitchActionMap(string name)
    {
        foreach (InputActionMap map in listActionMap)
        {
            if (map.name == name)
            {
                map.Disable();
                currentActionMap = map;
                currentActionMap.Enable();
                return (currentActionMap);
            }
        }
        Debug.LogWarning("Action Map : " + name + " not found.");
        return (currentActionMap);
    }

    public InputActionMap CreateActionMap(string nameMap, bool switchToNewMap = true)
    {
        InputActionMap map = new InputActionMap(nameMap);
        listActionMap.Add(map);
        if (switchToNewMap)
            currentActionMap = map;
        return (map);
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
        listActionMap = new List<InputActionMap>();
        if (currentActionMap == null)
            currentActionMap =  new InputActionMap("ActionMap");
        listActionMap.Add(currentActionMap);
        notificationBehavior = PlayerNotifications.InvokeUnityEvents;
        _controllerInputs = new Dictionary<TypeController, Type>();
        _controllerInputs.Add(TypeController.XRController, typeof(XRController));
        _controllerInputs.Add(TypeController.Gamepad, typeof(Gamepad));
        _controllerInputs.Add(TypeController.Keyboard, typeof(Keyboard));
        _controllerInputs.Add(TypeController.Mouse, typeof(Mouse));
    }

    public void DisableActionMap()
    {
        currentActionMap.Disable();
    }

    public void EnableActionMap()
    {
        currentActionMap.Enable();
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }
}
