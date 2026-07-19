using MyBox;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MapCharacterTokenDispla : MonoBehaviour
{
    [SerializeField] private Transform _tokenParent;
    [SerializeField] private GameObject _tokenPrefab;
    [SerializeField] private float _spawnRadius = 12;   
    [SerializeField] private AreaName _area;

    private List<CharacterToken> _spawnedTokens = new List<CharacterToken>();

    private void Start()
    {
        CharacterManager.i.OnCharacterMove.AddListener(DisplayTokens);
    }

    private void OnEnable()
    {
        DisplayTokens();
    }

    private void DisplayTokens()
    {
        if (!CharacterManager.i || TownGameManager.i.DemoMode) return;

        var IDsInArea = CharacterManager.i.GetIDsByArea(_area);

        var toRemove = new List<int>();
        for (int i = 0; i < _spawnedTokens.Count; i++) {
            if (IDsInArea.Contains(_spawnedTokens[i].ID)) {
                IDsInArea.Remove(_spawnedTokens[i].ID);
                continue;
            }
            else {
                if (_spawnedTokens[i]) Destroy(_spawnedTokens[i].gameObject);
                toRemove.Add(i);
            }
        }
        toRemove.OrderByDescending(x => x);
        for (int i = 0; i < toRemove.Count; i++) {
            if (_spawnedTokens.Count > toRemove[i])_spawnedTokens.RemoveAt(toRemove[i]);
        }
        
        foreach (var t in IDsInArea) SpawnToken(t);
    }

    private void SpawnToken(ID id)
    {
        var newToken = Instantiate(_tokenPrefab, _tokenParent);
        newToken.transform.SetAsLastSibling();
        newToken.GetComponent<CharacterToken>().Initialize(id);

        var numAttemps = 0;
        while (numAttemps < 50) 
        {             
            var pos = Random.insideUnitCircle.normalized * _spawnRadius;
            newToken.GetComponent<RectTransform>().anchoredPosition = pos;

            var farEnough = _spawnedTokens.TrueForAll(t => Vector2.Distance(t.GetComponent<RectTransform>().anchoredPosition, pos) > 90);
            if (farEnough) break;

            numAttemps++;
        } 

        _spawnedTokens.Add(newToken.GetComponent<CharacterToken>());
    }
}
