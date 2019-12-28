using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//List all GameObjects that entered the collider
//Remove GameObjects that exit the collider
// sort via
// select Current Target

public class TurretScanner : MonoBehaviour
{

    public List<GameObject> targets = new List<GameObject>();
    public float range = 0f;
public GameObject target;

    // Start is called before the first frame update
    void Start()
    {
        this.gameObject.transform.localScale = new Vector3(transform.localScale.x +range,transform.localScale.y+range,transform.localScale.z);
    }

    // Update is called once per frame
    void Update()
    {

    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Turrets" || other.gameObject.tag == "Bullet"|| other.gameObject.tag == "" || other.gameObject.tag == "TileMapBase") 
        {
        
        }
        else
        {
            targets.Add(other.gameObject);
        }

    }
     private void OnTriggerStay2D(Collider2D other) {
        
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        targets.Remove(other.gameObject);
    }

    
}
