using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildArranger : MonoBehaviour
{
    [SerializeField] private Vector2 _offset;
    [SerializeField] private int _numRows;

    [ButtonMethod]
    public void Arrange()
    {
        int childCount = transform.childCount;
        if (childCount == 0) return;

        for (int i = 0; i < childCount; i++) {
            Transform child = transform.GetChild(i);

            int x = i % _numRows;
            int y = i / _numRows;

            child.localPosition = new Vector3(
                _offset.x * x,
                0,
                _offset.y * y
            );

            Utils.SetDirty(child);
        }

        Utils.SetDirty(this);
    }
}
