using UnityEngine;

public class RaycastTargetFinder : ITargetFinder
{
    private readonly Camera _camera;

    public RaycastTargetFinder(Camera camera)
    {
        _camera = camera;
    }

    public void Attack(float fireRange, int damage)
    {
        Ray ray = _camera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));

        if (Physics.Raycast(ray, out RaycastHit hit, fireRange))
        {
            if (hit.transform.TryGetComponent(out UnitBone unitBone))
            {
                unitBone.OnHit(hit.point, ray.direction, damage);
            }
        }
    }
}
