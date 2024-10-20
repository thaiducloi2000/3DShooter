using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace Shooter
{
    public class MapInfor : MonoBehaviour
    {
        public static MapInfor Instance { get; private set; }

        [Header("Snap Spawn Point To Ground"), Space(20)]
        [SerializeField] private LayerMask groundLayerMask;
        [SerializeField] private float heightAdd = 0.2f;

        [Header("Start Spawn Point"), Space(20)]
        [SerializeField] private List<Transform> teamASpawnPoints;
        [SerializeField] private List<Transform> teamBSpawnPoints;


        private void Awake()
        {
            Instance = this;
        }

        private void OnDestroy()
        {
            Instance = null;
        }
        public Transform GetRandomSpawnPoint(int teamId)
        {
            return teamId == 1 ? teamASpawnPoints[Random.Range(0, teamASpawnPoints.Count)] : teamBSpawnPoints[Random.Range(0, teamASpawnPoints.Count)];
        }

        public List<Transform> GetAllSpawnPoint(int teamId)
        {
            return teamId == 1 ? teamASpawnPoints : teamBSpawnPoints;
        }

#if UNITY_EDITOR
        [Button("Snap ALL Spawn Point To Ground")]
        public void SnapSpawnPointToGround()
        {
            Snap(teamASpawnPoints.ToArray());
            Snap(teamBSpawnPoints.ToArray());

            void Snap(Transform[] spawnPoints)
            {
                for (int i = 0; i < spawnPoints.Length; i++)
                {
                    Vector3 start = spawnPoints[i].position;
                    start.y = 100;
                    if (Physics.Raycast(start, Vector3.down, out RaycastHit hit, 200, groundLayerMask, QueryTriggerInteraction.Ignore))
                    {
                        spawnPoints[i].position = new Vector3(start.x, hit.point.y + heightAdd, start.z);
                    }
                    else
                    {
                        Debug.LogError($"spawnPoints {i} cannot snap to ground");
                    }
                }
            }
        }
#endif
    }
}
