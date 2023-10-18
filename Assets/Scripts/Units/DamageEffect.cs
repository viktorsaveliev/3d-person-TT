using UnityEngine;

[RequireComponent(typeof(Unit), typeof(Animator))]
public class DamageEffect : MonoBehaviour, IUnitSystem
{
    [SerializeField] private ParticleSystem _damageFX;

    private Unit _unit;
    private HealthSystem _health;

    private void Start()
    {
        _unit = GetComponent<Unit>();
        _health = _unit.GetSystem<HealthSystem>();
        _health.OnTakedDamage += DamageFX;
    }

    private void OnDisable()
    {
        _health.OnTakedDamage -= DamageFX;
    }

    private void DamageFX(int damage)
    {
        if (_damageFX != null)
        {
            _damageFX.transform.position = _health.LastHitPosition;
            _damageFX.Play();
        }
    }
}
