using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryManager : MonoBehaviour
{
    public event EventHandler OnrecipeSpawned;
    public event EventHandler OnrecipeCompleted;
    public event EventHandler OnrecipeFailed;
    public event EventHandler OnrecipeSuccess;

    public static DeliveryManager Instance { get; private set; }


    [SerializeField] private RecipeListSO recipeListSO;

    private List<RecipeSO> waitingRecipeSOList;
    private float spawnRecipeTimer;
    private float spawnRecipeTimerMax = 4f;
    private int waitingRecipeMax = 4;
    private int successfulRecipeAmount;

    private void Awake()
    {
        Instance = this;

        waitingRecipeSOList= new List<RecipeSO>();
    }

    private void Update()
    {
        spawnRecipeTimer -= Time.deltaTime;
        if(spawnRecipeTimer <= 0f) 
        {
            spawnRecipeTimer = spawnRecipeTimerMax;
            if(waitingRecipeSOList.Count < waitingRecipeMax )
            {
                RecipeSO waitingRecipeSO = recipeListSO.recipeSOList[UnityEngine.Random.Range(0,recipeListSO.recipeSOList.Count)];
                //Debug.Log(waitingRecipeSO.recipeName);
                waitingRecipeSOList.Add(waitingRecipeSO);

                OnrecipeSpawned?.Invoke(this, EventArgs.Empty);
            }
        }
       
    }
    public void DeliverRecipe(PlateKitchenObject plateKitchenObject)
    {
        for (int i = 0; i < waitingRecipeSOList.Count; i++)
        {
            RecipeSO waitingRecipeSO = waitingRecipeSOList[i];

            if(waitingRecipeSO.kitchenObjectSOList.Count == plateKitchenObject.GetKitchenObjectSOList().Count)
            {
                
                bool plateContentsMatchesRecipe = false;
                //has same number of ingredient
                foreach(KitchenObjectSO recipeKitchenObjectSO in waitingRecipeSO.kitchenObjectSOList)
                {
               
                    //cycling through all the ingredient in recipe
                    bool ingredientFound = false;
                    foreach (KitchenObjectSO plateKitchenObjectSO in plateKitchenObject.GetKitchenObjectSOList())
                    {
                        
                        //cycling through all the ingredient in plate
                        if (plateKitchenObjectSO == recipeKitchenObjectSO)
                        {
                            
                            //Ingredient matches!
                            ingredientFound =true; break;
                        }
                    }
                    if(ingredientFound)
                    {
                        //This recipe ingredient was not found in the plate 
                        plateContentsMatchesRecipe= true;
                    }
                }
                if (plateContentsMatchesRecipe)
                {
                    //player delivered the correct recipe
                    //Debug.Log("Player delivered the correct recipe!");

                    waitingRecipeSOList.RemoveAt(i);
                    successfulRecipeAmount++;

                    OnrecipeCompleted?.Invoke(this, EventArgs.Empty);
                    OnrecipeSuccess?.Invoke(this, EventArgs.Empty);

                    return;
                }
            }
        }
        //no matches found
        //player does not deliver the correct recipe
        //Debug.Log("player not delivered the correct recipe");
        OnrecipeFailed?.Invoke(this, EventArgs.Empty);
    }

    public List<RecipeSO> GetWaitingRecipeSOList()
    {
        return waitingRecipeSOList;
    }

    public int GetSuccessfulRecipeAmount()
    {
        return successfulRecipeAmount;
    }
   
}
