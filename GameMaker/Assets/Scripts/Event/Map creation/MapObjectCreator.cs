using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapObjectCreator : MonoBehaviour
{

    public void AddObject()
    {
        GameObject newObject = GameObject.CreatePrimitive((gameObject.name == "Cube") ? PrimitiveType.Cube : PrimitiveType.Sphere);
        newObject.transform.position = GameObject.Find("Plane").transform.position;
        FieldPosition.target = newObject;
    }
}
