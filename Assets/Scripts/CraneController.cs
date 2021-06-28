using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraneController : MonoBehaviour
{
    [SerializeField] private float downSpeed = 10.0f;
    [SerializeField] private float upSpeed = 5.0f;
    [SerializeField] private float xzSpeed = 10.0f;

    [SerializeField] private float vertMoveDelay = 0.5f;
    [SerializeField] private float xzMoveDelay = 0.5f;

    [SerializeField] private float vertBreakDampener = 1.0f;
    [SerializeField] private float xzBreakDampener = 0.5f;
    [SerializeField] private float maxVelocity = 50.0f;

    private Rigidbody rb;

    private bool isMovingXZ = false;
    private bool isMovingDown = false;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        // Horizontal controls (forwards, backwards, left, right)
        if (!isMovingXZ && (Input.GetButton("Horizontal") || Input.GetButton("Vertical")))
        {
            StartCoroutine(MoveXZAfterDelay());
        }

        // Vertical Controls (up, down)
        if(!isMovingDown && Input.GetButton("Jump"))
        {
            StartCoroutine(MoveVertAfterDelay());
        }
    }

    void FixedUpdate()
    {
        if (!isMovingDown) rb.AddForce(Time.deltaTime * upSpeed * Vector3.up);

        if (!isMovingXZ)
        {
            Vector3 breakVec = new Vector3(-rb.velocity.normalized.x, 0, -rb.velocity.normalized.z);
            rb.AddForce(breakVec * xzBreakDampener);
        }
    }

    IEnumerator MoveXZAfterDelay()
    {
        isMovingXZ = true;

        yield return new WaitForSeconds(xzMoveDelay);

        while (isMovingXZ)
        {
            float horzDirection = Input.GetAxis("Horizontal");
            float vertDirection = Input.GetAxis("Vertical");

            Vector3 moveToPosition = (Vector3.right * horzDirection) + (Vector3.forward * vertDirection);

            if (moveToPosition.magnitude > 1) moveToPosition = moveToPosition.normalized;

            rb.AddForce(Time.deltaTime * xzSpeed * moveToPosition);
            rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxVelocity);

            if (!Input.GetButton("Horizontal") && !Input.GetButton("Vertical"))
            {
                rb.velocity = Vector3.zero;
                isMovingXZ = false;
                break;
            }

            yield return new WaitForFixedUpdate();
        }
    }

    IEnumerator MoveVertAfterDelay()
    {
        isMovingDown = true;

        yield return new WaitForSeconds(vertMoveDelay);

        while (isMovingDown)
        {
            if (Input.GetButton("Jump"))
            {
                rb.AddForce(Time.deltaTime * downSpeed * Vector3.down);
                rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxVelocity);
            }
            else
            {
                rb.velocity = Vector3.zero;
                isMovingDown = false;
                break;
            }

            yield return new WaitForFixedUpdate();
        }
    }
}
