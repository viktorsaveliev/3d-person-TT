using UnityEngine;

public abstract class TargetFinder
{
    protected readonly Camera CameraCache;

    public TargetFinder(Camera camera)
    {
        CameraCache = camera;
    }

    public abstract void Attack<T>(T config) where T : WeaponDataConfig;
}
