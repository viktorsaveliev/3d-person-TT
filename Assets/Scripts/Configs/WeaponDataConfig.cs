using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponDataConfig", menuName = "Game/WeaponDataConfig")]
public class WeaponDataConfig : ScriptableObject
{
    [Header("Data")]
    [SerializeField, Range(2, 50)] private int _damage;
    [SerializeField, Range(1, 50)] private int _maxAmmoCapacity;
    [SerializeField, Range(1, 10)] private float _reloadTime;
    [SerializeField, Range(0.03f, 5)] private float _delayBetweenShoots;
    [SerializeField, Range(10, 200)] private float _fireRange;

    public int Damage => _damage;
    public int MaxAmmo => _maxAmmoCapacity;
    public float ReloadTime => _reloadTime;
    public float DelayBetweenShoots => _delayBetweenShoots;
    public float FireRange => _fireRange;
}

[CustomEditor(typeof(WeaponDataConfig))]
public class WeaponDataConfigSaver : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        WeaponDataConfig scriptableObject = (WeaponDataConfig)target;
        if (GUILayout.Button("Save"))
        {
            EditorUtility.SetDirty(scriptableObject);
        }
    }
}
