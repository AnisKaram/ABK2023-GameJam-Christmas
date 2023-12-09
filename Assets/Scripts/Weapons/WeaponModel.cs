using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponModel : MonoBehaviour
{
    #region Fields
    [Header("Scripts")]
    [SerializeField] private WeaponPresenter _weaponPresenter;

    [Header("Camera")]
    [SerializeField] private Camera _playerCamera;

    [Header("Lists")]
    [SerializeField] private WeaponScriptableObject[] _loadout;
    [SerializeField] private List<int> _listOfWeaponsMagSize;
    [SerializeField] private List<int> _listOfWeaponsAmmoCapacity;
    [SerializeField] private List<Animator> _listOfWeaponsAnimator;

    [Header("Transforms")]
    [SerializeField] private Transform _weaponPlaceholder;

    [Header("LayerMasks")]
    [SerializeField] private LayerMask _hitLayer;

    private List<GameObject> _listOfWeaponInstances; // Holds all the spawned weapons in the loadout
    private List<ParticleSystem> _listOfMuzzleFlashes; // Holds all the muzzle paricles for every weapon

    private int _currentWeaponIndex;
    private GameObject _currentWeaponModel;

    [SerializeField] private List<WaitForSeconds> _listOfReloadTimes;
    #endregion

    #region Unity Methods
    private void Awake()
    {
        InputManager.OnFireTriggered += Shoot;
        InputManager.OnReloadTriggered += Reload;
        InputManager.OnSwitchTriggered += SwitchWeapon;

        SpawnAllWeaponsInLoadout();
        FetchAllMuzzleFlashesInWeapons();
        SetReloadTimes();
        FetchAllWeaponsAnimator();

        _currentWeaponIndex = 0;
        _currentWeaponModel = _listOfWeaponInstances[0];
        UnEquip(_currentWeaponIndex);
    }

    private void FetchAllWeaponsAnimator()
    {
        _listOfWeaponsAnimator = new List<Animator>();
        for (int weapon = 0; weapon < _listOfWeaponInstances.Count; weapon++)
        {
            Animator animator = _listOfWeaponInstances[weapon].GetComponent<Animator>();
            _listOfWeaponsAnimator.Add(animator);
        }
    }

    private void Start()
    {
        UpdateAmmoOnUI();
    }

    private void OnDestroy()
    {
        InputManager.OnFireTriggered -= Shoot;
        InputManager.OnReloadTriggered -= Reload;
        InputManager.OnSwitchTriggered -= SwitchWeapon;
    }

    private void Update()
    {
        _currentWeaponModel.transform.localPosition = Vector3.Lerp(_currentWeaponModel.transform.localPosition, Vector3.zero, Time.deltaTime * 4f);
        _currentWeaponModel.transform.localRotation = Quaternion.Lerp(_currentWeaponModel.transform.localRotation, Quaternion.identity, Time.deltaTime * 10f);
    }
    #endregion

    #region Private Methods
    private void SpawnAllWeaponsInLoadout()
    {
        _listOfWeaponInstances = new List<GameObject>();
        _listOfWeaponsMagSize = new List<int>();
        _listOfWeaponsAmmoCapacity = new List<int>();

        for (int weapon = 0; weapon < _loadout.Length; weapon++)
        {
            GameObject weaponInstance = Instantiate(_loadout[weapon].model, _weaponPlaceholder.position, _weaponPlaceholder.rotation, _weaponPlaceholder);
            weaponInstance.transform.localPosition = Vector3.zero;
            weaponInstance.transform.localEulerAngles = Vector3.zero;

            _listOfWeaponInstances.Add(weaponInstance);
            _listOfWeaponsMagSize.Add(_loadout[weapon].magSize);
            _listOfWeaponsAmmoCapacity.Add(_loadout[weapon].ammoCapacity);

            WeaponScriptableObject weaponSO = _loadout[weapon];
            weaponSO.weaponState = WeaponState.Idle;
        }
    }

    private void FetchAllMuzzleFlashesInWeapons()
    {
        _listOfMuzzleFlashes = new List<ParticleSystem>();

        for (int weapon = 0; weapon < _listOfWeaponInstances.Count; weapon++)
        {
            Transform weaponInstance = _listOfWeaponInstances[weapon].transform;
            for (int currentWeaponChild = 0; currentWeaponChild < weaponInstance.childCount; currentWeaponChild++)
            {
                if (weaponInstance.GetChild(currentWeaponChild).CompareTag("Muzzle"))
                {
                    _listOfMuzzleFlashes.Add(weaponInstance.GetChild(currentWeaponChild).GetComponent<ParticleSystem>());
                }
            }
        }
    }

    private void SetReloadTimes()
    {
        _listOfReloadTimes = new List<WaitForSeconds>();
        for (int weapon = 0; weapon < _loadout.Length; weapon++)
        {
            WaitForSeconds reloadTime = new WaitForSeconds(_loadout[weapon].reloadTime);
            _listOfReloadTimes.Add(reloadTime);
        }
    }

    private void Shoot()
    {
        CheckIfWeaponIsOutOfAmmo();

        WeaponState weaponState = _loadout[_currentWeaponIndex].weaponState;

        if (weaponState == WeaponState.Shooting || weaponState == WeaponState.Idle)
        {
            ChangeEquippedWeaponState(WeaponState.Shooting);

            PlayMuzzleFlashOnWeapon();
            DecrementAmmo();

            _currentWeaponModel.transform.Rotate(_loadout[_currentWeaponIndex].recoil, 0, 0);
            _currentWeaponModel.transform.position += _currentWeaponModel.transform.forward * _loadout[_currentWeaponIndex].kickback;

            RaycastHit hit;
            if (Physics.Raycast(_playerCamera.transform.position, _playerCamera.transform.forward, out hit, _loadout[_currentWeaponIndex].range, _hitLayer))
            {
                // TODO: Deal damage
                //Enemy enemy = hit.transform.GetComponent<Enemy>();
                //enemy.TakeDamage();
                Debug.Log("Enemy hit!");
            }
        }
    }

    private void UpdateAmmoOnUI()
    {
        int magSize = _listOfWeaponsMagSize[_currentWeaponIndex];
        int ammoCapacity = _listOfWeaponsAmmoCapacity[_currentWeaponIndex];

        _weaponPresenter.UpdateAmmoUI(magSize, ammoCapacity);
    }

    private void DecrementAmmo()
    {
        _listOfWeaponsMagSize[_currentWeaponIndex] -= 1;
        UpdateAmmoOnUI();

        int currentMagSize = _listOfWeaponsMagSize[_currentWeaponIndex];
        if (currentMagSize < 1)
        {
            Debug.Log($"MAG SIZE < 1");
            Reload();
            return;
        }
    }

    private IEnumerator WaitForReload()
    {
        Debug.Log($"Waiting....");
        yield return _listOfReloadTimes[_currentWeaponIndex];
        UpdateAmmoOnUI();
        ChangeEquippedWeaponState(WeaponState.Idle);
        Debug.Log($"Reload done");
    }

    // TODO Apply reloading animation
    private void Reload()
    {
        WeaponState weaponState = _loadout[_currentWeaponIndex].weaponState;
        if (weaponState != WeaponState.Reloading)
        {
            int defaultMagSize = _loadout[_currentWeaponIndex].magSize;

            int ammoCapacity = _listOfWeaponsAmmoCapacity[_currentWeaponIndex];
            int magSize = _listOfWeaponsMagSize[_currentWeaponIndex];

            // 1. We have to check if the magSize is equals to the default magSize
            // 2. We have to check if the magSize is equals to 0.
            // We have to check if the magSize is greater than 0.

            // Weapon can't be reloaded.
            if (magSize == defaultMagSize || ammoCapacity == 0)
            {
                Debug.Log($"No need to reload");
                return;
            }

            // Start of reloading
            ChangeEquippedWeaponState(WeaponState.Reloading);
            StartCoroutine(WaitForReload());

            if (magSize == 0)
            {
                Debug.Log($"Mag size is ZERO");
                if (ammoCapacity >= defaultMagSize) // We still have 30 or more in ammo capacity
                {
                    Debug.Log($"First if");
                    _listOfWeaponsMagSize[_currentWeaponIndex] += defaultMagSize; // We add 30 to the mag size
                    _listOfWeaponsAmmoCapacity[_currentWeaponIndex] -= defaultMagSize; // We subtract 30 from the ammo capacity
                }
                else // We have less than 30 in ammo capacity
                {
                    Debug.Log($"Second if");
                    _listOfWeaponsMagSize[_currentWeaponIndex] += ammoCapacity; // We add the ammo capacity
                    _listOfWeaponsAmmoCapacity[_currentWeaponIndex] = 0; // We set the ammo capacity to zero
                }
                return;
            }

            if (magSize > 0)
            {
                int shotAmmo = defaultMagSize - magSize;
                Debug.Log($"Mag size is greater than ZERO, shotAmmo: {shotAmmo}");

                if (ammoCapacity >= shotAmmo)
                {
                    Debug.Log($"FIRST IF, Shot ammo {shotAmmo}, adding to magSize {shotAmmo}, substracting {shotAmmo}");
                    _listOfWeaponsAmmoCapacity[_currentWeaponIndex] -= shotAmmo;
                    _listOfWeaponsMagSize[_currentWeaponIndex] += shotAmmo;
                }

                // Case where the shot ammo is greater than the ammo capacity
                if (shotAmmo >= ammoCapacity && ammoCapacity > 0)
                {
                    Debug.Log($"SECOND IF, Shot ammo {shotAmmo}, adding to mahSize {ammoCapacity}, substracting {ammoCapacity}");
                    _listOfWeaponsAmmoCapacity[_currentWeaponIndex] -= ammoCapacity;
                    _listOfWeaponsMagSize[_currentWeaponIndex] += ammoCapacity;
                }
            }
        }
    }

    private void CheckIfWeaponIsOutOfAmmo()
    {
        if (_listOfWeaponsAmmoCapacity[_currentWeaponIndex] < 1 && _listOfWeaponsMagSize[_currentWeaponIndex] < 1)
        {
            Debug.Log($"Weapon out of ammo");
            _loadout[_currentWeaponIndex].weaponState = WeaponState.OutOfAmmo;
        }
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
        UpdateAmmoOnUI();
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

    private void PlayMuzzleFlashOnWeapon()
    {
        _listOfMuzzleFlashes[_currentWeaponIndex].Play();
    }
    #endregion
}


public enum WeaponState
{
    Idle,
    Shooting,
    Reloading,
    OutOfAmmo
}