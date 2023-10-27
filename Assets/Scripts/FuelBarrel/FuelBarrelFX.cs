using UnityEngine;

[RequireComponent(typeof(FuelBarrel))]
public class FuelBarrelFX : MonoBehaviour
{
    [SerializeField] private ParticleSystem _explodeFX;
    [SerializeField] private ParticleSystem _flameFX;

    private FuelBarrel _fuelBarrel;

    private void Awake()
    {
        _fuelBarrel = GetComponent<FuelBarrel>();
    }

    private void OnEnable()
    {
        _fuelBarrel.OnExplode += PlayFX;
        _fuelBarrel.OnFlameEnded += StopFX;
    }

    private void OnDisable()
    {
        _fuelBarrel.OnExplode -= PlayFX;
        _fuelBarrel.OnFlameEnded -= StopFX;
    }

    private void PlayFX()
    {
        _explodeFX.transform.rotation = Quaternion.Euler(0, 0, 0);
        _flameFX.transform.rotation = Quaternion.Euler(0, 0, 0);

        _explodeFX.Play();
        _flameFX.Play();
    }

    private void StopFX()
    {
        _flameFX.Stop();
    }
}
