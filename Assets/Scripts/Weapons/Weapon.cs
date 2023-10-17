using System;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    public event Action OnReload;
    public event Action OnShot;

    [SerializeField] private WeaponDataConfig _config;

    private int _currentAmmoCapacity;
    private float _currentDelayBetweenShoots;
    private bool _isAvailable;

    public WeaponDataConfig Data => _config;
    public int CurrentAmmo => _currentAmmoCapacity;

    public virtual void Init()
    {
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

        Ray ray = new(transform.position, Camera.main.transform.forward);

        if (Physics.Raycast(ray, out RaycastHit hit, _config.FireRange))
        {
            Debug.Log("Попал в " + hit.transform.name);
        }
        else
        {
            print("nope");
        }

        OnShot?.Invoke();
    }

    private void OnAmmoEnded()
    {
        _isAvailable = false;

        Timer timer = new(this);
        timer.StartTimer(_config.ReloadTime);
        timer.OnTimerEnded += Reload;

        OnReload?.Invoke();
    }

    private void Reload()
    {
        _isAvailable = true;
        _currentAmmoCapacity = _config.MaxAmmo;
    }
}
