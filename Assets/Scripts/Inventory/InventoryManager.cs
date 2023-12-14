using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    #region Fields
    [SerializeField] private GameObject _presentInInventory;

    //private const float _maxItemsToHold = 1; // Player can only hold 1 present at a time
    #endregion

    #region Unity Methods
    private void Awake()
    {
        _presentInInventory = null;
    }
    #endregion

    #region Public Methods
    public void AddObjectToInventory(GameObject grabbedObject)
    {
        _presentInInventory = grabbedObject;
    }

    public void RemoveObjectFromInventory()
    {
        GameObject objectInInventory = _presentInInventory;
        Destroy(objectInInventory);
        _presentInInventory = null;
    }

    public bool IsInvetoryAtFullCapacity()
    {
        //return _presentInInventory.Count >= _maxItemsToHold ? true : false;
        return _presentInInventory != null ? true : false;
    }
    #endregion
}