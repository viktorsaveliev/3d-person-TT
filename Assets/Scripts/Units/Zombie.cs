using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Zombie : Unit
{
    private HealthSystem _health;
    private Rigidbody _rigidbody;

    public override void Init()
    {
        base.Init();

        _health = GetSystem<HealthSystem>();
        _health.OnTakedDamage += OnTakedDamage;

        _rigidbody = GetComponent<Rigidbody>();

        AISystem ai = new(this);
        ai.Init();
        AddSystem(ai);
    }

    private void OnTakedDamage(int damage)
    {
        if (_health.Health > 0)
        {
            StringBus stringBus = new();
            Animator.SetTrigger(stringBus.ANIM_REACTION_HIT);
        }
    }

    protected override void OnDead()
    {
        base.OnDead();
        _rigidbody.constraints = RigidbodyConstraints.None;
        GetSystem<AISystem>().CurrentState.Exit();
    }

    private void AttackTarget() // Animation event
    {
        GetSystem<AISystem>()?.AttackTarget();
    }
}
