using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseTracker : MonoBehaviour
{
    [SerializeField]
    private float heightBounds;
    [SerializeField]
    private float widthBounds;

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

        ConfigureBounds();
        
    }

    private void GatherReferences()
    {

        myTransform = this.transform;//cache tranny
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>() as Camera;
    }

    /// <summary>
    /// Not yet integrated.
    /// </summary>
    private void ConfigureBounds()
    {

    }

    private void TrackMousePosition()
    {
        var mousePos = Input.mousePosition;//get mouse position

        var mouseCoords = mainCamera.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, offset));//mouse position to screen space, plus an offset to move it in front of cannon

        myTransform.position = mouseCoords;
    }

    // Update is called once per frame
    void Update()
    {

        TrackMousePosition();
        //keep in bounds
    }
}
