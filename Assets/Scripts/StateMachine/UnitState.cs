public abstract class UnitState : State
{
    protected readonly Unit Unit;

    public UnitState(Unit unit)
    {
        Unit = unit;
    }
}
