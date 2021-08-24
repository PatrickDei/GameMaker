using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonLoader
{
    /*public static void LoadButtonsForStep<T>(string step)
    {
        T items = (T)GetPropertyValue(GameInstance.GameParameters, step);
        
        foreach(var item in items)
        {

        }
    }*/
    private static object GetPropertyValue(object src, string propName)
    {
        return src.GetType().GetProperty(propName).GetValue(src, null);
    }
}
