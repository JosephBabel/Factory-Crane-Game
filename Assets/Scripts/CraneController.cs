using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraneController : MonoBehaviour
{
    [SerializeField] private float downForce = 40000.0f;
    [SerializeField] private float upForce = 80000.0f;
    [SerializeField] private float xzForce = 50000.0f;

    [SerializeField] private float breakDampener = 400.0f;
    [SerializeField] private float clampXZVelocity = 5.0f;
    [SerializeField] private float clampVertVelocity = 10.0f;
    [SerializeField] private float clampVelocity = 12.0f;
    [SerializeField] private float ceilingYBuffer = 43.0f;

    private Rigidbody rb;

    private bool isMovingXZ = false;
    private bool isMovingDown = false;
    private bool isInDropArea = false;

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
            StartCoroutine(MoveXZ());

        // Vertical Controls (up, down)
        if(!isInDropArea && !isMovingDown && Input.GetButton("Jump"))
            StartCoroutine(MoveVert());
    }

    void FixedUpdate()
    {
        if (!isMovingDown)
        {
            if (transform.position.y < ceilingYBuffer)
            {
                rb.AddForce(Time.deltaTime * upForce * Vector3.up, ForceMode.Acceleration);

            }
            else
            {
                Vector3 breakVec = new Vector3(0, -rb.velocity.normalized.y, 0);
                rb.AddForce(breakVec * breakDampener, ForceMode.Acceleration);
            }
        }

        if (!isMovingXZ)
        {
            Vector3 breakVec = new Vector3(-rb.velocity.normalized.x, 0, -rb.velocity.normalized.z);
            rb.AddForce(breakVec * breakDampener, ForceMode.Acceleration);
        }

        rb.velocity = Vector3.ClampMagnitude(rb.velocity, clampVelocity);
    }

    IEnumerator MoveXZ()
    {
        isMovingXZ = true;

        while (isMovingXZ)
        {
            float horzDirection = Input.GetAxis("Horizontal");
            float vertDirection = Input.GetAxis("Vertical");

            Vector3 moveToPosition = (Vector3.right * horzDirection) + (Vector3.forward * vertDirection);

            if (moveToPosition.magnitude > 1) 
                moveToPosition = moveToPosition.normalized;

            rb.AddForce(Time.deltaTime * xzForce * moveToPosition, ForceMode.Acceleration);
            rb.velocity = Vector3.ClampMagnitude(rb.velocity, clampXZVelocity);

            if (!Input.GetButton("Horizontal") && !Input.GetButton("Vertical"))
            {
                isMovingXZ = false;
                break;
            }

            yield return new WaitForFixedUpdate();
        }
    }

    IEnumerator MoveVert()
    {
        isMovingDown = true;

        AudioManager.instance.PlayCraneSound();

        while (isMovingDown)
        {
            if (Input.GetButton("Jump"))
            {
                rb.AddForce(Time.deltaTime * downForce * Vector3.down, ForceMode.Acceleration);
                rb.velocity = Vector3.ClampMagnitude(rb.velocity, clampVertVelocity);
            }
            else
            {
                AudioManager.instance.StopCraneSound();
                isMovingDown = false;
                break;
            }

            yield return new WaitForFixedUpdate();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        isInDropArea = true;
    }

    void OnTriggerExit(Collider other)
    {
        isInDropArea = false;
    }
}
