using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace WallFoam
{
    public enum WallFoamWorkState
    {
        None,
        NailGun,
        Saw,
        LevelComplete
    }
    public class WallFoamController : MonoBehaviour
    {
        public static WallFoamController Instance;
        public WallFoamWorkState workState;
        public GameObject confettiFX;
        [HideInInspector] public NailGun nailGun;
        [HideInInspector] public Saw saw;
        private bool levelCompleted;

        private void Awake()
        {
            Instance = this;
        }

        private void Update()
        {
            switch (workState)
            {
                case WallFoamWorkState.NailGun:
                    nailGun.Work();
                    break;

                case WallFoamWorkState.Saw:
                    saw.Work();
                    break;

                case WallFoamWorkState.LevelComplete:
                    if (levelCompleted == false)
                        StartCoroutine(LevelComplete());
                    break;
            }
        }

        public IEnumerator LevelComplete()
        {
            levelCompleted = true;
            CameraFollowScript.Instance.cameraTarget = null;
            CameraFollowScript.Instance.dist = CameraFollowScript.Instance.standartDist;
            CameraFollowScript.Instance.rot = CameraFollowScript.Instance.standartRot;
            yield return new WaitForSeconds(0.25f);
            confettiFX.SetActive(true);
            yield return new WaitForSeconds(0.5f);
            UIManager.Instance.levelCompletePanel.SetActive(true);
        }

        public void NextLevel()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}