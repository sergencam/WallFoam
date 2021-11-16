using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace WallFoam
{
    public class CameraFollowScript : MonoBehaviour
    {
        public static CameraFollowScript Instance;
        public Transform cameraTarget;

        public float standartSmoothSpeed;
        [HideInInspector] public float sSpeed;

        public Vector3 standartDist;
        public Vector3 sawDist;
        [HideInInspector] public Vector3 dist;

        public Quaternion standartRot;
        public Quaternion sawRot;
        [HideInInspector] public Quaternion rot;

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            sSpeed = standartSmoothSpeed;
            dist = standartDist;
            rot = standartRot;
        }

        void FixedUpdate()
        {
            if (cameraTarget)
            {
                transform.position = Vector3.Lerp(transform.position, cameraTarget.position + dist, sSpeed * Time.deltaTime);
                transform.rotation = Quaternion.Slerp(transform.rotation, rot, sSpeed * Time.deltaTime);
            }
            else
            {
                transform.position = Vector3.Lerp(transform.position, dist, sSpeed * Time.deltaTime);
                transform.rotation = Quaternion.Slerp(transform.rotation, rot, sSpeed * Time.deltaTime);
            }
        }
    }
}