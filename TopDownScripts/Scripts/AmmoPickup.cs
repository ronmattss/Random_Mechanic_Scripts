using System.Collections;
using System.Collections.Generic;
using UnityEngine;



// TODO: WEAPON PROPERTIES WILL BE CLASS, NOT A SCRIPTABLE OBJECT
// REFACTOR: WEAPONS AND SHOOTING SHITS
public class AmmoPickup : MonoBehaviour
{
    // Ammo pickup GameObject
    // this will be spawned in the spawn manager
    public int ammo;
    void Start()
    {
        ammo = Random.Range(30, 91);

    }
 
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            WeaponManager weapon = other.gameObject.GetComponent<WeaponManager>();
            weapon.currWeapon.weaponProperties.maxAmmo += ammo;
            Destroy(this.gameObject);
            Debug.Log($"{other.gameObject.tag} {weapon.weapons[0].weaponProperties.maxAmmo}");

        }
    }


}
