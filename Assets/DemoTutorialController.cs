using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DemoTutorialType { SHOP, HAPPINES, FREIDNSHIP, COOKING}

[System.Serializable]
public class DemoTutorialData
{
    [HideInInspector] public string DisplayName;
    public DemoTutorialType Type;
    public GameObject TutorialParent;
}

public class DemoTutorialController : MonoBehaviour
{
    [SerializeField] private List<DemoTutorialData> _tutorials = new List<DemoTutorialData>();

    private System.Action _onCompleteCallback = null;

    private void Start()
    {
        HideAll();
    }

    public void ShowTutorial(DemoTutorialType type, System.Action callback = null)
    {
        foreach (var t in _tutorials) if (t.Type == type) t.TutorialParent.SetActive(true);
    }

    private void HideAll()
    {
        foreach (var t in _tutorials) t.TutorialParent.SetActive(false);
    }

    public void CloseTutorial()
    {
        HideAll();
        if (_onCompleteCallback != null) _onCompleteCallback.Invoke();
        _onCompleteCallback = null;
    }
}
