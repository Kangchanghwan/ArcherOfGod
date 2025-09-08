using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotArrow : AttackBase
{
    [Header("ArrowSettings")] [SerializeField]
    private Arrow arrowPrefab;

    private Queue<Arrow> arrows = new Queue<Arrow>();


    public override void Attack()
    {
        PoolArrow();
    }


    private void PoolArrow()
    {
        Arrow arrow = null;

        if (arrows.Count > 0)
        {
            for (int i = 0; i < arrows.Count; i++)
            {
                arrow = arrows.Dequeue();
                if (arrow.gameObject.activeSelf == false)
                {
                    Arrow(arrow);
                    arrows.Enqueue(arrow);
                    return;
                }

                arrows.Enqueue(arrow);
            }
        }

        arrow = Instantiate(arrowPrefab, StartPoint.position, Quaternion.Euler(0, 0, -180f),
            GameManager.Instance.transform);
        Arrow(arrow);
        arrows.Enqueue(arrow);
    }

    private void Arrow(Arrow arrow)
    {
        arrow.gameObject.SetActive(true);
        Vector2 p0 = StartPoint.position + (Vector3.up * 0.5f);
        Vector2 p1 = Vector2.up * 8f;
        Vector2 p2 = EndPoint.transform.position;
        arrow.ShotArrow(p0, p1, p2);
    }
}