using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{

    public static void OnSceneLoad(string name)
    {
        Debug.LogFormat("Loading scene {0}", name);
        SceneManager.LoadScene(name);
    }
}