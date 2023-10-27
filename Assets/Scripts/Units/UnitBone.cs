using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class UnitBone : MonoBehaviour, IDamageable
{
    public event Action<Vector3, Vector3, BoneData.BoneType, int> OnBoneHit;

    [SerializeField] private BoneData.BoneType _boneType;

    private Rigidbody _rigidbody;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    public void OnHit(Vector3 hitPosition, Vector3 attackerPosition, int damage)
    {
        Vector3 shootDirection = hitPosition - attackerPosition;
        ApplyImpulse(shootDirection, damage);

        OnBoneHit?.Invoke(hitPosition, attackerPosition, _boneType, damage);
    }

    public void SetKinematic(bool isEnable)
    {
        _rigidbody.isKinematic = isEnable;
    }

    private void ApplyImpulse(Vector3 direction, float force)
    {
        SetKinematic(false);
        _rigidbody.AddForce(direction * force, ForceMode.Impulse);
    }
}

public class BoneData
{
    public enum BoneType
    {
        Head,
        Spine,
        Hips,
        Arm,
        Leg,
        Foot
    }
}