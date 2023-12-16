using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Colors", menuName = "ScriptableObjects/Colors", order = 1)]
public class ColorsData : ScriptableObject
{
    public List<Color> listOfEnemyHealthColors;
}
