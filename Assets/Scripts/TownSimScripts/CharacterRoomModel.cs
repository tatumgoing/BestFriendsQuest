using MyBox;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class MaterialColorChangeData
{
    [HideInInspector] public string DisplayName;
    [SerializeField] private MeshRenderer _renderer;
    [SerializeField] private List<MeshRenderer> _additional = new List<MeshRenderer>();
    [SerializeField] private int _materialIndex;
    [SerializeField] private Color _tintColor = Color.black;
    [SerializeField, Range(0, 1)] private float _tintBlendPercent = 0.1f;
    [SerializeField, Range(0, 1)] private float _originalBlenderPercent = 0.5f;

    private Color _originalColor;
    private bool _initialized;

    public void OnValidate()
    {
        if (_renderer == null) DisplayName = "Missing Renderer";
        _materialIndex = Mathf.Clamp(_materialIndex, 0, _renderer.sharedMaterials.Length-1);
        DisplayName = _renderer.gameObject.name + " (" + _renderer.sharedMaterials[_materialIndex].name + ")";
    }

    public void Initialize()
    {   
        if (_initialized) return;
        _initialized = true;

        //Debug.Log("trying to get color for material: " + _renderer.sharedMaterials[_materialIndex].name + " for object: " + _renderer.gameObject);
        _originalColor = _renderer.sharedMaterials[_materialIndex].GetColor("_BASE_COLOR");
    }

    public void Apply(Color inputColor)
    {
        var color = Color.Lerp(inputColor, _tintColor, _tintBlendPercent);
        color = Color.Lerp(color, _originalColor, _originalBlenderPercent);
        _renderer.materials[_materialIndex].SetColor("_BASE_COLOR", color);

        foreach (var obj in _additional)
        {
            obj.materials[_materialIndex].SetColor("_BASE_COLOR", color);
        }
    }
}

public class CharacterRoomModel : MonoBehaviour
{
    [SerializeField] private Transform _characterSpawnSpot;
    [SerializeField] private Color _favoriteColor;
    [SerializeField] private List<ColorData> _favoriteColors;
    [SerializeField] private List<MaterialColorChangeData> _colorChangeObjects = new List<MaterialColorChangeData>();
    [SerializeField] private Transform _camera;

    private SpawnedCharacter _spawnedCharacter;

    public void Hide() => gameObject.SetActive(false);

    //TESTING
    private int _currentFavorite;

    private void OnValidate()
    {
        var options = Utils.EnumToList<FavoriteColor>();
        while (_favoriteColors.Count < options.Count) _favoriteColors.Add(new ColorData());
        for (int i = 0; i < _favoriteColors.Count; i++) {
            if (i >= options.Count) {
                _favoriteColors.RemoveAt(i);
                i -= 1;
            }
            else {
                _favoriteColors[i].Color = options[i];
                _favoriteColors[i].DisplayName = options[i].ToString();
            }
        }

        foreach (var obj in _colorChangeObjects) obj.OnValidate();
    }

    private void OnEnable()
    {
        foreach (var obj in _colorChangeObjects) obj.Initialize();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q)) {
            _currentFavorite += 1;
            _favoriteColor = _favoriteColors[_currentFavorite % _favoriteColors.Count].UseColor;
            UpdateColor();
        }
    }

    public void WearClothingItem(ItemData clothing)
    {
        _spawnedCharacter.WearOrMakeOutfit(clothing);
    }

    public void Show(ID id)
    {
        gameObject.SetActive(true);
        SpawnCharacter(id);

        UpdateColor(CharacterManager.i.GetFavoriteColor(id));
    }

    private void UpdateColor(FavoriteColor color)
    {
        _favoriteColor = _favoriteColors[(int)color].UseColor;
        UpdateColor();
    }

    [ButtonMethod]
    public void UpdateColor()
    {
        foreach (var obj in _colorChangeObjects) obj.Apply(_favoriteColor);
    }

    public void SpawnCharacter(ID character)
    {
        if (_spawnedCharacter != null) Destroy(_spawnedCharacter);
        _spawnedCharacter = CharacterManager.i.SpawnCharacter(character, _characterSpawnSpot);
        _spawnedCharacter.CharacterLookAt(_camera, true);
    }
    
    private void OnDisable()
    {
        if (_spawnedCharacter != null) Destroy(_spawnedCharacter.gameObject);
    }
}
