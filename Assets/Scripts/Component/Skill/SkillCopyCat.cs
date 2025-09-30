using System.Threading;
using Component.Skill;
using Cysharp.Threading.Tasks;
using MVC.Controller.CopyCat;
using UnityEngine;

namespace Component.SkillSystem
{
    public class SkillCopyCat : SkillBase
    {
        [Header("Skill Settings")]
        [SerializeField] private GameObject cloningObject;
        [SerializeField] private int hp;
        [SerializeField] private float duration;

        public override void Initialize(Rigidbody2D rigidbody, Animator anim)
        {
            base.Initialize(rigidbody, anim);
            SkillType = SkillType.CopyCat;
            AnimationName = "SkillCasting";
        }
        public override async UniTask SkillTask(CancellationToken cancellationToken)
        {
            await UniTask.Delay(System.TimeSpan.FromSeconds(1f), cancellationToken: cancellationToken);
            var copyCat = Instantiate(cloningObject, transform.position, Quaternion.identity);
            copyCat.GetComponent<CopyCatController>().Init();
        }
    }
}