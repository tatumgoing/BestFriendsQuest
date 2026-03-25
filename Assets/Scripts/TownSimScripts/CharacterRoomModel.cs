using MyBox;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MaterialColorChangeData
{
    [SerializeField] private MeshRenderer _renderer;
    [SerializeField] private int _materialIndex;
    [SerializeField] private Color _tintColor = Color.black;
    [SerializeField, Range(0, 1)] private float _tintBlendPercent = 0.1f;
    [SerializeField, Range(0, 1)] private float _originalBlenderPercent = 0.5f;

    private Color _originalColor;
    private bool _initialized;

    public void Initialize()
    {
        if (_initialized) return;
        _initialized = true;

        _originalColor = _renderer.sharedMaterials[_materialIndex].GetColor("_BASE_COLOR");
    }

    public void Apply(Color inputColor)
    {
        var color = Color.Lerp(inputColor, _tintColor, _tintBlendPercent);
        color = Color.Lerp(color, _originalColor, _originalBlenderPercent);
        _renderer.materials[_materialIndex].SetColor("_BASE_COLOR", color);
    }
}

public class CharacterRoomModel : MonoBehaviour
{
    [SerializeField] private Transform _characterSpawnSpot;
    [SerializeField] private Color _favoriteColor;
    [SerializeField] private List<MaterialColorChangeData> _colorChangeObjects = new List<MaterialColorChangeData>();

    private GameObject _spawnedCharacter;

    public void Hide() => gameObject.SetActive(false);

    private void OnEnable()
    {
        foreach (var obj in _colorChangeObjects) obj.Initialize();
    }

    public void Show(ID id)
    {
        gameObject.SetActive(true);
        SpawnCharacter(id);
    }

    [ButtonMethod]
    public void UpdateColor()
    {
        foreach (var obj in _colorChangeObjects) obj.Apply(_favoriteColor);
    }

    public void SpawnCharacter(ID character)
    {
        if (_spawnedCharacter != null) Destroy(_spawnedCharacter);
        _spawnedCharacter = CharacterManager.i.SpawnCharacter(character, _characterSpawnSpot).gameObject;
    }
    
    private void OnDisable()
    {
        if (_spawnedCharacter != null) Destroy(_spawnedCharacter);
    }
}
