using UnityEngine;

public class PlayerPrefsSaver : ISaveData
{
    private readonly StringBus _stringBus = new();

    public int GetDefeatsCount() 
        => PlayerPrefs.GetInt(_stringBus.SAVE_DEFEATS_COUNT);

    public int GetWinsCount()
        => PlayerPrefs.GetInt(_stringBus.SAVE_WINS_COUNT);

    public void SaveDefeatsCount()
    {
        PlayerPrefs.SetInt(_stringBus.SAVE_DEFEATS_COUNT,
            PlayerPrefs.GetInt(_stringBus.SAVE_DEFEATS_COUNT)
            + 1);

        PlayerPrefs.Save();
    }

    public void SaveWinsCount()
    {
        PlayerPrefs.SetInt(_stringBus.SAVE_WINS_COUNT,
            PlayerPrefs.GetInt(_stringBus.SAVE_WINS_COUNT)
            + 1);

        PlayerPrefs.Save();
    }
}
