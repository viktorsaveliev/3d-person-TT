
public class WaitingState : UnitState
{
    public WaitingState(Unit unit) : base(unit)
    {
    }

    public override void Enter()
    {
        StringBus stringBus = new();
        Unit.Animator.SetFloat(stringBus.ANIM_MOVE, 0);
    }

    public override void Update()
    {
        
    }

    public override void Exit()
    {

    }
}
