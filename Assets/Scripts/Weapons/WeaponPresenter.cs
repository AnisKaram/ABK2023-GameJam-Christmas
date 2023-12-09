using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WeaponPresenter : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _magSizeText;
    [SerializeField] private TextMeshProUGUI _ammoCapacityText;

    public void UpdateAmmoUI(float magSize, float ammoCapacity)
    {
        _magSizeText.text = $"{magSize}";
        _ammoCapacityText.text = $"{ammoCapacity}";
    }
}