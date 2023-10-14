using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class PlateKitchenObject : KitchenObject
{
    public event EventHandler<OnInredientAddedEventArgs> OnIngredientAdded;
    public class OnInredientAddedEventArgs : EventArgs
    {
        public KitchenObjectSO KitchenObjectSO;
    }

    [SerializeField] private List<KitchenObjectSO> validKitchenObjectSOList;
    private List<KitchenObjectSO> kitchenObjectSOList;

    private void Awake()
    {
        kitchenObjectSOList = new List<KitchenObjectSO>();
    }

    public bool  TryAddIngredient(KitchenObjectSO kitchenObjectSO)
    {
        if (!validKitchenObjectSOList.Contains(kitchenObjectSO))
        {
            //Not a valid ingredient 
            return false;
        }
        if (kitchenObjectSOList.Contains(kitchenObjectSO))
        {
            //Already has that type
            return false;
        }
        else
        {
            kitchenObjectSOList.Add(kitchenObjectSO);
            
            OnIngredientAdded?.Invoke(this, new OnInredientAddedEventArgs
            {
                KitchenObjectSO= kitchenObjectSO,
            });
            return true;
        }
       
    }

    public List<KitchenObjectSO> GetKitchenObjectSOList()
    {
        return kitchenObjectSOList;
    }
}
