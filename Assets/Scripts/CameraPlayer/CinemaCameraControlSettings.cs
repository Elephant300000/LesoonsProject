using System;
using UnityEngine;

namespace Lucky38.MyCamera
{
    [Serializable]
    public sealed class CinemaCameraControlSettings
    {
        [Header("Rotation")]
        [Tooltip("Mouse look sensitivity.")]
        [Range(1f, 10f)]
        public float mouseSensitivity = 5f;

        [Tooltip("Camera rotation slerp speed.")]
        [Range(1f, 10f)]
        public float rotationLerpSpeed = 9f;

        [Tooltip("Min pitch (look down).")]
        [Range(-90f, 0f)]
        public float minPitchAngle = -70f;

        [Tooltip("Max pitch (look up).")]
        [Range(0f, 90f)]
        public float maxPitchAngle = 70f;

        [Header("Zoom")]
        [Tooltip("Scroll zoom speed.")]
        [Range(1f, 10f)]
        public float zoomSpeed = 5f;

        [Tooltip("Min camera distance.")]
        [Range(0.5f, 2f)]
        public float minDistance = 1.5f;

        [Tooltip("Max camera distance.")]
        [Range(2f, 10f)]
        public float maxDistance = 10f;

        [Tooltip("Initial orbit distance.")]
        [Range(0.5f, 10f)]
        public float initialZoom = 5f;
    }
}
