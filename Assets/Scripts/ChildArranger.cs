using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildArranger : MonoBehaviour
{
    [SerializeField] private Vector3 _offset = new Vector3(35, 50, 0);
    [SerializeField] private int _numRows;
    [SerializeField] private bool _updateOnValidate;

    private void OnValidate()
    {
        if (_updateOnValidate) Arrange();
    }

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
                _offset.y * y,
                _offset.z * y
            );

            Utils.SetDirty(child);
        }

        Utils.SetDirty(this);
    }
}
