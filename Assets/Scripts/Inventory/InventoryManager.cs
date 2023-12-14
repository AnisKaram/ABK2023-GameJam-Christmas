using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    #region Fields
    [SerializeField] private List<GameObject> _listOfObjects;

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
        Debug.Log($"{grabbedObject.name} added.");
    }

    public void RemoveObjectFromInventory()
    {
        _listOfObjects.RemoveAt(0);
        Debug.Log($"removed.");
    }

    public bool IsInvetoryAtFullCapacity()
    {
        return _listOfObjects.Count >= _maxItemsToHold ? true : false;
    }
    #endregion
}