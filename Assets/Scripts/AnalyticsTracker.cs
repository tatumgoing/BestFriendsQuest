using JetBrains.Annotations;
using MyBox;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    static string _deviationEyesID = "entry.1855584517";
    static string _deviationNoseID = "entry.1755763105";
    static string _deviationMouthID = "entry.350863271";
    static string _deviationEyebrowsID = "entry.378960691";
    static string _deviationExtrasID = "entry.43626244";
    static string _deviationBodyID = "entry.512424644";

    static string _timeEyesID = "entry.1587597076";
    static string _timeNoseID = "entry.1581770759";
    static string _timeMouthID = "entry.2046527975";
    static string _timeEyebrowsID = "entry.712381159";
    static string _timeExtrasID = "entry.2101347696";
    static string _timeHairID = "entry.157892271";
    static string _timeStrandsID = "entry.524053232";
    static string _timeSkinID = "entry.954402558";
    static string _timeBodyID = "entry.478587131";
    static string _timeDataID = "entry.112423647";

    private GameMode _mode;
    private string _saveString;
    private float _timeSpent;

    private Dictionary<AnalyticsInputType, int> _clicks = new Dictionary<AnalyticsInputType, int>();
    private List<float> _timeTotals = new List<float>();

    public void TickTime(AnalyticsInputType type)
    {
        _timeSpent += Time.deltaTime;
        if (type != AnalyticsInputType.NONE) _timeTotals[(int)type] += Time.deltaTime;
    }

    public ResearchData()
    {
        var clickOptions = Utils.EnumToList<AnalyticsInputType>();
        foreach (var key in clickOptions) _clicks.Add(key, 0);
        _timeTotals = new List<float>() { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0};
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

            {_timeEyesID, _timeTotals[0].ToString()},
            {_timeNoseID, _timeTotals[1].ToString()},
            {_timeMouthID, _timeTotals[2].ToString()},
            {_timeEyebrowsID, _timeTotals[3].ToString()},
            {_timeExtrasID, _timeTotals[4].ToString()},
            {_timeHairID, _timeTotals[5].ToString()},
            {_timeStrandsID, _timeTotals[6].ToString()},
            {_timeSkinID, _timeTotals[7].ToString()},
            {_timeBodyID, _timeTotals[8].ToString()},
            {_timeDataID, _timeTotals[9].ToString()},
        };

        var devtiations = CalculateDeviation();
        foreach (var deviation in devtiations) {
            formData.Add(deviation.Item1, deviation.Item2);
        }

        var content = new FormUrlEncodedContent(formData);
        return content;
    }

    private List<(string, string)> CalculateDeviation()
    {
        var res = new List<(string, string)>();

        var saveString = _saveString.Substring(SaveSystem.IDLength);

        var categories = saveString.Split('|');
        var facePieces = categories[0].Split("&");
        var allFeatures = Resources.LoadAll<FeatureSOData>("FacialFeatures").OrderByDescending(x => x.Priority).ToList();

        List<float> eyeScores = new List<float>();
        List<float> noseScores = new List<float>();
        List<float> mouthScores = new List<float>();
        List<float> eyebrowsScores = new List<float>();
        List<float> extrasScores = new List<float>();

        foreach (var facePart in facePieces) {
            if (facePart.Length <= 1) continue; 
            
            var parts = facePart.Split("~");
            FeatureSOData selected = null;
            foreach (var f in allFeatures) if (f.Icon.name == parts[0]) selected = f;

            var total = 0f;
            total += float.Parse(parts[1].Substring(0, 3)) / 1000; //horizontal position
            total += float.Parse(parts[1].Substring(3, 3)) / 1000; //vertical position
            total += float.Parse(parts[1].Substring(6, 3)) / 1000; //size
            total += float.Parse(parts[1].Substring(9, 3)) / 1000; //rotation
            var average = Mathf.Abs(((total / 4) * 2) - 1);

            if (selected.SubType == FeatureSubType.EYES) eyeScores.Add(average);
            else if (selected.SubType == FeatureSubType.NOSE) noseScores.Add(average);
            else if (selected.SubType == FeatureSubType.LIPS) mouthScores.Add(average);
            else if (selected.SubType == FeatureSubType.BROWS) eyebrowsScores.Add(average);
            else extrasScores.Add(average);
        }

        if (eyeScores.Count > 0) res.Add((_deviationEyesID, eyeScores.Average().ToString()));
        if (noseScores.Count > 0) res.Add((_deviationNoseID, noseScores.Average().ToString()));
        if (mouthScores.Count > 0) res.Add((_deviationMouthID, mouthScores.Average().ToString()));
        if (eyebrowsScores.Count > 0) res.Add((_deviationEyebrowsID, eyebrowsScores.Average().ToString()));
        if (extrasScores.Count > 0) res.Add((_deviationExtrasID, extrasScores.Average().ToString()));

        var bodyParts = categories[4].Split("%");
        List<float> bodyScores = new List<float>();

        foreach (var part in bodyParts) {
            var value = float.Parse(part);
            value = Mathf.Abs((value * 2) - 1);
            bodyScores.Add(value);
        }

        if (bodyScores.Count > 0) res.Add((_deviationBodyID, bodyScores.Average().ToString()));

        return res;
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

        _data.TickTime(_currentInputType);
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
