using UnityEngine;

public abstract class ItemBase : MonoBehaviour
{
    [SerializeField]
    private Collider itemCollider;

    private void OnTriggerEnter(Collider other)
    {
        // TODO: access player through global variable instead of below method

        if (other.gameObject.tag == "Player")
        {
            PickUpBehavior();

            Destroy(gameObject);
        }
    }

    protected abstract void PickUpBehavior();
}
