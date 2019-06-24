using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseTracker : MonoBehaviour
{

    [SerializeField]
    private float verticalSpeed = 1.0f;
    [SerializeField]
    private float horizontalSpeed = 1.0f;

    private Transform myTransform;
    private Camera mainCamera;

    private float offset = 5.0f;

    // Start is called before the first frame update
    void Start()
    {
        GatherReferences();

    }

    private void GatherReferences(){

        myTransform = this.transform;//cache tranny
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>() as Camera;
    }

    private void TrackMousePosition()
    {
        //get inputs
        var moveVector = Vector3.zero;
        moveVector.x = Input.GetAxis("Horizontal") * horizontalSpeed;
        moveVector.y = Input.GetAxis("Vertical") * verticalSpeed;
        var mousePos = Input.mousePosition;//get mouse position

        myTransform.position += moveVector * Time.deltaTime;
        var mouseCoords = mainCamera.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, offset));//mouse position to screen space, plus an offset to move it in front of cannon

        myTransform.position = mouseCoords;
    }

    // Update is called once per frame
    void Update()
    {
        TrackMousePosition();

    }
}
