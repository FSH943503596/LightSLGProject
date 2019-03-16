/*
 *	Author:	贾树永
 *	CreateTime:	2019-03-07-15:08:30
 *	Vertion: 1.0	
 *
 *	Description:
 *
*/

using Pathfinding;
using Pathfinding.RVO;
using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(RVOController))]
[RequireComponent(typeof(Seeker))]
public class AStarPathFindingAgent : MonoBehaviour, IActorAgent
{
    private event Action _CompleteCallBack;
    private float _NextRepath = 0;
    private Vector3 _Target;
    private bool _CanSearchAgain = true;
    private RVOController _Controller;
    private Path _Path = null;
    private List<Vector3> _VectorPath;
    private int _Wp;
    private Seeker _Seeker;

    [SerializeField] private float _StopDistance = 0;
    [SerializeField] private float _RepathRate = 1;
    [SerializeField] private float _MaxSpeedOrigin = 10;
    [SerializeField] private float _MaxSpeed = 10;
    [SerializeField] private float _MoveNextDist = 1;
    //[SerializeField] private float _SlowdownDistance = 0;
    [SerializeField] private LayerMask _GroundMask;
    [SerializeField] private GameObject _ActorBody;

    public Func<Vector3, float> GetSpeedParam;

    public float StopDistance { get => _StopDistance; set => _StopDistance = value; }
    public float RepathRate { get => _RepathRate; set => _RepathRate = value; }
    public float MaxSpeedOrigin { get => _MaxSpeedOrigin; set => _MaxSpeedOrigin = value; }
    public float MaxSpeed { get => _MaxSpeed; set => _MaxSpeed = value; }
    public float MoveNextDist { get => _MoveNextDist; set => _MoveNextDist = value; }

    /// <summary>Set the point to move to</summary>
    public void SetTarget(Vector3 target)
    {
        this._Target = target;
        RecalculatePath();
    }

    public void AddCompleteListener(Action completeCallBack)
    {
        _CompleteCallBack += completeCallBack;
    }

    public void RemoveCompleteListener(Action completeCallBack)
    {
        _CompleteCallBack -= completeCallBack;
    }

    public void ClearCompleteEvent()
    {
        _CompleteCallBack = null;
    }


