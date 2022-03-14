using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InputController : MonoBehaviour
{
   
    [SerializeField] private Camera m_camera;
    RaycastHit currentHit;      //хранит коллайдер на который нажали мышкой
    RaycastHit mouseMoveHit;    //хранит коллайдер под указателем
    Ray ray;

    public UnityEvent<RaycastHit> mouseDown;
    public UnityEvent<RaycastHit> mouseUp;
    public UnityEvent<RaycastHit> mouseSelect;
    public UnityEvent<RaycastHit> mouseUnSelect;

    private void Start()
    {
        ray = m_camera.ScreenPointToRay(Input.mousePosition);
        Physics.Raycast(ray, out mouseMoveHit, 100);
    }

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
            mouseUp?.Invoke(currentHit);
        }
        MouseMove();
    }

    private void MouseMove()
    {
        RaycastHit newMouseMoveHit;
        Physics.Raycast(ray, out newMouseMoveHit, 100);
        if (newMouseMoveHit.collider != mouseMoveHit.collider)
        {
            mouseUnSelect?.Invoke(mouseMoveHit);
            mouseMoveHit = newMouseMoveHit;
            mouseSelect?.Invoke(mouseMoveHit);
        }
    }
}
