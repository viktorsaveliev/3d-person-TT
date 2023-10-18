using System;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    public event Action<bool> OnReloadStateChanged;
    public event Action OnShot;

    [SerializeField] private WeaponDataConfig _config;
    
    private int _currentAmmoCapacity;
    private float _currentDelayBetweenShoots;
    private bool _isAvailable;

    private RaycastTarget _raycastTarget;

    public WeaponDataConfig Data => _config;
    public int CurrentAmmo => _currentAmmoCapacity;

    public virtual void Init()
    {
        _raycastTarget = new(Camera.main);
        _currentAmmoCapacity = _config.MaxAmmo;
        _isAvailable = true;
    }

    public bool TryShot()
    {
        if (!_isAvailable
            || _currentDelayBetweenShoots >= Time.time)
        {
            return false;
        }

        Shot();

        return true;
    }

    public void StartReloading()
    {
        _isAvailable = false;

        Timer timer = new(this);
        timer.StartTimer(_config.ReloadTime);
        timer.OnTimerEnded += Reload;

        OnReloadStateChanged?.Invoke(true);
    }

    protected virtual void Shot()
    {
        if (--_currentAmmoCapacity <= 0)
        {
            OnAmmoEnded();
        }
        else
        {
            _currentDelayBetweenShoots = Time.time + _config.DelayBetweenShoots;
        }

        Unit unit = _raycastTarget.GetTarget(out Vector3 hitPoint, _config.FireRange);

        if (unit != null)
        {
            HealthSystem health = unit.GetSystem<HealthSystem>();
            if (health.Health > 0)
            {
                health.TakeDamage(hitPoint, _config.Damage);
            }
        }

        OnShot?.Invoke();
    }

    private void OnAmmoEnded()
    {
        StartReloading();
    }

    private void Reload()
    {
        _isAvailable = true;
        _currentAmmoCapacity = _config.MaxAmmo;

        OnReloadStateChanged?.Invoke(false);
    }
}
