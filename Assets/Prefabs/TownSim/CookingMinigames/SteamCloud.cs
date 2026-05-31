using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SteamCloud : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] float currentTime;

    float startTime;
    float endTime;

    [SerializeField] Image steamCloud;

    public bool tooLate = false;
    public bool clicked = false;

    float rotationSpeed;

    Vector2 movementDirection;

    private void OnEnable()
    {
        steamCloud = GetComponent<Image>();

        currentTime = Time.time;

        //set alpha

        Color c = steamCloud.color;
        c.a = 1;
        steamCloud.color = c;

        rotationSpeed = (Random.Range(50.0f,100.0f)) * (Random.Range(0, 2) * 2 - 1);

        movementDirection = Random.insideUnitCircle.normalized;

    }

    // Update is called once per frame
    void Update()
    {
        SteamFade();

        if (currentTime > endTime)
        {
            tooLate = true;
        }

        transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);

        GetComponent<RectTransform>().anchoredPosition += movementDirection * rotationSpeed * Time.deltaTime;
    }

    public void SetTime(float length)
    {
        startTime = Time.time;
        endTime = Time.time + length;
    }

    public void SteamFade()
    {
        currentTime += Time.deltaTime;

        float lerpV = (currentTime - startTime)/(endTime-startTime);

        steamCloud.color = Color.Lerp(Color.white, Color.red, Mathf.Clamp(lerpV-.3f, 0f, 1f));


        Color c = steamCloud.color;
        c.a = Mathf.Lerp(1, 0, lerpV); 
        steamCloud.color = c;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        clicked= true;

        steamCloud.color = Color.red;
    }
}
