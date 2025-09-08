using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AttackBase : MonoBehaviour
{


    public abstract void Attack(Transform startPoint, Transform endPoint);
    
}
