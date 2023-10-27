using UnityEngine;

public interface IDamageable
{
    public void OnHit(Vector3 hitPoint, Vector3 direction, int damage);
}
