using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterStatusMenu : MonoBehaviour
{
    [SerializeField] private Slider _happinessSlider;
    [SerializeField] private CharacterProfileDataDisplay _profileDisplay;
    [SerializeField] private GameObject _relationshipBannerPrefab;
    [SerializeField] private Transform _relationshipListParent;
    [SerializeField] private CharacterPortraitNameDisplay _bestFriend;
    [SerializeField] private CharacterPortraitNameDisplay _enemy;

    private List<RelationshipBanner> _spawnedBanners = new List<RelationshipBanner>();

    public void Show(ID id)
    {
        gameObject.SetActive(true);
        _happinessSlider.value = CharacterManager.i.GetHappiness(id)/100;
        _profileDisplay.Show(id);

        BuildRelationshipBanners(id);
    }

    private void BuildRelationshipBanners(ID id)
    {
        foreach (var b in _spawnedBanners) Destroy(b.gameObject);
        _spawnedBanners.Clear();

        ID bestFriend = null;
        var bestRelo = Mathf.NegativeInfinity;
        ID enemy = null;
        var worstRelo = Mathf.Infinity;

        var allIds = CharacterManager.i.AllIDs();
        foreach (var otherID in allIds) {
            if (otherID == id) continue;

            var relationshipValue = CharacterManager.i.GetRelationship(id, otherID);
            if (relationshipValue > bestRelo) {
                bestFriend = otherID;
                bestRelo = relationshipValue;
            }
            if (relationshipValue < worstRelo) {
                enemy = otherID;
                worstRelo = relationshipValue;
            }

            SpawnRelationshipBanner(relationshipValue, otherID);
        }

        _bestFriend.Show(bestFriend);
        _enemy.Show(enemy);
    }

    private void SpawnRelationshipBanner(float relationshipValue, ID otherID)
    {
        var newRelationshipEntry = Instantiate(_relationshipBannerPrefab, _relationshipListParent).GetComponent<RelationshipBanner>();
        newRelationshipEntry.ShowRelationship(otherID, relationshipValue, "Buddy");
        _spawnedBanners.Add(newRelationshipEntry);
    }

    public void Close()
    {
        gameObject.SetActive(false); 
    }
}
