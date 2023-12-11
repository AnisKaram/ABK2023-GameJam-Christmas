using UnityEngine;

public class ItemHealthPack : ItemBase
{
    [SerializeField]
    private int healthRestored = 25;

    protected override void PickUpBehavior()
    {
        GlobalConsts.characterHealth.GainHealth(healthRestored);
    }
}
