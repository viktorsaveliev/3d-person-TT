using UnityEngine;

[RequireComponent(typeof(Unit))]
public class DamageEffect : MonoBehaviour, IUnitSystem
{
    [SerializeField] private ParticleSystem _damageFX;
    [SerializeField] private bool _isChangePositionFX;

    private Unit _unit;
    private HealthSystem _health;

    private void Start()
    {
        _unit = GetComponent<Unit>();
        _health = _unit.GetSystem<HealthSystem>();
        _health.OnTakedDamage += DamageFX;
    }

    private void DamageFX(int damage)
    {
        if (_damageFX != null)
        {
            if(_isChangePositionFX) 
                _damageFX.transform.position = _health.LastHitPosition;
            _damageFX.Play();
        }
    }
}
