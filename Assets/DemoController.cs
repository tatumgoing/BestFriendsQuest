using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class AreaUnlockedData
{
    public AreaName Area;
    public bool Unlocked;
}

public class DemoController : MonoBehaviour
{
    public static DemoController i;

    public List<AreaUnlockedData> _data = new List<AreaUnlockedData>();

    private void Awake()
    {
        i = this;
    }

    public bool IsUnlocked(AreaName area)
    {
        if (!TownGameManager.i.DemoMode) return false;

        var selected = _data.Where(x => x.Area == area).ToList();
        if (selected.Count > 0) return selected[0].Unlocked;
        return true;
    }
}
