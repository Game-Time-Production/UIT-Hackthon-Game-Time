
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;
public class PlayerEnterTextBehavior : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI text;
    [SerializeField] Image backgroundImage;
    private void Start()
    {
        GetComponent<RectTransform>().anchoredPosition = Vector2.left;
        StartCoroutine(FadeCoroutine(4f));
    }
    public IEnumerator FadeCoroutine(float duration)
    {
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            yield return new WaitForEndOfFrame();
            elapsedTime += Time.deltaTime;
            Color backgroundColor = backgroundImage.color;
            Color textColor = text.color;
            backgroundColor.a = duration - elapsedTime;
            textColor.a = duration - elapsedTime;
            backgroundImage.color = backgroundColor;
            text.color = textColor;
        }
        Destroy(gameObject);
    }
}
