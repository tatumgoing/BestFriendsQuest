using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class Walker
{
    public Transform Obj;
    public Transform TargetPoint { get; private set; }
    public Vector3 TargetPosition;
    public Transform PreviousPoint { get; private set; }
    public bool OnRoute;
    public float CheckTimeCooldown;
    public float TurnLerpFactor;

    public float DistanceLeft()
    {
        var selfPos = new Vector2(Obj.position.x, Obj.position.z);
        var targetPos = new Vector2(TargetPosition.x, TargetPosition.z);
        return Vector2.Distance(selfPos, targetPos);
    } 

    public Walker(Transform walker)
    {
        Obj = walker;
        TargetPoint = null;
        OnRoute = false;
        Obj.transform.GetChild(0).localEulerAngles = new Vector3(0, 180, 0);
    }

    public void SetTargetPoint(Transform targetPoint, float radius = 5)
    {
        if (TargetPoint != null) PreviousPoint = TargetPoint;

        TargetPoint = targetPoint;
        PickTargetPosition(radius);
    }

    public void PickTargetPosition(float radius)
    {
        var offset = Random.insideUnitSphere * Random.Range(0, radius);
        offset.y = 0;
        TargetPosition = TargetPoint.position + offset;
    }

    public void MoveTowardPoint(float speed, LayerMask walkSurface, LayerMask collideLayers)
    {
        if (TargetPoint == null) return;

        var originalRot = Obj.rotation; 
        var rot = Obj.localEulerAngles;
        Obj.LookAt(TargetPosition);
        rot.y = Obj.localEulerAngles.y;
        Obj.localEulerAngles = rot;
        Obj.rotation = Quaternion.Slerp(originalRot, Obj.rotation, TurnLerpFactor * Time.deltaTime);

        var willCollide = Physics.Raycast(Obj.position + Vector3.up * 2, Obj.forward, out var collideInfo, 3, collideLayers);
        if (willCollide) {
            Obj.Rotate(Vector3.up, 2);
            speed *= 0.25f;
        }

        //var dir = (TargetPosition - Obj.position).normalized;
        var dir = Obj.forward.normalized;
        Obj.position += speed * Time.deltaTime * dir;

        var didHit = Physics.Raycast(Obj.transform.position + Vector3.up * 10, Vector3.down, out var hitInfo, 1000, walkSurface);
        if (!didHit) return;

        var pos = Obj.position;
        pos.y = hitInfo.point.y;
        Obj.position = pos;
    }

    public void DrawGizmos()
    {
        if (!Obj || !TargetPoint) return;

        Gizmos.DrawWireSphere(TargetPosition, 2);
        Gizmos.DrawLine(Obj.position, TargetPosition);

        Gizmos.color = Color.red;
        Gizmos.DrawLine(Obj.position + Vector3.up * 15, Obj.position + Vector3.up * 2 + Obj.forward * 3);
    }
}

public class CharacterPointWalker : MonoBehaviour
{
    [SerializeField] private List<Transform> _pointParents;
    [SerializeField] private float _pointRadius = 1;
    [SerializeField, Range(0, 1)] private float _pickNewTargetChance = 1;
    [SerializeField] private Vector2 _pickNewTargetFreq = new Vector2(1, 3);
    [SerializeField] private float _walkSpeed = 0;
    [SerializeField, Range(0, 1)] private float _turnBackChance = 0.2f;
    [SerializeField] private LayerMask _walkSurface;
    [SerializeField] private LayerMask _collideLayers;

    private List<Walker> _walkers = new List<Walker>();

    private List<Transform> _allPoints = new List<Transform>();

    private void Start()
    {
        _allPoints = new List<Transform>();
        foreach (var p in _pointParents) {
            foreach (Transform point in p) if (point.gameObject.activeInHierarchy) _allPoints.Add(point);
        }
    }

    private void Update()
    {
        foreach (var w in _walkers) if (w.Obj) Walk(w);
    }

    private Transform PickRandomNextOption(Transform currentPoint, Transform previousPoint)
    {
        var options = getNextOptions(currentPoint);

        if (options.Count == 1) return options[0];
        if (previousPoint == null) return options[Random.Range(0, options.Count)];

        options = options.Where(x => x != previousPoint).ToList();
        if (Random.Range(0, 1f) < _turnBackChance) return previousPoint;
        else return options[Random.Range(0, options.Count)];
    }

    private List<Transform> getNextOptions(Transform _currentPoint)
    {
        var options = new List<Transform>();
        var siblingIndex = _currentPoint.GetSiblingIndex();
        var parent = _currentPoint.parent;
        if (siblingIndex != 0) options.Add(parent.GetChild(siblingIndex - 1));
        if (siblingIndex != parent.childCount-1) options.Add(parent.GetChild(siblingIndex + 1));

        if (options.Count < 2) {
            foreach (var p in _allPoints) {
                if (Vector3.Distance(_currentPoint.position, p.position) < _pointRadius * 2) options.Add(p);
            }
        }

        options = options.Where(x => x.gameObject.activeInHierarchy).ToList();

        return options;
    }

    private void Walk(Walker walker)
    {
        if (walker.TargetPoint == null) {
            var closestPoint = _allPoints.OrderBy(x => Vector3.Distance(walker.Obj.position, x.position)).First();
            walker.SetTargetPoint(closestPoint, _pointRadius * closestPoint.transform.localScale.x);
        }

        walker.CheckTimeCooldown -= Time.deltaTime;
        if (walker.CheckTimeCooldown <= 0) {
            if (Random.Range(0, 1f) < _pickNewTargetChance) walker.PickTargetPosition(_pointRadius * walker.TargetPoint.localScale.x);
            walker.CheckTimeCooldown = Utils.Rand(_pickNewTargetFreq);
        }

        if (walker.DistanceLeft() > _pointRadius) walker.MoveTowardPoint(_walkSpeed, _walkSurface, _collideLayers);
        else walker.SetTargetPoint(PickRandomNextOption(walker.TargetPoint, walker.PreviousPoint));
    }

    public void AddWalker(Transform walker)
    {
        var newWalker = new Walker(walker);
        newWalker.CheckTimeCooldown = Utils.Rand(_pickNewTargetFreq);
        newWalker.TurnLerpFactor = Random.Range(2.5f, 10f);
        _walkers.Add(newWalker);
    }

    private void OnDrawGizmosSelected()
    {
        var allPoints = new List<Transform>();
        foreach (var p in _pointParents) {
            foreach (Transform point in p) if (point.gameObject.activeInHierarchy) allPoints.Add(point);
        }

        foreach (var p in allPoints) {
            Gizmos.DrawWireSphere(p.position, _pointRadius * p.localScale.x);

            if (p.GetSiblingIndex() != 0) {
                Gizmos.DrawLine(p.position, p.parent.GetChild(p.GetSiblingIndex()-1).position);
            }
        }

        foreach (var w in _walkers) w.DrawGizmos();
    }
}
