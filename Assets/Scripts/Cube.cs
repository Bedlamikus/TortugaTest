using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube : MonoBehaviour
{
    [SerializeField] private CubeSettings settings;                 //�������� ��������� ����, �������� ����, ������ � �.�.
    [SerializeField] private GameObject visual;                     //������ �� ���������� ���������� ��� ��������

    //������� ������ ��������, ����������� � �.�. 
    private Coroutine ScaleCoroutine;
    private Coroutine MoveCoroutine;
    public bool mooving;


    void Start()
    {
        var inputcontroller = FindObjectOfType<InputController>();
        if (inputcontroller == null)
        {
            print("Not find <InputController>");
            return;
        }

        //����������� ����������
        //inputcontroller.mouseLeftDown.AddListener(Rotate);
        //inputcontroller.mouseLeftUp.AddListener(Move);
        inputcontroller.mouseSelect.AddListener(Select);
        inputcontroller.mouseUnSelect.AddListener(UnSelect);

        //������ ��������� ��������� 
        transform.localScale = new Vector3(settings.DefaultScale, settings.DefaultScale, settings.DefaultScale);
    }

    // �������� ������� around X
    private void Rotate(Collider collider)
    {
        if (HitOnMe(collider) == false) return;
        StartCoroutine(c_Rotate());
    }
    IEnumerator c_Rotate()
    {
        float StartAngle = settings.DefaultRotation;
        float currentAngle = transform.localEulerAngles.x;
        float EndAngle = settings.AnimationAngle + StartAngle;

        float currentTime = ((currentAngle - settings.DefaultRotation) / (settings.AnimationAngle)) * settings.AnimationAngleTime;
        while (true)
        {
            currentTime += Time.deltaTime;

            var newAngle = Mathf.LerpAngle(StartAngle, EndAngle, currentTime / settings.AnimationAngleTime);
            transform.localRotation = Quaternion.Euler(new Vector3(newAngle, 0, 0));

            yield return null;

            if (currentTime > settings.AnimationAngleTime) break;
        }
    }

    // �������� ����������� loacalScale 
    private void UpScale(Collider collider)
    {
        if (HitOnMe(collider) == false) return;
        if (ScaleCoroutine != null)
            StopCoroutine(ScaleCoroutine);
        ScaleCoroutine = StartCoroutine(c_UpScale());
    }
    IEnumerator c_UpScale()
    {
        Vector3 StartScale = new Vector3(settings.DefaultScale, settings.DefaultScale, settings.DefaultScale);
        Vector3 currentScale = transform.localScale;
        Vector3 EndScale = new Vector3(settings.AnimationScale,
                                       settings.AnimationScale,
                                       settings.AnimationScale) +
                                       StartScale;

        float currentTime = ((currentScale.x - settings.DefaultScale) / (settings.AnimationScale)) * settings.AnimationScaleTime;
        while (true)
        {
            currentTime += Time.deltaTime;

            var newscale = Vector3.Lerp(StartScale, EndScale, currentTime / settings.AnimationScaleTime);
            transform.localScale = newscale;

            yield return null;

            if (currentTime > settings.AnimationScaleTime) break;
        }
    }

    // �������� ��������� localScale �� ���������� ���������
    private void DownScale(Collider collider)
    {
        if (HitOnMe(collider) == false) return;
        if (ScaleCoroutine != null)
            StopCoroutine(ScaleCoroutine);
        ScaleCoroutine = StartCoroutine(c_DownScale());
    }
    IEnumerator c_DownScale()
    {
        Vector3 StartScale = new Vector3(settings.DefaultScale, settings.DefaultScale, settings.DefaultScale);
        Vector3 currentScale = transform.localScale;
        Vector3 EndScale = new Vector3(settings.AnimationScale,
                                       settings.AnimationScale,
                                       settings.AnimationScale) +
                                       StartScale;

        float currentTime = (1 - (currentScale.x - settings.DefaultScale) / (settings.AnimationScale)) * settings.AnimationScaleTime;

        while (true)
        {
            currentTime += Time.deltaTime;

            var newscale = Vector3.Lerp(StartScale,EndScale, 1-currentTime / settings.AnimationScaleTime);
            transform.localScale = newscale;

            yield return null;
            if (currentTime > settings.AnimationScaleTime) break;
        }
    }

    //����� �����
    private void Select(Collider collider)
    {
        UpScale(collider);
    }

    //������ ������ �����
    private void UnSelect(Collider collider)
    {
        DownScale(collider);
    }

    //�������� ����������� �� ���� hit ������ ����
    public bool HitOnMe(Collider collider)
    {
        var result = false;
        if (collider == null) return result;
        if (collider.gameObject == this.gameObject) result = true;
        return result;
    }

    //���������� Scale
    public Vector3 GetScale()
    {
        return new Vector3(settings.DefaultScale, settings.DefaultScale, settings.DefaultScale);
    }


    public void Move(Vector3 direction, float time)
    {
        if (mooving == true) return;
        if (MoveCoroutine != null)
            StopCoroutine(MoveCoroutine);
        mooving = true;
        MoveCoroutine = StartCoroutine(c_Move(direction, time));
    }
    IEnumerator  c_Move(Vector3 direction, float time)
    {
        Vector3 StartPoint = transform.position;
        var EndPoint = new Vector3(direction.x, direction.y, direction.z);

        float currentTime = 0;
        while (true)
        {
            currentTime += Time.deltaTime;
            var newPoint = Vector3.Lerp(StartPoint, EndPoint, currentTime / time);
            transform.position = newPoint;

            yield return null;
            if (currentTime > time) break;
        }
        mooving = false;
    }
}
