using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;

[DefaultExecutionOrder(-100)]
public class SpawnManager : MonoBehaviour
{
    #region Fields
    [Header("GameObjects")]
    [SerializeField] private GameObject _presentItem;
    [SerializeField] private GameObject _enemyRed;

    [Header("Lists Presents Positions")]
    [SerializeField] private List<Transform> _listOfPresentPositions_Area1;
    [SerializeField] private List<Transform> _listOfPresentPositions_Area2;

    [Header("Lists Enemies Positions")]
    [SerializeField] private List<Transform> _listOfEnemiesPositions_Area1;
    [SerializeField] private List<Transform> _listOfEnemiesPositions_Area2;

    [Header("Texts - TextMeshPro")]
    [SerializeField] private TextMeshProUGUI _presentsGoalText;

    private int _presentsSpawned;
    private int _presentsCollected;

    private Transform _player;
    #endregion

    #region Events
    public static event UnityAction OnGoalReached;
    #endregion

    #region Unity Methods
    private void Awake()
    {
        LootManager.OnPresentDroppedInLoot += IncrementPresentsCollected;
        EnemyParent.OnEnemyDied += SpawnNewEnemy;

        _player = GameObject.FindGameObjectWithTag("Player").transform;

        ShuffleLists();

        SpawnPresentsInArea1();
        SpawnPresentsInArea2();
        SpawnEnemiesInArea1();

        _presentsCollected = 0;
        UpdateGoalTextOnUI();
    }   

    private void OnDestroy()
    {
        LootManager.OnPresentDroppedInLoot -= IncrementPresentsCollected;
        EnemyParent.OnEnemyDied -= SpawnNewEnemy;
    }
    #endregion

    #region Public Methods
    public void IncrementPresentsCollected()
    {
        _presentsCollected += 1;
        UpdateGoalTextOnUI();
        CheckIfGoalReached();
    }
    #endregion

    #region Private Methods
    private void SpawnEnemiesInArea1()
    {
        for (int i = 0; i < _listOfEnemiesPositions_Area1.Count; i++)
        {
            GameObject enemyInstance = Instantiate(_enemyRed);
            enemyInstance.transform.localPosition = _listOfEnemiesPositions_Area1[i].position;

            EnemyParent enemyParent = enemyInstance.GetComponent<EnemyParent>();
            enemyParent.PlayerTarget = _player;
            enemyParent.PositionIndex = i;
        }
    }

    private void SpawnNewEnemy(int index)
    {
        GameObject enemyInstance = Instantiate(_enemyRed);
        enemyInstance.transform.position = _listOfEnemiesPositions_Area1[index].position;

        EnemyParent enemyParent = enemyInstance.GetComponent<EnemyParent>();
        enemyParent.PlayerTarget = _player;
        enemyParent.PositionIndex = index;
    }

    private void ShuffleLists()
    {
        for (int times = 0; times < 5; times++)
        {
            Shuffle(_listOfPresentPositions_Area1);
            Shuffle(_listOfPresentPositions_Area2);
        }
    }

    private void SpawnPresentsInArea1()
    {
        int amountToDivide = Random.Range(1, 3);
        for (int present = 0; present < _listOfPresentPositions_Area1.Count / amountToDivide; present++)
        {
            GameObject presentInstance = Instantiate(_presentItem);
            presentInstance.transform.position = _listOfPresentPositions_Area1[present].position;
            _presentsSpawned += 1;
        }
    }

    private void SpawnPresentsInArea2()
    {
        int amountToDivide = Random.Range(1, 3);
        for (int present = 0; present < _listOfPresentPositions_Area2.Count / amountToDivide; present++)
        {
            GameObject presentInstance = Instantiate(_presentItem);
            presentInstance.transform.position = _listOfPresentPositions_Area2[present].position;
            _presentsSpawned += 1;
        }
    }

    private void Shuffle(List<Transform> list)
    {
        var count = list.Count;
        var last = count - 1;
        for (var i = 0; i < last; ++i)
        {
            var r = Random.Range(i, count);
            var tmp = list[i];
            list[i] = list[r];
            list[r] = tmp;
        }
    }

    private void UpdateGoalTextOnUI()
    {
        _presentsGoalText.text = $"{_presentsCollected}/{_presentsSpawned}";
    }

    private void CheckIfGoalReached()
    {
        if (_presentsCollected == _presentsSpawned)
        {
            OnGoalReached?.Invoke();
        }
    }
    #endregion
}