using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;


public class HoverTextTownMap : MonoBehaviour
{
    public float xPos;
    public float yPos;


    public TMP_Text locationName;
    public TMP_Text locationDescription;

    public static HoverTextTownMap i;
    void Awake()
    {
        i = this;
    }

    private void Start()
    {
        gameObject.SetActive(false);

        xPos = transform.position.x;
        yPos = transform.position.y;    
    }

    public void ActivateMenu(string newName, string newDesc)
    {
        transform.position = new Vector2(xPos, yPos);

        StopAllCoroutines();

        gameObject.SetActive(true);

        UpdateName(newName);
        UpdateDescription(newDesc);
    }

    public IEnumerator DeactivateMenu()
    {
        yield return new WaitForSeconds(1f);

        while(transform.position.y >= -400)
        {
            float newY = transform.position.y - 500f*Time.deltaTime;

            transform.position= new Vector2(xPos, newY);

            yield return new WaitForSeconds(.01f);

        }
    }

    public  void UpdateName(string newName)
    {
        locationName.text = newName;
    }

    public void UpdateDescription(string newDesc) 
    { 
    
        locationDescription.text = newDesc;
    }
}
