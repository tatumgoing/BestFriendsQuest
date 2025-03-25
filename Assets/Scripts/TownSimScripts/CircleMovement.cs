using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleMovement : MonoBehaviour
{
    public float radius;
    public float moveSpeed;
    public Vector2 centerPoint = new Vector2(0, 0);

    private float currentAngle;
    private RectTransform rectTransform;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    void Update()
    {
        currentAngle += moveSpeed * Time.deltaTime;

        // Calculate the new position using trigonometry
        float x = centerPoint.x + Mathf.Cos(currentAngle) * radius;
        float y = centerPoint.y + Mathf.Sin(currentAngle) * radius;

        // Update the RectTransform position
        rectTransform.anchoredPosition = new Vector2(x, y);
    }
}
