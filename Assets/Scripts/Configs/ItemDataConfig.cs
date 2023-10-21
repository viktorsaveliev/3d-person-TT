using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemDataConfig", menuName = "Game/ItemDataConfig")]
public class ItemDataConfig : ScriptableObject
{
    [SerializeField] private Sprite _icon;
    [SerializeField] private string _name;
    [SerializeField] private string _description;
    [SerializeField, Range(1, 30)] private int _amount;

    public Sprite Icon => _icon;
    public string Name => _name;
    public string Description => _description;
    public int Amount => _amount;
}
