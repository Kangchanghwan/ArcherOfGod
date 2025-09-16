using System.Collections;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static System.String;

public class UISkillSlot : MonoBehaviour
{
    [SerializeField] private Image cooldownImage;
    [SerializeField] private TextMeshProUGUI countDownText;

    public void StartCooldown(float cooldown)
    {
        cooldownImage.fillAmount = 1;
        StartCoroutine(CooldownCo(cooldown));
    }

    private IEnumerator CooldownCo(float duration)
    {
        float timePassed = 0;

        while (timePassed < duration)
        {
            timePassed += Time.deltaTime;
            cooldownImage.fillAmount = 1f - (timePassed / duration);
            countDownText.text = Mathf.Ceil(duration - timePassed + 0.01f).ToString(CultureInfo.InvariantCulture);
            yield return null;
        }

        cooldownImage.fillAmount = 0;
        countDownText.text = Empty;
    }
}
