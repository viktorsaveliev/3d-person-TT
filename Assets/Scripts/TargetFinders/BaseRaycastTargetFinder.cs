using UnityEngine;

public class BaseRaycastTargetFinder : TargetFinder
{
    public BaseRaycastTargetFinder(Camera camera) : base(camera)
    {
    }

    public override void Attack<T>(T config)
    {
        Ray ray = CameraCache.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));

        if (Physics.Raycast(ray, out RaycastHit hit, config.FireRange))
        {
            if (hit.transform.TryGetComponent(out IDamageable target))
            {
                target.OnHit(hit.point, ray.origin, config.Damage);
            }
        }
    }
}
