using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InputController : MonoBehaviour
{
    [SerializeField] private GameObject particle;
    [SerializeField] private Camera m_camera;
    RaycastHit hit;
    Ray ray;

    public UnityEvent<Vector3> mouseDown;



    void Update()
    {
        ray = m_camera.ScreenPointToRay(Input.mousePosition);
        if (Input.GetButtonDown("Fire1"))
        {

            Physics.Raycast(ray, out hit, 100);
            Instantiate(particle, hit.point, Quaternion.identity);
            mouseDown?.Invoke(hit.point);
        }
        Debug.DrawLine(ray.origin, hit.point, Color.green);
    }
}
