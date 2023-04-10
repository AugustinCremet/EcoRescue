using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[ExecuteInEditMode, ExecuteAlways]
public class FixTrees : MonoBehaviour
{
    private void OnEnable()
    {
        BoxCollider boxCollider = GetComponent<BoxCollider>();
        if (boxCollider == null)
            return;
        
        GameObject newCollider = new GameObject("Collider");
        newCollider.transform.SetParent(transform);
        newCollider.transform.localPosition = Vector3.zero;
        newCollider.transform.rotation = transform.rotation;
        newCollider.transform.localScale = Vector3.one;


        BoxCollider newBoxCollider = boxCollider;
        //newBoxCollider.transform.SetParent(newCollider.transform);

        Instantiate(newCollider);
        BoxCollider newBox = newCollider.AddComponent<BoxCollider>();
        newBox.center = boxCollider.center;
        //newBox.center = new Vector3(0, boxCollider.center.y, 0);
        newBox.size = boxCollider.size;
        
    }
}
