﻿using System;
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
                        SceneController.OnSceneLoad("RuleSelection");

                    GameObject.Find("ProgressText").GetComponent<Text>().text = "Choose the figure for player " + (SelectedFigureCount + 1).ToString();
                }
                break;

            case "MenuItem":
                NextSelectionStep("Object");
                break;

            case "Rule":
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

        switch (RuleSelectionStep)
        {
            case 1:
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
            case 2:
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


        switch (ObjectSelectionStep)
        {
            case 1:
                Resources.FindObjectsOfTypeAll<GameObject>().First(n => n.name == "PlayerNumber").SetActive(true);
                GameObject.Find("ProgressText").GetComponent<Text>().text = "Choose number of players";

                break;

            case 2:
                Destroy(GameObject.Find("PlayerNumber"));

                GameInstance.SharedInstance.Players = new List<Player>();
                for (int i = 1; i <= Int32.Parse(GameObject.Find("ChosenPlayerNum").GetComponent<Text>().text); i++)
                    GameInstance.SharedInstance.Players.Add(new Player("Player " + i));

                GameObject loader = GameObject.Find("Loader script");
                loader.GetComponent<LoadObject>().enabled = false;
                loader.GetComponent<LoadObject>().AddressablesGroup = "Figures";
                loader.GetComponent<LoadObject>().enabled = true;

                GameObject.Find("ProgressText").GetComponent<Text>().text = "Choose the figure for player 1";
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
                Debug.LogFormat("Selected game end condition: {0}", selectedObject.name);
                GameInstance.SharedInstance.GameEndCondition = new KeyValuePair<string, bool>(selectedObject.name, GameInstance.GameParameters.GameEnd.Win.Contains(selectedObject.name));
                break;

            default:
                Debug.LogError("Rule couldn't be applied!");
                break;
        }
    }
}
