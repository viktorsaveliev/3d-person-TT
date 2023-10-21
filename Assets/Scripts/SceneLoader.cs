using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader
{
    public enum Scene
    {
        Menu,
        Level
    }

    public void LoadMenu()
    {
        Cursor.visible = true;
        ResetData();
        SceneManager.LoadSceneAsync((int) Scene.Menu, LoadSceneMode.Single);
    }

    public void LoadLevel()
    {
        ResetData();
        SceneManager.LoadSceneAsync((int) Scene.Level, LoadSceneMode.Single);
    }

    private void ResetData()
    {
        DOTween.Clear();
    }
}
