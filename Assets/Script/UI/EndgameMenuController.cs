using System;
using System.Resources;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Canvas))]
public class EndgameMenuController : MonoBehaviour
{
    private Canvas _canvas;

    private static EndgameMenuController _instance;
    public static EndgameMenuController Instance => _instance;

    [SerializeField] private TextMeshProUGUI subtext;
    [SerializeField] private TextMeshProUGUI score;


    public void Awake()
    {
        _instance = this;
        _canvas = GetComponent<Canvas>();
        _canvas.enabled = false;
    }

    public void Show()
    {
        Time.timeScale = 0;
        subtext.text = ResourcesManager.Instance.GetWaterCount() < 0 ? "Your tree dried out!" : "The critters destroyed your tree";
        score.text = $"You survived for {TimeSpan.FromSeconds(GameManager.Instance.GetTotalTime()).ToString("mm\\:ss")} !";
        _canvas.enabled = true;
    }

    public void Retry()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Exit()
    {
        Application.Quit();
    }
}
