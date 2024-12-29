using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DeliveryResultUI : MonoBehaviour{

    private const string POPUP = "PopUp";

    [SerializeField] private Image backgroundImage;
    [SerializeField] private Image iconImage;
    [SerializeField] private TextMeshProUGUI messageText;
    [SerializeField] private Color successColor;
    [SerializeField] private Color failureColor;
    [SerializeField] private Sprite successIcon;
    [SerializeField] private Sprite failureIcon;

    private Animator animator;

    private void Awake() {
        animator = GetComponent<Animator>();
    }


    private void Start() {
        DeliveryManager.Instance.OnrecipeSuccess += DeliveryManager_OnrecipeSuccess;
        DeliveryManager.Instance.OnrecipeFailed += DeliveryManager_OnrecipeFailed;
        gameObject.SetActive(false);
    }

    private void DeliveryManager_OnrecipeFailed(object sender, System.EventArgs e) {
        gameObject.SetActive(true);
        animator.SetTrigger(POPUP);
        backgroundImage.color = failureColor;
        iconImage.sprite = failureIcon;
        messageText.text = "DELIVERY\nFAILED";
    }

    private void DeliveryManager_OnrecipeSuccess(object sender, System.EventArgs e) {
        gameObject.SetActive(true);
        animator.SetTrigger(POPUP);
        backgroundImage.color = successColor;
        iconImage.sprite = successIcon;
        messageText.text = "DELIVERY\nSUCCESS";
    }
}
