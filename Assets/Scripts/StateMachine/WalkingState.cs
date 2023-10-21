using UnityEngine;
using UnityEngine.AI;

public class WalkingState : UnitState
{
    private readonly NavMeshAgent _navMeshAgent;

    private Vector3 _targetPosition;
    private bool _isReachedPoint;

    public WalkingState(Unit unit) : base(unit)
    {
        _navMeshAgent = unit.GetComponent<NavMeshAgent>();
    }

    public override void Enter()
    {
        _navMeshAgent.enabled = true;
        _navMeshAgent.speed = Unit.Data.RegularSpeed;
    }

    public override void Update()
    {
        if (_isReachedPoint) return;

        if (_navMeshAgent.remainingDistance < 0.2f)
        {
            Stop();
            GoToRandomPoint();
        }
    }

    public override void Exit()
    {
        Stop();

        _isReachedPoint = true;
        _navMeshAgent.isStopped = true;
        _navMeshAgent.enabled = false;
    }

    public void GoToRandomPoint()
    {
        float maxDistance = 50f;
        if (NavMesh.SamplePosition(Vector3.zero, out NavMeshHit hit, maxDistance, NavMesh.AllAreas))
        {
            Vector3 randomDirection = Random.insideUnitSphere * maxDistance;
            randomDirection += hit.position;
            NavMesh.SamplePosition(randomDirection, out hit, maxDistance, NavMesh.AllAreas);

            GoTo(hit.position);
        }
    }

    public void GoTo(Vector3 targetPosition)
    {
        _targetPosition = targetPosition;
        Move();
    }

    private void Move()
    {
        _isReachedPoint = false;

        if (_navMeshAgent.isOnNavMesh && _navMeshAgent.isActiveAndEnabled)
        {
            _navMeshAgent.isStopped = false;
            _navMeshAgent.SetDestination(_targetPosition);
        }

        StringBus stringBus = new();
        Unit.Animator.SetFloat(stringBus.ANIM_MOVE, Unit.Data.RegularSpeed);
    }

    private void Stop()
    {
        _isReachedPoint = true;
        _navMeshAgent.isStopped = true;

        StringBus stringBus = new();
        Unit.Animator.SetFloat(stringBus.ANIM_MOVE, 0);
    }
}
