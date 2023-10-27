using UnityEngine;

public class MultiplieTargetFinder : TargetFinder
{
    public MultiplieTargetFinder(Camera camera) : base(camera)
    {
    }

    public override void Attack<T>(T config)
    {
        ShotgunDataConfig shotgunConfig = config as ShotgunDataConfig;

        if (shotgunConfig == null) return;

        for (int i = 0; i < shotgunConfig.PelletCount; i++)
        {
            float spreadAngleX = Random.Range(-shotgunConfig.SpreadRange, shotgunConfig.SpreadRange);
            float spreadAngleY = Random.Range(-shotgunConfig.SpreadRange, shotgunConfig.SpreadRange);
            Quaternion spreadRotation = Quaternion.Euler(spreadAngleX, spreadAngleY, 0f);

            Ray ray = CameraCache.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
            ray.direction = spreadRotation * ray.direction;

            if (Physics.Raycast(ray, out RaycastHit hit, shotgunConfig.FireRange))
            {
                Debug.DrawLine(ray.origin, hit.point, Color.red, 5.0f);
                if (hit.transform.TryGetComponent(out IDamageable target))
                {
                    target.OnHit(hit.point, ray.origin, shotgunConfig.Damage / shotgunConfig.PelletCount);
                }
            }
            else
            {
                Debug.DrawRay(ray.origin, ray.direction * shotgunConfig.FireRange, Color.blue, 5.0f);
            }
        }
    }

}
