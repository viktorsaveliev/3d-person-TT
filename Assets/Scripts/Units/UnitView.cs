using UnityEngine;

[RequireComponent(typeof(Unit), typeof(Animator))]
public class UnitView : MonoBehaviour, IUnitSystem
{
    [SerializeField] private ParticleSystem _deathFX;

    private Animator _animator;
    private Unit _unit;
    private HealthSystem _health;

    private void OnEnable()
    {
        _health.OnDead += OnDead;
    }

    private void OnDisable()
    {
        _health.OnDead -= OnDead;
    }

    public void Init()
    {
        _animator = GetComponent<Animator>();
        _unit = GetComponent<Unit>();
        _health = _unit.GetSystem<HealthSystem>();
    }

    private void OnDead()
    {
        if (_deathFX != null)
        {
            _deathFX.Play();
        }

        StringBus stringBus = new();
        _animator.SetTrigger(stringBus.ANIM_DEATH_2);
    }
}
