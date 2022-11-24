using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
public enum SkillType
{
    PUSH,
    SHIELD,
}
public class SkillCoolDownUIController : MonoBehaviour
{

    [SerializeField] Image cooldownBackground;
    [SerializeField] TextMeshProUGUI cooldownText;
    [SerializeField] float cooldownDuration = 1f;
    public SkillType skillType;
    private void Start()
    {
        cooldownText.enabled = false;
        cooldownBackground.fillAmount = 0;
        //gameObject.SetActive(false);
    }
    public void SetUp(float cdDuration)
    {
        cooldownDuration = cdDuration;
    }
    IEnumerator CoolDownUIEffect()
    {
        cooldownText.enabled = true;
        cooldownBackground.fillAmount = 1;
        float elapsedTime = 0f;
        while (elapsedTime < cooldownDuration)
        {
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
            cooldownText.text = (cooldownDuration - elapsedTime).ToString("0.00");
            cooldownBackground.fillAmount = 1 - elapsedTime / cooldownDuration;
        }
        cooldownText.enabled = false;
    }
    public void OnSkillUse(object sender, EventArgs e)
    {
        StartCoroutine(CoolDownUIEffect());
    }
  
}