    private void RecalculatePath()
    {
        _CanSearchAgain = false;
        _NextRepath = Time.time + _RepathRate * (UnityEngine.Random.value + 0.5f);
        _Seeker.StartPath(transform.position, _Target, OnPathComplete);
    }
    private void OnPathComplete(Path _p)
    {
        ABPath p = _p as ABPath;

        _CanSearchAgain = true;

        if (_Path != null) _Path.Release(this);
        _Path = p;
        p.Claim(this);

        if (p.error)
        {
            _Wp = 0;
            _VectorPath = null;
            return;
        }


        Vector3 p1 = p.originalStartPoint;
        Vector3 p2 = transform.position;
        p1.y = p2.y;
        float d = (p2 - p1).magnitude;
        _Wp = 0;

        _VectorPath = p.vectorPath;
        Vector3 waypoint;

        if (_MoveNextDist > 0)
        {
            for (float t = 0; t <= d; t += _MoveNextDist * 0.6f)
            {
                _Wp--;
                Vector3 pos = p1 + (p2 - p1) * t;

                do
                {
                    _Wp++;
                    waypoint = _VectorPath[_Wp];
                } while (_Controller.To2D(pos - waypoint).sqrMagnitude < _MoveNextDist * _MoveNextDist && _Wp != _VectorPath.Count - 1);
            }
        }
    }
    private void Awake()
    {
        _Seeker = GetComponent<Seeker>();
        _Controller = GetComponent<RVOController>();
        _GroundMask = LayerMask.GetMask(GlobalSetting.LAYER_MASK_NAME_GROUND);
    }
    private void Update()
    {
        //更新最大速度, 获得地块对于速度的影响
        if (GetSpeedParam != null) _MaxSpeed = GetSpeedParam(transform.localPosition) * _MaxSpeedOrigin;
    }
    private void LateUpdate()
    {
        if (Time.time >= _NextRepath && _CanSearchAgain)
        {
            RecalculatePath();
        }

        Vector3 pos = transform.position;

        if (_VectorPath != null && _VectorPath.Count != 0)
        {
            while ((_Controller.To2D(pos - _VectorPath[_Wp]).sqrMagnitude < _MoveNextDist * _MoveNextDist && _Wp != _VectorPath.Count - 1) || _Wp == 0)
            {
                _Wp++;
            }

            // Current path segment goes from vectorPath[wp-1] to vectorPath[wp]
            // We want to find the point on that segment that is 'moveNextDist' from our current position.
            // This can be visualized as finding the intersection of a circle with radius 'moveNextDist'
            // centered at our current position with that segment.
            var p1 = _VectorPath[_Wp - 1];
            var p2 = _VectorPath[_Wp];

            // Calculate the intersection with the circle. This involves some math.
            var t = VectorMath.LineCircleIntersectionFactor(_Controller.To2D(transform.position), _Controller.To2D(p1), _Controller.To2D(p2), _MoveNextDist);
            // Clamp to a point on the segment
            t = Mathf.Clamp01(t);
            Vector3 waypoint = Vector3.Lerp(p1, p2, t);

            // Calculate distance to the end of the path
            float remainingDistance = _Controller.To2D(waypoint - pos).magnitude + _Controller.To2D(waypoint - p2).magnitude;
            for (int i = _Wp; i < _VectorPath.Count - 1; i++) remainingDistance += _Controller.To2D(_VectorPath[i + 1] - _VectorPath[i]).magnitude;

            // Set the target to a point in the direction of the current waypoint at a distance
            // equal to the remaining distance along the path. Since the rvo agent assumes that
            // it should stop when it reaches the target point, this will produce good avoidance
            // behavior near the end of the path. When not close to the end point it will act just
            // as being commanded to move in a particular direction, not toward a particular point
            var rvoTarget = (waypoint - pos).normalized * remainingDistance + pos;
            // When within [slowdownDistance] units from the target, use a progressively lower speed
            // 减速功能，不需要
            //var desiredSpeed = Mathf.Clamp01(remainingDistance / _SlowdownDistance) * _MaxSpeed;

            //增加寻路完成回调
            if (remainingDistance <= _StopDistance && _CompleteCallBack != null)
            {
                _CompleteCallBack();
                _CompleteCallBack = null;
            }
            Debug.DrawLine(transform.position, waypoint, Color.red);

            //不需要减速
            //_Controller.SetTarget(rvoTarget, desiredSpeed, _MaxSpeed);
            _Controller.SetTarget(rvoTarget, _MaxSpeed, _MaxSpeed);
        }
        else
        {

            // Stand still
            _Controller.SetTarget(pos, _MaxSpeed, _MaxSpeed);
        }

        // Get a processed movement delta from the rvo controller and move the character.
        // This is based on information from earlier frames.
        var movementDelta = _Controller.CalculateMovementDelta(Time.deltaTime);
        pos += movementDelta;

        // Rotate the character if the velocity is not extremely small
        if (Time.deltaTime > 0 && movementDelta.magnitude / Time.deltaTime > 0.01f)
        {
            var rot = transform.rotation;
            var targetRot = Quaternion.LookRotation(movementDelta, _Controller.To3D(Vector2.zero, 1));
            const float RotationSpeed = 5;
            if (_Controller.movementPlane == MovementPlane.XY)
            {
                targetRot = targetRot * Quaternion.Euler(-90, 180, 0);
            }
            transform.rotation = Quaternion.Slerp(rot, targetRot, Time.deltaTime * RotationSpeed);
        }

        if (_Controller.movementPlane == MovementPlane.XZ)
        {
            RaycastHit hit;
            if (Physics.Raycast(pos + Vector3.up, Vector3.down, out hit, 2, _GroundMask))
            {
                pos.y = hit.point.y;
            }
        }

        transform.position = pos;
    }

    private void OnBecameInvisible()
    {
        if (_ActorBody) _ActorBody.SetActive(false);
    }
    private void OnBecameVisible()
    {
        if (_ActorBody) _ActorBody.SetActive(true);
    }
}