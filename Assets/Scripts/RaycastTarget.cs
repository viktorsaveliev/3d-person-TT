using UnityEngine;

public class RaycastTarget
{
    private readonly Camera _camera;
    private readonly LayerMask _layerMask;

    public RaycastTarget(Camera camera)
    {
        _camera = camera;
        _layerMask = LayerMask.GetMask("Unit");
    }

    public Unit GetTarget(out Vector3 hitPoint, float fireRange)
    {
        hitPoint = Vector3.zero;

        Ray ray = _camera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));

        if (Physics.Raycast(ray, out RaycastHit hit, fireRange, _layerMask))
        {
            if (hit.transform.TryGetComponent(out Unit unit))
            {
                hitPoint = hit.point;
                return unit;
            }
        }

        return null;
    }
}
