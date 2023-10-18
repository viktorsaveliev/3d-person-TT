using UnityEngine;

public class UnitDetection : MonoBehaviour
{
    [SerializeField] private Unit _unit;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out UserCharacter unit))
        {
            if (_unit.GetSystem<HealthSystem>().Health > 0)
            {
                AISystem ai = _unit.GetSystem<AISystem>();
                ai.Pursuit(unit);
            }
        }
    }
}
