using UnityEngine;

[ExecuteAlways]
[RequireComponent(typeof(RectTransform))]
public class ConnectionLine : MonoBehaviour
{
    [SerializeField] private RectTransform start;
    [SerializeField] private RectTransform end;

    private RectTransform rect;
    private RectTransform parentRect;

    private void OnEnable()
    {
        CacheReferences();
        UpdateLine();
    }

    private void OnValidate()
    {
        CacheReferences();
        UpdateLine();
    }

    private void Update()
    {
        UpdateLine();
    }

    private void CacheReferences()
    {
        if (rect == null)
            rect = GetComponent<RectTransform>();

        if (rect != null)
            parentRect = rect.parent as RectTransform;
    }

    private void UpdateLine()
    {
        if (rect == null || parentRect == null || start == null || end == null)
            return;

        Camera cam = null;

        if (parentRect.GetComponentInParent<Canvas>() is Canvas canvas &&
            canvas.renderMode != RenderMode.ScreenSpaceOverlay) {
            cam = canvas.worldCamera;
        }

        Vector2 startScreen = RectTransformUtility.WorldToScreenPoint(cam, start.position);
        Vector2 endScreen = RectTransformUtility.WorldToScreenPoint(cam, end.position);

        if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(parentRect, startScreen, cam, out Vector2 startLocal))
            return;

        if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(parentRect, endScreen, cam, out Vector2 endLocal))
            return;

        Vector2 delta = endLocal - startLocal;
        float length = delta.magnitude;

        rect.anchoredPosition = startLocal;
        rect.localRotation = Quaternion.Euler(0, 0, Mathf.Atan2(delta.y, delta.x) * Mathf.Rad2Deg);
        rect.sizeDelta = new Vector2(length, rect.sizeDelta.y);
    }
}
