using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


//Отслеживание нажатых кнопок и прочего

public class InputController : MonoBehaviour
{
   
    [SerializeField] private Camera m_camera;
    RaycastHit currentHit;      //хранит коллайдер на который нажали мышкой
    RaycastHit mouseMoveHit;    //хранит коллайдер под указателем
    Ray ray;

    public UnityEvent<RaycastHit> mouseLeftDown;        //нажата левая кнопка мыши
    public UnityEvent<RaycastHit> mouseLeftUp;          //отпущена левая кнопка мыши
    public UnityEvent<RaycastHit> mouseSelect;          //Invoke если под мышкой сменился объект
    public UnityEvent<RaycastHit> mouseUnSelect;        //Invoke если под мышкой сменился объект

    private void Start()
    {
        ray = m_camera.ScreenPointToRay(Input.mousePosition);
        Physics.Raycast(ray, out mouseMoveHit, 100);
    }

    void Update()
    {
        ray = m_camera.ScreenPointToRay(Input.mousePosition);
        if (MouseLeftDown())
        {
            Physics.Raycast(ray, out currentHit, 100);
            mouseLeftDown?.Invoke(currentHit);
        }
        if (MouseLeftUp())
        {
            mouseLeftUp?.Invoke(currentHit);
        }
        MouseMove();
    }

    //если нажали левую кнопку мыши то вернет true
    private bool MouseLeftDown()
    {
        if (Input.GetButtonDown("Fire1")) return true;
        return false;
    }
    //если отпустили левую кнопку мыши то вернет true
    private bool MouseLeftUp()
    {
        if (Input.GetButtonUp("Fire1")) return true;
        return false;
    }
    // вызывает mouseSelect для текущего hit и mouseUnselect для прошлого
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
