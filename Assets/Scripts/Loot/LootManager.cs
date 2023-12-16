using UnityEngine;
using UnityEngine.Events;

public class LootManager : MonoBehaviour
{
    public static event UnityAction OnPresentDroppedInLoot;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            bool isInventoryFull = GlobalConsts.playerInventory.IsInvetoryAtFullCapacity();
            if (isInventoryFull)
            {
                GlobalConsts.playerInventory.RemoveObjectFromInventory();
                GameAudioManager.Instance.PlaySFX("Present Drop");
                OnPresentDroppedInLoot?.Invoke();
            }
        }
    }
}
