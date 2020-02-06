using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//handle weapons used by the player
public class WeaponManager : MonoBehaviour
{
    public Weapon currWeapon;
    public Weapon pistol; // default weapon
    public List<Weapon> weapons = new List<Weapon>();

     void Start() {
       weapons.Add(pistol);
       currWeapon = weapons[0];
    }



}
