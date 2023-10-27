
public class WaitingState : UnitState
{
    public WaitingState(Unit unit) : base(unit)
    {
    }

    public override void Enter()
    {
        Unit.Animator.SetFloat(Unit.AnimCache.MoveIndex, 0);
    }

    public override void Update()
    {
        
    }

    public override void Exit()
    {

    }
}
