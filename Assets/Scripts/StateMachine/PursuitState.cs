using UnityEngine.AI;
using UnityEngine;

public class PursuitState : UnitState
{
    private readonly NavMeshAgent _navMeshAgent;
    private readonly float _attackDistance = 1.7f;

    private Unit _target;
    private float _attackDelay;

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
        float distance = Vector3.Distance(_target.transform.position, Unit.transform.position);
        if (distance <= _attackDistance)
        {
            TryAttack();
        }
        else
        {
            Pursuit(_target);
        }
    }

    public override void Exit()
    {
        Stop();
        _navMeshAgent.enabled = false;
    }

    public void Pursuit(Unit target)
    {
        _target = target;

        if (_navMeshAgent.isOnNavMesh && _navMeshAgent.isActiveAndEnabled)
        {
            _navMeshAgent.isStopped = false;
            _navMeshAgent.SetDestination(_target.transform.position);

            StringBus stringBus = new();
            Unit.Animator.SetBool(stringBus.ANIM_SPRINT, true);
        }
    }

    private void TryAttack()
    {
        Stop();

        Unit.transform.LookAt(_target.transform);

        StringBus stringBus = new();
        Unit.Animator.SetTrigger(stringBus.ANIM_ATTACK_HANDS);
    }

    public void AttackTarget()
    {
        if (_attackDelay > Time.time) return;

        float distance = Vector3.Distance(_target.transform.position, Unit.transform.position);
        if (distance <= _attackDistance)
        {
            HealthSystem targetHealth = _target.GetSystem<HealthSystem>();
            if (targetHealth != null)
            {
                targetHealth.TakeDamage(Unit.transform.position, Unit.Data.AttackDamage);
                _attackDelay = Time.time + 2;
            }
        }
    }

    private void Stop()
    {
        if (_navMeshAgent.isOnNavMesh && _navMeshAgent.isActiveAndEnabled)
        {
            _navMeshAgent.isStopped = true;
        }

        StringBus stringBus = new();
        Unit.Animator.SetBool(stringBus.ANIM_SPRINT, false);
    }
}