using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WaveScriptableObject", menuName = "ScriptableObjects/Wave", order = 2)]
public class WaveScriptableObject : ScriptableObject
{
    [field: SerializeField] public EnemyParent[] waveEnemyTypes { get; private set; }

    // TODO - Change field type at line below from GameObject to Present after merging with presents branch!
    [field: SerializeField] public GameObject present { get; private set; }

    [field: SerializeField] public int numberOfEnemies { get; private set; }
    [field: SerializeField] public int numberOfPresents { get; private set; }
}
