using System;
using UnityEngine;

public class ProgressFillShader : MonoBehaviour
{
    [SerializeField] private float progressSteps = 10;
    [SerializeField] private float _FillRateValue;
    [SerializeField] private float progressBorder;
    [SerializeField] private float stepSize;
    
    private Material material;

    private void Start()
    {
        material = GetComponent<Renderer>().material;
        
        progressBorder = GetComponent<MeshFilter>().mesh.bounds.size.y / 2f;
        material.SetFloat("_ProgressBorder", progressBorder);
        
        _FillRateValue = -progressBorder;
        material.SetFloat("_FillRate", _FillRateValue);
        stepSize = (2 * progressBorder) / progressSteps;
    }

    public void ChangeValue(bool increase)
    {
        if (increase)
        {
            _FillRateValue += stepSize;
        }
        else
        {
            _FillRateValue -= stepSize;
        }
        material.SetFloat("_FillRate", _FillRateValue);
    }
}
