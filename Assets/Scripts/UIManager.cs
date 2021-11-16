using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WallFoam
{
    public class UIManager : MonoBehaviour
    {
        public static UIManager Instance;
        public GameObject levelCompletePanel;

        private void Awake()
        {
            Instance = this;
        }
    }
}