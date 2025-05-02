using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;

public class BFQResultsScreen : MonoBehaviour
{

    [Header("Sprites")]
    public Image charOne;
    public Image charTwo;
    public Image treasureChest;

    public Sprite chestClosed;
    public Sprite chestOpen;

    [Header("Animation")]

    public GameObject transitionScreen;
    public GameObject successScreen;
    public GameObject resultsScreen;

    public IEnumerator ResultsAnimation(bool succeeded)
    {
        FunAnimator anim = treasureChest.gameObject.GetComponent<FunAnimator>();
        anim.doesRot = false;
        var rotSpeed = anim.rotSpeed;

        yield return new WaitForSeconds(3f);

        anim.doesRot = true;
        anim.rotSpeed = rotSpeed*4;

        yield return new WaitForSeconds(1f);

        treasureChest.sprite = chestOpen;
        anim.rotSpeed = rotSpeed;

        if (succeeded)
        {
            StartCoroutine(SuccessAnimation());
        }
        else
        {
            StartCoroutine(FailAnimation());
        }

    }

    public IEnumerator SuccessAnimation()
    {
        yield return new WaitForSeconds(3f);

    }

    public IEnumerator FailAnimation()
    {
        yield return new WaitForSeconds(3f);
    }


}
