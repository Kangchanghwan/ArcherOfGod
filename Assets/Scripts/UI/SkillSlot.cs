using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UISkillSlot : MonoBehaviour
{
    [SerializeField] private Image cooldownImage;
    [SerializeField] private TextMeshProUGUI countDownText;

 
    public void SetupSkillSlot()
    {

        Color color = Color.black; color.a = .6f;
        cooldownImage.color = color;

        countDownText.text = "1";

    }

    public void StartCooldown(float cooldown)
    {
        cooldownImage.fillAmount = 1;
        StartCoroutine(CooldownCo(cooldown));
    }

    public void ResetCooldown() => cooldownImage.fillAmount = 0;

    private IEnumerator CooldownCo(float duration)
    {
        float timePassed = 0;

        while (timePassed < duration)
        {
            timePassed = timePassed + Time.deltaTime;
            cooldownImage.fillAmount = 1f - (timePassed / duration);
            yield return null;
        }

        cooldownImage.fillAmount = 0;
    }
}
