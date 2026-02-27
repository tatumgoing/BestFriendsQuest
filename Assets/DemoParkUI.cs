using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DemoParkUI : MonoBehaviour
{
    [SerializeField] private GameObject _endScreenParent;
    [SerializeField] private TextMeshProUGUI _buttonText;

    public void Continue()
    {
        if (!_endScreenParent.activeInHierarchy) {
            _endScreenParent.SetActive(true);
            _buttonText.text = "Finish";
            return;
        }

        SceneManager.LoadScene(0);
    }
}
