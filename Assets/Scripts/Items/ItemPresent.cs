using UnityEngine;

public class ItemPresent : MonoBehaviour
{
    [SerializeField] private bool _canBePickedUp = false;

    private void Awake()
    {
        InputManager.OnInteractionStarted += TryPickUpPresent;
    }

    private void OnDestroy()
    {
        InputManager.OnInteractionStarted -= TryPickUpPresent;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("OnTriggerEnter");
        if (other.CompareTag("Player"))
        {
            Debug.Log("PLAYER");
            _canBePickedUp = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("OnTriggerExit");
        if (other.CompareTag("Player"))
        {
            Debug.Log($"PLAYER");
            _canBePickedUp = false;
        }
    }

    private void TryPickUpPresent()
    {
        if (!_canBePickedUp || GlobalConsts.playerInventory.IsInvetoryAtFullCapacity())
        {
            return;
        }

        GlobalConsts.playerInventory.AddObjectToInventory(gameObject);
        gameObject.SetActive(false);
    }
}
