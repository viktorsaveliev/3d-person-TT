using UnityEngine;

[CreateAssetMenu(fileName = "ShotgunData", menuName = "Weapon Data/Shotgun")]
public class ShotgunDataConfig : WeaponDataConfig
{
    [Header("Shotgun Data")]
    [SerializeField, Range(5, 20)] private int _pelletCount;

    public int PelletCount => _pelletCount;
}