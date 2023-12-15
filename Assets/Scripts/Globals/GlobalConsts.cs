using UnityEngine;

public class GlobalConsts : MonoBehaviour
{
    public static CharacterHealth playerHealth;
    public static WeaponModel playerWeapons;
    public static InventoryManager playerInventory;

    public static WeaponScriptableObject assaultRifle;
    public static WeaponScriptableObject pistol;

    private void Start()
    {
        GameManager.Instance.StartGame();

        if (GameObject.FindWithTag("Player").TryGetComponent(out playerHealth) == false)
        {
            Debug.LogError("Could not find player health");
        }

        if (GameObject.FindWithTag("Player").TryGetComponent(out playerWeapons) == false)
        {
            Debug.LogError("Could not find player weapon script");
        }

        if (GameObject.FindWithTag("Player").TryGetComponent(out playerInventory) == false)
        {
            Debug.LogError("Could not find player inventory");
        }

        assaultRifle = Resources.Load<WeaponScriptableObject>("AssaultRifle");
        pistol = Resources.Load<WeaponScriptableObject>("Pistol");

        if (assaultRifle == null || pistol == null)
        {
            Debug.LogError("Could not find player weapon assets");
        }
    }
}
