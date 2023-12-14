using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootManager : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            bool isInventoryFull = GlobalConsts.playerInventory.IsInvetoryAtFullCapacity();
            if (isInventoryFull)
            {
                GlobalConsts.playerInventory.RemoveObjectFromInventory();
                // TODO Update objective list.
            }
        }
    }
}
