using UnityEngine;

[CreateAssetMenu(fileName = "Unit data", menuName = "Game/Unit data")]
public class UnitDataConfig : ScriptableObject
{
    [SerializeField, Range(1, 200)] private int _health;
    [SerializeField, Range(1, 20)] private int _attackDamage;

    [SerializeField, Range(1, 10)] private float _regularSpeed = 2f;
    [SerializeField, Range(1, 10)] private float _aimSpeed = 1f;
    [SerializeField, Range(1, 10)] private float _sprintSpeed = 6f;

    public int Health => _health;
    public int AttackDamage => _attackDamage;

    public float RegularSpeed => _regularSpeed;
    public float AimSpeed => _aimSpeed;
    public float SprintSpeed => _sprintSpeed;
}
