using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonGeneral : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPointerDown()
    {
        GetComponent<RectTransform>().anchoredPosition = new Vector2(transform.anchoredPosition.x, transform.anchoredPosition.x -15);

    }

    public void OnPointerUp()
    {
        GetComponent<RectTransform>().anchoredPosition = new Vector2(transform.anchoredPosition.x, transform.anchoredPosition.x + 15);
    }

} 
