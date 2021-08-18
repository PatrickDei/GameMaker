using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class SelectionController : MonoBehaviour
{
    public static int SelectionStep { get; set; }

    public static void SelectItem(GameObject selectedObject)
    {
        switch (selectedObject.tag)
        {
            case "Map": 
                GameInstance.SharedInstance.MapName = selectedObject.name;
                Debug.LogFormat("Selected map: {0}", GameInstance.SharedInstance.MapName);
                break;

            default:
                Debug.Log("Type not recognised");
                break;
        }
    }

    public static void NextSelectionStep()
    {
        SelectionStep++;

        GameObject buttonHolder = GameObject.Find("Buttons");

        foreach (Transform child in buttonHolder.transform)
            GameObject.Destroy(child.gameObject);


        switch (SelectionStep)
        {
            case 1:
                GameObject newButton = Instantiate(Resources.Load<GameObject>("Prefabs/DefaultButton"), new Vector3(), Quaternion.identity);
                newButton.transform.parent = buttonHolder.transform;
                break;
            default:
                Debug.LogFormat("Rule selection steps have been de-synchronised. Current step: {0}", SelectionStep);
                break;
        }
    }
}
