using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    private Rigidbody rb;
    [SerializeField] private float m_ImpulseVelocity;   //Сила с которой мы двигаемся 
    [SerializeField] private float m_StopVelocity;      //Сила с которой мы тормозим
    [SerializeField] private InputController m_inputController;
    [SerializeField] private float m_time;
    [SerializeField] private Circle m_circle;

    private Coroutine current_Coroutine;

    // Start is called before the first frame update
    void Start()
    {
        m_inputController.mouseDown.AddListener(move);
        rb = GetComponent<Rigidbody>();
        if (rb == null) Debug.Log("RigidBody == null");
    }

    private void move(Vector3 point)
    {
        if (current_Coroutine !=null)
            StopCoroutine(current_Coroutine);
        if (point.magnitude<m_circle.Radius)
            current_Coroutine = StartCoroutine(MoveToPoint(point, m_time));
        Debug.Log(point.magnitude);
        Debug.Log(point);
    }

    private IEnumerator MoveToPoint(Vector3 toPoint, float time)
    {
        float m_time = 0.0f;
        while (true)
        {
            var newPoint = Vector3.Lerp(transform.position, toPoint, m_time/time);
            newPoint.y = transform.position.y;
            transform.position = newPoint;
            m_time += Time.deltaTime;
            if (m_time > time) break;
            yield return null;
        }
    }
}
