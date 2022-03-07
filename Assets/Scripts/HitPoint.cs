using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitPoint : MonoBehaviour
{
    [SerializeField] private float m_lifeTime;

    private float currentTime = 0.0f;

    // Update is called once per frame
    void Update()
    {
        currentTime += Time.deltaTime;
        if (currentTime > m_lifeTime)
            Terminate();
    }

    private void Terminate()
    {
        Destroy(this.gameObject);
    }
}
