using System.Threading;
using Component.Impact;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Component.Skill
{
    public class SkillWhirlWind : SkillBase
    {
        [Header("Skill Settings")]
        [SerializeField] private GameObject whirlWind;

        [Header("Whirlwind Settings")]
        [SerializeField] private float pullRadius = 3f;
        [SerializeField] private float swirlSpeed = 2f;
        [SerializeField] private float pullStrength = 0.5f;
        [SerializeField] private float randomness = 0.5f;
        [SerializeField] private float whirlDuration = 10f;
        [SerializeField] private float arrowsDuration = 1f;

        public override string AnimationName => "WhirlWind";
        public override SkillType SkillType => SkillType.WhirlWind;
        

        public override async UniTask SkillTask(CancellationToken cancellationToken)
        {
            var poolObject = ObjectPool.Instance.GetObject(whirlWind, transform);
            poolObject.transform.position = new Vector2(transform.position.x, transform.position.y + 0.5f);
            
            var whirlWindComponent = poolObject.GetComponent<WhirlWind>();
            if (whirlWindComponent != null)
            {
                var command = new WhirlWindCommand
                {
                    target = Target,
                    pullRadius = pullRadius,
                    swirlSpeed = swirlSpeed,
                    pullStrength = pullStrength,
                    randomness = randomness,
                    whirlDuration = whirlDuration,
                    arrowsDuration = arrowsDuration
                };

                whirlWindComponent.Initialize(command);
            }

            await UniTask.Delay(System.TimeSpan.FromSeconds(whirlDuration + 1f), cancellationToken: cancellationToken);
            ObjectPool.Instance.ReturnObject(poolObject);
        }
    }
}