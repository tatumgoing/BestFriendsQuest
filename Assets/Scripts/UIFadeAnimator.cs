using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System.Threading.Tasks;

public class UIFadeAnimator : MonoBehaviour
{
    [SerializeField] private float delay = 3f;
    [SerializeField] private float fadeSpeed = 1f;
    TextMeshProUGUI text;
    private bool fading;


    void Update()
    {
        if (fading && text.color.a > 0)
        {
            var newColor = text.color;

            newColor.a -= fadeSpeed * Time.deltaTime;

            text.color = newColor;

        }
    }

    private void OnEnable()
    {
        if (text == null) text = GetComponent<TextMeshProUGUI>();

        fading = false;

        var newColor = text.color;

        newColor.a = 1.0f;

        text.color = newColor;
        FadeOut();
    }

    private async Task FadeOut()
    {
        await Task.Delay(Mathf.RoundToInt(delay * 1000.0f));
        fading= true;
    }
}
