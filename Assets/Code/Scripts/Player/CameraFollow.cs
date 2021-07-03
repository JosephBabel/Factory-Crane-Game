using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;

    [SerializeField] Vector3 cameraOffset;

    [SerializeField] float followSpeed = 1.0f;

    void LateUpdate()
    {
        Vector3 fixedPlayerPos = new Vector3(player.position.x + cameraOffset.x, cameraOffset.y, player.position.z + cameraOffset.z);
        transform.position = Vector3.Lerp(transform.position, fixedPlayerPos, followSpeed);
    }
}
