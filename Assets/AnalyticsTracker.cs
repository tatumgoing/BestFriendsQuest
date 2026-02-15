using MyBox;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using UnityEngine;

public enum AnalyticsInputType { EYES, NOSE, MOUTH, EYEBROWS, EXTRAS, HAIR, STRANDS, SKIN, BODY, DATA, NONE}

[System.Serializable]
public class ResearchData
{
    static string _advQuestionID = "entry.1812678872";
    static string _saveStringID = "entry.2023510223";
    static string _timeSpentID = "entry.1037860004";

    static string _numClicksEyesID = "entry.673637550";
    static string _numClicksNoseID = "entry.1479917884";
    static string _numClicksMouthID = "entry.711107730";
    static string _numClicksEyebrowsID = "entry.477156503";
    static string _numClicksExtrasID = "entry.319009139";
    static string _numClicksHairID = "entry.1304621926";
    static string _numClicksStrandsID = "entry.1642542334";
    static string _numClicksSkinID = "entry.634606026";
    static string _numClicksDataID = "entry.874449072";
    static string _numClicksBodyID = "entry.1293387308";

    private GameMode _mode;
    private string _saveString;
    private float _timeSpent;

    private Dictionary<AnalyticsInputType, int> _clicks = new Dictionary<AnalyticsInputType, int>();

    public void TickTime() => _timeSpent += Time.deltaTime;

    public ResearchData()
    {
        var clickOptions = Utils.EnumToList<AnalyticsInputType>();
        foreach (var key in clickOptions) _clicks.Add(key, 0);
    }

    public void Finalize(string saveString)
    {
        _saveString = saveString;
        _mode = GameManager.i.Advanced ? GameMode.ADVANCED : GameMode.SIMPLE;
    }

    public void SubtractClick(AnalyticsInputType type)
    {
        if (type == AnalyticsInputType.NONE) return;

        if (!_clicks.ContainsKey(type)) _clicks.Add(type, 0);
        else _clicks[type]--;
    }

    public void Click(AnalyticsInputType type)
    {
        if (type == AnalyticsInputType.NONE) return;

        if (!_clicks.ContainsKey(type)) _clicks.Add(type, 0);
        _clicks[type]++;
    }

    public FormUrlEncodedContent EncodeData()
    {
        var formData = new Dictionary<string, string>()
        {
            { _advQuestionID, _mode.ToString()},
            { _saveStringID, _saveString},
            { _timeSpentID, Mathf.Round(_timeSpent).ToString()},

            { _numClicksEyesID, _clicks[AnalyticsInputType.EYES].ToString() },
            { _numClicksNoseID, _clicks[AnalyticsInputType.NOSE].ToString() },
            { _numClicksMouthID, _clicks[AnalyticsInputType.MOUTH].ToString() },
            { _numClicksEyebrowsID, _clicks[AnalyticsInputType.EYEBROWS].ToString() },
            { _numClicksExtrasID, _clicks[AnalyticsInputType.EXTRAS].ToString() },
            { _numClicksHairID, _clicks[AnalyticsInputType.HAIR].ToString() },
            { _numClicksStrandsID, _clicks[AnalyticsInputType.STRANDS].ToString() },
            { _numClicksSkinID, _clicks[AnalyticsInputType.SKIN].ToString() },
            { _numClicksBodyID, _clicks[AnalyticsInputType.BODY].ToString() },
            { _numClicksDataID, _clicks[AnalyticsInputType.DATA].ToString() },
        };

        var content = new FormUrlEncodedContent(formData);
        return content;
    }
}

public class AnalyticsTracker : MonoBehaviour
{
    static string _formURL = "https://docs.google.com/forms/d/e/1FAIpQLSd7055nRC71OqJzBFHombwjAR4i2my7YGhUEtZRf-QNLek6QQ/formResponse";
    
    private bool _makingCharacter;
    private ResearchData _data = new ResearchData();
    private AnalyticsInputType _currentInputType;
    private AnalyticsInputType _previousInputType;

    private void Update()
    {
        if (!_makingCharacter) return;

        _data.TickTime();
        if (Input.GetMouseButtonDown(0)) _data.Click(_currentInputType);
    }


    public void SwitchToHair() => SwitchCategory(AnalyticsInputType.HAIR);
    public void SwitchToStrands() => SwitchCategory(AnalyticsInputType.STRANDS);
    public void SwitchToSkin() => SwitchCategory(AnalyticsInputType.SKIN);
    public void SwitchToNone() => SwitchCategory(AnalyticsInputType.NONE);
    public void SwitchToBody() => SwitchCategory(AnalyticsInputType.BODY);
    public void SwitchToData() => SwitchCategory(AnalyticsInputType.DATA);
    public void SwitchToPreviousCategory() => SwitchCategory(_previousInputType);
    public void SwitchCategory(int category) => SwitchCategory( (AnalyticsInputType) category);
    public void SwitchCategory(AnalyticsInputType category)
    {
        _previousInputType = _currentInputType;
        _data.SubtractClick(_previousInputType);
        _currentInputType = category;
    }

    public void StartNew()
    {
        _data = new ResearchData();
        _makingCharacter = true;
    }

    public void FinishCharacter(string saveString)
    {
        _makingCharacter = false;

        _data.Finalize(saveString);
        var content = _data.EncodeData();
        Send(content);
    }

    public static async void Send(FormUrlEncodedContent content)
    {
        try {
            using var client = new HttpClient();

            HttpResponseMessage response = await client.PostAsync(_formURL, content);

            if ((response.IsSuccessStatusCode)) print("Sent data to google form");
            else {
                print(response.StatusCode);

                var text = await response.Content.ReadAsStringAsync();
                print(text);
            }
        }
        catch (System.Exception e) { print("error: " + e); };
    }
}
