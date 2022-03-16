using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�������� ��������� ��� �����, ����� �������, ����������

[CreateAssetMenu(fileName = "Settings", menuName = "ScriptableObjects/CubeSettings", order = 1)]
public class CubeSettings : ScriptableObject
{
    public float DefaultScale;              //������ �� ���������
    public float AnimationScale;            //��������� ������� ��� ����������� ��������
    public float AnimationScaleTime;        //����� �������� ��������� �������
    public float DefaultRotation;
    public float AnimationAngle;            //���� ��� �������� ��������
    public float AnimationAngleTime;        //����� �������� ��������
}
