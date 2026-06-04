using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapCharacterTokenDispla : MonoBehaviour
{
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
        var IDsInArea = CharacterManager.i.GetIDsByArea(_area);

        var toRemove = new List<int>();
        for (int i = 0; i < _spawnedTokens.Count; i++) {
            if (IDsInArea.Contains(_spawnedTokens[i].ID)) {
                IDsInArea.Remove(_spawnedTokens[i].ID);
                continue;
            }
            else {
                Destroy(_spawnedTokens[i].gameObject);
                toRemove.Add(i);
            }
        }
        for (int i = 0; i < toRemove.Count; i++) {
            _spawnedTokens.RemoveAt(toRemove[i]);
        }
        
        foreach (var t in IDsInArea) SpawnToken(t);
    }

    private void SpawnToken(ID id)
    {
        var newToken = Instantiate(_tokenPrefab, transform.GetChild(0));
        newToken.transform.SetAsFirstSibling();
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
