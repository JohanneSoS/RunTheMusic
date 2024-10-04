using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventManager : MonoBehaviour
{
    public static UnityAction OnButtonClick;

    public static UnityAction OnPlayMenuMusic;
    
    public static UnityAction<BackGroundType> OnPlayLevelMusic;
    
    public static UnityAction<BackGroundType> OnDoorEnter;

    public static UnityAction<BackGroundType> OnDoorLeave;

    public static UnityAction<SwapDoors> OnEnterDoorHover;

    public static UnityAction OnExitDoorHover;

    public static UnityAction OnLockPlayerMovement;
    
    public static UnityAction OnUnlockPlayerMovement;

    public static UnityAction OnDash;

    public static UnityAction<Blackscreen.BlackScreenState> OnChangeBlackScreenState;


}
