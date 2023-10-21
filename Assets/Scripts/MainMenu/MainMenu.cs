using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Button _levelButton;
    [SerializeField] private Button _exitButton;

    private void OnEnable()
    {
        _levelButton.onClick.AddListener(LoadLevel);
        _exitButton.onClick.AddListener(Exit);
    }

    private void OnDisable()
    {
        _levelButton.onClick.RemoveListener(LoadLevel);
        _exitButton.onClick.RemoveListener(Exit);
    }

    private void LoadLevel()
    {
        SceneLoader sceneLoader = new();
        sceneLoader.LoadLevel();
    }

    private void Exit()
    {
        Application.Quit();
    }
}
