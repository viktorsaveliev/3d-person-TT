using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class TakeDamageView : MonoBehaviour
{
    [SerializeField] private Image _damageFade;
    [SerializeField] private Image _healthBar;

    private UserCharacter _user;
    private HealthSystem _health;

    private readonly float _damageFadeDuration = 0.1f;

    public void Init()
    {
        _health = _user.GetSystem<HealthSystem>();
        _health.OnTakedDamage += OnTakeDamage;
    }

    private void OnTakeDamage(int damage)
    {
        _damageFade.DOFade(0.03f, _damageFadeDuration).OnComplete(() => 
        {
            _damageFade.DOFade(0, _damageFadeDuration);
        });

        float healthNormalized = (float) _health.Health / _health.MaxHealth;
        _healthBar.DOFillAmount(healthNormalized, 0.5f);
    }

    [Inject]
    public void Construct(UserCharacter user)
    {
        _user = user;
    }
}
