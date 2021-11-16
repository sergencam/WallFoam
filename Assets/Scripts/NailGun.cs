using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace WallFoam
{
    public class NailGun : MonoBehaviour
    {
        public GameObject foamPiece;
        public Transform foamPiecesParent;
        public MeshRenderer foamWallModel;
        public Material foamWallMatOnFoamWallFull;
        public ParticleSystem sprayFX;
        public int targetFoamPieceCount;
        public int currentFoamPieceCount;

        public GameObject brush;
        public Transform brushParent;
        public float BrushSize = 0.1f;

        private void Start()
        {
            WallFoamController.Instance.nailGun = this;
            WallFoamController.Instance.workState = WallFoamWorkState.NailGun;
        }

        public void Work()
        {
            if (WallFoamController.Instance.saw.gameObject.activeSelf == true)
                WallFoamController.Instance.saw.gameObject.SetActive(false);

            if (Input.GetMouseButton(0))
            {
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hit, 100.0f))
                {
                    var targetPos = hit.point;
                    targetPos.z = transform.position.z;
                    transform.position = targetPos;

                    //Oyuncunun parmağı köpük yapmamız gereken duvardaysa spray efektini çalıştırıyoruz
                    if (hit.collider.gameObject.CompareTag("FoamWall") || hit.collider.gameObject.CompareTag("FoamPiece"))
                        sprayFX.Play();
                    else
                        sprayFX.Stop();

                    if (hit.collider.gameObject.CompareTag("FoamWall"))
                    {
                        if (hit.collider.gameObject.CompareTag("FoamPiece")) return;

                        //Oyuncunun parmağı duvardaysa duvarı köpük ile kaplıyoruz
                        var spawnPos = hit.point;
                        spawnPos.z -= 0.15f;

                        var foamPieceObj = Instantiate(foamPiece, spawnPos, foamPiece.transform.rotation);
                        foamPieceObj.transform.parent = foamPiecesParent;
                        foamPieceObj.transform.localScale = Vector3.zero;

                        //Oluşturulan köpükleri rastgele boyutlandırıyoruz
                        var targetSclae = Vector3.one * Random.Range(0.7f, 1.3f);
                        targetSclae.y = 0.6f;
                        foamPieceObj.transform.DOScale(targetSclae, 1f);

                        currentFoamPieceCount = foamPiecesParent.childCount;

                        //Köpük oluşturuken aynı zamanda arka taraftaki duvarı boyuyoruz
                        var brushObj = Instantiate(brush, hit.point + Vector3.up * 0.1f, brush.transform.rotation);
                        brushObj.transform.parent = brushParent;

                        var brushObjPos = brushObj.transform.position;
                        brushObjPos.z = brushParent.transform.position.z;
                        brushObj.transform.position = brushObjPos;

                        brushObj.transform.localScale = brush.transform.localScale * BrushSize;
                    }
                }
            }
            else
            {
                sprayFX.Stop();
            }

            if (currentFoamPieceCount >= targetFoamPieceCount)
            {
                //Oluşturulan köpük sayısı istenen köpük sayısına ulaşmışsa bir sonraki adım olan testere ile kesmeye geçiyoruz
                sprayFX.Stop();
                WallFoamController.Instance.saw.gameObject.SetActive(true);
                foamPiecesParent.GetChild(0).gameObject.SetActive(true);
                foamWallModel.material = foamWallMatOnFoamWallFull;

                brushParent.gameObject.SetActive(false);
                WallFoamController.Instance.workState = WallFoamWorkState.Saw;
            }
        }
    }
}