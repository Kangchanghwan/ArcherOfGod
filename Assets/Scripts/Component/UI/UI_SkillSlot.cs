using System;
using System.Collections;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static System.String;

public class UI_SkillSlot : MonoBehaviour
{
    private Image _cooldownImage;
    private TextMeshProUGUI _countDownText;

    private void Awake()
    {
        _cooldownImage = GetComponentInChildren<UI_SkillCooldownImage>().GetComponent<Image>();
        _countDownText = GetComponentInChildren<TextMeshProUGUI>();
    }

    public void StartCooldown(float cooldown)
    {
        _cooldownImage.fillAmount = 1;
        StartCoroutine(CooldownCo(cooldown));
    }

    private IEnumerator CooldownCo(float duration)
    {
        float timePassed = 0;

        while (timePassed < duration)
        {
            timePassed += Time.deltaTime;
            _cooldownImage.fillAmount = 1f - (timePassed / duration);
            _countDownText.text = Mathf.Ceil(duration - timePassed + 0.01f).ToString(CultureInfo.InvariantCulture);
            yield return null;
        }

        _cooldownImage.fillAmount = 0f;
        _countDownText.text = Empty;
    }
}
