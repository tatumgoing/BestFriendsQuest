using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MaterialColorChangeData
{
    [SerializeField] private MeshRenderer _renderer;
    [SerializeField] private int _materialIndex;
    [SerializeField] private Color _blendColor = Color.black;
    [SerializeField] private float _blendPercent = 0.1f;

    public void Apply(Color inputColor)
    {
        var color = Color.Lerp(inputColor, _blendColor, _blendPercent);
        _renderer.materials[_materialIndex].color = color;
    }
}

public class CharacterRoomModel : MonoBehaviour
{
    [SerializeField] private Transform _characterSpawnSpot;
    [SerializeField] private Color _favoriteColor;
    [SerializeField] private List<MaterialColorChangeData> _colorChangeObjects = new List<MaterialColorChangeData>();

    private GameObject _spawnedCharacter;

    public void Hide() => gameObject.SetActive(false);

    public void Show(ID id)
    {
        gameObject.SetActive(true);
        SpawnCharacter(id);
    }

    public void UpdateColor()
    {

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
