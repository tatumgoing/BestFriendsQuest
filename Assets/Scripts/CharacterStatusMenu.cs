using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterStatusMenu : MonoBehaviour
{
    [SerializeField] private HappinessBar _happinessSlider;
    [SerializeField] private CharacterProfileDataDisplay _profileDisplay;

    [Header("CurrentBestie")]
    [SerializeField] private Image _bestiePortrait;
    [SerializeField] private TextMeshProUGUI _bestieName;
    [SerializeField] private GameObject _bestieParent;

    [Header("Relationships")]
    [SerializeField] private GameObject _relationshipPanelParent;
    [SerializeField] private GameObject _relationshipBannerPrefab;
    [SerializeField] private Transform _relationshipListParent;
    [SerializeField] private GameObject _multiPageParent;
    [SerializeField] private SelectableItem _prevPageButton;
    [SerializeField] private SelectableItem _nextPageButton;
    [SerializeField] private TextMeshProUGUI _pageCoutnerText;

    private List<RelationshipBanner> _spawnedBanners = new List<RelationshipBanner>();
    private int _currentPage;

    public void Show(ID id)
    {
        gameObject.SetActive(true);
        _happinessSlider.Initialize(id);
        _profileDisplay.Show(id);

        BuildRelationshipBanners(id);
    }

    private void BuildRelationshipBanners(ID id)
    {
        foreach (var b in _spawnedBanners) Destroy(b.gameObject);
        _spawnedBanners.Clear();
        _currentPage = 0;

        if (CharacterManager.i.AllIDs().Count < 2) {
            _bestieParent.SetActive(false);
            _relationshipPanelParent.SetActive(false);
            return;
        }
        _bestieParent.SetActive(true);
        _relationshipPanelParent.SetActive(true);

        ID bestFriend = null;
        var bestRelo = Mathf.NegativeInfinity;
        ID enemy = null;
        var worstRelo = Mathf.Infinity;

        var allIds = CharacterManager.i.AllIDs().OrderByDescending(x => CharacterManager.i.GetRelationship(id, x));
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

        _multiPageParent.transform.SetAsLastSibling();
        _multiPageParent.SetActive(_spawnedBanners.Count > 5);
        if (_multiPageParent.activeInHierarchy) {
            UpdatePageDisplay();
        }

        _bestiePortrait.sprite = CharacterManager.i.GetPortrait(bestFriend);
        _bestieName.text = CharacterManager.i.GetNameFormatted(bestFriend);
    }

    private void UpdatePageDisplay()
    {
        for (int i = 0; i < _spawnedBanners.Count; i++) {
            _spawnedBanners[i].gameObject.SetActive(i < (_currentPage + 1) * 5 && i >= _currentPage * 5);
        }
        _pageCoutnerText.text = (_currentPage + 1) + "/" + (Mathf.Floor(_spawnedBanners.Count / 5) + 1);

        _prevPageButton.SetDisabled(_currentPage == 0);
        _nextPageButton.SetDisabled(_spawnedBanners[_spawnedBanners.Count-1].gameObject.activeInHierarchy);
    }

    public void NextPage()
    {
        _currentPage += 1;
        UpdatePageDisplay();
    }

    public void PrevPage()
    {
        _currentPage -= 1;
        UpdatePageDisplay();
    }

    private void SpawnRelationshipBanner(float relationshipValue, ID otherID)
    {
        var newRelationshipEntry = Instantiate(_relationshipBannerPrefab, _relationshipListParent).GetComponent<RelationshipBanner>();
        newRelationshipEntry.ShowRelationship(otherID, relationshipValue);
        if (_spawnedBanners.Count >= 5) newRelationshipEntry.gameObject.SetActive(false); //TEMP

        _spawnedBanners.Add(newRelationshipEntry);
    }

    public void Close()
    {
        gameObject.SetActive(false); 
    }
}
