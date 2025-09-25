namespace System.SkillSystem
{
    using UnityEngine;
    using Cysharp.Threading.Tasks;

    public class SkillManagerV2 : MonoBehaviour
    {
        [SerializeField] private SkillBaseV2[] skills;

        public async UniTask UseSkill(SkillType skillType)
        {
            var skill = System.Array.Find(skills, s => s.name.Contains(skillType.ToString()));
            if (skill != null && skill.CanUseSkill())
            {
                skill.SetSkillOnCooldown();
                await skill.ExecuteSkill();
            }
        }
    }
}