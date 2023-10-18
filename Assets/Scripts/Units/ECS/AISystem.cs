using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AISystem : IUnitSystem
{
    private readonly StateMachine _stateMachine = new();
    public IState CurrentState => _stateMachine.CurrentState;

    private readonly Unit _unit;

    public AISystem(Unit unit)
    {
        _unit = unit;
    }

    public void Init()
    {
        InitStates();
        RandomAction();

        _unit.StartCoroutine(SecondTimer());
    }

    public void Waiting()
    {
        IState waitingState = _stateMachine.GetState<WaitingState>();
        _stateMachine.ChangeState(waitingState);
    }

    public void Walk(Vector3 targetPosition)
    {
        IState walkingState = _stateMachine.GetState<WalkingState>();
        _stateMachine.ChangeState(walkingState);

        WalkingState walk = (WalkingState)walkingState;
        walk.GoTo(targetPosition);
    }

    public void Walk()
    {
        IState walkingState = _stateMachine.GetState<WalkingState>();
        _stateMachine.ChangeState(walkingState);

        WalkingState walk = (WalkingState)walkingState;
        walk.GoToRandomPoint();
    }

    public void Pursuit(Unit target)
    {
        IState pursuitState = _stateMachine.GetState<PursuitState>();
        _stateMachine.ChangeState(pursuitState);

        PursuitState pursuit = (PursuitState)pursuitState;
        pursuit.Pursuit(target);
    }

    public void RandomAction()
    {
        switch(UnityEngine.Random.Range(0, 2))
        {
            case 0:
                Waiting();
                break;

            case 1:
                Walk();
                break;
        }
    }

    private void InitStates()
    {
        _stateMachine.StateMap = new Dictionary<Type, IState>
        {
            [typeof(WalkingState)] = new WalkingState(_unit),
            [typeof(WaitingState)] = new WaitingState(_unit),
            [typeof(PursuitState)] = new PursuitState(_unit)
        };
    }

    private IEnumerator SecondTimer()
    {
        WaitForSeconds waitForSeconds = new(1f);
        yield return waitForSeconds;

        while (_stateMachine.CurrentState != null)
        {
            _stateMachine.CurrentState.Update();
            yield return waitForSeconds;
        }
    }
}
