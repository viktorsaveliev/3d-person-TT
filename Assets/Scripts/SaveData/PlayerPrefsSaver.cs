using UnityEngine;

public class PlayerPrefsSaver : ISaveData
{
    public int GetDefeatsCount() 
        => PlayerPrefs.GetInt(StringBus.SAVE_DEFEATS_COUNT);

    public int GetWinsCount()
        => PlayerPrefs.GetInt(StringBus.SAVE_WINS_COUNT);

    public void SaveDefeatsCount()
    {
        PlayerPrefs.SetInt(StringBus.SAVE_DEFEATS_COUNT,
            PlayerPrefs.GetInt(StringBus.SAVE_DEFEATS_COUNT)
            + 1);

        PlayerPrefs.Save();
    }

    public void SaveWinsCount()
    {
        PlayerPrefs.SetInt(StringBus.SAVE_WINS_COUNT,
            PlayerPrefs.GetInt(StringBus.SAVE_WINS_COUNT)
            + 1);

        PlayerPrefs.Save();
    }
}
