using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace AimingTrainingRoom
{
    public class AimingTrainingScene : MonoBehaviour
    {
        [SerializeField] private int level;
        [SerializeField] GameObject dartboardPrefab;
        private List<GameObject> dartboards;

        public void LevelUp()
        {
            
        }
    }
}