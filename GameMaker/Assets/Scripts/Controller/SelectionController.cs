using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
            case "Figure":
                GameInstance.SharedInstance.Players.Add(new PlayerInstance(new List<Figure>(), selectedObject));
                GameInstance.SharedInstance.Players.First().Figures.Add(new Figure());
                break;

            default:
                Debug.Log("Type not recognised");
                break;
        }
    }

    public static void NextSelectionStep(string type)
    {
        if (type == "Rule")
            NextRuleSelectionStep();
        else if (type == "Object")
            NextObjectSelectionStep();
    }

    private static void NextRuleSelectionStep()
    {
        SelectionStep++;

        GameObject buttonHolder = GameObject.Find("Buttons");

        foreach (Transform child in buttonHolder.transform)
            GameObject.Destroy(child.gameObject);


        switch (SelectionStep)
        {
            case 1:
                GameObject newButton = Instantiate(Resources.Load<GameObject>("Prefabs/DefaultButton"), new Vector3(), Quaternion.identity);
                newButton.transform.SetParent(buttonHolder.transform);
                break;
            default:
                Debug.LogFormat("Rule selection steps have been de-synchronised. Current step: {0}", SelectionStep);
                break;
        }
    }

    private static void NextObjectSelectionStep()
    {
        SelectionStep++;

        GameObject buttonHolder = GameObject.Find("SelectionArea");

        foreach (Transform child in buttonHolder.transform)
            GameObject.Destroy(child.gameObject);


        switch (SelectionStep)
        {
            case 1:
                GameObject loader = GameObject.Find("Loader script");
                loader.GetComponent<LoadObject>().enabled = false;
                loader.GetComponent<LoadObject>().AddressablesGroup = "Figures";
                loader.GetComponent<LoadObject>().enabled = true;
                break;
            default:
                Debug.LogFormat("Object selection steps have been de-synchronised. Current step: {0}", SelectionStep);
                break;
        }
    }
}
