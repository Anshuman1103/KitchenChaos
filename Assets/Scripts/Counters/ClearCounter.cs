using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ClearCounter : BaseCounter
{
    [SerializeField] private KitchenObjectSO kitchenObjectSO;

    public override void Interact(Player player)
    {
        if (!HasKitchenObject())
        {
            // has no kitchenobject
            if (player.HasKitchenObject())
            {
                // player carrying something
                player.GetKitchenObject().SetKitchenObjectParent(this);
            }
            else
            {
                // Player is not carrying anything
            }
        }
        else
        {
            //There is A kitchenObject here
            if (player.HasKitchenObject())
            { 
                //Player is carrying something
                if (player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject))
                {
                    //player is holding a plate
                    if (plateKitchenObject.TryAddIngredient(GetKitchenObject().GetkitchenObjectSO()))
                    {
                        GetKitchenObject().DestroySelf();
                    }
                }
                else
                {
                    //Player is not carrying a plate but something else
                    if (GetKitchenObject().TryGetPlate(out plateKitchenObject))
                    {
                        //plateKitchenObject = Plate
                        //Counter is holding a plate
                        if (plateKitchenObject.TryAddIngredient(player.GetKitchenObject().GetkitchenObjectSO()))
                        {
                            player.GetKitchenObject().DestroySelf();
                        }
                    }
                }   
                
            }
            else
            {
                //Player is carrying nothing
                GetKitchenObject().SetKitchenObjectParent(player);
            }
        }
    }
}
