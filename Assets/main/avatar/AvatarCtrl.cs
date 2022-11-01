using UnityEngine;
using UnityEngine.AI;


public class AvatarCtrl
{
    private GameObject _go;

    private float oriSpeed = 3.5f;
    private float speedFactor = 1;
    private float moveTick = 0.25f;
    private Transform _lookRotation;
    private NavMeshAgent _agent;
    private Animator _animator;
    private Camera _mainCamera;
    private Transform _roleCanvas;

    private float cameraSpeedH = 0.2f;
    private Transform _body;

    public AvatarCtrl(GameObject go)
    {
        _go = go;
        _body = _go.transform.Find("__Body");
        _lookRotation = _go.transform.Find("lookRotation");
        _agent = _go.GetComponent<NavMeshAgent>();
        _agent.speed = oriSpeed;
        _agent.updateRotation = false;
        _agent.updatePosition = true;
        _agent.stoppingDistance = 0.001f;
        _agent.obstacleAvoidanceType = ObstacleAvoidanceType.NoObstacleAvoidance;
        // _agent.enabled = false;

        Transform goModel = _body.transform.GetChild(0);
        _animator = goModel.gameObject.GetComponent<Animator>();

        _mainCamera = Camera.main;

        _roleCanvas = goModel.transform.Find("RoleCanvas");
    }

    public void OnMove(Vector2 v)
    {
        if (_go != null && _lookRotation && _agent != null)
        {
            Vector3 temp = new Vector3(v.x, 0, v.y);
            Vector3 dir = _lookRotation.localRotation * temp;
            Vector3 pos = _go.transform.position + dir * oriSpeed * speedFactor * moveTick;
            _agent.SetDestination(pos);
        }
    }

    public Transform GetLookRotation()
    {
        return _lookRotation;
    }

    public void OnMoveEnd()
    {
    }

    public void OnUpdate()
    {
        if (_agent != null)
        {
            Vector3 v = _agent.velocity;
            float speed = v.sqrMagnitude;
            if (speed > 0)
            {
                Vector3 forward = new Vector3(v.x, 0, v.z);
                _body.transform.rotation = Quaternion.LookRotation(forward.normalized);
            }
            _animator.SetFloat("speed", speed);
        }
        if (_roleCanvas != null && _mainCamera != null)
        {
            _roleCanvas.LookAt(_mainCamera.transform);
            // Debug.Log("Test look at");
        }
    }

    public void OnTouchPadMove(Vector2 v)
    {
        Transform trans = _lookRotation.transform;
        Quaternion rotation = trans.localRotation;

        Quaternion res = Quaternion.Euler(0, -v.x * cameraSpeedH, 0) * rotation;
        trans.localRotation = res;
        
        // local trans = self.LookRotation.transform
        // local rotation = trans.localRotation
        // local res = Quaternion.Euler(0, -speed.x * self.cameraSpeedH, 0) * rotation
        // trans.localRotation = res
    }

    public void OnJump()
    {
        _animator?.SetTrigger("jump");
    }
}