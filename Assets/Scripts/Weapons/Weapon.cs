using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField]
    private WeaponScriptableObject[] loadout;
    [SerializeField]
    private Transform weaponParent;

    private int currentWeaponIndex;
    private GameObject currentWeaponModel;

    /*
     * private int damage;
     * private int ammoCapacity;
     * private int magSize;
     * private int fireRate;
     * private float range;
     * private float reloadTime;
     */

    void Awake()
    {
        Equip(0);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) { Equip(0); }
        if (Input.GetKeyDown(KeyCode.Alpha2)) { Equip(1); }
        if (Input.GetMouseButtonDown(0)) { Shoot(); }

        currentWeaponModel.transform.localPosition = Vector3.Lerp(currentWeaponModel.transform.localPosition, Vector3.zero, Time.deltaTime * 4f);
        currentWeaponModel.transform.localRotation = Quaternion.Lerp(currentWeaponModel.transform.localRotation, Quaternion.identity, Time.deltaTime * 10f);
    }

    private void Shoot()
    {
        Transform spawnPoint = transform.Find("Main Camera");

        //TODO: Add bloom

        RaycastHit hit;
        if (Physics.Raycast(spawnPoint.position, spawnPoint.forward, out hit, loadout[currentWeaponIndex].range))
        {
            Debug.Log("Has hit something!");
            // TODO: Deal damage
        }

        currentWeaponModel.transform.Rotate(loadout[currentWeaponIndex].recoil, 0, 0);
        currentWeaponModel.transform.position += currentWeaponModel.transform.forward * loadout[currentWeaponIndex].kickback;
    }

    private void Reload()
    {
        // Reload weapon
    }

    private void Equip(int index)
    {
        if(currentWeaponModel != null) { Destroy(currentWeaponModel); }

        currentWeaponIndex = index;

        GameObject newEquippedWeaponModel = Instantiate(loadout[index].model, weaponParent.position, weaponParent.rotation, weaponParent) as GameObject;
        newEquippedWeaponModel.transform.localPosition = Vector3.zero;
        newEquippedWeaponModel.transform.localEulerAngles = Vector3.zero;

        currentWeaponModel = newEquippedWeaponModel;
    }
}
