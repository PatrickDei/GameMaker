using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AnimationController : MonoBehaviour
{

    public static IEnumerator AnimateObject(GameObject obj, Action<string> callback)
    {
        Animation animation = obj.GetComponent<Animation>();

        if (obj.tag == "MenuItem")
            animation.Play();
        yield return new WaitForSeconds(animation.clip.length);
        //object name decides the scene
        callback(obj.name);
    }

    /*public void OnAnimationFinish(GameObject obj)
    {
        
    }*/
}
