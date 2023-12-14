using UnityEngine;

public class ItemPresent : MonoBehaviour
{
    private bool _canBePickedUp = false;

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
        if (other.gameObject.tag == "Player")
        {
            _canBePickedUp = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            _canBePickedUp = false;
        }
    }

    private void TryPickUpPresent()
    {
        if (_canBePickedUp == false || GlobalConsts.playerInventory.IsInvetoryAtFullCapacity() == true)
        {
            return;
        }

        GlobalConsts.playerInventory.AddObjectToInventory(gameObject);
        gameObject.SetActive(false);
    }
}
