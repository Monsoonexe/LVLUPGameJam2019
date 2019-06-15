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

    // Start is called before the first frame update
    void Start()
    {
        GatherReferences();

        ConfigureBounds();
    }

    private void GatherReferences(){

        myTransform = this.transform;//cache tranny
    }

    private void ConfigureBounds(){

    }

    // Update is called once per frame
    void Update()
    {
        //get inputs
        var moveVector = Vector3.zero;
        moveVector.x = Input.GetAxis("Horizontal") * horizontalSpeed;
        moveVector.y = Input.GetAxis("Vertical") * verticalSpeed;

        myTransform.position += moveVector * Time.deltaTime;


        //keep in bounds
    }
}
