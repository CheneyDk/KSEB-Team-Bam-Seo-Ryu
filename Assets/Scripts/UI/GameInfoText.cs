using System.Collections;
using TMPro;
using UnityEngine;

public class GameInfoManager : MonoBehaviour
{
    public static GameInfoManager Instance;

    public TextMeshProUGUI gameInfoPredab;
    public Transform gameInformationUI;
    public float displayTime = 5f;
    public float fadeOutTime = 1f;


    private void Awake()
    {
        Instance = this;
    }

    public void DisplayGameInfo(string message)
    {
        var text = Instantiate(gameInfoPredab, gameInformationUI);
        text.text = message;
        text.color = new Color(text.color.r, text.color.g, text.color.b, 1);

        StartCoroutine(FadeOutText(text));
    }

    private IEnumerator FadeOutText(TextMeshProUGUI text)
    {
        yield return new WaitForSeconds(displayTime);

        float elapsedTime = 0f;
        Color originalColor = text.color;

        while (elapsedTime < fadeOutTime)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeOutTime);
            text.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            yield return null;
        }
        Destroy(text.gameObject);
    }
}
