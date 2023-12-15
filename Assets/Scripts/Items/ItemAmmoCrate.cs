using UnityEngine;

public class ItemAmmoCrate : ItemBase
{
    protected override bool PickUpBehavior()
    {
        if (GlobalConsts.playerWeapons.CheckIfAmmoIsFull() == true)
        {
            return false;
        }

        GlobalConsts.playerWeapons.RefillAmmo();
        GameAudioManager.Instance.PlaySFX("Ammo Crates Pickup");

        return true;
    }
}
