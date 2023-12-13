using System.Collections;
using UnityEngine;

public abstract class ItemBase : MonoBehaviour
{
    [SerializeField]
    private float _respawnDelay = 15f;

    [SerializeField]
    private Collider _itemCollider;

    [SerializeField]
    private GameObject _itemVisuals, _itemCooldownBar;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (PickUpBehavior() == true)
            {
                StartCoroutine(PickUpAndRespawnItem());
            }
        }
    }

    /// <summary>
    /// What to do when the player runs over the item
    /// </summary>
    /// <returns>A bool stating whether the item was picked up</returns>
    protected abstract bool PickUpBehavior();

    private IEnumerator PickUpAndRespawnItem()
    {
        _itemCollider.enabled = false;
        _itemVisuals.SetActive(false);
        _itemCooldownBar.SetActive(true);

        yield return new WaitForSeconds(_respawnDelay);

        _itemCooldownBar.SetActive(false);
        _itemVisuals.SetActive(true);
        _itemCollider.enabled = true;
    }

    public float GetRespawnDelay()
    {
        return _respawnDelay;
    }
}
