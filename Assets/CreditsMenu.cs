using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CreditsMenu : MonoBehaviour
{
    [SerializeField] private List<CreditData> _credits;
    [SerializeField] private GameObject _prefab;
    [SerializeField] private Transform _listParent;
    [SerializeField] private int _numPerPage;
    [SerializeField] private TextMeshProUGUI _pageText;
    [SerializeField] private SelectableItem _prevPage;
    [SerializeField] private SelectableItem _nextPage;

    private List<Credit> _spawnedCredits = new List<Credit>();
    private int _currentPage = 0;

    private void OnEnable()
    {
        foreach (var c in _spawnedCredits) Destroy(c.gameObject);
        _spawnedCredits.Clear();

        foreach (var c in _credits) {
            var newCredit = Instantiate(_prefab, _listParent).GetComponent<Credit>();
            newCredit.Initialize(c);
            _spawnedCredits.Add(newCredit);
        }

        UpdatePageDisplay();
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

    private void UpdatePageDisplay()
    {
        for (int i = 0; i < _listParent.childCount; i++) {
            _listParent.GetChild(i).gameObject.SetActive(i >= _currentPage * _numPerPage && i < (_currentPage + 1) * _numPerPage);
        }

        var maxPages = Mathf.CeilToInt(_listParent.childCount / (float)_numPerPage);
        _prevPage.SetDisabled(_currentPage == 0);
        _nextPage.SetDisabled(_currentPage >= maxPages);
        _pageText.text = (_currentPage + 1) + "/" + (maxPages + 1);
    }
}
