using UnityEngine;

public class ItemHealthPack : ItemBase
{
    [SerializeField]
    private int _healthRestored = 25;

    protected override bool PickUpBehavior()
    {
        if (GlobalConsts.playerHealth.Health == 100)
        {
            return false;
        }

        GameAudioManager.Instance.PlaySFX("Health Pack Pickup");
        GlobalConsts.playerHealth.GainHealth(_healthRestored);

        return true;
    }
}
