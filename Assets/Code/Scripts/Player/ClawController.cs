using UnityEngine;

/// <summary>
/// Controls opening and closing of claw.
/// </summary>
public class ClawController : MonoBehaviour
{
    // Inspector Settings
    [SerializeField] private float openTargetPos = 40f;
    [SerializeField] private float closedTargetPos = 40f;

    public GameObject leftArm;
    public GameObject rightArm;
    public GameObject backArm;
    public GameObject frontArm;

    private HingeJoint leftArmJoint;
    private HingeJoint rightArmJoint;
    private HingeJoint backArmJoint;
    private HingeJoint frontArmJoint;

    private JointSpring hingeSpring;

    void Start()
    {
        leftArmJoint = leftArm.GetComponent<HingeJoint>();
        rightArmJoint = rightArm.GetComponent<HingeJoint>();
        backArmJoint = backArm.GetComponent<HingeJoint>();
        frontArmJoint = frontArm.GetComponent<HingeJoint>();

        CloseClaw(); // Start closed
    }

    void Update()
    {
        if (Input.GetButtonDown("Jump"))
            OpenClaw();
        
        if (Input.GetButtonUp("Jump"))
            CloseClaw();
    }

    void OpenClaw()
    {
        hingeSpring = leftArmJoint.spring;

        // Left and back arm
        hingeSpring.targetPosition = -openTargetPos;
        leftArmJoint.spring = hingeSpring;
        backArmJoint.spring = hingeSpring;

        // Right and front arm
        hingeSpring.targetPosition = openTargetPos;
        rightArmJoint.spring = hingeSpring;
        frontArmJoint.spring = hingeSpring;
    }

    void CloseClaw()
    {
        hingeSpring = leftArmJoint.spring;

        // Left and back arm
        hingeSpring.targetPosition = closedTargetPos;
        leftArmJoint.spring = hingeSpring;
        backArmJoint.spring = hingeSpring;

        // Right and front arm
        hingeSpring.targetPosition = -closedTargetPos;
        rightArmJoint.spring = hingeSpring;
        frontArmJoint.spring = hingeSpring;
    }
}
