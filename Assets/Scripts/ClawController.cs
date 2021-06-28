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
        {
            OpenClaw();
        }
        
        if (Input.GetButtonUp("Jump"))
        {
            CloseClaw();
        }
    }

    // ABSTRACTION
    void OpenClaw()
    {
        // Left arm
        hingeSpring = leftArmJoint.spring;
        hingeSpring.targetPosition = -openTargetPos;
        leftArmJoint.spring = hingeSpring;

        // Right arm
        hingeSpring = rightArmJoint.spring;
        hingeSpring.targetPosition = openTargetPos;
        rightArmJoint.spring = hingeSpring;

        // Back arm
        hingeSpring = backArmJoint.spring;
        hingeSpring.targetPosition = -openTargetPos;
        backArmJoint.spring = hingeSpring;

        // Front arm
        hingeSpring = frontArmJoint.spring;
        hingeSpring.targetPosition = openTargetPos;
        frontArmJoint.spring = hingeSpring;
    }

    // ABSTRACTION
    void CloseClaw()
    {
        // Left arm
        hingeSpring = leftArmJoint.spring;
        hingeSpring.targetPosition = closedTargetPos;
        leftArmJoint.spring = hingeSpring;

        // Right arm
        hingeSpring = rightArmJoint.spring;
        hingeSpring.targetPosition = -closedTargetPos;
        rightArmJoint.spring = hingeSpring;

        // Back arm
        hingeSpring = backArmJoint.spring;
        hingeSpring.targetPosition = closedTargetPos;
        backArmJoint.spring = hingeSpring;

        // Front arm
        hingeSpring = frontArmJoint.spring;
        hingeSpring.targetPosition = -closedTargetPos;
        frontArmJoint.spring = hingeSpring;
    }
}
