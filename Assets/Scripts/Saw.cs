using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WallFoam
{
    public class Saw : MonoBehaviour
    {
        public float targetY;
        public float sawDownSpeed;
        public Transform foamPiecesParent;

        private void Start()
        {
            WallFoamController.Instance.saw = this;
        }

        public void Work()
        {
            if (WallFoamController.Instance.nailGun.gameObject.activeSelf == true)
                WallFoamController.Instance.nailGun.gameObject.SetActive(false);

            CameraFollowScript.Instance.rot = CameraFollowScript.Instance.sawRot;
            CameraFollowScript.Instance.dist = CameraFollowScript.Instance.sawDist;
            CameraFollowScript.Instance.cameraTarget = this.transform;

            var pos = transform.position;

            if (transform.position.y <= targetY)
            {
                //Testere istenen y pozisyonuna ulaşmışsa köpükleri düşürüp level'ı bitiriyoeuz
                foamPiecesParent.GetComponent<Rigidbody>().isKinematic = false;
                WallFoamController.Instance.workState = WallFoamWorkState.LevelComplete;
                gameObject.SetActive(false);
                return;
            }

            if (Input.GetMouseButton(0))
            {
                //Oyuncu parmağını ekrana basılı tutuyorsa testereyi aşağı doğru indiriyoruz
                pos.y -= sawDownSpeed * Time.deltaTime;
                transform.position = pos;

                //Kesilme hissini arttırmak için köpüklerin içinde olduğu ana objeyi öne doğru eğiyoruz
                var foamParentRot = foamPiecesParent.eulerAngles;
                foamParentRot.x -= sawDownSpeed * Time.deltaTime;
                foamPiecesParent.transform.eulerAngles = foamParentRot;
            }
        }
    }
}
