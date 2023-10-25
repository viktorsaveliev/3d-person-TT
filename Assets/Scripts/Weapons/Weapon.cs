using System;
using UnityEngine;
using Zenject;

public abstract class Weapon : Item
{
    public event Action<bool> OnReloadStateChanged;
    public event Action OnShot;

    [SerializeField] private WeaponDataConfig _weaponConfig;
    [SerializeField] private AmmoData.AmmoType _ammoType;

    private int _currentAmmoCapacity;
    private float _currentDelayBetweenShoots;
    private bool _isCanShot;

    private ITargetFinder _targetFinder;
    private IAmmoCounter _ammoCounter;

    public WeaponDataConfig WeaponData => _weaponConfig;
    public int CurrentAmmoCapacity => _currentAmmoCapacity;

    public AmmoData.AmmoType AmmoType => _ammoType;

    private void Awake()
    {
        Init();
    }

    public override void Init()
    {
        base.Init();

        _currentAmmoCapacity = _weaponConfig.MaxAmmo;
        _isCanShot = true;
    }

    public bool TryShot()
    {
        if (!_isCanShot
            || _currentDelayBetweenShoots >= Time.time)
        {
            return false;
        }

        Shot();

        return true;
    }

    public void StartReloading()
    {
        int ammoCount = _ammoCounter.GetAmmoCount(_ammoType);
        if (ammoCount <= 0) return;

        Timer timer = new(this);
        timer.StartTimer(_weaponConfig.ReloadTime);
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
            _currentDelayBetweenShoots = Time.time + _weaponConfig.DelayBetweenShoots;
        }

        _targetFinder.Attack(_weaponConfig.FireRange, _weaponConfig.Damage);

        OnShot?.Invoke();
    }

    private void OnAmmoEnded()
    {
        _isCanShot = false;
        StartReloading();
    }

    private void Reload()
    {
        int ammoBeforeReload = _currentAmmoCapacity;
        int ammoCount = _ammoCounter.GetAmmoCount(_ammoType);

        if (ammoCount > _weaponConfig.MaxAmmo)
        {
            _currentAmmoCapacity = _weaponConfig.MaxAmmo;
        }
        else
        {
            _currentAmmoCapacity = ammoCount;
        }

        _ammoCounter.SpendAmmo(_ammoType, _currentAmmoCapacity - ammoBeforeReload);

        if (_currentAmmoCapacity > 0)
        {
            _isCanShot = true;
        }

        OnReloadStateChanged?.Invoke(false);
    }

    [Inject]
    public void Construct(IAmmoCounter ammoCounter, ITargetFinder targetFinder)
    {
        _ammoCounter = ammoCounter;
        _targetFinder = targetFinder;
    }
}
