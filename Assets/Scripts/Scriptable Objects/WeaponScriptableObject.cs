using UnityEngine;

[CreateAssetMenu(fileName = "WeaponScriptableObject", menuName = "ScriptableObjects/Weapon", order = 1)]
public class WeaponScriptableObject : ScriptableObject
{
    public GameObject model;
    public Transform bulletPlaceholder;

    private void OnEnable()
    {
        bulletPlaceholder = model.transform.GetChild(0);
    }

    public int damage;
    public int ammoCapacity;
    public int magSize;
    public float fireRate;

    public float range;
    public float reloadTime;
    public float bloom;
    public float recoil;
    public float kickback;

    public WeaponState weaponState;
    public FireMode fireMode;
}
