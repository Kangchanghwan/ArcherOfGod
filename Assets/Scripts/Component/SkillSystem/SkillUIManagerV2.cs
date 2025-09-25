namespace System.SkillSystem
{
    using UnityEngine;
    using System.Collections.Generic;

    public class SkillUIManagerV2 : MonoBehaviour
    {
        [System.Serializable]
        public class SkillUIMapping
        {
            public SkillType skillType;
            public UISkillSlot uiSkillSlot;
        }

        [SerializeField] private SkillUIMapping[] skillUIMappings;
        private Dictionary<SkillType, UISkillSlot> uiSlotDictionary;

        private void Awake()
        {
            uiSlotDictionary = new Dictionary<SkillType, UISkillSlot>();
            foreach (var mapping in skillUIMappings)
            {
                uiSlotDictionary[mapping.skillType] = mapping.uiSkillSlot;
            }

            SkillBaseV2.OnSkillCooldownStarted += HandleSkillCooldownStarted;
        }

        private void OnDestroy()
        {
            SkillBaseV2.OnSkillCooldownStarted -= HandleSkillCooldownStarted;
        }

        private void HandleSkillCooldownStarted(SkillEventArgs args)
        {
            if (args.CooldownTime.HasValue && uiSlotDictionary.TryGetValue(args.SkillType, out var uiSlot))
            {
                uiSlot.StartCooldown(args.CooldownTime.Value);
                Debug.Log($"Started cooldown for {args.SkillType}: {args.CooldownTime.Value}s");
            }
        }
    }
}