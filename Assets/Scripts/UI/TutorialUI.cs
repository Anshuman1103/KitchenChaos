using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TutorialUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI keyMoveUp;
    [SerializeField] private TextMeshProUGUI keyMoveDown;
    [SerializeField] private TextMeshProUGUI keyMoveRight;
    [SerializeField] private TextMeshProUGUI keyMoveLeft;
    [SerializeField] private TextMeshProUGUI keyInteract;
    [SerializeField] private TextMeshProUGUI keyInteractAlt;
    [SerializeField] private TextMeshProUGUI keyPause;
    //[SerializeField] private TextMeshProUGUI keyGamepadInteract;
    //[SerializeField] private TextMeshProUGUI keyGamepadInteractAlt;
    //[SerializeField] private TextMeshProUGUI keyGamepadPause;

    private void Start() {
        GameInput.Instance.OnRebindBinding += GameInput_OnRebindBinding;
        KitchenGameManager.instance.OnStateChanged += KitchenGameManager_OnStateChanged;
        UpdateVisual();
        Show();
    }

    private void KitchenGameManager_OnStateChanged(object sender, System.EventArgs e) {
        if (KitchenGameManager.instance.IsCountdownTOStartActive()) {
            Hide();
        }
    }

    private void GameInput_OnRebindBinding(object sender, System.EventArgs e) {
        UpdateVisual();
    }

    private void UpdateVisual() {

        keyMoveUp.text = GameInput.Instance.GetBindingText(GameInput.Binding.Move_Up);
        keyMoveDown.text = GameInput.Instance.GetBindingText(GameInput.Binding.Move_Down);
        keyMoveRight.text = GameInput.Instance.GetBindingText(GameInput.Binding.Move_Right);
        keyMoveLeft.text = GameInput.Instance.GetBindingText(GameInput.Binding.Move_Left);
        keyInteract.text = GameInput.Instance.GetBindingText(GameInput.Binding.Interact);
        keyInteractAlt.text = GameInput.Instance.GetBindingText(GameInput.Binding.Interact_Alternate);
        keyPause.text = GameInput.Instance.GetBindingText(GameInput.Binding.Pause);
        //keyPause.text = GameInput.Instance.GetBindingText(GameInput.Binding.Move_Up);
        //keyGamepadInteractAlt.text = GameInput.Instance.GetBindingText(GameInput.Binding.Move_Up);
        //keyGamepadPause.text = GameInput.Instance.GetBindingText(GameInput.Binding.Move_Up);
    }

    private void Show() {
        gameObject.SetActive(true);
    }

    private void Hide() {
        gameObject?.SetActive(false);   
    }
}
