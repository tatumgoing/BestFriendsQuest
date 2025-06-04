using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float _headZoom;
    [SerializeField] private float _bodyZoom;
    [SerializeField] private float _headYOffset = 0;
    [SerializeField] private float _bodyYOffset = -2;

    [SerializeField] private Vector2 _minMaxZoom;
    [SerializeField] private float _zoomSpeed;
    [SerializeField] private Transform _character;

    private bool _body;
    
    [SerializeField]
    private void Update()
    {
        if (!EventSystem.current.IsPointerOverGameObject()) Scroll();
    }

    [ButtonMethod]
    public void SetHeadDistance()
    {
        SetDist(_headZoom, _headYOffset);
        _body = false;
    }

    [ButtonMethod]
    public void SetBodyDistance()
    {
        SetDist(_bodyZoom, _bodyYOffset);
        _body = true;
    }

    public void SetDist(float dist, float yOffset = 0)
    {
        var dir = (_character.position - transform.position).normalized;
        dir.y = 0;
        transform.position = _character.position;
        transform.position -= dir * dist;
        transform.position += Vector3.up * yOffset;
    }

    private void Scroll()
    {
        var dist = Vector3.Distance(transform.position, _character.position);
        var dir = (_character.position - transform.position).normalized;

        float baseZoom = _headZoom;
        if (_body) baseZoom = _bodyZoom;
        var limits = new Vector2(_minMaxZoom.x + baseZoom, _minMaxZoom.y + baseZoom);

        float scrollDelta = Input.mouseScrollDelta.y;

        var ES = EventSystem.current;
        if (ES && ES.IsPointerOverGameObject() || !Application.isFocused) scrollDelta = 0;
        var mousePos = Input.mousePosition;
        if (mousePos.x < 0 || mousePos.x > Screen.width || mousePos.y < 0 || mousePos.y > Screen.height) scrollDelta = 0;
#if UNITY_EDITOR
        //if (!UnityEditor.EditorWindow.focusedWindow || UnityEditor.EditorWindow.focusedWindow.titleContent.text != "Game") scrollDelta = 0;
#endif

        var dirMod = scrollDelta * _zoomSpeed * Time.deltaTime * 10;
        if ( (dist + dirMod > limits.y && scrollDelta < 0) || (dist + dirMod < limits.x && scrollDelta > 0)) return;
        
        var posDelta = dir * dirMod;
        transform.position += posDelta;
    }
}
