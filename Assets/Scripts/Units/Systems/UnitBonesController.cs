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

    private void OnBoneHit(Vector3 hitPosition, BoneData.BoneType boneType, int damage)
    {
        _unit.GetSystem<HealthSystem>().TakeDamage(hitPosition, damage);
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
