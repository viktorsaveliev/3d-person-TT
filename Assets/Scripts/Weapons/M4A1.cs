using UnityEngine;

public class M4A1 : Weapon
{
    public override void Init()
    {
        base.Init();
        TargetFinder = new BaseRaycastTargetFinder(Camera.main);
    }
}
