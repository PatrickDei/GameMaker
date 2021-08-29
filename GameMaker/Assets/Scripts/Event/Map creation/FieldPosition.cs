﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FieldPosition : MonoBehaviour
{
    [SerializeField] GameObject parent;
    [SerializeField] public static GameObject target = null;

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
        if (Input.GetMouseButtonDown(0) && target)
        {
            target.transform.SetParent(parent.transform);

            List<string> names = new List<string>();
            foreach (Transform child in parent.transform)
            {
                names.Add(child.name);
            }
            target.name = UnityEditor.ObjectNames.GetUniqueName(names.ToArray(), target.name);

            target = null;
        }
    }
}
