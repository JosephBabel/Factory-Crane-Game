using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateSkyBox : MonoBehaviour
{
    [SerializeField] private float rotateSpeed = 1.0f;

    void Update()
    {
        RenderSettings.skybox.SetFloat("_Rotation", Time.time * rotateSpeed);
    }
}
