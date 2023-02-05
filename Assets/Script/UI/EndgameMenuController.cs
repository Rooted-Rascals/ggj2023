using System;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Canvas))]
public class EndgameMenuController : MonoBehaviour
{
    private Canvas _canvas;

    private static EndgameMenuController _instance;
    public static EndgameMenuController Instance => _instance;

    public void Awake()
    {
        _instance = this;
        _canvas = GetComponent<Canvas>();
        _canvas.enabled = false;
    }

    public void Show()
    {
        _canvas.enabled = true;
    }

    public void Retry()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Exit()
    {
        Application.Quit();
    }
}
