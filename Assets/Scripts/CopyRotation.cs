using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CopyRotation : MonoBehaviour
{
    public Transform TargetObject;
    // Start is called before the first frame update
    private void LateUpdate()
    {
        
        transform.rotation= TargetObject.rotation;
        
    }
}
