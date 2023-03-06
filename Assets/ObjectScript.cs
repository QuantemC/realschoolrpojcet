using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectScript : MonoBehaviour
{

    public Vector3 Velocity;
    public float Speed;
    public float Mass;

    public void Setup(Vector3 Vel, float Sp, float M){
        Velocity = Vel;
        Speed    = Sp;
        Mass     = M;
    }
}
