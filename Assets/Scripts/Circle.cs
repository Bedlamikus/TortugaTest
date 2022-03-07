using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Circle : MonoBehaviour
{
    [SerializeField] private float m_radius;

    public float Radius => m_radius;

    private void Start()
    {
        transform.localScale = new Vector3(m_radius*2,0.001f,m_radius*2);
    }
}
