using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InputController : MonoBehaviour
{
   
    [SerializeField] private Camera m_camera;
    RaycastHit currentHit;
    Ray ray;

    public UnityEvent<RaycastHit> mouseDown;
    public UnityEvent<RaycastHit> mouseUp;



    void Update()
    {
        ray = m_camera.ScreenPointToRay(Input.mousePosition);
        if (Input.GetButtonDown("Fire1"))
        {

            Physics.Raycast(ray, out currentHit, 100);
            mouseDown?.Invoke(currentHit);
        }
        if (Input.GetButtonUp("Fire1"))
        {

            if (currentHit.collider == null) return;
            mouseUp?.Invoke(currentHit);
        }
    }
}
