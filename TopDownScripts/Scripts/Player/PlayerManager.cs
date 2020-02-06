using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerScripts;
public class PlayerManager : MonoBehaviour
{

     Status properties;
    
    void Start()
    {
        properties = this.GetComponent<Status>();
        properties.healthPoints = 100;
    }

    // Update is called once per frame
    void Update()
    {

    }

}
