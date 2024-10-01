using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventManager : MonoBehaviour
{
    public static UnityAction OnDoorEnter;

    public static UnityAction OnDoorLeave;

    public static UnityAction<SwapDoors> OnEnterDoorHover;

    public static UnityAction OnExitDoorHover;

    public static UnityAction OnLockPlayerMovement;
    
    public static UnityAction OnUnlockPlayerMovement;

    public static UnityAction OnDash;

    public static UnityAction<Blackscreen.BlackScreenState> OnChangeBlackScreenState;


}
