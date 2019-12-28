using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Base Script for turrets
//Functions 
// Rotate, shoot, look, lock on target
// Problem is Bullet force or target velocity
public class Turret : MonoBehaviour
{
    public TurretScanner rangeFinder;
    public GameObject target;
    public GameObject turretBase;
    public float turnSpeed = 5f;
    [Range(-1, 1)]
    public float angleOffset = 0f;
    public float projectileForce = 75;
    public Shooting shooting;
    public float angleFixer = 90f;
    public Rigidbody2D rb;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    void FixedUpdate()
    {
        Aim();

    }

    void Aim()
    { // http://wiki.unity3d.com/index.php?title=Calculating_Lead_For_Projectiles&_ga=2.235450625.1677248304.1577098127-707113775.1576845268
        target = rangeFinder.targets[0];
        // Debug.Log("Location Turret, Turret Base: " + turretBase.position + " " + this.gameObject.transform.position);
        //  Debug.Log("Location Target: " + target.gameObject.transform.position);

        // formula for leading turret Aim
        /*
        float distance = Vector3.Distance(turretPos, targetPos);//distance in between in meters
        float travelTime = distance/turretMuzzleVelocity;//time in seconds the shot would need to arrive at the target
        Vector3 aimPoint = target.position +targetVelocity*travelTime;
        */

        Vector2 turretPosition = turretBase.gameObject.transform.position;
        Vector2 targetPosition = target.gameObject.transform.position;
        float distance = Vector2.Distance(turretPosition, targetPosition);
        float travelTime = distance / (shooting.bulletForce);
        Vector2 otherCalculatedVector = targetPosition + target.gameObject.GetComponent<Rigidbody2D>().velocity * travelTime;
        Vector2 calculatedPosition = targetPosition - turretPosition;
        Vector2 calculatedPositionWithVelocity = (targetPosition + (target.gameObject.GetComponent<Rigidbody2D>().velocity * angleOffset/* /(angleOffset + shooting.bulletForce*Time.deltaTime )*/)) - turretPosition;
        // use latter if these are same when not moving
                                                                                                                                              //    Debug.Log("Vectors manual subtaction: "+ calculatedPosition+ " Vectors from Distance()"+ Vector2.Distance(targetPosition,turretPosition));
        float distanceFormulaAngle = AngleBetweenTwoVectors(calculatedPositionWithVelocity);
        float targetAngle = AngleBetweenTwoVectors(calculatedPosition);
       Vector2 interception =  SmartAim(); // minus to turretPosition
       float interceptAngle =AngleBetweenTwoVectors(interception - turretPosition);
  //  Debug.Log("Default Target Position: " + targetPosition+ "Intercepted Point: "+ interception);
    Debug.Log("Default Angle Computation: " + targetAngle+ "Intercepted Angle: "+ interceptAngle);
        //  float velocityAngle = AngleBetweenTwoVectors(target.GetComponent<Rigidbody2D>().magnitude);

        //   Debug.Log(" Target Angle: " + targetAngle + "\" Advanced Angle:  \""+ distanceFormulaAngle );

        //  Vector2 advancedPosition = SmartAim(target);
        // float advancedPositionAngle = AngleBetweenTwoVectors(advancedPosition);




        this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.Euler(0, 0, interceptAngle), turnSpeed * Time.deltaTime);
        shooting.Fire();
    }

    private float AngleBetweenTwoVectors(Vector2 vectors)
    {
        return Mathf.Rad2Deg * Mathf.Atan2(vectors.y, vectors.x) - angleFixer; ;
    }
      private float AngleBetweenTwoVectorsFixed(Vector2 vectors)
    {
        return Mathf.Rad2Deg * Mathf.Atan2(vectors.y, vectors.x);
    }

    private Vector2 PredictPosition(Rigidbody2D targetBody)
    {
        Vector2 pos = targetBody.position;
        Vector2 dir = targetBody.velocity;
        Vector2 turretPosition = this.transform.position;
        float dist = (pos - turretPosition).magnitude;
        return pos + (dist / shooting.bulletForce) * dir;
    }

    public Vector2 FloatToVector2(float toConvert)
    {
        return new Vector2(Mathf.Cos(toConvert * Mathf.Deg2Rad), Mathf.Sin(toConvert * Mathf.Deg2Rad));
    }


    // BUGGY  if not at origin (0,0)
    public Vector2 SmartAim()
    {
        float  projectileSpeed = projectileForce;
        target = rangeFinder.targets[0];

        //positions
        Vector2 turretPosition = turretBase.transform.position;
        Vector2 targetPosition = target.transform.position;
        //velocities
        Vector2 turretVelocity = turretBase.GetComponent<Rigidbody2D>() ? turretBase.GetComponent<Rigidbody2D>().velocity : Vector2.zero;
        Vector2 targetVelocity = target.GetComponent<Rigidbody2D>() ? target.GetComponent<Rigidbody2D>().velocity : Vector2.zero;

        Vector2 interceptPoint = FirstOrderIntercept(turretPosition,turretVelocity,projectileSpeed,targetPosition,targetVelocity);
       // Debug.Log("Intercept Point:"+interceptPoint);
        return interceptPoint;
    
    }

    public Vector2 FirstOrderIntercept(Vector2 shooterPosition, Vector2 shooterVelocity, float shotSpeed, Vector2 targetPosition, Vector2 targetVelocity)
    {
        Vector2 targetRelativePosition = targetPosition - shooterPosition;
        Vector2 targetRelativeVelocity = targetVelocity - shooterVelocity;
        float t = FirstOrderInterceptTime(shotSpeed, targetRelativePosition, targetRelativeVelocity);
        return targetPosition + t * (targetRelativeVelocity);
    }

    public float FirstOrderInterceptTime(float shotSpeed, Vector2 targetRelativePosition, Vector2 targetRelativeVelocity)
    {
        float velocitySquared = targetRelativeVelocity.sqrMagnitude;
        if (velocitySquared < 0.001f)
            return 0f;
        float a = velocitySquared - shotSpeed * shotSpeed;
        if (Mathf.Abs(a) < 0.001f)
        {
            float t = -targetRelativePosition.sqrMagnitude / (2f * Vector2.Dot(targetRelativeVelocity, targetRelativePosition));
            return Mathf.Max(t, 0f);
        }

        float b = 2f * Vector2.Dot(targetRelativeVelocity, targetRelativePosition);
        float c = targetRelativePosition.sqrMagnitude;
        float determinant = b * b - 4f * a * c;

        if (determinant > 0f)
        {
            float t1 = (-b + Mathf.Sqrt(determinant)) / (2f * a), t2 = (-b - Mathf.Sqrt(determinant)) / (2f * a);
            if (t1 > 0f)
            {
                if (t2 > 0f)
                    return Mathf.Min(t1, t2); //both are positive
                else
                    return t1; //only t1 is positive
            }
            else
                return Mathf.Max(t2, 0f); //don't shoot back in time
        }
        else if (determinant < 0f)
            return 0f;
        else return Mathf.Max(-b / (2f * a), 0f);



    }
}
