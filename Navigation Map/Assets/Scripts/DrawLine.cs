using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawLine : MonoBehaviour
{
    public  LineRenderer line;
    public  Transform start;
    public Transform middle;
    public Transform end;
    void Start()
    {
        line.SetPosition(0,start.position);
        line.SetPosition(1,middle.position);
        line.SetPosition(2,end.position);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
