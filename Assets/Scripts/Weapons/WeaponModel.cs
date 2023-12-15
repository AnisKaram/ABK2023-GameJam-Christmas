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
    private List<int> _listOfWeaponsMagSize;
    private List<int> _listOfWeaponsAmmoCapacity;
    private List<Animator> _listOfWeaponsAnimator;
    private List<GameObject> _listOfWeaponInstances; // Holds all the spawned weapons in the loadout
    private List<ParticleSystem> _listOfMuzzleFlashes; // Holds all the muzzle paricles for every weapon
    private List<WaitForSeconds> _listOfReloadTimes;

    [Header("Transforms")]
    [SerializeField] private Transform _weaponPlaceholder;

    [Header("LayerMasks")]
    [SerializeField] private LayerMask _hitLayer;

    private int _currentWeaponIndex;
    private GameObject _currentWeaponModel;

    private float _nextTimeToFire = 0f;

    public static FireMode FireMode;
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

    private void Start()
    {
        UpdateAmmoOnUI();
    }

    private void Update()
    {
        _currentWeaponModel.transform.localPosition = Vector3.Lerp(_currentWeaponModel.transform.localPosition, Vector3.zero, Time.deltaTime * 4f);
        _currentWeaponModel.transform.localRotation = Quaternion.Lerp(_currentWeaponModel.transform.localRotation, Quaternion.identity, Time.deltaTime * 10f);
    }

    private void OnDestroy()
    {
        InputManager.OnFireTriggered -= Shoot;
        InputManager.OnReloadTriggered -= Reload;
        InputManager.OnSwitchTriggered -= SwitchWeapon;
    }
    #endregion

    #region Private Methods
    /// <summary>
    /// This method is used to spawn all weapons available in the loadout
    /// scriptable object and add in a dedicated list both the magSize and
    /// the ammoCapacity for every weapon instance.
    /// </summary>
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

    /// <summary>
    /// This method is used to fetch every muzzle flash in every weapon
    /// and add it to the list of ParticleSystem.
    /// </summary>
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

    /// <summary>
    /// This method is used to add a new WaitForSeconds for every
    /// weapon based on the reloadTime to every weapon.
    /// </summary>
    private void SetReloadTimes()
    {
        _listOfReloadTimes = new List<WaitForSeconds>();
        for (int weapon = 0; weapon < _loadout.Length; weapon++)
        {
            WaitForSeconds reloadTime = new WaitForSeconds(_loadout[weapon].reloadTime);
            _listOfReloadTimes.Add(reloadTime);
        }
    }

    /// <summary>
    /// This method is used to fetch and add every animator attached
    /// in every weapon in the list of animators.
    /// </summary>
    private void FetchAllWeaponsAnimator()
    {
        _listOfWeaponsAnimator = new List<Animator>();
        for (int weapon = 0; weapon < _listOfWeaponInstances.Count; weapon++)
        {
            Transform weaponInstance = _listOfWeaponInstances[weapon].transform;
            for (int currentWeaponChild = 0; currentWeaponChild < weaponInstance.childCount; currentWeaponChild++)
            {
                if (weaponInstance.GetChild(currentWeaponChild).CompareTag("WeaponModel"))
                {
                    _listOfWeaponsAnimator.Add(_listOfWeaponInstances[weapon].transform.GetChild(currentWeaponChild).GetComponent<Animator>());
                }
            }
        }
    }

    private void Shoot()
    {
        CheckIfWeaponIsOutOfAmmo();

        WeaponState weaponState = _loadout[_currentWeaponIndex].weaponState;

        if (weaponState == WeaponState.Shooting || weaponState == WeaponState.Idle)
        {
            ShootSingleOrAutoShot();
        }
    }

    private void ShootSingleOrAutoShot()
    {
        FireMode fireMode = _loadout[_currentWeaponIndex].fireMode;

        // Auto FireMode
        if (fireMode == FireMode.Auto)
        {
            if (Time.time >= _nextTimeToFire)
            {
                _nextTimeToFire = Time.time + 1f / 15f;

                ChangeEquippedWeaponState(WeaponState.Shooting);

                PlayMuzzleFlashOnWeapon();
                DecrementAmmo();

                _currentWeaponModel.transform.Rotate(_loadout[_currentWeaponIndex].recoil, 0, 0);
                _currentWeaponModel.transform.position += _currentWeaponModel.transform.forward * _loadout[_currentWeaponIndex].kickback;

                RaycastHit autoHit;
                if (Physics.Raycast(_playerCamera.transform.position, _playerCamera.transform.forward, out autoHit, _loadout[_currentWeaponIndex].range, _hitLayer))
                {
                    DealDamageToEnemy(hit: autoHit);
                }
            }
            return;
        }

        // Single FireMode
        if (fireMode == FireMode.Single)
        {
            ChangeEquippedWeaponState(WeaponState.Shooting);

            PlayMuzzleFlashOnWeapon();
            DecrementAmmo();

            _currentWeaponModel.transform.Rotate(_loadout[_currentWeaponIndex].recoil, 0, 0);
            _currentWeaponModel.transform.position += _currentWeaponModel.transform.forward * _loadout[_currentWeaponIndex].kickback;

            RaycastHit singleHit;
            if (Physics.Raycast(_playerCamera.transform.position, _playerCamera.transform.forward, out singleHit, _loadout[_currentWeaponIndex].range, _hitLayer))
            {
                DealDamageToEnemy(hit: singleHit);
            }
        }
    }

    private void DealDamageToEnemy(RaycastHit hit)
    {
        EnemyParent enemy = hit.transform.GetComponent<EnemyParent>();
        int weaponDamage = _loadout[_currentWeaponIndex].damage;
        enemy.TakeDamage(weaponDamage);
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
            Reload();
            return;
        }
    }

    private IEnumerator WaitForReload()
    {
        yield return _listOfReloadTimes[_currentWeaponIndex];
        UpdateAmmoOnUI();
        ChangeEquippedWeaponState(WeaponState.Idle);
        EnableDisableReloadAnimation(isReloading: false);
    }

    private void Reload()
    {
        WeaponState weaponState = _loadout[_currentWeaponIndex].weaponState;
        if (weaponState != WeaponState.Reloading)
        {
            int defaultMagSize = _loadout[_currentWeaponIndex].magSize;

            int ammoCapacity = _listOfWeaponsAmmoCapacity[_currentWeaponIndex];
            int magSize = _listOfWeaponsMagSize[_currentWeaponIndex];

            // Weapon can't be reloaded.
            if (magSize == defaultMagSize || ammoCapacity == 0)
            {
                return;
            }

            // Start reloading
            ChangeEquippedWeaponState(WeaponState.Reloading);
            StartCoroutine(WaitForReload());
            EnableDisableReloadAnimation(isReloading: true);

            if (magSize == 0)
            {
                if (ammoCapacity >= defaultMagSize) // We still have 30 or more in ammo capacity
                {
                    _listOfWeaponsMagSize[_currentWeaponIndex] += defaultMagSize; // We add 30 to the mag size
                    _listOfWeaponsAmmoCapacity[_currentWeaponIndex] -= defaultMagSize; // We subtract 30 from the ammo capacity
                }
                else // We have less than 30 in ammo capacity
                {
                    _listOfWeaponsMagSize[_currentWeaponIndex] += ammoCapacity; // We add the ammo capacity
                    _listOfWeaponsAmmoCapacity[_currentWeaponIndex] = 0; // We set the ammo capacity to zero
                }
                return;
            }

            if (magSize > 0)
            {
                int shotAmmo = defaultMagSize - magSize;

                // Case where the shot ammo is less than the ammo capacity
                if (ammoCapacity >= shotAmmo)
                {
                    _listOfWeaponsAmmoCapacity[_currentWeaponIndex] -= shotAmmo;
                    _listOfWeaponsMagSize[_currentWeaponIndex] += shotAmmo;
                }

                // Case where the shot ammo is greater than the ammo capacity
                if (shotAmmo >= ammoCapacity && ammoCapacity > 0)
                {
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
            _loadout[_currentWeaponIndex].weaponState = WeaponState.OutOfAmmo;
        }
    }

    public bool CheckIfAmmoIsFull()
    {
        bool isAmmoFull = true;

        if (_listOfWeaponsAmmoCapacity[0] != GlobalConsts.assaultRifle.ammoCapacity)
        {
            isAmmoFull = false;
        }

        if (_listOfWeaponsAmmoCapacity[1] != GlobalConsts.pistol.ammoCapacity)
        {
            isAmmoFull = false;
        }

        return isAmmoFull;
    }

    public void RefillAmmo()
    {
        // Looping through all the weapons available for the player
        for (int weapon = 0; weapon < _listOfWeaponInstances.Count; weapon++)
        {
            // .. if a weapon is OutOfAmmo, we refill the ammoCapacity and magSize
            if (_loadout[weapon].weaponState == WeaponState.OutOfAmmo)
            {
                int ammoToRefill = _loadout[weapon].ammoCapacity + _loadout[weapon].magSize;
                _listOfWeaponsAmmoCapacity[weapon] = ammoToRefill;
            }
            // .. if not, we refill the ammoCapacity only
            else
            {
                int ammoToRefill = _loadout[weapon].ammoCapacity;
                _listOfWeaponsAmmoCapacity[weapon] = ammoToRefill;
            }
        }

        // .. update the UI for the current enabled weapon.
        int magSize = _listOfWeaponsMagSize[_currentWeaponIndex];
        int ammoCapacity = _listOfWeaponsAmmoCapacity[_currentWeaponIndex];
        _weaponPresenter.UpdateAmmoUI(magSize, ammoCapacity);
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
            FireMode = _loadout[_currentWeaponIndex].fireMode;
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

    private void EnableDisableReloadAnimation(bool isReloading)
    {
        _listOfWeaponsAnimator[_currentWeaponIndex].SetBool("IsReloading", isReloading);
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

public enum FireMode
{
    Auto,
    Single
}