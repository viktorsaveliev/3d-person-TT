using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class EndGameView : MonoBehaviour
{
    [SerializeField] private GameObject _endGameScreen;

    [SerializeField] private TMP_Text _header;

    [SerializeField] private TMP_Text _winsCount;
    [SerializeField] private TMP_Text _defeatsCount;

    [SerializeField] private Button _restart;
    [SerializeField] private Button _mainMenu;

    private GameStatus _gameStats;
    private ISaveData _saveData;

    private void OnEnable()
    {
        _gameStats.OnGameOver += Show;

        _restart.onClick.AddListener(Restart);
        _mainMenu.onClick.AddListener(MainMenu);
    }

    private void OnDisable()
    {
        _gameStats.OnGameOver -= Show;

        _restart.onClick.RemoveListener(Restart);
        _mainMenu.onClick.RemoveListener(MainMenu);
    }

    private void Show(GameStatus.EndReason reason)
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        string header = reason == GameStatus.EndReason.Win ? "YOU WIN" : "YOU LOSE";

        _header.text = $"{header}";
        _winsCount.text = $"{_saveData.GetWinsCount()}";
        _defeatsCount.text = $"{_saveData.GetDefeatsCount()}";

        _endGameScreen.SetActive(true);
    }

    private void Restart()
    {
        SceneLoader sceneLoader = new();
        sceneLoader.LoadLevel();
    }

    private void MainMenu()
    {
        SceneLoader sceneLoader = new();
        sceneLoader.LoadMenu();
    }

    [Inject]
    public void Construct(GameStatus gameStats, ISaveData saveData)
    {
        _gameStats = gameStats;
        _saveData = saveData;
    }
}
