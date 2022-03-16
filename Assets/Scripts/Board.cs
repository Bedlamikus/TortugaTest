using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    [SerializeField] private float m_width;
    [SerializeField] private float m_height;

    private List<Cube> lines;
}
