using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pancam : MonoBehaviour
{
    Vector3 newPos;
    [SerializeField] float mouseSensitivity = -0.015f;
    [SerializeField] int max_x = int.MaxValue;
    [SerializeField] int max_y = int.MaxValue;
    [SerializeField] int min_x = int.MinValue;
    [SerializeField] int min_y = int.MinValue;
 
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            newPos = Input.mousePosition;
        }

        if (Input.GetMouseButton(0))
        {
            Vector3 delta = Input.mousePosition - newPos;
            transform.Translate(delta.x * mouseSensitivity, delta.y * mouseSensitivity, 0);
            newPos = Input.mousePosition;
        }
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, min_x, max_x), Mathf.Clamp(transform.position.y, min_y, max_y), transform.position.z);
    }
}
