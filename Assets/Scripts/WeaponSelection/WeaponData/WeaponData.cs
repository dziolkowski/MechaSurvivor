using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "Weapons/Weapon Data")]
public class WeaponData : ScriptableObject
{
    public string weaponName;
    public Sprite weaponIcon;
    public GameObject topModelPrefab;
    public GameObject sideModelPrefab;
    public MonoScript topWeaponScript;
    public MonoScript sideWeaponScript;
}
