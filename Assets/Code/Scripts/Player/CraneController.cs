using UnityEngine;

/// <summary>
/// Manages control of crane movement on XZ-axis and vertically.
/// </summary>
public class CraneController : MonoBehaviour
{
    [Header("Crane Movement Settings")]
    [SerializeField] private float breakDampener = 400f;
    [SerializeField] private float clampVelocity = 12f;

    [Header("Vertical Movement Settings")]
    [SerializeField] private float downForce = 40000f;
    [SerializeField] private float upForce = 80000f;
    [SerializeField] private float clampVertVelocity = 10f;
    [SerializeField] private float ceilingYBuffer = 43f;

    [Header("XZ Movement Settings")]
    [SerializeField] private float xzForce = 50000f;
    [SerializeField] private float clampXZVelocity = 5f;

    private Rigidbody rb => GetComponent<Rigidbody>();

    private bool isMovingXZ = false;
    private bool isMovingDown = false;
    private bool isInDropArea = false;

    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.isRunning)
        {
            // Horizontal controls (forwards, backwards, left, right)
            if (Input.GetButton("Horizontal") || Input.GetButton("Vertical"))
                isMovingXZ = true;
            else
                isMovingXZ = false;

            // Vertical Controls (up, down)
            if (!isMovingDown && Input.GetButton("Jump") && !isInDropArea && transform.position.y >= 20)
            {
                isMovingDown = true;
                AudioManager.instance.StartCraneSound();
            }
            else if ((isMovingDown && !Input.GetButton("Jump")) || transform.position.y < 20)
            {
                isMovingDown = false;
                AudioManager.instance.StopCraneSound();
            }
        }
    }

    void FixedUpdate()
    {
        if (isMovingDown)
            MoveDown();
        else
            MoveUp();

        if (isMovingXZ)
            MoveXZ();
        else
            StopMoveXZ();

        rb.velocity = Vector3.ClampMagnitude(rb.velocity, clampVelocity);
    }

    void MoveXZ()
    {
        float horzDirection = Input.GetAxis("Horizontal");
        float vertDirection = Input.GetAxis("Vertical");

        Vector3 moveToPosition = (Vector3.right * horzDirection) + (Vector3.forward * vertDirection);

        if (moveToPosition.magnitude > 1) 
            moveToPosition = moveToPosition.normalized;

        rb.AddForce(Time.deltaTime * xzForce * moveToPosition, ForceMode.Acceleration);
        rb.velocity = Vector3.ClampMagnitude(rb.velocity, clampXZVelocity);
    }

    void StopMoveXZ()
    {
        Vector3 breakVec = new Vector3(-rb.velocity.normalized.x, 0, -rb.velocity.normalized.z);
        rb.AddForce(breakVec * breakDampener, ForceMode.Acceleration);
    }

    void MoveDown()
    {
        rb.AddForce(Time.deltaTime * downForce * Vector3.down, ForceMode.Acceleration);
        rb.velocity = Vector3.ClampMagnitude(rb.velocity, clampVertVelocity);
    }

    void MoveUp()
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

    void OnTriggerEnter(Collider other)
    {
        isInDropArea = true;
    }

    void OnTriggerExit(Collider other)
    {
        isInDropArea = false;
    }
}
