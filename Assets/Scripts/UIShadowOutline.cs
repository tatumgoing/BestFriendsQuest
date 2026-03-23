using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//[RequireComponent(typeof(Outline))]
public class UIShadowOutline : MonoBehaviour
{
    [ButtonMethod]
    public void Set()
    {
        var outline = gameObject.GetOrAddComponent<Outline>();
        outline.effectColor = Color.white;
        outline.effectDistance = Vector2.one * 10;

        Shadow shadow = null;
        foreach (var s in GetComponents<Shadow>()) {
            if (s.GetType() == typeof(Shadow)) {
                shadow = s;
                break;
            }
        }
        if (shadow == null) shadow = gameObject.AddComponent<Shadow>();

        shadow.effectColor = new Color(0, 0, 0, 0.3f);
        shadow.effectDistance = new Vector2(5, -5);

        Utils.SetDirty(gameObject);
    }
}
