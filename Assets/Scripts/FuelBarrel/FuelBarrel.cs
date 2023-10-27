using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Collider), typeof(Rigidbody))]
public class FuelBarrel : MonoBehaviour, IDamageable
{
    public event Action OnExplode;
    public event Action OnFlameEnded;

    [SerializeField] private GameObject _fuelBarrel;

    private readonly float _flameDuration = 10f;

    private Collider _collider;
    private Rigidbody _rigidbody;

    private void Awake()
    {
        _collider = GetComponent<Collider>();
        _rigidbody = GetComponent<Rigidbody>();
    }

    public void OnHit(Vector3 hitPoint, Vector3 direction, int damage)
    {
        _fuelBarrel.SetActive(false);

        _collider.enabled = false;
        _rigidbody.isKinematic = true;

        StartCoroutine(FlameTimer());

        OnExplode?.Invoke();
    }

    private IEnumerator FlameTimer()
    {
        yield return new WaitForSeconds(_flameDuration);
        
        OnFlameEnded?.Invoke();

        yield return new WaitForSeconds(5);

        Destroy(gameObject);
    }
}
