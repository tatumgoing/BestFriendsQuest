using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

/// <summary>
/// Stores all the nonChanging data about a character, generated from character creator.
/// saved/loaded via saveSystem class
/// </summary>
[System.Serializable]
public class StaticCharacterData
{
    public ID ID;
    public string Name;
    public Gender Gender;
    public Pronoun Pronouns;
    public Attraction Attraction;
    public DateTime Birthday;
    public FavoriteColor FavColor;
    public Sprite Icon;

    private const string seperator = "%";
    private const string dateTimeFormat = "MMddyyyy";

    /// <summary>
    /// Loads character profile data from save file based on ID
    /// for use when you want to load a character's data from file without a characterMetaController
    /// </summary>
    public StaticCharacterData(ID ID)
    {
        this.ID = ID;
        var saveString = SaveSystem.GetStaticSaveString(ID);
        if (saveString == "") {
            Birthday = new DateTime();
            return;
        }

        FromString(ID, saveString.Split('|')[5]);
    }

    /// <summary>
    /// Basic default empty constructor
    /// </summary>
    public StaticCharacterData()
    {
        Birthday = new DateTime();
    }

    public override string ToString()
    {
        var list = new List<string>
        {
            Name,
            Utils.EnumInt(Gender),
            Utils.EnumInt(Pronouns),
            Utils.EnumInt(Attraction),
            Birthday.ToString(dateTimeFormat),
            Utils.EnumInt(FavColor)
        };

        var joined = string.Join(seperator, list);

        if (Name == "" || Name == null || Name.Length < 1) return "___" + joined; 

        return joined;
    }

    /// <summary>
    /// Parses character data from the 5th part of the static save string.
    /// see characterMetaController for format and usage
    /// </summary>
    /// <param name="inputString"></param>
    public void FromString(ID id, string inputString)
    {
        ID = id;
        var parts = inputString.Split(seperator);
        Name = parts[0];
        Gender = Utils.IntEnum<Gender>(parts[1]);
        Pronouns = Utils.IntEnum<Pronoun>(parts[2]);
        Attraction = Utils.IntEnum<Attraction>(parts[3]);
        Birthday = DateTime.ParseExact(parts[4], dateTimeFormat, CultureInfo.InvariantCulture);
        FavColor = Utils.IntEnum<FavoriteColor>(parts[5]);
        Icon = SaveSystem.GetPortrait(ID);
    }

    public void FromStaticSaveString(string saveString)
    {
        ID = new ID(saveString[..SaveSystem.IDLength]);
        saveString = saveString.Substring(ID.ToString().Length);
        if (saveString == "") {
            Birthday = new DateTime();
            return;
        }

        FromString(ID, saveString.Split('|')[5]);
    }
}
