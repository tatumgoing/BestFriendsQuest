using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StickerRandomizer : MonoBehaviour
{
    [SerializeField] private List<Image> _spots = new List<Image>();
    [SerializeField] private List<Sprite> _options = new List<Sprite>();
    [SerializeField] private Vector2Int _numRange = new Vector2Int(2, 4);
    [SerializeField] private bool _randomizeOnEnable;

    private void OnEnable()
    {
        if (_randomizeOnEnable) Randomize();  
    }

    [ButtonMethod]
    public void Randomize()
    {
        int numStickers = Utils.Rand(_numRange);
        numStickers = Mathf.Min(_spots.Count, numStickers, _options.Count);
        foreach (var spot in _spots) spot.enabled = false;

        _spots.Shuffle();
        _options.Shuffle();
        for (int i = 0; i < numStickers; i++) {
            _spots[i].enabled = true;
            _spots[i].sprite = _options[i];
            _spots[i].transform.localEulerAngles = Vector3.forward * Random.Range(0, 360);
        }

        if (!Application.isPlaying) Utils.SetDirty(this);
    }
}
