using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldPosition : MonoBehaviour
{
    [SerializeField] Camera cam;
    [SerializeField] public static GameObject target;

    void Update()
    {
        if (target)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                target.transform.position = new Vector3(hit.point.x, target.transform.position.y, hit.point.z);
            }
        }
    }
}
