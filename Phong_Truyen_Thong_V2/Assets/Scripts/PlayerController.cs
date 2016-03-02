using UnityEngine;
using System.Collections;
using System;

[RequireComponent(typeof(NavMeshAgent))]
public class PlayerController : MonoBehaviour {

    Camera camera;
    NavMeshAgent myNavMeshAgent;

    Transform[] waypoints;
    public int currentPoint = 0;
    bool waiting;
    enum State
    {
        lookwalk = 1,
        freezee = 0,
        none = -1
    }
    State state;

    void Awake()
    {
        camera = Camera.main;
        myNavMeshAgent = GetComponent<NavMeshAgent>();
        myNavMeshAgent.updateRotation = false;
    }

     
    public void MoveNavMesh(Vector3 target)
    {
        if (target != null)
        {
                myNavMeshAgent.SetDestination(target);
        }
    }

    private State checkState( int  currentPointView)
    {
        int result;
        result = currentPointView - currentPoint;
        if (result == 0)
        {
            return State.freezee;
        }
        else
        {
            if (result == 1)
            {
                return State.lookwalk;
            }
            else
            {
                return State.none;
            }
        }
    }

    public void RotateView(Quaternion _CharactorRot, Quaternion _CameraRot)
    {
       this.transform.localRotation = _CharactorRot;
        camera.transform.localRotation = _CameraRot;
    }

    public void AutoMoving(PathManager _pathContains)
    {
        if (_pathContains != null)
        {
            waypoints = new Transform[_pathContains.Waypoints.Length];
            Array.Copy(_pathContains.Waypoints, waypoints, _pathContains.Waypoints.Length);

            Stop();
            StartCoroutine(Move());
        }
    }

    private IEnumerator RoDstCamera(Transform pointview = null)
    {
        if (pointview != null)
        {
            Vector3 temp = Vector3.up;
            while (temp != camera.transform.forward)
            {
                Quaternion rotation = Quaternion.LookRotation(pointview.position - camera.transform.position, pointview.up);

                temp = camera.transform.forward;
                camera.transform.rotation = Quaternion.Slerp(camera.transform.rotation, rotation, Time.deltaTime);

                yield return null;
            }
        }
        
        yield return StartCoroutine(Wait());

    }

    private void OnLookPointChange()
    {
        //int count = PointContainer.pointViewcontainer[currentLookview].count;

        //if (state == State.lookwalk)
        //{
        //    IEnumerator  rotdirec = RotateDirection(PointContainer.pointViewcontainer[currentLookview].element);
        //    StartCoroutine(rotdirec);
        //}
        //else
        //{
        //    if (state == State.freezee)
        //    {
        //        IEnumerator rotdirec = RotateDirection(PointContainer.pointViewcontainer[currentLookview].element);
        //        StartCoroutine(rotdirec);
        //    }
        //    else
        //    {
        //        if (currentPoint == (count - 2))
        //        {

        //            IEnumerator rotdirec = RotateDirection(PointContainer.pointViewcontainer[currentLookview].element);
        //            StartCoroutine(rotdirec);
        //        }
        //    }
        //}

    }

    IEnumerator Move()
    {

        myNavMeshAgent.Resume();

        myNavMeshAgent.SetDestination(waypoints[currentPoint].position);
        yield return StartCoroutine(WaitForDestination());

        StartCoroutine(NextWaypoint());
    }

    private IEnumerator NextWaypoint()
    {

        while (waiting) yield return null;
        Transform next = null;
         
        currentPoint++;

        currentPoint = Mathf.Clamp(currentPoint, 0, waypoints.Length - 1);

        if (next == null) next = waypoints[currentPoint];


        myNavMeshAgent.SetDestination(next.position);
        yield return StartCoroutine(WaitForDestination());

        StartCoroutine(NextWaypoint());
    }

    private IEnumerator WaitForDestination()
    {
        yield return new WaitForEndOfFrame();
        while (myNavMeshAgent.pathPending)
            yield return null;
        yield return new WaitForEndOfFrame();

        float remain = myNavMeshAgent.remainingDistance;

        while (remain == Mathf.Infinity || remain - myNavMeshAgent.stoppingDistance > float.Epsilon
        || myNavMeshAgent.pathStatus != NavMeshPathStatus.PathComplete)
        {
            float DstPath = Vector3.Distance(transform.position, waypoints[currentPoint].position);
            if (DstPath < 1.5f)
            {
                break;
            }
            remain = myNavMeshAgent.remainingDistance;
            yield return null;
        }
    }

    public void Stop()
    {
        StopAllCoroutines();
        myNavMeshAgent.Stop();
    }

    public void Resume()
    {
        waiting = false;
        myNavMeshAgent.Resume();
    }

    public void Pause(float seconds = 0f)
    {
        StopCoroutine(Wait());
        waiting = true;
        myNavMeshAgent.Stop();
    }

    private IEnumerator Wait(float secs = 0f)
    {
        yield return new WaitForSeconds(secs);
        Resume();
    }

}
