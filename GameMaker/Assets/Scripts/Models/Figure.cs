using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Figure
{
    public GameObject Figurine { get; set; }

    public Figure(GameObject prefab)
    {
        Figurine = prefab;
    }

    public void MoveTo(GameObject target, GameObject movedObject)
    {
        Figurine.transform.position = target.transform.position;
    }
}
