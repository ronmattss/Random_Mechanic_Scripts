using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class FindRoom : MonoBehaviour
{

    public static FindRoom instance;
    private FindRoom _instance;
    Transform path;

    public GameObject seeker;
    public GameObject pathGameObject;
    public GameObject target;
    public Canvas canvas;
    public TMP_Text buttonText;
    // Start is called before the first frame update
    void Start()
    {
        _instance = this;
        instance = _instance;
        pathGameObject = GameObject.FindGameObjectWithTag("Capsule");
        target = GameObject.FindGameObjectWithTag("Target");
        seeker = GameObject.FindGameObjectWithTag("Seeker");
        canvas = transform.Find("Canvas").gameObject.GetComponent<Canvas>();
        canvas.worldCamera = Camera.main;
        buttonText = canvas.transform.Find("Button").gameObject.GetComponent<Button>().transform.Find("Text (TMP)").GetComponent<TMP_Text>();
        buttonText.text = GetComponentInParent<Transform>().name;
        Vector3 x = transform.Find("A*Path").position;
        Vector3 y = new Vector3(0, 0.5f, 0);
        transform.Find("A*Path").position += y;


    }

    // Update is called once per frame
    void Update()
    {

    }

    public void GetPath()
    {
        path = transform.Find("A*Path");
        if (path != null)
        {
            Debug.Log(path.name);
            pathGameObject.transform.position = path.position;
        }

    }

    public void SetSeeker()
    {
        seeker.transform.position = pathGameObject.transform.position;
    }
    public void SetTarget()
    {
        target.transform.position = pathGameObject.transform.position;
    }
}
