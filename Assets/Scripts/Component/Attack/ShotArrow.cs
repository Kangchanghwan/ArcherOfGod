using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
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

        public void Attack(ShotArrowCommand command)
        {
            UniTask.FromResult(PoolArrow(command));
        }


        private async UniTask PoolArrow(ShotArrowCommand command)
        {

            var arrow = ObjectPool.Instance.GetObject(arrowPrefab.gameObject, ObjectPool.Instance.transform).GetComponent<Arrow>();
            arrow.damage = command.Damage;
            arrow.duration = command.Duration;
            
            Vector2 p0 = command.StartPoint;
            Vector2 p1 = Vector2.up * 7f;
            Vector2 p2 = command.EndPoint.transform.position;
            await arrow.Shoot(p0, p1, p2);
        }
        
    }
}