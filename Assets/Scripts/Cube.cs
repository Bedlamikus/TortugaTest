using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube : MonoBehaviour
{
    [SerializeField] private CubeSettings settings;
    [SerializeField] private GameObject visual;

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
    }

    private void UpScale(RaycastHit hit)
    {
        c_UpScale();
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
        c_DownScale();
    }
    IEnumerator c_DownScale()
    {
        float currentTime = 0.0f;
        Vector3 EndScale = new Vector3(1, 1, 1);
        Vector3 StartScale = transform.localScale;
        while (true)
        {
            currentTime += Time.deltaTime;

            var newscale = Vector3.Lerp(StartScale,EndScale, currentTime / settings.AnimationScaleTime);
            transform.localScale = newscale;

            yield return null;
            if (currentTime > settings.AnimationScaleTime) break;
        }
    }

}
