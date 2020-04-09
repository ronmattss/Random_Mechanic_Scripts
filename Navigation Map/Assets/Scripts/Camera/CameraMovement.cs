using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraMovement : MonoBehaviour
{
    public static CameraMovement instance;
    private CameraMovement _instance;
    public Camera mainCamera;
    public int floor = 0;
    Vector3 velocity = Vector3.zero;
    Transform cameraTransform;
    Vector3 targetPosition;
    public float smoothTime = 0.3f;
    public float cameraMoveValue = 10f;
    public GameObject[] floors = new GameObject[6];
    void Start()
    {
        _instance = this;
        instance = _instance;
        mainCamera = Camera.main;
        cameraTransform = mainCamera.transform;

    }

    // Update is called once per frame
    void Update()
    {

    }

    // Hide UI
    // move left
    public void MoveLeft()
    {
        targetPosition = new Vector3(cameraTransform.position.x - cameraMoveValue, cameraTransform.position.y, cameraTransform.position.z);
        cameraTransform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime * Time.deltaTime);
    }
    public void MoveRight()
    {
        targetPosition = new Vector3(cameraTransform.position.x + cameraMoveValue, cameraTransform.position.y, cameraTransform.position.z);
        cameraTransform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime * Time.deltaTime);
    }
    public void MoveUp()
    {
        targetPosition = new Vector3(cameraTransform.position.x, cameraTransform.position.y, cameraTransform.position.z + cameraMoveValue);
        cameraTransform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime * Time.deltaTime);
    }
    public void MoveDown()
    {
        targetPosition = new Vector3(cameraTransform.position.x, cameraTransform.position.y, cameraTransform.position.z - +cameraMoveValue);
        cameraTransform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime * Time.deltaTime);
    }

    public void RotateLeft()
    {
        cameraTransform.Rotate(0, 0, 45);
    }

    public void RotateRight()
    {
        cameraTransform.Rotate(0, 0, -45);
    }
    public void ZoomIn()
    {
        targetPosition = new Vector3(cameraTransform.position.x, cameraTransform.position.y - cameraMoveValue, cameraTransform.position.z);
        cameraTransform.position = targetPosition;
    }
    public void ZoomOut()
    {
        targetPosition = new Vector3(cameraTransform.position.x, cameraTransform.position.y + cameraMoveValue, cameraTransform.position.z);
        cameraTransform.position = targetPosition;
    }

    public void ShowFloorUp()
    {
        if (floor != 5)
        {
            floor++;
            floors[floor].SetActive(true);
        }
        else
        {

        }
    }
    public void ShowFloorDown()
    {
        if (floor != 0)
        {
            floors[floor].SetActive(false);
            floor--;
        }
        else
        {

        }
    }
    public void SetActiveFloors()
    {
        foreach (GameObject f in floors)
        {
            f.SetActive(true);
        }
    }

    public void GotoScene(int sceneNumber)
    {
        SceneManager.LoadScene(sceneNumber);
    }
    // move right
    // move up
    // move down
    // rotate counter
    // rotate clockwise
    // zoom in
    // zoom out
    // display floor +
    // display floor -
}
