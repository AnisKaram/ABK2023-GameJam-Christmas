using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponScriptableObject", menuName = "ScriptableObjects/Weapon")]
public class WeaponScriptableObject : ScriptableObject
{
    public GameObject model;

    public int damage;
    public int ammoCapacity;
    public int magSize;
    public int fireRate;

    public float range;
    public float reloadTime;
    public float bloom;
    public float recoil;
    public float kickback;

}
