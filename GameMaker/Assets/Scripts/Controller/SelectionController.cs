using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SelectionController : MonoBehaviour
{
    private static int RuleSelectionStep { get; set; }
    private static int ObjectSelectionStep { get; set; }
    private static int SelectedFigureCount { get; set; }


    public static void SelectItem(GameObject selectedObject)
    {
        Debug.LogFormat("Selected object: {0}, tag: {1}", selectedObject.name, selectedObject.tag);

        switch (selectedObject.tag)
        {
            case "Map": 
                GameInstance.SharedInstance.MapName = selectedObject.name;
                Debug.LogFormat("Selected map: {0}", GameInstance.SharedInstance.MapName);
                SelectionController.NextSelectionStep("Object");
                break;
            case "Figure":
                if (GameInstance.SharedInstance.Players.Last().FigureName != null)
                {
                    Debug.LogError("All of the figures have already been selected!");
                    SceneController.OnSceneLoad("RuleSelection");
                }
                else
                {
                    SelectedFigureCount++;
                    GameInstance.SharedInstance.Players[SelectedFigureCount - 1].FigureName = selectedObject.name;

                    if (SelectedFigureCount == GameInstance.SharedInstance.Players.Count)
                        NextObjectSelectionStep();

                    else
                        GameObject.Find("ProgressText").GetComponent<Text>().text = "Choose the figure texture for player " + (SelectedFigureCount + 1).ToString();
                }
                break;

            case "MenuItem":
                NextSelectionStep("Object");
                break;

            case "Rule":
                if(selectedObject.name != "DefaultButton" && selectedObject.name != "DefaultButton(Clone)")
                    ApplyRule(selectedObject);
                NextSelectionStep("Rule");
                break;

            default:
                Debug.LogWarning("Type not recognised");
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

        if (GameInstance.SharedInstance.MovementStyle == "Free movement" && RuleSelectionStep == 1)
            RuleSelectionStep++;

        if (GameInstance.SharedInstance.GameEndCondition.Key != "Reach goal" && RuleSelectionStep == 3)
            RuleSelectionStep++;
        Debug.LogWarningFormat("Ruleselectionstep: {0} ... {1}", RuleSelectionStep, GameInstance.SharedInstance.GameEndCondition.Key);

        Destroy(GameObject.Find(GameInstance.SharedInstance.MapName).GetComponent<BoxCollider>());

        switch (RuleSelectionStep)
        {
            case 1:// transfer to gamecontroller and delegate to this class
                foreach(Transform child in GameObject.Find("Fields").transform)
                {
                    Destroy(child.gameObject.GetComponent<OnClickObject>());
                    child.gameObject.AddComponent<FieldOrderSelector>();
                }
                GameInstance.SharedInstance.Fields = new List<KeyValuePair<string, int>>();

                GameObject instructions = Instantiate(Resources.Load<GameObject>("Prefabs/DefaultMenuText"), new Vector3(600f, 750f, 0), Quaternion.identity);
                instructions.name = "Instructions";
                instructions.transform.GetComponent<Text>().text = "Select fields in order you wish to play with";
                instructions.transform.SetParent(buttonHolder.transform);

                GameObject continueButton = Instantiate(Resources.Load<GameObject>("Prefabs/DefaultButton"), new Vector3(1400f, 100f, 0), Quaternion.identity);
                continueButton.transform.GetChild(0).GetComponent<Text>().text = "continue";
                continueButton.transform.SetParent(buttonHolder.transform);
                break;
            case 2:
                GameEnd endingConditions = GameInstance.GameParameters.GameEnd;
                int i = 0;
                List<string> conditions = endingConditions.Win.Concat(endingConditions.Lose).ToList();

                foreach(var condition in conditions)
                {
                    GameObject o = Instantiate(Resources.Load<GameObject>("Prefabs/DefaultButton"), new Vector3(400f, 750f - i++ * 200f, 0), Quaternion.identity);
                    o.name = condition;
                    o.transform.GetChild(0).GetComponent<Text>().text = condition;
                    o.transform.SetParent(buttonHolder.transform);
                }
                break;
            case 3:
                foreach (Transform child in GameObject.Find("Fields").transform)
                {
                    Destroy(child.gameObject.GetComponent<FieldOrderSelector>());
                    child.gameObject.AddComponent<GoalFieldSelector>();
                }

                GameObject newInstructions = Instantiate(Resources.Load<GameObject>("Prefabs/DefaultMenuText"), new Vector3(600f, 750f, 0), Quaternion.identity);
                newInstructions.name = "Instructions";
                newInstructions.transform.GetComponent<Text>().text = "Select fields you win if you step on";
                newInstructions.transform.SetParent(buttonHolder.transform);

                GameObject continueToLethal = Instantiate(Resources.Load<GameObject>("Prefabs/DefaultButton"), new Vector3(1400f, 100f, 0), Quaternion.identity);
                continueToLethal.transform.GetChild(0).GetComponent<Text>().text = "continue";
                continueToLethal.transform.SetParent(buttonHolder.transform);
                break;

            case 4:
                foreach (Transform child in GameObject.Find("Fields").transform)
                {
                    Destroy(child.gameObject.GetComponent<GoalFieldSelector>());
                    child.gameObject.AddComponent<DeathFieldSelector>();
                }

                GameObject lethalInstructions = Instantiate(Resources.Load<GameObject>("Prefabs/DefaultMenuText"), new Vector3(600f, 750f, 0), Quaternion.identity);
                lethalInstructions.name = "Instructions";
                lethalInstructions.transform.GetComponent<Text>().text = "Select fields that destoy figures";
                lethalInstructions.transform.SetParent(buttonHolder.transform);

                GameObject continueToGameButton = Instantiate(Resources.Load<GameObject>("Prefabs/DefaultButton"), new Vector3(1400f, 100f, 0), Quaternion.identity);
                continueToGameButton.transform.GetChild(0).GetComponent<Text>().text = "continue";
                continueToGameButton.transform.SetParent(buttonHolder.transform);
                break;

            case 5:
            case 6:
                SceneController.OnSceneLoad("Gameplay");
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

        Debug.LogFormat("Object selection step: {0}", ObjectSelectionStep);

        switch (ObjectSelectionStep)
        {
            case 1:
                Resources.FindObjectsOfTypeAll<GameObject>().First(n => n.name == "PlayerNumber").SetActive(true);
                GameObject.Find("ProgressText").GetComponent<Text>().text = "Choose number of players";

                break;

            case 2:
                GameInstance.SharedInstance.Players = new List<Player>();
                
                for (int i = 1; i <= Int32.Parse(GameObject.Find("ChosenPlayerNum").GetComponent<Text>().text); i++)
                    GameInstance.SharedInstance.Players.Add(new Player("Player " + i));

                Debug.LogFormat("Amount of players: {0}", GameInstance.SharedInstance.Players.Count);
                GameObject loader = GameObject.Find("Loader script");
                loader.GetComponent<LoadObject>().enabled = false;
                loader.GetComponent<LoadObject>().AddressablesGroup = "Figures";
                loader.GetComponent<LoadObject>().enabled = true;

                GameObject.Find("ProgressText").GetComponent<Text>().text = "Choose the figure texture for player 1";

                GameObject.Find("PlayerNumber").SetActive(false);

                break;

            case 3:
                GameObject obj = Resources.FindObjectsOfTypeAll<GameObject>().First(n => n.name == "PlayerNumber");
                obj.SetActive(true);
                obj.transform.GetChild(0).GetComponent<Text>().text = "Number of figures";
                GameObject.Find("ChosenPlayerNum").GetComponent<Text>().text = String.Empty;
                GameObject.Find("ProgressText").GetComponent<Text>().text = "Choose number of figures per player";
                break;

            case 4:
                Debug.Log("Number of figures chosen");
                GameInstance.SharedInstance.numOfFiguresPerPlayer = Int32.Parse(GameObject.Find("ChosenPlayerNum").GetComponent<Text>().text);
                SceneController.OnSceneLoad("RuleSelection");
                break;
            default:
                Debug.LogErrorFormat("Object selection steps have been de-synchronised. Current step: {0}", ObjectSelectionStep);
                break;
        }
    }

    private static void ApplyRule(GameObject selectedObject)
    {
        switch (RuleSelectionStep)
        {
            case 0:
                Debug.LogFormat("Selected movement style: {0}", selectedObject.name);
                GameInstance.SharedInstance.MovementStyle = selectedObject.name;
                break;
            case 1:
            case 2:
                Debug.LogFormat("Selected game end condition: {0}", selectedObject.name);
                GameInstance.SharedInstance.GameEndCondition = new KeyValuePair<string, bool>(selectedObject.name, GameInstance.GameParameters.GameEnd.Win.Contains(selectedObject.name));
                break;

            default:
                Debug.LogError("Rule couldn't be applied!");
                break;
        }
    }
}
