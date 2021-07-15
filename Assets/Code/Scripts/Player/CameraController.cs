using System.Collections;
using UnityEngine;

/// <summary>
/// Camera smooth follow a GameObject.
/// </summary>
public class CameraController : MonoBehaviour
{
    // Inspector settings
    [Header("Camera Follow Settings")]
    public Transform followTarget;
    [SerializeField] Vector3 cameraOffset;
    [SerializeField] float followSpeed = 1f;

    [Header("Camera Shake Settings")]
    [SerializeField] float shakeIntensity = 1f;
    [SerializeField] float shakeDuration = 2f;

    void LateUpdate()
    {
        if (GameManager.instance.isRunning)
        {
            Vector3 fixedPlayerPos = new Vector3(followTarget.position.x + cameraOffset.x, cameraOffset.y, followTarget.position.z + cameraOffset.z);
            transform.position = Vector3.Lerp(transform.position, fixedPlayerPos, followSpeed);
        }
    }

    IEnumerator StartCameraShake()
    {
        float timer = 0f;
        float intensity = shakeIntensity;
        while (timer <= shakeDuration)
        {
            if (GameManager.instance.isRunning)
            {
                timer += Time.deltaTime;
                transform.localPosition += Random.insideUnitSphere * intensity;
                intensity = Mathf.Lerp(shakeIntensity, 0f, timer / shakeDuration);
            }
            yield return null;
        }
    }

    public void ShakeCamera() { StartCoroutine(StartCameraShake()); }
}
