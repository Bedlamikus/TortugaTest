using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

//����������� ������
public enum Direction { Left, Right, Up, Down, Idle }

//������������ ������� ������ � �������
public class InputController : MonoBehaviour
{
   
    [SerializeField] private Camera m_camera;
    RaycastHit currentHit;      //������ ��������� �� ������� ������ ������
    RaycastHit mouseMoveHit;    //������ ��������� ��� ����������
    Ray ray;

    public UnityEvent<Collider> mouseLeftDown;        //������ ����� ������ ����
    public UnityEvent<Collider> mouseLeftUp;          //�������� ����� ������ ����
    public UnityEvent<Collider, Direction> mouseSwipe;
    public UnityEvent<Collider> mouseSelect;          //Invoke ���� ��� ������ �������� ������
    public UnityEvent<Collider> mouseUnSelect;        //Invoke ���� ��� ������ �������� ������

    // ��������������� ���������� ��� ������
    private Vector2 mouseLastPointDown;
    private Vector2 mouseCurrentPoint;

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
            mouseLastPointDown = Input.mousePosition;
            mouseLeftDown?.Invoke(currentHit.collider);
        }
        if (MouseLeftUp())
        {
            CheckSwipe();
            mouseLeftUp?.Invoke(currentHit.collider);
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
            mouseUnSelect?.Invoke(mouseMoveHit.collider);
            mouseMoveHit = newMouseMoveHit;
            mouseSelect?.Invoke(mouseMoveHit.collider);
        }
        mouseCurrentPoint = Input.mousePosition;
    }

    //�������� �� ����� � ����� MouseSwipe(currentHit, Direction)
    private void CheckSwipe()
    {
        Direction result = Direction.Idle;

        var direction = mouseLastPointDown - mouseCurrentPoint;
        //print(direction+" = "+mouseLastPointDown+" - "+mouseCurrentPoint);
        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y)*1.2)
        {
            result = Direction.Left;
            if (direction.x < 0)
                result = Direction.Right;
        }
        if (Mathf.Abs(direction.x)*1.2 < Mathf.Abs(direction.y))
        {
            result = Direction.Down;
            if (direction.y < 0)
                result = Direction.Up;
        }
        if (result != Direction.Idle)
            mouseSwipe?.Invoke(currentHit.collider, result);
    }
}
