using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube : MonoBehaviour
{
    [SerializeField] private CubeSettings settings;                 //основные настройки куба, скорость аним, размер и т.д.
    [SerializeField] private GameObject visual;                     //ссылка на визуальную компоненту для анимаций

    //текущая задача анимация, перемещения и т.д. 
    private Coroutine currentCoroutine;

    void Start()
    {
        var inputcontroller = FindObjectOfType<InputController>();
        if (inputcontroller == null)
        {
            print("Not find <InputController>");
            return;
        }

        //настраиваем слушателей
        inputcontroller.mouseLeftDown.AddListener(Rotate);
        //inputcontroller.mouseLeftUp.AddListener(DownScale);
        inputcontroller.mouseSelect.AddListener(Select);
        inputcontroller.mouseUnSelect.AddListener(UnSelect);

        //задаем начальные настройки 
        transform.localScale = new Vector3(settings.DefaultScale, settings.DefaultScale, settings.DefaultScale);
    }

    // корутина поворот around X
    private void Rotate(RaycastHit hit)
    {
        if (HitOnMe(hit) == false) return;
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

    // корутина увеличиваем loacalScale 
    private void UpScale(RaycastHit hit)
    {
        if (HitOnMe(hit) == false) return;
        if (currentCoroutine != null)
            StopCoroutine(currentCoroutine);
        currentCoroutine = StartCoroutine(c_UpScale());
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

    // корутина уменьшаем localScale до начального состояния
    private void DownScale(RaycastHit hit)
    {
        if (HitOnMe(hit) == false) return;
        if (currentCoroutine != null)
            StopCoroutine(currentCoroutine);
        currentCoroutine = StartCoroutine(c_DownScale());
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

    //выбор фишки
    private void Select(RaycastHit hit)
    {
        UpScale(hit);
    }

    //отмена выбора фишки
    private void UnSelect(RaycastHit hit)
    {
        DownScale(hit);
    }

    //проверка принадлежит ли этот hit нашему кубу
    private bool HitOnMe(RaycastHit hit)
    {
        var result = false;
        if (hit.collider == null) return result;
        if (hit.collider.gameObject == this.gameObject) result = true;
        return result;
    }
}
