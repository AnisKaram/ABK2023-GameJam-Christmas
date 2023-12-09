using UnityEngine;
using TMPro;

public class WeaponPresenter : MonoBehaviour
{
    #region Fields
    [Header("Texts - TextMeshPro")]
    [SerializeField] private TextMeshProUGUI _magSizeText;
    [SerializeField] private TextMeshProUGUI _ammoCapacityText;
    #endregion

    #region Public Methods
    public void UpdateAmmoUI(float magSize, float ammoCapacity)
    {
        _magSizeText.text = $"{magSize}";
        _ammoCapacityText.text = $"{ammoCapacity}";
    }
    #endregion
}