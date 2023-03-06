using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehavior : MonoBehaviour
{

    public float Speed         = 1f;
    public float Mass          = 1f;
    public float Gravity       = 5f;
    public float TurnSpeed     = 4f;
    private float TurnAngle    = 90f;
    private float JumpStrength = 10f;
    private float FallAmt      = 0f;
    private float FallTime     = 0f; 
    private bool  InAir        = false;
    private bool  JumpCd       = false;
    public Vector3 Velocity;
    private GameObject Camera;
    private float RotX;

    void Start()
    {
       Camera = GameObject.Find("Main Camera");
       Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {   
       MainUpd();
    }

    IEnumerator JumpCooldown(){
        JumpCd = true;
        yield return new WaitForSeconds(3);
        JumpCd = false;
    }

    Vector3 DirectionCollisionCheck(Vector3 Direction){
        RaycastHit Hit;

        if(Physics.Raycast(transform.position, Direction, out Hit, .75f) && Hit.transform.tag == "CollisionObject"){
           ObjectScript Script = Hit.transform.GetComponent<ObjectScript>();

           if(Script == null || Script.Velocity == null || Script.Velocity == Vector3.zero){
            return Vector3.zero;
           }else{
            return Velocity - (Script.Velocity * Script.Mass);
           }
        }

        return Direction;
    }

    bool InAirCheck(){
        RaycastHit Hit;

        if(Physics.Raycast(transform.position, -Vector3.up, out Hit, 1) && Hit.transform.tag == "CollisionObject"){
            return false;
        }

        return true;
    }

    void MainUpd(){
        float X        = Input.GetAxis("Horizontal");
        float Z        = Input.GetAxis("Vertical");
        Vector3 HitForce;

        Velocity = (X * transform.right) + (Z * transform.forward);

        HitForce              = DirectionCollisionCheck(Velocity);
        InAir                 = InAirCheck();
        
        Velocity = HitForce;

        if (Input.GetKey(KeyCode.Space) && InAir == false && JumpCd == false){
            JumpCooldown();

            FallAmt = -JumpStrength/20;
            Velocity += new Vector3(0f, -FallAmt, 0f);
        }

        if(InAir){
            FallTime      += Time.deltaTime;
            FallAmt       += Gravity/750 + FallTime/3750;
            Velocity += new Vector3(0f, -FallAmt, 0f);
        }else{
            FallTime = 0;
        }

        float y = Input.GetAxis("Mouse X") * TurnSpeed;
        RotX    = Mathf.Clamp(RotX + Input.GetAxis("Mouse Y") * TurnSpeed, -TurnAngle, TurnAngle);

        Camera.transform.eulerAngles = new Vector3(-RotX, Camera.transform.eulerAngles.y + y, 0);
        Camera.transform.position    = transform.position;

        transform.rotation = Quaternion.Euler(0f, Camera.transform.eulerAngles.y, 0f);
        transform.position = Vector3.Lerp(transform.position, transform.position + Velocity, Time.deltaTime * 3);
    }
    
}