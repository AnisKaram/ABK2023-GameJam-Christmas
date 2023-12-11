using UnityEngine;

public class GlobalConsts : MonoBehaviour
{
    public static CharacterHealth characterHealth;

    private static void Start()
    {
        if (GameObject.FindWithTag("Player").TryGetComponent(out characterHealth) == false)
        {
            Debug.LogError("Could not find player health");
        }
    }
}
