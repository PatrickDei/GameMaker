using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SelectionController : MonoBehaviour
{
    public static int RuleSelectionStep { get; set; }
    public static int ObjectSelectionStep { get; set; }

    public static void SelectItem(GameObject selectedObject)
    {
        switch (selectedObject.tag)
        {
            case "Map": 
                GameInstance.SharedInstance.MapName = selectedObject.name;
                Debug.LogFormat("Selected map: {0}", GameInstance.SharedInstance.MapName);
                SelectionController.NextSelectionStep("Object");
                break;
            case "Figure":
                GameInstance.SharedInstance.Players = new List<Player>(
                    new Player[] {
                        new Player(
                            new List<Figure>(new Figure[] { new Figure() }), selectedObject.name
                        ) 
                    });
                GameInstance.SharedInstance.Players.First().Figures.Add(new Figure());
                GameInstance.SharedInstance.Players.First().Name = selectedObject.name;
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
        RuleSelectionStep++;

        GameObject buttonHolder = GameObject.Find("Buttons");

        foreach (Transform child in buttonHolder.transform)
            GameObject.Destroy(child.gameObject);


        switch (RuleSelectionStep)
        {
            case 1:
                GameObject newButton = Instantiate(Resources.Load<GameObject>("Prefabs/DefaultButton"), new Vector3(), Quaternion.identity);
                newButton.transform.SetParent(buttonHolder.transform);
                break;
            default:
                Debug.LogFormat("Rule selection steps have been de-synchronised. Current step: {0}", RuleSelectionStep);
                break;
        }
    }

    private static void NextObjectSelectionStep()
    {
        ObjectSelectionStep++;

        GameObject buttonHolder = GameObject.Find("SelectionArea");

        foreach (Transform child in buttonHolder.transform)
            GameObject.Destroy(child.gameObject);


        switch (ObjectSelectionStep)
        {
            case 1:
                GameObject loader = GameObject.Find("Loader script");
                loader.GetComponent<LoadObject>().enabled = false;
                loader.GetComponent<LoadObject>().AddressablesGroup = "Figures";
                loader.GetComponent<LoadObject>().enabled = true;

                GameObject.Find("ProgressText").GetComponent<Text>().text = "Choose the figure";
                break;
            default:
                Debug.LogFormat("Object selection steps have been de-synchronised. Current step: {0}", ObjectSelectionStep);
                break;
        }
    }
}
