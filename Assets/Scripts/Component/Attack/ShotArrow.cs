using System.Collections.Generic;
using Manager;
using UnityEngine;

namespace Component.Attack
{
    public struct ShotArrowCommand
    {
        public int Damage { get; }
        public float Duration { get; }
        public Vector2 StartPoint { get; }
        public Transform EndPoint { get; }

        public ShotArrowCommand(int damage, float duration, Vector2 startPoint, Transform endPoint)
        {
            Damage = damage;
            Duration = duration;
            StartPoint = startPoint;
            EndPoint = endPoint;
        }
    }

    public class ShotArrow : MonoBehaviour
    {
        [Header("ArrowSettings")] [SerializeField]
        private Arrow arrowPrefab;

        private Queue<Arrow> arrows = new Queue<Arrow>();

        public void Attack(ShotArrowCommand command)
        {
            PoolArrow(command);
        }


        private void PoolArrow(ShotArrowCommand command)
        {
            Arrow arrow = null;

            if (arrows.Count > 0)
            {
                for (int i = 0; i < arrows.Count; i++)
                {
                    arrow = arrows.Dequeue();
                    if (arrow.gameObject.activeSelf == false)
                    {
                        BezierArrow(arrow, command.StartPoint, command.EndPoint);
                        arrows.Enqueue(arrow);
                        return;
                    }

                    arrows.Enqueue(arrow);
                }
            }

            arrow = Instantiate(
                arrowPrefab,
                command.StartPoint,
                Quaternion.Euler(0, 0, -180f),
                    GameManager.Instance.transform
            );
            arrow.damage = command.Damage;
            arrow.duration = command.Duration;
            BezierArrow(arrow, command.StartPoint, command.EndPoint);
            arrows.Enqueue(arrow);
        }

        private void BezierArrow(Arrow arrow, Vector2 startPoint, Transform endPoint)
        {
            arrow.gameObject.SetActive(true);
            Vector2 p0 = startPoint;
            Vector2 p1 = Vector2.up * 7f;
            Vector2 p2 = endPoint.transform.position;
            arrow.ShotArrow(p0, p1, p2);
        }
    }
}