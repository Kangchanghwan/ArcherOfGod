using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AttackBase : MonoBehaviour
{
    protected Transform EndPoint { get; private set; }
    protected Transform StartPoint { get; private set; }

    public abstract void Attack();

    public void Initialize(Transform startPoint, Transform endPoint)
    {
        StartPoint = startPoint;
        EndPoint = endPoint;
    }
}
