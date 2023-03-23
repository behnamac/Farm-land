using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace Controller
{
    public class CameraController : MonoBehaviour
    {
        public static CameraController Instance;

        public Transform target;
        [SerializeField] private float moveSpeed;
        [SerializeField] private CameraSetting[] cameraSettings;

        private Dictionary<string, CameraSetting> settingDic;
        private Transform playerCamera;
        private void Awake()
        {
            Instance = this;

            settingDic = new Dictionary<string, CameraSetting>();
            for (int i = 0; i < cameraSettings.Length; i++)
            {
                settingDic.Add(cameraSettings[i].settingName, cameraSettings[i]);
            }
            playerCamera = Camera.main.transform;
        }
        
        // Update is called once per frame
        private void LateUpdate()
        {
            CameraHolderControl();
        }
        private void FixedUpdate()
        {
        }
        private void CameraHolderControl()
        {
            if (target != null)
                transform.position = Vector3.Lerp(transform.position, target.position, moveSpeed * Time.deltaTime);
        }
        
        public void ChangeCameraPos(string settingName)
        {
            Vector3 position = settingDic[settingName].Position;
            Vector3 rotation = settingDic[settingName].Rotation;

            float positionTime = settingDic[settingName].changePositionTime;
            float rotationTime = settingDic[settingName].changeRitationTime;

            playerCamera.DOLocalMove(position, positionTime);
            playerCamera.DOLocalRotate(rotation, rotationTime);
        }
    }
    [System.Serializable]
    public class CameraSetting
    {
        public string settingName;
        public Vector3 Position;
        public Vector3 Rotation;
        public float changePositionTime;
        public float changeRitationTime;
    }
}
