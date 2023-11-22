using DG.Tweening;
using UnityEngine;
using ThirdPersonMeleeSystem.Timers;
using UnityEngine.Events;

public class SensorComponent : MonoBehaviour
{
    #region Public Fields
    
    
    #endregion
    
    #region Private Fields

    private Transform _playerRef;
    private bool _canSeePlayer;
    private bool _engageTarget;
    private bool _suspicious;
    private float _timeBeenSeen;
    private Vector3 _playerPosition;
    
    //private Indicator _indicator;
    private Timer scanTimer;
    private Timer suspiciousTimer;

    #endregion
    
    #region Serialized Fields

    [SerializeField] private float radius;
    [SerializeField] private float instantDetectionRange;
    [SerializeField][Range(0, 360)] private float angle;
    [SerializeField] private LayerMask targetLayer;
    [SerializeField] private LayerMask obstructionLayer;
    [SerializeField] private float scanFrequency;
    [SerializeField] private float timeUntilDetection;
    [SerializeField] private float detectionRate;
    [SerializeField] private float timeUntilSuspicious;

    [SerializeField] private UnityEvent OnSuspiciousEvent;
    [SerializeField] private UnityEvent OnTargetLostEvent;
    [SerializeField] private UnityEvent OnEngageEvent;

    #endregion
    
    #region Getters

    public float Radius => radius;
    public float InstantDetectionRange => instantDetectionRange;
    public float Angle => angle;
    public LayerMask TargetLayer => targetLayer;
    public LayerMask ObstructionLayer => obstructionLayer;

    public Transform PlayerRef => _playerRef;
    public Vector3 PlayerPosition => _playerPosition;
    public bool CanSeePlayer => _canSeePlayer;
    public bool EngageTarget => _engageTarget;
    public bool Suspicious => _suspicious;

    #endregion

    private void Start()
    {
        //_playerRef = GameManager.Instance.playerRef;
        scanTimer = new Timer(scanFrequency);
        suspiciousTimer = new Timer(timeUntilSuspicious);
        //_indicator = IndicatorManager.Instance.CreateNewIndicator();
        //_indicator.gameObject.SetActive(false);
    }

    private void Update()
    {
        ScanForTargets();
        HandleDetectionState();
        HandleTargetOutOfSight();
        
        if (_engageTarget)
        {
            transform.DOLookAt(_playerRef.position, 0.5f, AxisConstraint.Y);
        }
       // _indicator.RotateTowardsTarget(transform.position);
    }

    private void HandleTargetOutOfSight()
    {
        if (!_canSeePlayer && _timeBeenSeen > 0)
        {
            _timeBeenSeen = Mathf.MoveTowards(_timeBeenSeen, 0f, Time.deltaTime);
            //_indicator.SetDetectionBarFill(_timeBeenSeen, timeUntilDetection, Time.deltaTime);

            if (_timeBeenSeen == 0)
            {
                OnTargetLostEvent.Invoke();
                //_indicator.Disable(0f);
                _engageTarget = false;
                _suspicious = false;
                _playerPosition = Vector3.zero;
            }
        }
    }

    private void HandleDetectionState()
    {
        if (_canSeePlayer && !_engageTarget)
        {
            if (_timeBeenSeen < timeUntilDetection)
            {
                transform.DOLookAt(_playerRef.position, 0.5f, AxisConstraint.Y);
                float delta = Vector3.Distance(transform.position, _playerRef.position) < instantDetectionRange ?
                    detectionRate * Time.deltaTime : Time.deltaTime;
                _timeBeenSeen += delta;
                //_indicator.Enable();
                //_indicator.SetDetectionBarFill(_timeBeenSeen, timeUntilDetection, delta);
            }
            else
            {
                //_indicator.Disable(0.8f);
                _suspicious = false;
                _engageTarget = true;
                //GameManager.Instance.PlayerDetected = true;
            }

            if (!_engageTarget && !_suspicious && _timeBeenSeen >= timeUntilSuspicious)
            {
                _suspicious = true;
                OnSuspiciousEvent.Invoke();
            }
        }
    }

    private void ScanForTargets()
    {
        scanTimer.Tick(Time.deltaTime);
        
        if (scanTimer.IsTimerComplete)
        {
            InViewCheck();
            scanTimer.Reset();
        }
    }

    private void InViewCheck()
    {
        Collider[] targets = Physics.OverlapSphere(transform.position, radius, targetLayer);

        if (targets.Length != 0)
        {
            Transform target = targets[0].transform;
            Vector3 directionToTarget = (target.position - transform.position).normalized;
            float angleToTarget = Vector3.Angle(transform.forward, directionToTarget);

            if (angleToTarget < angle / 2)
            {
                float distanceToTarget = Vector3.Distance(transform.position, target.position);
                if (Physics.Raycast(transform.position, directionToTarget, distanceToTarget, obstructionLayer))
                {
                    _canSeePlayer = false;
                }
                else
                {
                    _canSeePlayer = true;
                }
            }
            else
            {
                _canSeePlayer = false;
            }
        }
        else if (_canSeePlayer)
        {
            _canSeePlayer = false;
        }
    }

    // private Vector3 GetPlayerPosition()
    // {
    //     return _playerPosition = GameManager.Instance.GetPlayerPosition();
    // }

}
