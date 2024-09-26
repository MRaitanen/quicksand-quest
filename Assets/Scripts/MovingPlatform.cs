using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [Tooltip("The speed at which the platform moves.")]
    [SerializeField] private float speed = 1f;

    // Variables
    private Transform _platform;
    private Transform _waypoints;
    private Transform[] _points;
    private int _currentPoint = 0;

    private void Awake()
    {
        // Get the platform object
        _platform = transform.Find("Platform");

        if (_platform == null)
        {
            Debug.LogError("Platform object not found! Please ensure a child named 'Platform' exists.");
            return;
        }

        // Get the waypoints object
        _waypoints = transform.Find("Waypoints");

        if (_waypoints == null)
        {
            Debug.LogError("Waypoints object not found! Please ensure a child named 'Waypoints' exists.");
            return;
        }

        // Get all children of waypoints
        _points = new Transform[_waypoints.childCount];
        for (int i = 0; i < _waypoints.childCount; i++)
            _points[i] = _waypoints.GetChild(i);

        if (_points.Length == 0)
        {
            Debug.LogError("No waypoints found! Please add children to the 'Waypoints' object.");
            return;
        }

        // Set the initial position of the platform to the first waypoint
        _platform.position = _points[0].position;
    }

    private void Update()
    {
        // If there are waypoints
        if (_points.Length > 0)
        {
            // Move the platform to the next waypoint using Vector3 for 3D space
            _platform.position = Vector3.MoveTowards(_platform.position, _points[_currentPoint].position, speed * Time.deltaTime);

            // If the platform has reached the waypoint
            if (Vector3.Distance(_platform.position, _points[_currentPoint].position) < 0.1f)
            {
                // Move to the next waypoint
                _currentPoint = (_currentPoint + 1) % _points.Length;
            }
        }
    }
}
