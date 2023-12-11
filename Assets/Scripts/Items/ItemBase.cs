using System.Collections;
using UnityEngine;

public abstract class ItemBase : MonoBehaviour
{
    [SerializeField]
    private float respawnDelay = 15f;

    private void OnTriggerEnter(Collider other)
    {
        // TODO: access player through global variable instead of below method

        if (other.gameObject.tag == "Player")
        {
            PickUpBehavior();

            StartCoroutine(WaitToRespawnItem());
        }
    }

    protected abstract void PickUpBehavior();

    private IEnumerator WaitToRespawnItem()
    {
        gameObject.SetActive(false);

        yield return new WaitForSeconds(respawnDelay);

        gameObject.SetActive(true);
    }
}
