using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    #region Fields
    [Header("Lists")]
    [SerializeField] private WeaponScriptableObject[] _loadout;

    [Header("Transforms")]
    [SerializeField] private Transform _weaponPlaceholder;

    private List<GameObject> _listOfWeaponInstances; // Holds all the spawned weapons in the loadout

    private int _currentWeaponIndex;
    private GameObject _currentWeaponModel;
    #endregion

    #region Unity Methods
    private void Awake()
    {
        InputManager.OnFireTriggered += Shoot;
        InputManager.OnReloadTriggered += Reload;
        InputManager.OnSwitchTriggered += SwitchWeapon;

        SpawnAllWeaponsInLoadout();

        _currentWeaponIndex = 0;
        _currentWeaponModel = _listOfWeaponInstances[0];
        UnEquip(_currentWeaponIndex);

    }

    private void OnDestroy()
    {
        InputManager.OnFireTriggered -= Shoot;
        InputManager.OnReloadTriggered -= Reload;
        InputManager.OnSwitchTriggered -= SwitchWeapon;
    }

    private void Update()
    {
        if (_loadout[_currentWeaponIndex].weaponState == WeaponState.Shooting)
        {
            _currentWeaponModel.transform.localPosition = Vector3.Lerp(_currentWeaponModel.transform.localPosition, Vector3.zero, Time.deltaTime * 4f);
            _currentWeaponModel.transform.localRotation = Quaternion.Lerp(_currentWeaponModel.transform.localRotation, Quaternion.identity, Time.deltaTime * 10f);
        }
    }
    #endregion

    #region Private Methods
    private void SpawnAllWeaponsInLoadout()
    {
        _listOfWeaponInstances = new List<GameObject>();

        for (int weapon = 0; weapon < _loadout.Length; weapon++)
        {
            GameObject weaponInstance = Instantiate(_loadout[weapon].model, _weaponPlaceholder.position, _weaponPlaceholder.rotation, _weaponPlaceholder);
            weaponInstance.transform.localPosition = Vector3.zero;
            weaponInstance.transform.localEulerAngles = Vector3.zero;

            _listOfWeaponInstances.Add(weaponInstance);

            WeaponScriptableObject weaponSO = _loadout[weapon];
            weaponSO.weaponState = WeaponState.Idle;
        }
    }

    private void Shoot()
    {
        //Transform spawnPoint = transform.Find("Main Camera");

        ////TODO: Add bloom

        //RaycastHit hit;
        //if (Physics.Raycast(spawnPoint.position, spawnPoint.forward, out hit, _loadout[currentWeaponIndex].range))
        //{
        //    Debug.Log("Has hit something!");
        //    // TODO: Deal damage
        //}
        Debug.Log("Shooting...");
        ChangeEquippedWeaponState(WeaponState.Shooting);
        _currentWeaponModel.transform.Rotate(_loadout[_currentWeaponIndex].recoil, 0, 0);
        _currentWeaponModel.transform.position += _currentWeaponModel.transform.forward * _loadout[_currentWeaponIndex].kickback;
    }

    private void Reload()
    {
        // Reload weapon
        ChangeEquippedWeaponState(WeaponState.Reloading);
    }

    private void Equip(int weaponIndexToEquip)
    {
        // Equip if not equipped yet.
        if (!_listOfWeaponInstances[weaponIndexToEquip].activeInHierarchy)
        {
            _currentWeaponIndex = weaponIndexToEquip;
            _currentWeaponModel = _listOfWeaponInstances[_currentWeaponIndex];
            _currentWeaponModel.SetActive(true);
            _currentWeaponModel.transform.localPosition = Vector3.zero;
            _currentWeaponModel.transform.localEulerAngles = Vector3.zero;
            ChangeEquippedWeaponState(WeaponState.Idle);
        }
    }

    private void SwitchWeapon()
    {
        // Incrementing the weapon index by 1 if less than the
        // count of the weapons spawned.
        if (_currentWeaponIndex < _listOfWeaponInstances.Count - 1)
        {
            _currentWeaponIndex += 1;
        }
        // Decrementing the weapon index by 1 if equals to the count
        // of the weapons spawned.
        else if (_currentWeaponIndex == _listOfWeaponInstances.Count - 1)
        {
            _currentWeaponIndex -= 1;
        }

        Equip(_currentWeaponIndex);
        UnEquip(_currentWeaponIndex);
    }

    private void UnEquip(int weaponIndexEquipped)
    {
        for (int weaponToUnequip = 0; weaponToUnequip < _listOfWeaponInstances.Count; weaponToUnequip++)
        {
            if (weaponIndexEquipped != weaponToUnequip)
            {
                _listOfWeaponInstances[weaponToUnequip].SetActive(false);
            }
        }
    }

    private void ChangeEquippedWeaponState(WeaponState state)
    {
        _loadout[_currentWeaponIndex].weaponState = state;
    }
    #endregion
}


public enum WeaponState
{
    Idle,
    Shooting,
    Reloading
}