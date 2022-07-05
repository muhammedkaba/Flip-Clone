using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    public void DeActiveAnimator()
    {
        gameObject.GetComponent<Animator>().enabled = false;
    }

}
