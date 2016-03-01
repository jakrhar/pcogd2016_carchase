using UnityEngine;
using System.Collections;

public class BoostPopup : MonoBehaviour
{
    private float timeElaplepsed = 0.0F;
    private Vector2 startPosition, endPosition;

    public float popupDelay = 20.0F;
    public float startFactor = 0.1f;
    public float endFactor = 0.7f;
    private RectTransform rectTransform;
    private bool animationSarted = false;
    private UnityEngine.UI.Text popupText;
    private string text = "";

    void Start()
    {
        rectTransform = gameObject.GetComponent<RectTransform>();
        startPosition = new Vector2(rectTransform.anchoredPosition.x, rectTransform.anchoredPosition.y + (Screen.height) * startFactor);
        endPosition = new Vector2(rectTransform.anchoredPosition.x, rectTransform.anchoredPosition.y + (Screen.height) * endFactor);

        popupText = gameObject.GetComponent<UnityEngine.UI.Text>();
        text = popupText.text;
    }

    /// <summary>
    /// Updates the Popup GUIText.
    /// </summary>
    void Update()
    {
        if (!animationSarted)
        {
            animationSarted = true;
            StartCoroutine(MovePopup());
        }
    }

    IEnumerator MovePopup()
    {
        while (timeElaplepsed < popupDelay)
        {
            rectTransform.anchoredPosition = Vector2.Lerp(startPosition, endPosition, timeElaplepsed / popupDelay);
            int timeLeft = (int)(popupDelay - timeElaplepsed);
            popupText.text = string.Format("{0} {1}", text, timeLeft); 
            timeElaplepsed += Time.deltaTime;
            yield return null;
        }

        //destroys the popup after the delay
        GameObject.Destroy(gameObject);

    }
}
