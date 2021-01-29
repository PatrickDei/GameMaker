using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{

    public static void AnimateObject(GameObject obj)
    {
        if (obj.tag == "MenuItem")
            obj.GetComponent<Animation>().Play();
    }
}
