using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameStartCountdownUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI countdownText;

    private void Start()
    {
        KitchenGameManager.instance.OnStateChanged += KitchenGameManager_OnStateChanged;

        Hide(); 
    }

    private void KitchenGameManager_OnStateChanged(object sender, System.EventArgs e)
    {
        if (KitchenGameManager.instance.IsCountdownTOStartActive())
        {
            Show();
        }
        else
        {
            Hide();
        }
    }

    private void Update()
    {
        countdownText.text = Mathf.Ceil( KitchenGameManager.instance.GetCountToStartTimer()).ToString(); 
        // we can also use ("F2") or ("#.##") inside to string to display just number 2 number after decimal
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }
    private void Hide()
    {
        gameObject.SetActive(false);
    }
}