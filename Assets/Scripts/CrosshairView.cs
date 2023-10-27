using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class CrosshairView : MonoBehaviour
{
    [SerializeField] private GameObject _crosshair;
    [SerializeField] private Image _crosshairRange;

    private UserCharacter _userUnit;
    private Weapon _currentWeapon;
    private QuickSlotsController _quickSlots;

    private WeaponSystem _weaponSystem;
    private IInputMode _inputMode;

    private readonly float _baseCrosshairScale = 1f;

    public void Init()
    {
        _inputMode.OnAimed += OnAimStateChange;

        _quickSlots.OnWeaponChanged += OnWeaponChanged;
        _weaponSystem = _userUnit.GetSystem<WeaponSystem>();

        if (_weaponSystem != null)
        {
            _weaponSystem.OnUnequipWeapon += Hide;
        }
    }

    private void OnAimStateChange(bool isAiming)
    {
        _crosshair.SetActive(isAiming);
    }

    private void OnWeaponChanged()
    {
        _currentWeapon = _userUnit.GetSystem<WeaponSystem>()?.CurrentWeapon;
        UpdateCrosshairScale();
    }

    private void UpdateCrosshairScale()
    {
        float newScale = _baseCrosshairScale + _currentWeapon.WeaponData.SpreadRange * 2f;
        _crosshairRange.transform.localScale = new Vector3(newScale, newScale, 1f);
    }

    private void Hide()
    {
        _crosshair.SetActive(false);
    }

    [Inject]
    public void Construct(IInputMode inputMode, UserCharacter userUnit, QuickSlotsController quickSlots)
    {
        _inputMode = inputMode;
        _userUnit = userUnit;
        _quickSlots = quickSlots;
    }
}
