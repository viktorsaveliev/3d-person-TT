using UnityEngine;

[RequireComponent(typeof(Weapon))]
public class WeaponView : MonoBehaviour
{
    [SerializeField] private ParticleSystem _shotFX;

    private Weapon _weapon;

    private void Awake()
    {
        _weapon = GetComponent<Weapon>();
    }

    private void OnEnable()
    {
        _weapon.OnShot += PlayShotFX;
    }

    private void OnDisable()
    {
        _weapon.OnShot -= PlayShotFX;
    }

    private void PlayShotFX()
    {
        _shotFX.Play();
    }
}
