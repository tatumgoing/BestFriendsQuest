using MyBox;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterProfileDataDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _idText;
    [SerializeField] private TextMeshProUGUI _expText;
    [SerializeField] private TextMeshProUGUI _lnText;
    [SerializeField] private TextMeshProUGUI _fnText;
    [SerializeField] private TextMeshProUGUI _addressText;
    [SerializeField] private TextMeshProUGUI _birthdayText;
    [SerializeField] private TextMeshProUGUI _signatureText;


    [SerializeField] private Image _favoriteColor;
    [SerializeField] private TextMeshProUGUI _genderText;
    [SerializeField] private TextMeshProUGUI _attractionText;
    [SerializeField] private TextMeshProUGUI _pronounText;
    [SerializeField] private Image _portrait;
    [SerializeField] private List<Color> _favoriteColors = new List<Color>();

    public void Show(ID id)
    {
        gameObject.SetActive(true);

        var cMan = CharacterManager.i;
        _portrait.sprite = cMan.GetPortrait(id);

        var idNum = (int)id;
        var idNum2 = (idNum + 923939492223) % 9999;
        var idNum3 = (idNum2 + 98065646897) % 9999;

        _idText.text = idNum + "-" + idNum2 + "-" + idNum3;

        var time = System.DateTime.Now;
        var years = time.Year + 8;
        var month = time.Month;
        var day = time.Day;
        _expText.text = day + "/" + month + "/" + years;

        var name = cMan.GetNameFormatted(id);
        if (!name.Contains(" ")) {
            _lnText.text = "";
            _fnText.text = name;
        }
        else {
            var nameParts = name.Split(' ');
            _fnText.text = nameParts[0];
            nameParts = nameParts.RemoveAt(0);
            if (nameParts.Length > 0) _lnText.text = string.Join(" ", nameParts);
            else _lnText.text = "";
        }
        

        var streets = new List<string>() { "HARTEST ST", "ROLAND RD", "MAIN ST", "MORT AVE", "ROOFTOP DR" };
        var streetNum = Random.Range(100, 9999);
        _addressText.text = streetNum + " " + streets[Random.Range(0, streets.Count)] + "\nCENTRAL, ISLE OF FRIENDSHIP, 44013";

        _birthdayText.text = cMan.GetBirthdayFormatted(id);

        _signatureText.text = name;

        _genderText.text = cMan.GetGender(id).ToString();

        var pronoun = cMan.GetPronoun(id);
        var pronounString = Utils.CapitalFirst(pronoun.ToString().ToLower()) + "/";
        if (pronoun == Pronoun.HE) pronounString += "Him";
        if (pronoun == Pronoun.SHE) pronounString += "Her";
        if (pronoun == Pronoun.THEY) pronounString += "Them";
        _pronounText.text = pronounString;

        var attraction = cMan.GetAttraction(id);

        _attractionText.text = "";
        var male = (attraction & Attraction.MALE) != 0;
        var female = (attraction & Attraction.FEMALE) != 0;
        var nonBinary = (attraction & Attraction.NONBINARY) != 0;
        var attractString = new List<string>();
        if (male) attractString.Add("M");
        if (female) attractString.Add("F");
        if (nonBinary) attractString.Add("NB");
        _attractionText.text = string.Join("/", attractString);

        _favoriteColor.color = _favoriteColors[(int)cMan.GetFavoriteColor(id)];
    }
}
