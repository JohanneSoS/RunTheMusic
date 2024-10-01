using System;using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Blackscreen : MonoBehaviour
{
    [SerializeField] private GameObject TopBar;
    [SerializeField] private GameObject BottomBar;

    public BlackScreenState BlackScreen;

    [SerializeField] private float TopBarStartPos;
    [SerializeField] private float TopBarCinematicPos;
    [SerializeField] private float TopBarBlackscreenPos;
    
    [SerializeField] private float BottomBarStartPos;
    [SerializeField] private float BottomBarCinematicPos;
    [SerializeField] private float BottomBarBlackscreenPos;

    [SerializeField] private Vector3 TopBarPos;
    [SerializeField] private Vector3 BottomBarPos;

    [SerializeField] private Canvas canvas;

    void Start()
    {
        OnChangeBlackScreenState(BlackScreenState.NoScreen);
    }

    void OnEnable()
    {
        EventManager.OnChangeBlackScreenState += OnChangeBlackScreenState;
    }
    public void OnChangeBlackScreenState(BlackScreenState newState)
    {
        //ResetCanvas();
        BlackScreen = newState;
        
        switch (BlackScreen)
        {
            case BlackScreenState.NoScreen:
                TopBarPos.y = TopBarStartPos;
                BottomBarPos.y = BottomBarStartPos;
                break;
            
            case BlackScreenState.Cinematic:
                TopBarPos.y = TopBarCinematicPos;
                //TopBar.transform.position = TopBarPos;
                BottomBarPos.y = BottomBarCinematicPos;
                //BottomBar.transform.position = BottomBarPos;
                break;
            
            case BlackScreenState.BlackScreen:
                TopBarPos.y = TopBarBlackscreenPos;
                //TopBar.transform.position = TopBarPos;
                BottomBarPos.y = BottomBarBlackscreenPos;
                //BottomBar.transform.position = BottomBarPos;
                break;
        }
        
        TopBar.transform.DOLocalMoveY(TopBarPos.y,1);//.SetEase(Ease.InOutSine);
        BottomBar.transform.DOLocalMoveY (BottomBarPos.y, 1);//.SetEase(Ease.InOutSine);
    }

    private void ResetCanvas()
    {
        var pos = canvas.GetComponent<RectTransform>();
        pos.anchoredPosition = new Vector2(0, 0);
        pos.localPosition = new Vector2(0, 0);
    }

    public enum BlackScreenState
    {
        None,
        NoScreen,
        Cinematic,
        BlackScreen
    }
}