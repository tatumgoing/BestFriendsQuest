using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DemoParkUI : MonoBehaviour
{
    [SerializeField] private GameObject _endScreenParent;
    [SerializeField] private TextMeshProUGUI _buttonText;
    [SerializeField] private IdPhotoController _idPhoto;

    public void Continue()
    {
        if (!_endScreenParent.activeInHierarchy) {
            _endScreenParent.SetActive(true);
            _buttonText.text = "Finish";

            var character = CharacterManager.i.allCharacters.Last();
            _idPhoto.ShowPicture(character);

            return;
        }

        SceneManager.LoadScene(0);
    }
}
