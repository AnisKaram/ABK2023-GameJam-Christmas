using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    public WaveScriptableObject[] waves;

    // I couldn't come up with a better way to do this... the idea is that each time the player
    // collects a present in a wave, we would increment this counter below. But making it public
    // does not seem the best way to do it... I don't know, see what you think
    public int collectedPresentsInWave;

    [SerializeField] private Transform[] _enemySpawnPoints;
    [SerializeField] private Transform[] _presentSpawnPoints;
    private WaveScriptableObject _currentWave;
    private int _currentWaveIndex = 0;
    private bool _stopWaveSpawn = false;

    void Awake()
    {
        _currentWave = waves[_currentWaveIndex];
    }

    private void Start()
    {
        SpawnWave();
    }

    void Update()
    {
        if (_stopWaveSpawn)
        {
            return;
        }

        // If there's a way to do this with some kind of event instead of checking it every frame it would be awesome, but I have no idea how...
        // If it's possible and not too much work, I think it would be a good idea - something along the lines of every time the player collects a
        // present an event is triggered. A method is called to handle the event, and then this specific method would do the verification below and
        // call the other methods accordingly.
        if (collectedPresentsInWave == _currentWave.numberOfPresents)
        {
            //TODO - Destroy all remaining enemies and presents

            IncrementWave();
            SpawnWave();
        }
    }

    private void SpawnWave()
    {
        if (!_stopWaveSpawn)
        {
            collectedPresentsInWave = 0;

            for (int i = 0; i < _currentWave.numberOfPresents; i++)
            {
                int presentSpawnPointIndex = Random.Range(0, _presentSpawnPoints.Length);

                // TODO - Change  type at line below from GameObject to Present after merging with presents branch!
                GameObject present = Instantiate(_currentWave.present, _presentSpawnPoints[presentSpawnPointIndex].position,
                    _presentSpawnPoints[presentSpawnPointIndex].rotation);
                present.transform.SetParent(_presentSpawnPoints[presentSpawnPointIndex]);
            }

            for (int i = 0; i < _currentWave.numberOfEnemies; i++)
            {
                int enemyTypeIndex = Random.Range(0, _currentWave.waveEnemyTypes.Length);
                int enemySpawnPointIndex = Random.Range(0, _enemySpawnPoints.Length);

                EnemyParent enemy = Instantiate(_currentWave.waveEnemyTypes[enemyTypeIndex], _enemySpawnPoints[enemySpawnPointIndex].position,
                    _enemySpawnPoints[enemySpawnPointIndex].rotation);
                enemy.transform.SetParent(_enemySpawnPoints[enemySpawnPointIndex]);
            }
        }
    }

    private void IncrementWave()
    {
        if (_currentWaveIndex < waves.Length)
        {
            _currentWaveIndex++;
            _currentWave = waves[_currentWaveIndex];
        }
        else
        {
            _stopWaveSpawn = true;
        }
    }
}
