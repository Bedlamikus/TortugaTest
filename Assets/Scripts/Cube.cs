using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube : MonoBehaviour
{
    [SerializeField] private CubeSettings settings;
    [SerializeField] private GameObject visual;

    private Coroutine currentCoroutine;

    void Start()
    {
        var inputcontroller = FindObjectOfType<InputController>();
        if (inputcontroller == null)
        {
            print("Not find <InputController>");
            return;
        }
        inputcontroller.mouseDown.AddListener(UpScale);
        inputcontroller.mouseUp.AddListener(DownScale);
        inputcontroller.mouseSelect.AddListener(Select);
        inputcontroller.mouseUnSelect.AddListener(UnSelect);
    }

    private void UpScale(RaycastHit hit)
    {
        if (HitOnMe(hit) == false) return;
        if (currentCoroutine != null)
            StopCoroutine(currentCoroutine);
        currentCoroutine = StartCoroutine(c_UpScale());
    }
    IEnumerator c_UpScale()
    {
        float currentTime = 0.0f;
        Vector3 EndScale = new Vector3(1 + settings.AnimationScale,
                                        1 + settings.AnimationScale,
                                        1 + settings.AnimationScale);
        Vector3 StartScale = transform.localScale;
        while (true)
        {
            currentTime += Time.deltaTime;

            var newscale = Vector3.Lerp(StartScale, EndScale, currentTime / settings.AnimationScaleTime);
            transform.localScale = newscale;

            yield return null;

            if (currentTime > settings.AnimationScaleTime) break;
        }
    }
    private void DownScale(RaycastHit hit)
    {
        if (HitOnMe(hit) == false) return;
        if (currentCoroutine != null)
            StopCoroutine(currentCoroutine);
        currentCoroutine = StartCoroutine(c_DownScale());
    }
    IEnumerator c_DownScale()
    {
        Vector3 StartScale = new Vector3(1, 1, 1);
        Vector3 currentScale = transform.localScale;
        Vector3 EndScale = new Vector3(1 + settings.AnimationScale,
                                1 + settings.AnimationScale,
                                1 + settings.AnimationScale);

        float currentTime = (1- (currentScale.x-1) / (settings.AnimationScale))* settings.AnimationScaleTime;

        while (true)
        {
            currentTime += Time.deltaTime;

            var newscale = Vector3.Lerp(StartScale,EndScale, 1-currentTime / settings.AnimationScaleTime);
            transform.localScale = newscale;

            yield return null;
            if (currentTime > settings.AnimationScaleTime) break;
        }
    }

    private void Select(RaycastHit hit)
    {
        UpScale(hit);
    }
    private void UnSelect(RaycastHit hit)
    {
        DownScale(hit);
    }

    private bool HitOnMe(RaycastHit hit)
    {
        var result = false;
        if (hit.collider == null) return result;
        if (hit.collider.gameObject == this.gameObject) result = true;
        return result;
    }
}
