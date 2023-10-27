using UnityEngine;

public class AnimationCache
{
    private readonly int _sprintIndex;
    private readonly int _moveIndex;
    private readonly int _jumpIndex;
    private readonly int _aimRifleIndex;
    private readonly int _deathIndex;
    private readonly int _hitReactionIndex;
    private readonly int _reloadRifleIndex;
    private readonly int _zombieAttackIndex;
    private readonly int _withRifleIndex;

    public int SprintIndex => _sprintIndex;
    public int MoveIndex => _moveIndex;
    public int JumpIndex => _jumpIndex;
    public int AimRifleIndex => _aimRifleIndex;
    public int DeathIndex => _deathIndex;
    public int HitReactionIndex => _hitReactionIndex;
    public int ReloadRifleIndex => _reloadRifleIndex;
    public int ZombieAttackIndex => _zombieAttackIndex;
    public int WithRifleIndex => _withRifleIndex;

    public AnimationCache()
    {
        _sprintIndex = Animator.StringToHash(StringBus.ANIM_SPRINT);
        _moveIndex = Animator.StringToHash(StringBus.ANIM_MOVE);
        _jumpIndex = Animator.StringToHash(StringBus.ANIM_JUMP);
        _aimRifleIndex = Animator.StringToHash(StringBus.ANIM_AIM_RIFLE);
        _deathIndex = Animator.StringToHash(StringBus.ANIM_DEATH_2);
        _hitReactionIndex = Animator.StringToHash(StringBus.ANIM_REACTION_HIT);
        _reloadRifleIndex = Animator.StringToHash(StringBus.ANIM_RELOAD_RIFLE);
        _zombieAttackIndex = Animator.StringToHash(StringBus.ANIM_ATTACK_HANDS);
        _withRifleIndex = Animator.StringToHash(StringBus.ANIM_WITH_RIFLE);
    }
}
