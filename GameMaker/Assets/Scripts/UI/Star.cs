using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Star : MonoBehaviour
{
    private float Offset = -1;
    public string FieldName { get; set; }

    private float HeightOffset(GameObject target)
    {
        if (Offset == -1)
        {
            Offset = target.GetComponent<MeshRenderer>().bounds.size.y / 2 + GetComponent<MeshRenderer>().bounds.size.y / 2;
            return Offset;
        }
        return Offset;
    }

    public void MoveTo(GameObject target)
    {
        transform.position = target.transform.position;
        transform.position += new Vector3(0, HeightOffset(target), 0);
    }
}
