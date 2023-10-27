using UnityEngine;

[RequireComponent(typeof(Animator), typeof(Unit))]
public class UnitBonesController : MonoBehaviour
{
    [SerializeField] private UnitBone[] _bones;

    private Animator _animator;
    private Unit _unit;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _unit = GetComponent<Unit>();
    }

    private void OnEnable()
    {
        _unit.OnInit += Init;

        AssignBones();
    }

    private void OnDisable()
    {
        _unit.OnInit -= Init;

        DeassignBones();
    }

    public void EnableRagdoll()
    {
        if (_bones.Length == 0) return;

        _animator.enabled = false;

        foreach (var bone in _bones)
        {
            bone.SetKinematic(false);
        }
    }

    public void DisableRagdoll()
    {
        if (_bones.Length == 0) return;

        _animator.enabled = true;

        foreach (var bone in _bones)
        {
            bone.SetKinematic(true);
        }
    }

    private void OnBoneHit(Vector3 hitPosition, Vector3 attackerPosition, BoneData.BoneType boneType, int damage)
    {
        HealthSystem health = _unit.GetSystem<HealthSystem>();
        health.TakeDamage(hitPosition, damage);

        if (health.Health > 0)
        {
            _unit.GetSystem<AISystem>()?.Run(attackerPosition);
        }
    }

    private void AssignBones()
    {
        foreach (var bone in _bones)
        {
            bone.OnBoneHit += OnBoneHit;
        }
    }

    private void DeassignBones()
    {
        foreach (var bone in _bones)
        {
            bone.OnBoneHit -= OnBoneHit;
        }
    }

    private void Init()
    {
        DisableRagdoll();
        _unit.GetSystem<HealthSystem>().OnDead += EnableRagdoll;
    }
}
