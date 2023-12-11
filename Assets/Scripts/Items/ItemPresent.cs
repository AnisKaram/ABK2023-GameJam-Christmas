using UnityEngine;

public class ItemPresent : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            // TODO: if player inventory is empty, add present to player inventory and destroy gameobject
            // TODO: if player inventory is full, do nothing
        }
    }
}
