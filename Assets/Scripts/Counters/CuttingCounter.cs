using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuttingCounter : BaseCounter, IHasProgress
{
    public static event EventHandler OnAnyCut;

    public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;

    public event EventHandler OnCut;

    new public static void ResetStaticData()
    {
        OnAnyCut= null;
    }

    [SerializeField] private CuttingRecipeSO[] cuttingRecipeSOArray;

    private int cuttingProgress; 

    public override void Interact(Player player)
    {
        if (!HasKitchenObject())
        {
            // has no kitchenobject
            if (player.HasKitchenObject())
            {
                // player carrying something
                if(HasRecipeWithInput(player.GetKitchenObject().GetkitchenObjectSO()))
                {
                    //player carrying something that can be cut
                    cuttingProgress= 0;
                    CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(player.GetKitchenObject().GetkitchenObjectSO());
                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                    {
                        progressNormalised = (float)cuttingProgress/cuttingRecipeSO.cuttingProgressMax
                    });

                    player.GetKitchenObject().SetKitchenObjectParent(this);
                }
               
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
            }
            else
            {
                //Player is carrying nothing
                GetKitchenObject().SetKitchenObjectParent(player);
            }
        }
    }

    public override void InteractAlternate(Player player)
    {
        if (HasKitchenObject() && HasRecipeWithInput(GetKitchenObject().GetkitchenObjectSO()))
        {
            //There is a kitchenObject here AND that can be cut
            cuttingProgress++;
            OnCut?.Invoke(this, EventArgs.Empty);
            OnAnyCut?.Invoke(this, EventArgs.Empty);    

            CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(GetKitchenObject().GetkitchenObjectSO());

            OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
            {
                progressNormalised = (float)cuttingProgress / cuttingRecipeSO.cuttingProgressMax
            });

            if (cuttingProgress >= cuttingRecipeSO.cuttingProgressMax ) 
            {
                KitchenObjectSO ouptputKitchenObjectSO = GetOutPutFromInput(GetKitchenObject().GetkitchenObjectSO());

                GetKitchenObject().DestroySelf();
                KitchenObject.SpawnKitchenObject(ouptputKitchenObjectSO, this);
            }
        }
            
    }

    private bool HasRecipeWithInput(KitchenObjectSO inputKitchenObjectSO)
    {
        CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(inputKitchenObjectSO);
        return cuttingRecipeSO != null;
    }

    private KitchenObjectSO GetOutPutFromInput(KitchenObjectSO inputKitchenObjectSO)
    {
        CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(inputKitchenObjectSO);
        if (cuttingRecipeSO != null)
        {
            return cuttingRecipeSO.output;
        }  
        else
        {
            return null;
        }   
    }

    private CuttingRecipeSO GetCuttingRecipeSOWithInput(KitchenObjectSO inputKitchenObjectSO)
    {
        foreach (CuttingRecipeSO cuttingRecipeSO in cuttingRecipeSOArray)
        {
            if (cuttingRecipeSO.input == inputKitchenObjectSO)
            {
                return cuttingRecipeSO;
            }
        }
        return null;
    }
}
