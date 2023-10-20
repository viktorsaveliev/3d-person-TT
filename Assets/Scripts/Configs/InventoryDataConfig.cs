using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "InventoryDataConfig", menuName = "Game/InventoryDataConfig")]
public class InventoryDataConfig : ScriptableObject
{
    [SerializeField, Range(2, 5)] private int _quickAccessSlotsCapacity;
    [SerializeField, Range(3, 15)] private int _slotsCapacity;

    [SerializeField] private Color _selectedSlotColor;
    [SerializeField] private Color _regularSlotColor;

    public int QuickSlotsCapacity => _quickAccessSlotsCapacity;
    public int SlotsCapacity => _slotsCapacity;
    public Color SelectedSlotColor => _selectedSlotColor;
    public Color RegularSlotColor => _regularSlotColor;
}

[CustomEditor(typeof(InventoryDataConfig))]
public class InventoryDataConfigSaver : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        InventoryDataConfig scriptableObject = (InventoryDataConfig)target;
        if (GUILayout.Button("Save"))
        {
            EditorUtility.SetDirty(scriptableObject);
        }
    }
}

