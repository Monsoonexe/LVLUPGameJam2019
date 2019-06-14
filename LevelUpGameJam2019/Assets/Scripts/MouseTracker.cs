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
        var verticalInput = Input.GetAxis("Vertical");
        var horizontalInput = Input.GetAxis("Horizontal");

        //move tracker
        myTransform.Translate(0, verticalInput * verticalSpeed * Time.deltaTime, 0);

        myTransform.Translate(horizontalInput * horizontalSpeed * Time.deltaTime, 0, 0);

        //keep in bounds
    }
}
