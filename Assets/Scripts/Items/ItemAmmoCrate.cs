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

        return true;
    }
}
