using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class PowerUpSlotUI : MonoBehaviour
{
    [SerializeField] List<Image> powerUpIcon = new List<Image>();
    public static PowerUpSlotUI instance;
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
    }
    public void SetSlotIcon(int index, Sprite icon)
    {
        powerUpIcon[index].sprite = icon;
        powerUpIcon[index].enabled = true;
    }
    public void RemoveSlotIcon(int index)
    {
        powerUpIcon[index].enabled = false;
    }
}
