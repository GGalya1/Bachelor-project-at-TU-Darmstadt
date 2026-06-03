using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class BusController : MonoBehaviour
{
    [Header("UI Elements")]
    public Slider speedSlider;

    [Header("Motion settings")]
    public GameObject spherePrefab;      // Sphere prefab (aka signal)
    public float movementSpeed = 7f;     // Velocity of sphere

    [Header("All bus segments")]
    // This is where we store references to all LineRenderers in the scene.
    // These are assigned via the Inspector.
    public LineRenderer[] busSegments;

    // A dictionary to store the path for each bus so it doesn't have to be calculated every time
    private Dictionary<LineRenderer, Vector3[]> busPaths = new Dictionary<LineRenderer, Vector3[]>();
    private Dictionary<LineRenderer, Vector3[]> reverseBusPaths = new Dictionary<LineRenderer, Vector3[]>();

    private List<BusSignal> activeSignals = new List<BusSignal>();
    public bool NoActiveSignals => activeSignals.Count == 0;

    void Start()
    {
        InitializePaths();

        if (spherePrefab != null) spherePrefab.SetActive(false);

        if (speedSlider != null)
        {
            speedSlider.onValueChanged.AddListener(OnSliderValueChanged);
            movementSpeed = speedSlider.value;
        }
        else {
            Debug.LogError("Slider is null !");
        }

        Debug.Log($"The controller has loaded {busPaths.Count} bus segments.");
    }

    /// <summary>
    /// A public method for triggering a signal on a specific bus.
    /// You will call this method from other scripts (for example, when clicking on the CPU block).
    /// </summary>
    /// <param name="targetBus">LineRenderer, through which the signal should be sent.</param>
    public void StartBusSignal(LineRenderer targetBus, bool reversedPath = false)
    {
        Vector3[] pathPoints = GetPathPoints(targetBus, reversedPath);
        if (pathPoints == null || pathPoints.Length < 2) return;

        // Create a sphere
        GameObject currentSphere = Instantiate(spherePrefab);
        currentSphere.SetActive(true);
        
        currentSphere.transform.position = pathPoints[0];

        // Retrieving the BusSignal component
        BusSignal signal = currentSphere.GetComponent<BusSignal>();
        
        // 2. Important: Adjust the speed and add it to the monitoring list
        if (signal != null)
        {
            activeSignals.Add(signal);
        }

        StartCoroutine(MoveSphereAlongPath(signal, pathPoints));
    }

    public void StartBusSignal(LineRenderer targetBus, int value, bool reversedPath = false)
    {
        Vector3[] pathPoints = GetPathPoints(targetBus, reversedPath);
        if (pathPoints == null || pathPoints.Length < 2) return;

        // Create a sphere
        GameObject currentSphere = Instantiate(spherePrefab);
        currentSphere.SetActive(true);

        currentSphere.transform.position = pathPoints[0];

        // Retrieving the BusSignal component
        BusSignal signal = currentSphere.GetComponent<BusSignal>();
        signal.UIRegisterPanel.Display("Signal", $"{value}");

        // 2. Important: Adjust the speed and add it to the monitoring list
        if (signal != null)
        {
            activeSignals.Add(signal);
        }

        StartCoroutine(MoveSphereAlongPath(signal, pathPoints));
    }

    // A coroutine to move a ball along a given array of points
    IEnumerator MoveSphereAlongPath(BusSignal signal, Vector3[] pathPoints)
    {
        if (signal == null) yield break;

        for (int i = 1; i < pathPoints.Length; i++)
        {
            Vector3 startPoint = pathPoints[i - 1];
            Vector3 targetPoint = pathPoints[i];

            while (signal.transform.position != targetPoint)
            {
                // Take the current speed (normal or fast-forward)
                float currentSpeed = movementSpeed;

                signal.transform.position = Vector3.MoveTowards(
                    signal.transform.position,
                    targetPoint,
                    currentSpeed * Time.deltaTime
                );
                yield return null;
            }
        }

        // 4. Remove from the list before destruction
        activeSignals.Remove(signal);
        Destroy(signal.UIRegisterPanel.gameObject);
        Destroy(signal.gameObject);
    }

    private void OnSliderValueChanged(float value)
    {
        movementSpeed = value;
        
    }

    private void InitializePaths() {
        foreach (LineRenderer lr in busSegments)
        {
            // Check for null (if there are empty slots in the inspector)
            if (lr == null)
            {
                Debug.LogWarning("An empty LineRenderer slot was found in the busSegments array. Skipping it.");
                continue;
            }

            // The transform relative to which the LineRenderer stores its points
            Transform busTransform = lr.transform;

            int pointCount = lr.positionCount;
            if (pointCount < 2)
            {
                Debug.LogWarning($"The '{lr.gameObject.name}' bus has fewer than 2 vertices and will not be loaded.");
                continue;
            }

            // Retrieve local coordinates from the LineRenderer
            Vector3[] localPoints = new Vector3[pointCount];
            lr.GetPositions(localPoints);

            // Arrays for storing paths in world space
            Vector3[] worldPoints = new Vector3[pointCount];
            Vector3[] reversedWorldPoints = new Vector3[pointCount];

            // --- 2. CONVERSION OF LOCAL POINTS TO WORLD COORDINATES ---

            for (int i = 0; i < pointCount; i++)
            {
                // We use TransformPoint to convert the local position (localPoints[i])
                // to a global (world) position, taking into account the position and rotation of the bus's parent (busTransform).
                worldPoints[i] = busTransform.TransformPoint(localPoints[i]);
            }

            // --- 3. SAVING THE STRAIGHT PATH ---

            // We store an array of world coordinates for forward motion
            busPaths.Add(lr, worldPoints);

            // --- 4. SAVING THE RETURN PATH (REVERSE) ---

            // Copy the array of world coordinates
            System.Array.Copy(worldPoints, reversedWorldPoints, pointCount);

            // Reverse the order of the elements for backward movement
            System.Array.Reverse(reversedWorldPoints);

            // We store an array of inverse global coordinates
            reverseBusPaths.Add(lr, reversedWorldPoints);
        }
    }

    private Vector3[] GetPathPoints(LineRenderer targetBus, bool reversed)
    {
        if (targetBus == null) return null;
        var dict = reversed ? reverseBusPaths : busPaths;
        return dict.ContainsKey(targetBus) ? dict[targetBus] : null;
    }
}
