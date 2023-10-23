using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class UnitBone : MonoBehaviour
{
    public event Action<Vector3, BoneData.BoneType, int> OnBoneHit;

    [SerializeField] private BoneData.BoneType _boneType;

    private readonly float _impulseMultiplier = 2;
    private Rigidbody _rigidbody;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    public void OnHit(Vector3 hitPosition, Vector3 direction, int damage)
    {
        ApplyImpulse(direction, damage * _impulseMultiplier);
        OnBoneHit?.Invoke(hitPosition, _boneType, damage);
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