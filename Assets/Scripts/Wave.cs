using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Waves/WaveEnemies", fileName = "WaveEnemies")]
public class Wave : ScriptableObject
{
    [SerializeField] private List<GameObject> enemyList;
}


/* 
 * list of waves (sriptable objects) on spawner
 * go to next wave each X seconds
 * spawn all enemies from list
 * 
 * scriptable object with list of enemies v
 * 
 */