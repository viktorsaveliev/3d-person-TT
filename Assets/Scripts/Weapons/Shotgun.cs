
using UnityEngine;

public class Shotgun : Weapon
{
    public override void Init()
    {
        base.Init();
        TargetFinder = new MultiplieTargetFinder(Camera.main);
    }
}
