using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AnimationController : MonoBehaviour
{

    public static IEnumerator AnimateObject(GameObject obj, Action<String> callback = null)
    {
        Animation animation = obj.GetComponent<Animation>();

        Debug.Log("Playing animation for obj " + obj.name);

        animation.Play();
        yield return new WaitForSeconds(animation.clip.length);

        callback(obj.name);
    }
}
