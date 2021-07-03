using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveDropBox : MonoBehaviour
{
    private float speed => GameManager.instance.dropBoxSpeed;

    [SerializeField] private float leftBounds = -5f;
    [SerializeField] private float rightBounds = 5f;

    private Vector3 leftPos;
    private Vector3 rightPos;

    private bool isMovingLeft = true;

    void Start()
    {
        leftPos = transform.TransformPoint(new Vector3(leftBounds, 0, 0));
        rightPos = transform.TransformPoint(new Vector3(rightBounds, 0, 0));
    }

    void FixedUpdate()
    {
        if (isMovingLeft)
            transform.Translate(Time.deltaTime * speed * transform.TransformDirection(Vector3.left));
        else
            transform.Translate(Time.deltaTime * speed * transform.TransformDirection(Vector3.right));

        if (transform.position.x <= leftPos.x)
            isMovingLeft = false;
        else if (transform.position.x >= rightPos.x)
            isMovingLeft = true;
    }
}
