using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor.Hardware;
using UnityEngine;

public class Player : MonoBehaviour, IKitchenObjectParent
{
    public event EventHandler OnPickedUpSomething;

    public static Player Instance { get; private set; }
    public event EventHandler<OnSelectedCounterChangedEventArgs>  OnSelectedCounterChanged;
    public class OnSelectedCounterChangedEventArgs : EventArgs
    {
        public BaseCounter selectedCounter;
    }

    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private GameInput gameInput;
    [SerializeField] private LayerMask counterLayerMask;
    [SerializeField] private Transform KitchenObjectHoldPoint;

    private bool isWalking;
    private Vector3 lastInteractDir;
    private BaseCounter selectedCounter;
    private KitchenObject kitchenObject;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.Log("there is more than one player intance");
        }
        Instance = this;

    }        

    private void Start()
    {
        gameInput.OnInteractAction += GameInput_OnInteractAction;
        gameInput.OnInteractAlternateAction += GameInput_OnInteractAlternateAction;
    }

    private void GameInput_OnInteractAlternateAction(object sender, EventArgs e)
    {
        if(!KitchenGameManager.instance.IsGamePlaying())  return; 

        if (selectedCounter != null)
        {
            selectedCounter.InteractAlternate(this);
        }
    }

    private void GameInput_OnInteractAction(object sender, System.EventArgs e)
    {
        if (!KitchenGameManager.instance.IsGamePlaying()) return;

        if (selectedCounter!= null)
        {
            selectedCounter.Interact(this);
        }
       
    }
    private void Update()
    {
        HandleMovement();
        HandleInteraction();

    }

    public bool IsWalking()
    {
        return isWalking;
    }
   
    private void HandleInteraction()
    {
        Vector2 inputVector = gameInput.GetMovementVectorNormalised();

        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);

        if (moveDir != Vector3.zero)
        {
            lastInteractDir = moveDir;
        }
        float interactDistance = 2f;

        if (Physics.Raycast(transform.position, lastInteractDir, out RaycastHit raycastHit, interactDistance, counterLayerMask))
        {

            if (raycastHit.transform.TryGetComponent(out BaseCounter baseCounter))
            {
                // has clear counter
                if(baseCounter != selectedCounter)
                {
                    selectedCounter= baseCounter;
                    SetSelectedCounter(selectedCounter);
                }
            }
            else
            {
                SetSelectedCounter(null);
            }
        }
        else
        {
            SetSelectedCounter(null);
        }
        //Debug.Log(selectedCounter);
    }


    private void HandleMovement()
    {
        Vector2 inputVector = gameInput.GetMovementVectorNormalised();

        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);

        float moveDistance = moveSpeed * Time.deltaTime;
        float playerRadius = .7f;
        float playerHeight = 2f;
        bool canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDir, moveDistance);

        if (!canMove)
        {
            // can't move towards moveDir
            //Attempt only Z movement
            Vector3 moveDirX = new Vector3(moveDir.x, 0, 0).normalized;

            canMove = moveDir.x !=0 &&!Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirX, moveDistance);

            if (canMove)
            {
                //can move only on the x
                moveDir = moveDirX;
            }

            else
            {
                //cannot move only on the x
                //Attempt only z motion
                Vector3 moveDirZ = new Vector3(0, 0, moveDir.z).normalized;

                canMove = moveDir.z!=0 && !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirZ, moveDistance);

                if (canMove)
                {
                    moveDir = moveDirZ;
                }
                else
                {
                    //can't move in any direction
                }
            }

        }
        if (canMove)
        {
            transform.position += moveDir * moveDistance;
        }


        isWalking = moveDir != Vector3.zero;



        float rotateSpeed = 10f;
        transform.forward = Vector3.Slerp(transform.forward, moveDir, Time.deltaTime * rotateSpeed);
        //Debug.Log(inputVector);
    }

    private void SetSelectedCounter(BaseCounter selectedCounter)
    {
        this.selectedCounter = selectedCounter;

        OnSelectedCounterChanged?.Invoke(this, new OnSelectedCounterChangedEventArgs
        {
            selectedCounter = selectedCounter
        });
    }

    public Transform GetKitchenObjectFollowTransform()
    {
        return KitchenObjectHoldPoint;
    }

    public void SetkitchenObject(KitchenObject kitchenObject)
    {
        this.kitchenObject = kitchenObject;
        if(kitchenObject != null)
        {
            OnPickedUpSomething?.Invoke(this, EventArgs.Empty);
        }
    }

    public KitchenObject GetKitchenObject()
    {
        return kitchenObject;
    }

    public void ClearKitchenObject()
    {
        kitchenObject = null;
    }

    public bool HasKitchenObject()
    {
        return kitchenObject != null;
    }
}
