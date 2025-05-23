using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatesCounter : BaseCounter
{
    public event EventHandler OnPlateSpawned;
    public event EventHandler OnPlateRemoved;


    [SerializeField] private KitchenObjectSO platekitchenObjectSO;

    private float spawnPlateTimer;
    private float spawnPlateTimerMax = 4f;
    private int plateSpawnedAmount;
    private int plateSpawnedAmountMax = 4;

    private void Update()
    {
        spawnPlateTimer += Time.deltaTime;
        if(spawnPlateTimer > spawnPlateTimerMax)
        {
            spawnPlateTimer= 0f;
            //KitchenObject.SpawnKitchenObject(platekitchenObjectSO, this);
            if(KitchenGameManager.instance.IsGamePlaying() && plateSpawnedAmount< plateSpawnedAmountMax )
            {
                plateSpawnedAmount++;
                OnPlateSpawned?.Invoke(this, EventArgs.Empty);
            }
        }

    }

    public override void Interact(Player player)
    {
        if (!player.HasKitchenObject())
        {
            //player is empty handed
            if (plateSpawnedAmount > 0)
            {
                //There at least one plate here
                plateSpawnedAmount--;

                KitchenObject.SpawnKitchenObject(platekitchenObjectSO, player);
                OnPlateRemoved?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}
