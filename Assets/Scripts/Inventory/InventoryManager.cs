using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    #region Fields
    private List<GameObject> _listOfObjects;

    private const float _maxItemsToHold = 1; // Player can only hold 1 present at a time
    #endregion

    #region Unity Methods
    private void Awake()
    {
        _listOfObjects = new List<GameObject>();
    }
    #endregion

    #region Public Methods
    public void AddObjectToInventory(GameObject grabbedObject)
    {
        _listOfObjects.Add(grabbedObject);
    }

    public void RemoveObjectFromInventory(GameObject thrownObject)
    {
        _listOfObjects.Remove(thrownObject);
    }

    public bool IsInvetoryAtFullCapacity()
    {
        return _listOfObjects.Count >= _maxItemsToHold ? true : false;
    }
    #endregion
}