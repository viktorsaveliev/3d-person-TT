using UnityEngine.AI;

public class PursuitState : UnitState
{
    private readonly NavMeshAgent _navMeshAgent;

    private Unit _target;
    private bool _isReachedPoint;

    public PursuitState(Unit unit) : base(unit)
    {
        _navMeshAgent = unit.GetComponent<NavMeshAgent>();
    }

    public override void Enter()
    {
        _navMeshAgent.enabled = true;
        _navMeshAgent.speed = Unit.Data.SprintSpeed;
    }

    public override void Update()
    {
        if (_isReachedPoint) return;

        if (_navMeshAgent.remainingDistance < 0.2f)
        {
            //Stop();
        }

        Pursuit(_target);
    }

    public override void Exit()
    {
        Stop();

        _isReachedPoint = true;
        _navMeshAgent.isStopped = true;
        _navMeshAgent.enabled = false;
    }

    public void Pursuit(Unit target)
    {
        _target = target;
        _isReachedPoint = false;
        _navMeshAgent.isStopped = false;

        _navMeshAgent.SetDestination(_target.transform.position);

        StringBus stringBus = new();
        Unit.Animator.SetBool(stringBus.ANIM_SPRINT, true);
    }

    private void Stop()
    {
        _isReachedPoint = true;
        _navMeshAgent.isStopped = true;

        StringBus stringBus = new();
        Unit.Animator.SetBool(stringBus.ANIM_SPRINT, false);
    }
}