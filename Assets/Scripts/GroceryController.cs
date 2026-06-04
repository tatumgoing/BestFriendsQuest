using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroceryController : MonoBehaviour
{

    private PostProcessingManager postProcessingManager;
    [SerializeField] private PostProcessingSetting postProcessingSetting;


    private void OnEnable()
    {

        postProcessingManager = PostProcessingManager.i;

        postProcessingManager?.SetPostProcessing(postProcessingSetting);

    }

    private void OnDisable()
    {

        postProcessingManager?.SetGeneric();

    }
}
