using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterCollision : MonoBehaviour
{
    [Header("Transforms")]
    [SerializeField] private Transform _playerTarget;

    [SerializeField] private int _damageTaken;

    private CharacterHealth _playerHealth;

    private void Start()
    {
        _playerHealth = _playerTarget.GetComponent<CharacterHealth>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Projectile"))
        {
            _playerHealth.TakeDamage(_damageTaken);
        }
    }
}
