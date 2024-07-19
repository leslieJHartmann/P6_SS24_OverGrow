using UnityEngine;
using UnityEngine.UI;
using Klak.TestTools;
using MediaPipe.HandPose;
using System.Collections.Generic;

public sealed class HandAnimator : MonoBehaviour
{
    #region Editable attributes

    [SerializeField] ImageSource _source = null;
    [Space]
    [SerializeField] ResourceSet _resources = null;
    [SerializeField] bool _useAsyncReadback = true;
    [Space]
    [SerializeField] Mesh _jointMesh = null;
    [SerializeField] Mesh _boneMesh = null;
    [Space]
    [SerializeField] Material _jointMaterial = null;
    [SerializeField] Material _boneMaterial = null;
    [Space]
    [SerializeField] RawImage _monitorUI = null;
    [Space]
    [SerializeField] GameObject _cubeObject = null; // Reference to the cube object
    [Space]
    [SerializeField] float _movementScale = 10.0f; // Scaling factor for cube movement

    #endregion

    #region Private members

    HandPipeline _pipeline;

    static readonly (int, int)[] BonePairs =
    {
        (0, 1), (1, 2), (1, 2), (2, 3), (3, 4),     // Thumb
        (5, 6), (6, 7), (7, 8),                     // Index finger
        (9, 10), (10, 11), (11, 12),                // Middle finger
        (13, 14), (14, 15), (15, 16),               // Ring finger
        (17, 18), (18, 19), (19, 20),               // Pinky
        (0, 17), (2, 5), (5, 9), (9, 13), (13, 17)  // Palm
    };

    static readonly int[] FingertipIndices = { 4, 8, 12, 16, 20 };
    static readonly int[] LowerFingerIndices = { 3, 7, 11, 15, 19 };

    Matrix4x4 CalculateJointXform(Vector3 pos)
      => Matrix4x4.TRS(pos, Quaternion.identity, Vector3.one * 0.07f);

    Matrix4x4 CalculateBoneXform(Vector3 p1, Vector3 p2)
    {
        var length = Vector3.Distance(p1, p2) / 2;
        var radius = 0.03f;

        var center = (p1 + p2) / 2;
        var rotation = Quaternion.FromToRotation(Vector3.up, p2 - p1);
        var scale = new Vector3(radius, length, radius);

        return Matrix4x4.TRS(center, rotation, scale);
    }

    #endregion

    #region MonoBehaviour implementation

    void Start()
    {
        _pipeline = new HandPipeline(_resources);

        if (_cubeObject == null)
        {
            Debug.LogError("Cube object is not assigned in HandAnimator script!");
        }
    }

    void OnDestroy()
    {
        if (_pipeline != null)
        {
            _pipeline.Dispose();
            _pipeline = null;
        }
    }

    void LateUpdate()
    {
        // Feed the input image to the Hand pose pipeline.
        _pipeline.UseAsyncReadback = _useAsyncReadback;
        _pipeline.ProcessImage(_source.Texture);

        var layer = gameObject.layer;

        // List to store joint coordinates
        List<Vector3> jointCoordinates = new List<Vector3>();

        // Joint balls
        for (var i = 0; i < HandPipeline.KeyPointCount; i++)
        {
            var position = _pipeline.GetKeyPoint(i);
            jointCoordinates.Add(position);
            var xform = CalculateJointXform(position);
            Graphics.DrawMesh(_jointMesh, xform, _jointMaterial, layer);
        }

        // Bones
        foreach (var pair in BonePairs)
        {
            var p1 = _pipeline.GetKeyPoint(pair.Item1);
            var p2 = _pipeline.GetKeyPoint(pair.Item2);
            var xform = CalculateBoneXform(p1, p2);
            Graphics.DrawMesh(_boneMesh, xform, _boneMaterial, layer);
        }

        // UI update
        _monitorUI.texture = _source.Texture;

        // Calculate the center of the hand
        Vector3 handCenter = CalculateHandCenter(jointCoordinates);

        // Output the hand center coordinates
        Debug.Log($"Hand Center: {handCenter}");

        // Check if hand is in a fist gesture
        bool isHandClosed = IsFistGesture(jointCoordinates);
        Debug.Log($"Hand is closed: {isHandClosed}");

        // Move the cube if the hand is closed
        if (isHandClosed)
        {
            MoveCube(handCenter);
        }
    }

    Vector3 CalculateHandCenter(List<Vector3> jointCoordinates)
    {
        if (jointCoordinates.Count == 0)
            return Vector3.zero;

        Vector3 sum = Vector3.zero;
        foreach (var coord in jointCoordinates)
        {
            sum += coord;
        }
        return sum / jointCoordinates.Count;
    }

    void MoveCube(Vector3 handPosition)
    {
        // Ensure Camera.main is not null
        if (Camera.main == null)
        {
            Debug.LogError("Main camera is not found!");
            return;
        }

        // Convert handPosition from normalized to screen coordinates
        Vector3 screenPosition = new Vector3(
            (handPosition.x + 1) * 0.5f * Screen.width, // Map x from [-1, 1] to [0, Screen.width]
            (handPosition.y + 1) * 0.5f * Screen.height, // Map y from [-1, 1] to [0, Screen.height]
            Camera.main.nearClipPlane // Set z to the near clip plane of the camera
        );

        // Convert screen position to world position
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(screenPosition);

        // Set the cube's position to match the hand's position in world coordinates
        _cubeObject.transform.position = new Vector3(
            worldPosition.x * _movementScale, // Apply scaling to x-axis
            worldPosition.y * _movementScale, // Apply scaling to y-axis
            _cubeObject.transform.position.z // Keep the z-axis unchanged
        );
    }

    bool IsFistGesture(List<Vector3> jointCoordinates)
    {
        // Check distances between fingertip joints and their corresponding lower joints
        for (int i = 0; i < FingertipIndices.Length; i++)
        {
            var fingertip = jointCoordinates[FingertipIndices[i]];
            var lowerFinger = jointCoordinates[LowerFingerIndices[i]];

            // Adjust the distance threshold as needed
            float distanceThreshold = 0.05f;

            if (Vector3.Distance(fingertip, lowerFinger) > distanceThreshold)
            {
                return false;
            }
        }

        return true;
    }

    #endregion
}
