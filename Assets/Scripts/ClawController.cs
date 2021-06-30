using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClawController : MonoBehaviour
{
    public GameObject leftArm;
    public GameObject rightArm;
    public GameObject backArm;
    public GameObject frontArm;

    private HingeJoint leftArmJoint;
    private HingeJoint rightArmJoint;
    private HingeJoint backArmJoint;
    private HingeJoint frontArmJoint;

    private JointSpring hingeSpring;

    [SerializeField] private float openTargetPos = 40.0f;
    [SerializeField] private float closedTargetPos = 40.0f;

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

    // ABSTRACTION
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

    // ABSTRACTION
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
