using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//основные настройки дл€ кубов, кроме текстур, материалов

[CreateAssetMenu(fileName = "Settings", menuName = "ScriptableObjects/CubeSettings", order = 1)]
public class CubeSettings : ScriptableObject
{
    public float DefaultScale;              //–азмер по умолчанию
    public float AnimationScale;            //»зменение размера дл€ программной анимации
    public float AnimationScaleTime;        //врем€ анимации изменени€ размера
    public float DefaultRotation;
    public float AnimationAngle;            //угол дл€ анимации поворота
    public float AnimationAngleTime;        //¬рем€ анимации поворота
}
