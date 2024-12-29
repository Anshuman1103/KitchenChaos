using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamePauseUI : MonoBehaviour
{
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private Button optionButton;

    private void Awake()
    {
        resumeButton.onClick.AddListener(() =>
        {
            KitchenGameManager.instance.TogglePauseGame();
        });
        mainMenuButton.onClick.AddListener(() =>
        {
            Loader.Load(Loader.Scene.MainMenuScene);
        });
        optionButton.onClick.AddListener(() =>
        {
            Hide();
            OptionUI.Instance.Show(Show);
        });
    }
    private void Start()
    {
        KitchenGameManager.instance.OnGamePaused += Pause_OnGamePaused;
        KitchenGameManager.instance.OnGameResume += Resume_OnGameResume;
        

        Hide();
    }

    private void Resume_OnGameResume(object sender, System.EventArgs e)
    {
        Hide();
    }

    private void Pause_OnGamePaused(object sender, System.EventArgs e)
    {
        Show();
    }

    private void Show()
    {
        gameObject.SetActive(true);
        resumeButton.Select();  
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
