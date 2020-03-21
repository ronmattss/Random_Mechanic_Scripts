using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[ExecuteInEditMode]
public class AutoName : MonoBehaviour
{
    public int floorNumber = 1;
    public int roomNumber = 0;
    public string wing = "E";
    public bool renamed = false;

    // Start is called before the first frame update
    void Awake()
    {
        GameObject[] rooms;
        rooms = GameObject.FindGameObjectsWithTag("Room");
        if (renamed)
        {
            return;
        }
        else
        {
            //roomNumber++;
            Debug.Log($"Room Number{roomNumber}");
            renamed = true;

            this.transform.name = ChangeName();
        }

        // for(int i = 0; i<rooms.Length;i++)
        // {
        //     if(rooms[i].name == this.transform.name)
        //     {
        //         roomNumber = rooms[i].GetComponent<AutoName>().roomNumber + 2;
        //         break;
        //     }
        // }


    }

    // Update is called once per frame
    void Update()
    {
        // this.transform.name = ChangeName();
    }
    string ChangeName()
    {

        string roomName = $"{wing}{floorNumber}{(roomNumber).ToString("D2")}";
        return roomName;
    }
}
