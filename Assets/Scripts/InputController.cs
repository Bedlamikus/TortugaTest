using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


//������������ ������� ������ � �������

public class InputController : MonoBehaviour
{
   
    [SerializeField] private Camera m_camera;
    RaycastHit currentHit;      //������ ��������� �� ������� ������ ������
    RaycastHit mouseMoveHit;    //������ ��������� ��� ����������
    Ray ray;

    public UnityEvent<RaycastHit> mouseLeftDown;        //������ ����� ������ ����
    public UnityEvent<RaycastHit> mouseLeftUp;          //�������� ����� ������ ����
    public UnityEvent<RaycastHit> mouseSelect;          //Invoke ���� ��� ������ �������� ������
    public UnityEvent<RaycastHit> mouseUnSelect;        //Invoke ���� ��� ������ �������� ������

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

    //���� ������ ����� ������ ���� �� ������ true
    private bool MouseLeftDown()
    {
        if (Input.GetButtonDown("Fire1")) return true;
        return false;
    }
    //���� ��������� ����� ������ ���� �� ������ true
    private bool MouseLeftUp()
    {
        if (Input.GetButtonUp("Fire1")) return true;
        return false;
    }
    // �������� mouseSelect ��� �������� hit � mouseUnselect ��� ��������
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
