using UnityEngine;

public class AK74 : Weapon
{
    public override void Init()
    {
        base.Init();
        TargetFinder = new BaseRaycastTargetFinder(Camera.main);
    }
}
