using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RuntimeQuestData
{
    public Quest QuestData;
    public ID Character1;
    public ID Character2;
    public System.DateTime StartTime;

    public string GetSuccessChanceString() => Mathf.RoundToInt(SuccessChance() * 100) + "%";

    public string GetSaveString()
    {
        var res = new List<string>()
        {
            QuestData.name,
            Character1,
            Character2,
            StartTime.ToString("O")
        };

        return string.Join("|", res);
    }

    public void LoadFromSaveString(string saveString)
    {
        var parts = saveString.Split('|');
        var questName = parts[0];
        Character1 = new ID(parts[1]);
        Character2 = new ID(parts[2]);
        StartTime = System.DateTime.Parse(parts[3], System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.RoundtripKind);

        //Debug.Log("loaded quest: " + questName);
    }

    public RuntimeQuestData() { }
    public RuntimeQuestData(Quest quest, ID character1, ID character2, System.DateTime? startTime = null)
    {
        QuestData = quest;
        Character1 = character1;
        Character2 = character2;
        StartTime = startTime ?? System.DateTime.Now;
    }

    public string GetTimeLeftString()
    {
        var targetTime = StartTime.AddHours(QuestData.completionTime);
        var dif = targetTime - System.DateTime.Now;

        string formatted = string.Format("{0:D2}:{1:D2}:{2:D2}",
        (int)dif.TotalHours,
        dif.Minutes,
        dif.Seconds);

        return formatted;
    }

    public float SuccessChance()
    {
        var relationship = CharacterManager.i.GetRelationship(Character1, Character2);
        return Mathf.Clamp01(relationship / QuestData.relationshipRequirement);
    }

    public string FormatTemplate(string template)
    {
        var name1 = CharacterManager.i.GetNameFormatted(Character1);
        var name2 = CharacterManager.i.GetNameFormatted(Character2);
        return template.Replace("CHAR1", name1).Replace("CHAR2", name2).Replace("ITEM", QuestData.unlockedItem.Name);
    }

    public float percentDone()
    {
        var targetTime = StartTime.AddHours(QuestData.completionTime);
        var totalTime = (float)(targetTime - StartTime).TotalSeconds;
        var timeLeft = (float)(targetTime - System.DateTime.Now).TotalSeconds;
        return 1 - (timeLeft / totalTime);
    }
}
