using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bobber : MonoBehaviour
{

    public bool stuck;
    public bool victory;

    private void Start()
    {
        stuck = false;
        victory = false;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            Debug.Log("Player hit an obstacle!");
            stuck = true;
        }
        if (collision.gameObject.CompareTag("EndGoal"))
        {
            victory = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        stuck=false;
    }


}
