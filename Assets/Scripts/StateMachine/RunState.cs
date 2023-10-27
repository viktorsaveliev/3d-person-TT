using UnityEngine;
using UnityEngine.AI;

public class RunState : UnitState
{
    private readonly NavMeshAgent _navMeshAgent;

    public RunState(Unit unit) : base(unit)
    {
        _navMeshAgent = unit.GetComponent<NavMeshAgent>();
    }

    public override void Enter()
    {
        _navMeshAgent.enabled = true;
        _navMeshAgent.speed = Unit.Data.SprintSpeed;
    }

    public override void Exit()
    {
        if (_navMeshAgent.isOnNavMesh && _navMeshAgent.isActiveAndEnabled)
        {
            _navMeshAgent.isStopped = true;
        }

        Unit.Animator.SetBool(Unit.AnimCache.SprintIndex, false);
    }

    public override void Update()
    {
        if (!_navMeshAgent.isOnNavMesh || !_navMeshAgent.isActiveAndEnabled) return;

        if (_navMeshAgent.remainingDistance < 1f)
        {
            Unit.GetSystem<AISystem>()?.RandomAction();
        }
    }

    public void Run(Vector3 position)
    {
        if (_navMeshAgent.isOnNavMesh && _navMeshAgent.isActiveAndEnabled)
        {
            _navMeshAgent.isStopped = false;
            _navMeshAgent.SetDestination(position);

            Unit.Animator.SetBool(Unit.AnimCache.SprintIndex, true);
        }
    }
}
