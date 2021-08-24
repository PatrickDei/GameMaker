using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Figure
{
    public GameObject Figurine { get; set; }
    private static float Offset = -1;

    private float HeightOffset(GameObject target)
    {
        if (Offset == -1) {
            Offset = target.GetComponent<MeshRenderer>().bounds.size.y / 2 + Figurine.GetComponent<MeshRenderer>().bounds.size.y / 2;
            return Offset;
        }
        return Offset;
    }

    public Figure(GameObject prefab)
    {
        Figurine = prefab;
    }

    public void MoveTo(GameObject target)
    {
        Figurine.transform.position = target.transform.position;
        Figurine.transform.position += new Vector3(0, HeightOffset(target), 0);
    }
}
