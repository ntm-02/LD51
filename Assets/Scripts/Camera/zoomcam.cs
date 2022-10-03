using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class zoomcam : MonoBehaviour
{
    private float minSize = 3f;
    private float maxSize = 20f;
    private float sensitivity = 5f;
    private float camSize = 0f;
    private bool canZoom = true;

    private Camera mainCam;
    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 60;
        mainCam = GetComponent<Camera>();
        camSize = mainCam.orthographicSize;
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if (canZoom)
        {
            camSize += Input.GetAxis("Mouse ScrollWheel") * sensitivity * -1f;
            //print(Input.GetAxis("Mouse ScrollWheel"));
            camSize = Mathf.Clamp(camSize, minSize, maxSize);
            //print(camSize);
            mainCam.orthographicSize = camSize;
        }
    }

    public void SetCanZoom(bool zoom)
    {
        canZoom = zoom;
    }
}
