using UnityEngine;

/// <summary>
/// Rotates skybox
/// </summary>
public class RotateSkyBox : MonoBehaviour
{
    // Inspector settings
    [SerializeField] private float rotateSpeed = 1.0f;

    void Update()
    {
        RenderSettings.skybox.SetFloat("_Rotation", Time.time * rotateSpeed);
    }
}
