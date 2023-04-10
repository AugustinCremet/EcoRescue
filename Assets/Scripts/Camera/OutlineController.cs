using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutlineController : MonoBehaviour
{
    private Outline _outlineScript;
    private float _outlineWidthOriginal;

    private void Awake()
    {
        _outlineScript = GetComponent<Outline>();
        _outlineWidthOriginal = _outlineScript.OutlineWidth;
    }

    void Update()
    {
        float distToCenterX = Mathf.Abs(Camera.main.WorldToScreenPoint(transform.position).x - Screen.width / 2);
        float distToCenterY = Camera.main.WorldToScreenPoint(transform.position).y - Screen.height / 2;

        if (distToCenterX > 250f || distToCenterY > 200f)
        {
            if (_outlineScript.OutlineMode == Outline.Mode.Nothing)
                _outlineScript.OutlineMode = Outline.Mode.OutlineHidden;
            
            _outlineScript.OutlineWidth = _outlineWidthOriginal;
            
            return;
        }
        
        if(distToCenterX > 100f)
        {
            float width = 1 - (((250 - distToCenterX) / 150f) * _outlineWidthOriginal);
            _outlineScript.OutlineWidth = width;
        }
        else
        {
            _outlineScript.OutlineMode = Outline.Mode.Nothing;
        }
    }
}
