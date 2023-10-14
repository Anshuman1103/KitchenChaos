using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI recipeDeliveredText;


    private void Start()
    {
        KitchenGameManager.instance.OnStateChanged += KitchenGameManager_OnStateChanged;

        Hide();
    }
    private void KitchenGameManager_OnStateChanged(object sender, System.EventArgs e)
    {
        if (KitchenGameManager.instance.IsGameOver())
        {
            Show();
            recipeDeliveredText.text = DeliveryManager.Instance.GetSuccessfulRecipeAmount().ToString();
        }
        else
        {
            Hide();
        }
    }

    private void Update()
    {
      
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
