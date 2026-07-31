using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CopyText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _sourceText;
    private TextMeshProUGUI _selfText;

    private void Start()
    {
        _selfText = GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        _selfText.text = _sourceText.text;
    }
}
