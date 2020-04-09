using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace NavigationMap
{
    public class Unit : MonoBehaviour
    {
        // Start is called before the first frame update
        const float minPathUpdateTime = 0.2f;
        const float pathUpdateMoveThreshold = .5f;
        int currentAstar = 0;
        public GameObject[] aStarHolder;
        public Transform target;
        public Transform tempTarget;
        public HashSet<Vector3> pathWays = new HashSet<Vector3>();

        public GameObject[] s = new GameObject[8];
        LineRenderer line;
        public float speed = 20f;
        public float turnSpeed = 3f;
        public float turnDistance = 5;
        public float stoppingDistance = 10f;
        Vector3 stairs;
        Vector3 stairWell;
        Vector3 currentPosition;
        Vector3 localTarget;
        int currentElevation = 0;
        int targetElevation = 0;
        Path path;
        //    Vector3[] path;
        //       int targetIndex;

        void Awake()
        {
            currentPosition = transform.position;
            StairDistance(currentPosition);
            // StartCoroutine(UpdatePath());
        }
        void Update()
        {

            if (Input.GetKeyDown(KeyCode.Return))
            {
                StartPath();
            }

            // SeekerDistanceToClosestStairs(StairDistance(), "Stair");
            //   Debug.Log("Closest Stair" + ));
        }
        void InvokePath()
        {

        }

        public void StartPath()
        {

            // check if target is on the same floor

            if(pathWays.Count != 0)
            {
                RemovePath();
            }

            CameraMovement.instance.SetActiveFloors();
            // aStarHolder[currentAstar].SetActive(false);
            pathWays.Clear();

            currentPosition = transform.position;
            currentElevation = (int)currentPosition.y;
            targetElevation = (int)target.position.y;
            localTarget = target.position;
            pathWays.Add(currentPosition);
            StartCoroutine(AdjustPathToStairs());


        }

        public void RemovePath()
        {
            RemovePathWay();
            pathWays.Clear();
        }

        public void Simulate()
        {

            // check if target is on the same floor


            CameraMovement.instance.SetActiveFloors();
            // aStarHolder[currentAstar].SetActive(false);
            pathWays.Clear();

            currentPosition = transform.position;
            currentElevation = (int)currentPosition.y;
            targetElevation = (int)target.position.y;
            localTarget = target.position;
            pathWays.Add(currentPosition);
            // aStarHolder[currentAstar].GetComponent<PathRequestManager>().RequestSimulatePath(new PathRequest(currentPosition, stairs, OnPathFound));
            StartCoroutine(Simulation());



            if (Input.GetKeyDown(KeyCode.Escape))
            {
                //    StopCoroutine("UpdatePath");
                RemovePathWay();
                pathWays.Clear();
            }
        }
        IEnumerator Simulation()
        {
            Spawn.instance.ClearNodes();
            while (currentElevation != targetElevation)
            {
                stairs = SeekerDistanceToClosestStairs(currentElevation, "Stair");
                localTarget = stairs;
                //  Debug.Log("Started Coroutine from coroutine");
                StairDistance(currentPosition);
                aStarHolder[currentAstar].GetComponent<PathRequestManager>().RequestSimulatePath(new PathRequest(currentPosition, stairs, OnPathFound));
                yield return new WaitForSeconds(0.5f);
                //  Debug.Log("Returning from coroutine");
                if (currentPosition.y < targetElevation)
                {
                    stairWell = SeekerDistanceToClosestStairs(currentElevation, "StairWell");
                    pathWays.Add(stairWell);
                    currentElevation += 3;
                    stairWell = SeekerDistanceToClosestStairs(currentElevation, "StairWell");
                    pathWays.Add(stairWell);
                    currentPosition = stairWell;
                    Debug.Log("CurrentPosition: " + currentPosition);
                    stairs = SeekerDistanceToClosestStairs(currentElevation, "Stair");
                    pathWays.Add(stairs);
                    currentPosition = stairs;
                    Debug.Log("CurrentPosition: " + currentPosition);

                }
                else if (currentPosition.y > target.transform.position.y)
                {
                    stairWell = SeekerDistanceToClosestStairs(currentElevation, "StairWell");
                    pathWays.Add(stairWell);
                    currentElevation -= 3;
                    stairWell = SeekerDistanceToClosestStairs(currentElevation, "StairWell");
                    pathWays.Add(stairWell);
                    currentPosition = stairWell;
                    Debug.Log("CurrentPosition: " + currentPosition);
                    stairs = SeekerDistanceToClosestStairs(currentElevation, "Stair");
                    pathWays.Add(stairs);
                    currentPosition = stairs;
                    Debug.Log("CurrentPosition: " + currentPosition);
                }
            }
            int test = StairDistance(currentPosition);
            Debug.Log("Elevation:" + test);
            aStarHolder[currentAstar].GetComponent<PathRequestManager>().RequestSimulatePath(new PathRequest(currentPosition, target.position, OnPathFound));
            StopCoroutine(AdjustPathToStairs());
        }

        IEnumerator AdjustPathToStairs()
        {
            while (currentElevation != targetElevation)
            {
                stairs = SeekerDistanceToClosestStairs(currentElevation, "Stair");
                localTarget = stairs;
                //  Debug.Log("Started Coroutine from coroutine");
                StairDistance(currentPosition);
                aStarHolder[currentAstar].GetComponent<PathRequestManager>().RequestPath(new PathRequest(currentPosition, stairs, OnPathFound));
                yield return new WaitForSeconds(0.5f);
                //  Debug.Log("Returning from coroutine");
                if (currentPosition.y < targetElevation)
                {
                    stairWell = SeekerDistanceToClosestStairs(currentElevation, "StairWell");
                    pathWays.Add(stairWell);
                    currentElevation += 3;
                    stairWell = SeekerDistanceToClosestStairs(currentElevation, "StairWell");
                    pathWays.Add(stairWell);
                    currentPosition = stairWell;
                    Debug.Log("CurrentPosition: " + currentPosition);
                    stairs = SeekerDistanceToClosestStairs(currentElevation, "Stair");
                    pathWays.Add(stairs);
                    currentPosition = stairs;
                    Debug.Log("CurrentPosition: " + currentPosition);

                }
                else if (currentPosition.y > target.transform.position.y)
                {
                    stairWell = SeekerDistanceToClosestStairs(currentElevation, "StairWell");
                    pathWays.Add(stairWell);
                    currentElevation -= 3;
                    stairWell = SeekerDistanceToClosestStairs(currentElevation, "StairWell");
                    pathWays.Add(stairWell);
                    currentPosition = stairWell;
                    Debug.Log("CurrentPosition: " + currentPosition);
                    stairs = SeekerDistanceToClosestStairs(currentElevation, "Stair");
                    pathWays.Add(stairs);
                    currentPosition = stairs;
                    Debug.Log("CurrentPosition: " + currentPosition);
                }
            }
            int test = StairDistance(currentPosition);
            Debug.Log("Elevation:" + test);
            aStarHolder[currentAstar].GetComponent<PathRequestManager>().RequestPath(new PathRequest(currentPosition, target.position, OnPathFound));
            StopCoroutine(AdjustPathToStairs());
        }
        IEnumerator SimulatePath()
        {
            while (currentElevation != targetElevation)
            {
                stairs = SeekerDistanceToClosestStairs(currentElevation, "Stair");
                localTarget = stairs;
                //  Debug.Log("Started Coroutine from coroutine");
                StairDistance(currentPosition);
                aStarHolder[currentAstar].GetComponent<PathRequestManager>().RequestPath(new PathRequest(currentPosition, stairs, OnPathFound));
                yield return new WaitForSeconds(0.5f);
                //  Debug.Log("Returning from coroutine");
                if (currentPosition.y < targetElevation)
                {
                    stairWell = SeekerDistanceToClosestStairs(currentElevation, "StairWell");
                    pathWays.Add(stairWell);
                    currentElevation += 3;
                    stairWell = SeekerDistanceToClosestStairs(currentElevation, "StairWell");
                    pathWays.Add(stairWell);
                    currentPosition = stairWell;
                    Debug.Log("CurrentPosition: " + currentPosition);
                    stairs = SeekerDistanceToClosestStairs(currentElevation, "Stair");
                    pathWays.Add(stairs);
                    currentPosition = stairs;
                    Debug.Log("CurrentPosition: " + currentPosition);

                }
                else if (currentPosition.y > target.transform.position.y)
                {
                    stairWell = SeekerDistanceToClosestStairs(currentElevation, "StairWell");
                    pathWays.Add(stairWell);
                    currentElevation -= 3;
                    stairWell = SeekerDistanceToClosestStairs(currentElevation, "StairWell");
                    pathWays.Add(stairWell);
                    currentPosition = stairWell;
                    Debug.Log("CurrentPosition: " + currentPosition);
                    stairs = SeekerDistanceToClosestStairs(currentElevation, "Stair");
                    pathWays.Add(stairs);
                    currentPosition = stairs;
                    Debug.Log("CurrentPosition: " + currentPosition);
                }
            }
            int test = StairDistance(currentPosition);
            Debug.Log("Elevation:" + test);
            aStarHolder[currentAstar].GetComponent<PathRequestManager>().RequestPath(new PathRequest(currentPosition, target.position, OnPathFound));
            StopCoroutine(AdjustPathToStairs());
        }

        public void OnPathFound(Vector3[] waypoints, bool pathSuccessful)
        {
            if (pathSuccessful)
            {
                path = new Path(waypoints, transform.position, turnDistance, stoppingDistance);

                //    Debug.Log("How Many Times I am Called?");
                // pathWays.Add(this.gameObject.transform.position);
                pathWays.UnionWith(waypoints);
                AddPathWay(pathWays.ToList());
                StopCoroutine("FollowPath");
                // StartCoroutine("FollowPath");
            }
            else
            {
                Debug.Log(this + " No Path");
            }
        }

        // Add Line using the waypoints
        //
        public void AddPathWay(List<Vector3> waypoints)
        {
            line = GameObject.FindGameObjectWithTag("LineRenderer").GetComponent<LineRenderer>();
            line.positionCount = waypoints.Count;
            //   Debug.Log("LinerenderLINWTqweqw");
            line.SetPositions(waypoints.ToArray());

            // get all waypoints?
        }

        public void RemovePathWay()
        {
            line = GameObject.FindGameObjectWithTag("LineRenderer").GetComponent<LineRenderer>();
            line.positionCount = 0;
        }

        public int StairDistance(Vector3 position)
        {
            Vector3 currentElevation = position;
            switch (currentElevation.y)
            {
                case 0 when currentElevation.y < 1:
                    Debug.Log("Seeker at elevation 0");
                    aStarHolder[currentAstar].SetActive(false);
                    currentAstar = 0;
                    aStarHolder[currentAstar].SetActive(true);
                    return 0;

                case 3 when (currentElevation.y > 0 && currentElevation.y <= 3):
                    Debug.Log("Seeker at elevation 1");
                    aStarHolder[currentAstar].SetActive(false);
                    currentAstar = 1;
                    aStarHolder[currentAstar].SetActive(true);
                    return 3;

                case 6 when currentElevation.y > 3 && currentElevation.y <= 6:
                    Debug.Log("Seeker at elevation 2");
                    aStarHolder[currentAstar].SetActive(false);
                    currentAstar = 2;
                    aStarHolder[currentAstar].SetActive(true);
                    return 6;

                case 9 when currentElevation.y > 6 && currentElevation.y <= 9:
                    Debug.Log("Seeker at elevation 3");
                    aStarHolder[currentAstar].SetActive(false);
                    currentAstar = 3;
                    aStarHolder[currentAstar].SetActive(true);
                    return 9;

                case 12 when currentElevation.y > 9 && currentElevation.y <= 12:
                    Debug.Log("Seeker at elevation 4");
                    aStarHolder[currentAstar].SetActive(false);
                    currentAstar = 4;
                    aStarHolder[currentAstar].SetActive(true);
                    return 12;

                case 15 when currentElevation.y > 12 && currentElevation.y <= 15:
                    Debug.Log("Seeker at elevation 5");
                    aStarHolder[currentAstar].SetActive(false);
                    currentAstar = 5;
                    aStarHolder[currentAstar].SetActive(true);
                    return 15;
                default:
                    return (int)currentElevation.y;


            }
            //check elevation of seeker or last known stair position Y
            //switch statement for all stairs in that elevation
            // calculate distance formula for it
            // return nearest stair
        }

        public Vector3 SeekerDistanceToClosestStairs(int floor, string stair)
        {// get all stairs at floor
            GameObject[] stairs;
            stairs = GameObject.FindGameObjectsWithTag(stair + Mathf.RoundToInt(floor));
            float closestStair = Mathf.Infinity;
            s = stairs;

            // float closestStair; // distance of closestStair to seeker

            Vector3 currentPosition = this.gameObject.transform.position;
            // initial closest stair
            closestStair = Mathf.Abs(Vector3.Distance(currentPosition, stairs[0].transform.position));


            GameObject tempStair = stairs[0];
            for (int i = 0; i < stairs.Length; i++)
            {

                if (closestStair > Vector3.Distance(stairs[i].transform.position, currentPosition))
                {
                    closestStair = Mathf.Abs(Vector3.Distance(stairs[i].transform.position, currentPosition));
                    tempStair = stairs[i];
                }
                //      Debug.Log("Distance closest Stairs: " + closestStair + " " + tempStair.transform.position);

            }
            Debug.Log("Closest Stairs: " + tempStair.name + "Tag: " + tempStair.tag + " Position: " + tempStair.transform.position);
            stairs = null;
            return tempStair.transform.position;

        }

        IEnumerator UpdatePath() // initially a 
        {
            if (Time.timeSinceLevelLoad < .3f)      // anti delay of pathing
            {
                yield return new WaitForSeconds(.3f);
            }

            // ADD STAIRS to this code
            // While not on the same Y
            // Target should be the Stairs
            // stairDistance();

            aStarHolder[currentAstar].GetComponent<PathRequestManager>().RequestPath(new PathRequest(transform.position, target.position, OnPathFound));
            float sqrMoveThreshold = pathUpdateMoveThreshold * pathUpdateMoveThreshold;
            Vector3 targetPosOld = target.position;

            while (true)
            {
                yield return new WaitForSeconds(minPathUpdateTime);
                if ((target.position - targetPosOld).sqrMagnitude > sqrMoveThreshold)
                {
                    aStarHolder[currentAstar].GetComponent<PathRequestManager>().RequestPath(new PathRequest(transform.position, target.position, OnPathFound));
                    targetPosOld = target.position;
                }
            }
        }


        IEnumerator FollowPath()
        {
            bool followingPath = true;
            int pathIndex = 0;
            float speedPercent = 1f;
            transform.LookAt(path.lookPoints[0]);
            while (followingPath)
            {
                Vector2 pos2d = new Vector2(transform.position.x, transform.position.z);
                while (path.turnBoundaries[pathIndex].HasCrossedLine(pos2d)) // 
                {
                    if (pathIndex == path.finishedLineIndex)
                    {
                        followingPath = false;
                        break;
                    }
                    else
                    {
                        pathIndex++;
                    }
                }
                if (followingPath)
                {
                    if (pathIndex >= path.slowDownIndex && stoppingDistance > 0)
                    {
                        speedPercent = Mathf.Clamp01(path.turnBoundaries[path.finishedLineIndex].DistanceFromPoint(pos2d) / stoppingDistance);
                        if (speedPercent < 0.01f)
                        {
                            followingPath = false;
                        }
                    }
                    Quaternion targetRotation = Quaternion.LookRotation(path.lookPoints[pathIndex] - transform.position);
                    transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * turnSpeed);
                    transform.Translate(Vector3.forward * Time.deltaTime * speed * speedPercent, Space.Self);
                }
                yield return null;

            }
        }

        void OnDrawGizmos()
        {
            if (path != null)
            {
                path.DrawWithGizmos();
            }
        }

    }
}
