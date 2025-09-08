using System.Collections.Generic;
using UnityEngine;

public class ShotArrow : AttackBase
{
    [Header("ArrowSettings")] [SerializeField]
    private Arrow arrowPrefab;
    
    private Queue<Arrow> arrows = new Queue<Arrow>();

    public override void Attack(Transform startPoint, Transform endPoint)
    {
        PoolArrow(startPoint, endPoint);
    }


    private void PoolArrow(Transform startPoint, Transform endPoint)
    {
        Arrow arrow = null;

        if (arrows.Count > 0)
        {
            for (int i = 0; i < arrows.Count; i++)
            {
                arrow = arrows.Dequeue();
                if (arrow.gameObject.activeSelf == false)
                {
                    BezierArrow(arrow,startPoint, endPoint);
                    arrows.Enqueue(arrow);
                    return;
                }

                arrows.Enqueue(arrow);
            }
        }

        arrow = Instantiate(arrowPrefab, startPoint.position, Quaternion.Euler(0, 0, -180f),
            GameManager.Instance.transform);
        BezierArrow(arrow, startPoint, endPoint);
        arrows.Enqueue(arrow);
    }

    private void BezierArrow(Arrow arrow, Transform startPoint, Transform endPoint)
    {
        arrow.gameObject.SetActive(true);
        Vector2 p0 = startPoint.position;
        Vector2 p1 = Vector2.up * 8f;
        Vector2 p2 = endPoint.transform.position;
        arrow.ShotArrow(p0, p1, p2);
    } 

}