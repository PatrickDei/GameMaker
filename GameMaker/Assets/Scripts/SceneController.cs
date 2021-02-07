using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    static void OnSceneLoad(string scene)
    {
        SceneManager.LoadScene(scene);
    }
}