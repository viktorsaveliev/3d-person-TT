using System;
using Zenject;

public class GameStatus
{
    public event Action<EndReason> OnGameOver;

    private bool _isGameOver;

    public enum EndReason
    {
        Lose,
        Win
    }

    private ISaveData _saveData;
    private EnemyCounter _enemyCounter;
    private UserCharacter _user;

    public void Init()
    {
        _enemyCounter.OnAllEnemyKilled += Win;
        _user.GetSystem<HealthSystem>().OnDead += Lose;
    }

    private void Win()
    {
        if (_isGameOver) return;
        _isGameOver = true;

        _saveData.SaveWinsCount();
        OnGameOver?.Invoke(EndReason.Win);
    }

    private void Lose()
    {
        if (_isGameOver) return;
        _isGameOver = true;

        _saveData.SaveDefeatsCount();
        OnGameOver?.Invoke(EndReason.Lose);
    }

    [Inject]
    public void Construct(EnemyCounter counter, UserCharacter user, ISaveData saveData)
    {
        _enemyCounter = counter;
        _user = user;
        _saveData = saveData;
    }
}
