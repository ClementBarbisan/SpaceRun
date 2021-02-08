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
        thumbstick,
        touchpad,
        joystick,
        trackpad,
        leftStick,
        rightStick
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
        rightButton,
        leftTrigger,
        rightTrigger
    };

    private enum Gamepad
    {
        leftStick,
        rightStick,
        leftTrigger,
        rightTrigger
    }
    
    private enum Mouse
    {
        leftButton,
        rightButton,
    }
    private enum Keyboard
    {
        Space,
    }
    private enum XRController
    {
        triggerPressed,
        gripPressed,
        thumbstick,
        touchpad,
        joystick,
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
        gamepad
    }

    private string[] interactionType = new string[3] {"Press", "Press(behavior=1)", "Press(behavior=2)"};
    public List<InputActionMap> listActionMap;
    public static PlayerInputLite Instance;
    private Dictionary<TypeController, Type> controllerInputs;
    
    public InputAction CreateAction<T>(string currentName, T bindType,
        TypeHand hand = TypeHand.Any, TypeController control = TypeController.XRController, 
        InputActionType typeAction = InputActionType.Value, InteractionType interactions = InteractionType.PressOnly)
    {
        if (!Enum.IsDefined(controllerInputs[control], bindType.ToString()))
        {
            Debug.LogError("Incompatible binding " + control + "/" + bindType + "! Action not created!");
            string logFix = "Try one of these input type : ";
            foreach (string item in Enum.GetNames(controllerInputs[control]))
            {
                
                logFix += item + ", ";
            }

            string typeControl = control.ToString();
            foreach (string item in Enum.GetNames(typeof(TypeController)))
            {
                TypeController controller = (TypeController) Enum.Parse(typeof(TypeController), item);
                if (Enum.IsDefined(controllerInputs[controller], bindType.ToString()))
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
            interactionType[(int)interactions], null, null, typeof(T).Name);
        else
        {
           tmpAction = currentActionMap.AddAction(currentName, typeAction, "<" + control + ">/" + bindType,
               interactionType[(int)interactions], null, null, typeof(T).Name);

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
        controllerInputs = new Dictionary<TypeController, Type>();
        controllerInputs.Add(TypeController.XRController, typeof(XRController));
        controllerInputs.Add(TypeController.gamepad, typeof(Gamepad));
        controllerInputs.Add(TypeController.Keyboard, typeof(Keyboard));
        controllerInputs.Add(TypeController.Mouse, typeof(Mouse));
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
