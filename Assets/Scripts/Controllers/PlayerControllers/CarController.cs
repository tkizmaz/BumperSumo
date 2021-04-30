using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{

    private Rigidbody rigidBody;
    [Header("Speed Variables")]
    [Tooltip("Declare the speed of the car.")]
    [SerializeField] private float speedMultiplier;
    [Tooltip("Declare the division value of the car when car goes backwards to slow down.")]
    [SerializeField] private float speedDivisor;
    [Tooltip("Speed multiplier when turning.")]
    [SerializeField] private float turnSpeed;
    [Header("Steering Wheel")]
    [Tooltip("To rotate steering wheel.")]
    [SerializeField] private GameObject steeringWheel;
    [SerializeField] private float steeringWheelRotation;
    [Header("Camera")]
    [Tooltip("To access camera class.")]
    [SerializeField] private CameraController cameraShake;
    [Header("Crash Variables")]
    [Tooltip("To divide magnitude value, because it can be too large to shake.")]
    [SerializeField] private float crashMagnitudeDivider;
    [Tooltip("To declare how much force will be added.")]
    [SerializeField] private float forceMultiplier;

    //To check if car is going forward to turn oppsitely if it is going backwards.
    private float isGoingForward;

    //Checking if gas button is down on UI.
    private bool isGasDown;
    //Checking if brake button is down on UI.
    private bool isBrakeDown;

    //Checking if turn left button is down on UI.
    private bool isTurnLeftDown;

    //Checking if turn right button is down on UI.
    private bool isTurnRightDown;

    // Start is called before the first frame update
    void Start()
    {
        //Setting Rigidbody
        rigidBody = this.GetComponent<Rigidbody>();
    }

    //GasDown function is attached with GasPedal in UI with EventTrigger as Pointer Down.
    public void GasDown()
    {
        //Setting isGasDown as true to speed the car.
        isGasDown = true;
    }

    //GasExit function is attached with GasPedal exit in UI with EventTrigger.
    public void GasExit()
    {
        //Setting isGasDown as false to slow the car.
        isGasDown = false;

    }

    //BrakeDown function is attached with BrakePedal in UI with EventTrigger as Pointer Down.
    public void BrakeDown()
    {
        //Setting isBrakeDown as true to slow down the car.
        isBrakeDown = true;
    }

    //BrakeExit function is attached with BrakePedal exit in UI with EventTrigger.
    public void BrakeExit()
    {
        //Setting isBrakeExit as false to stop slowing the car.
        isBrakeDown = false;
    }

    //LeftKeyDown function is attached with Left Key Button in UI with EventTrigger as Pointer Down.
    public void LeftKeyDown()
    {
        //Setting isTurnLeftDown as true to turn car left.
        isTurnLeftDown = true;
    }

    //LeftKeyExit function is attached with Left Key Button exit in UI with EventTrigger.
    public void LeftKeyExit()
    {
        //Setting isTurnLeftDown as false to stop turning the car left.
        isTurnLeftDown = false;
    }

    //RightKeyDown function is attached with Right Key Button in UI with EventTrigger as Pointer Down.
    public void RightKeyDown()
    {
        //Setting isTurnRightDown as true to turn car right.
        isTurnRightDown = true;
    }

    //RightKeyExit function is attached with Right Key Button exit in UI with EventTrigger.
    public void RightKeyExit()
    {
        //Setting isTurnRightDown as false to stop turning the car right.
        isTurnRightDown = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //Getting velocity of rigidBody.
        Vector3 tempVelocity = rigidBody.velocity;
        //Returns the direction of velocity along z axis to declare car's direction (- is backwards + is forwards).
        isGoingForward = transform.InverseTransformDirection(tempVelocity).z;

        //If isGoingForwards, in other words velocity in z axis is below zero, it means that it goes backwards.
        if(isGoingForward < 0)
        {
            isGoingForward = -1f;
        }

        //If isGoingForwards, in other words velocity in z axis is above zero, it means that it goes forwards.
        else if (isGoingForward > 0)
        {
            isGoingForward = 1f;
        }

        //To declare speed to multiply.
        float tempSpeed = speedMultiplier;
        //Getting current speed by Rigidbody and multiplying with 3.6 to convert it to km/h
        float currentSpeed = rigidBody.velocity.magnitude * 3.6f;

        //Checking if player holds the gas pedal.
        if (isGasDown)
        {
            //If gas pedal is down, add relative force.
            rigidBody.AddRelativeForce(Vector3.forward * tempSpeed * 10);
        }

        //Checking if player holds the brake pedal.
        if (isBrakeDown)
        {
            //If the car goes backwards, dividing speedMultiplier by divisor.
            tempSpeed = speedMultiplier / speedDivisor;
            //If brake pedal is down, add relative force.
            rigidBody.AddRelativeForce(-Vector3.forward * tempSpeed * 10);
        }
        
        //The car must turn iff it has velocity.
        if(currentSpeed > 0)
        {
            //Checking if player holds the left turn button.
            if (isTurnLeftDown)
            {
                
                //Adding torque while turn left button is pressed and checking it if it is going forward or backwards .
                rigidBody.AddTorque(isGoingForward * -Vector3.up * turnSpeed * 10);

                //Rotating steering wheel.
                steeringWheel.transform.Rotate(0f, 0f, steeringWheelRotation);
            }

            //Checking if player holds the right turn button.
            if (isTurnRightDown)
            {

                //Adding torque while turn left button is pressed and checking it if it is going forward or backwards .
                rigidBody.AddTorque(isGoingForward * Vector3.up * turnSpeed * 10);

                //Rotating steering wheel.
                steeringWheel.transform.Rotate(0f, 0f, -steeringWheelRotation);
            }


        }
        
    }



    private void OnCollisionEnter(Collision collision)
    {
        //Getting crash magnitude to shake the camera and dividing it with divider to prevent large values.
        float crashPower = collision.relativeVelocity.magnitude/crashMagnitudeDivider;
        //Starting coroutine of camera shaking with 0.1 duration and our crashPower.
        StartCoroutine(cameraShake.CameraShake(0.4f, crashPower));

        //If collided object has a RigidBody.
        if (collision.rigidbody)
        {
            //Adding impulse to it with velocity of the car.
            collision.rigidbody.AddForce(transform.forward * rigidBody.velocity.x * forceMultiplier, ForceMode.Impulse);
        }
    }
}
