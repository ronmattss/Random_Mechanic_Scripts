using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;


//Enemy States
// when in range stop, Fire enemy
//      Needs range state
public class EnemyAI : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform target;
    public float speed = 10000f;
    public float nextWaypointDistance = 3f;

    Path path;
    int currentWaypoint = 0;
    bool reachedEndOfPath = false;

    Seeker seeker;
    Rigidbody2D rb;

    void Start()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();

        InvokeRepeating("UpdatePath",0f,.5f);
    }
void UpdatePath()
{
    if(seeker.IsDone())
     seeker.StartPath(rb.position, target.position, OnPathComplete);
}
    void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (path == null)
            return;
        if (currentWaypoint >= path.vectorPath.Count)
        {
            reachedEndOfPath = true;
            return;
        }
        else
        {
            reachedEndOfPath = false;
        }

        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
        Vector2 vectorAngle = target.position - this.transform.position;

        float angleBetweenTarget = Mathf.Rad2Deg * Mathf.Atan2(vectorAngle.y, vectorAngle.x) - 90;
        this.transform.rotation = Quaternion.Lerp(this.transform.rotation,Quaternion.Euler(0,0,angleBetweenTarget),5f * Time.deltaTime);
       
       
        Vector2 force = direction * speed * Time.deltaTime;
        rb.AddForce(force,ForceMode2D.Impulse);

        float distance = Vector2.Distance(rb.position,path.vectorPath[currentWaypoint]);

        if(distance < nextWaypointDistance)
        {
            currentWaypoint++;
        }

    }
}
