using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName= "New Weapon", menuName= "Weapon")]
public class WeaponProperties : ScriptableObject
{

public int maxAmmo;
public int clipSize;
public int currentAmmo;
public float fireRate;
public int damage;
}
