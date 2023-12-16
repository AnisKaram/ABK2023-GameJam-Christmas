using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    #region Fields
    [Header("GameObjects")]
    [SerializeField] private GameObject _presentInInventory;
    [SerializeField] private GameObject _giftImage;
    [SerializeField] private GameObject _noGiftImage;
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
        UpdateUI(isRemoved: false);
        _presentInInventory = grabbedObject;
    }

    public void RemoveObjectFromInventory()
    {
        UpdateUI(isRemoved: true);
        GameObject objectInInventory = _presentInInventory;
        Destroy(objectInInventory);
        _presentInInventory = null;
    }

    public bool IsInvetoryAtFullCapacity()
    {
        return _presentInInventory != null ? true : false;
    }
    #endregion

    #region Private Methods
    private void UpdateUI(bool isRemoved)
    {
        if (isRemoved)
        {
            _noGiftImage.SetActive(true);
            _giftImage.SetActive(false);
            return;
        }
        _noGiftImage.SetActive(false);
        _giftImage.SetActive(true);
    }
    #endregion
}