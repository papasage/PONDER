using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameFightingState : IState
{
    GameStateMachine _gameState;
    public delegate void OnStateFighting();
    public static OnStateFighting onStateFighting;

    public GameFightingState(GameStateMachine gameState)
    {
        _gameState = gameState;
    }
    public void Enter()
    {
        Debug.Log("GAME STATE ENTER: FIGHTING");
        onStateFighting();

    }
    public void Tick()
    {

    }
    public void FixedTick()
    {

    }
    public void Exit()
    {
        Debug.Log("GAME STATE EXIT: FIGHTING");
    }
}
