using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public static void OnSceneLoad(string scene)
    {
        Debug.Log("On scene load called");
        SceneManager.LoadScene(scene);
    }
}