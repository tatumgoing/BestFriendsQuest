using UnityEngine;

[System.Serializable]
public class RuntimeQuestData
{
    public Quest QuestData;
    public ID Character1;
    public ID Character2;
    public System.DateTime StartTime;

    public string GetSuccessChanceString() => Mathf.RoundToInt(SuccessChance() * 100) + "%";

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
}
