//////////
//  To hide the controllers and set the range of motion during game
//  Script attached to: player
//////////
using System.Collections;
using UnityEngine;
using Valve.VR.InteractionSystem;
using Valve.VR;

public class ShowControllers : MonoBehaviour
{
    private Hand rightHand;
    private Hand leftHand;
    //Flags to indicate first iteration
    private bool[] firsTime = new bool[2];
    private Coroutine coroutine;


    //================================================================================
    void Start()
    {
        firsTime[0] = true;
        firsTime[1] = true;
    }


    //================================================================================
    void Update()
    {
        //Get hands
        rightHand = Player.instance.rightHand;
        leftHand = Player.instance.leftHand;


        //CHECKS RIGHT HAND
        //The first time the hands get active, hide the controllers (hand.skeleton = hand model)
        if (rightHand.isPoseValid && rightHand.skeleton != null && firsTime[0])
        {
            StartCoroutine(SetupHand(rightHand));
            firsTime[0] = false;
        }
        //If a hand is inactive, keeps checking for activation to hide the controllers
        else if(rightHand.isPoseValid == false)
        {
            coroutine = StartCoroutine(CheckForControllerActive(rightHand));
        }


        //CHECKS LEFT HAND
        if (leftHand.isPoseValid && leftHand.skeleton != null && firsTime[1])
        {
            StartCoroutine(SetupHand(leftHand));
            firsTime[1] = false;
        }
        else if (leftHand.isPoseValid == false)
        {
            coroutine = StartCoroutine(CheckForControllerActive(leftHand));
        }
    }



    //================================================================================
    //If a hand is inactive (controller turned off), wait until it is activated and hide the controller
    private IEnumerator CheckForControllerActive(Hand hand)
    {
        //Wait until the next frame to check if the controller has been turned on
        yield return null;

        //If controller has been turned on, hides it
        if(hand.isPoseValid)
        {
            SetupHand(hand);
        }
    }



    //================================================================================
    //Hides the controllers and sets the range of motion
    private IEnumerator SetupHand(Hand hand)
    {
        //Hide controllers
        hand.HideController(false);
       
        //Waits until the hand model is loaded
        while (hand.skeleton == null)
            yield return null;

        //Get a flat hand instead of one with the fingers around the controller
        hand.skeleton.SetRangeOfMotion(EVRSkeletalMotionRange.WithController);
    }
}
