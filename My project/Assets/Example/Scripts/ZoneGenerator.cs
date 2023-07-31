using System;
using System.Linq;
using UnityEngine;

namespace minimap.sample
{
    public class ZoneGenerator : MonoBehaviour
    {
        [SerializeField] private GameObject _ground;
        [SerializeField] private GameObject _wallPrefabs;

        private void Start()
        {
            GenerateRandomWalls(10);
        }

        private void GenerateRandomWalls(int count)
        {
            for (int idx = 0; idx < count; idx++)
            {
                Vector3 randomPosition;
                do
                {
                    randomPosition = new Vector3(UnityEngine.Random.Range(-18, 18), 0, UnityEngine.Random.Range(-18, 18));
                } while (Math.Abs(randomPosition.x) < 10 && Math.Abs(randomPosition.y) < 10);

                Vector3 randomRotation = new Vector3(0, UnityEngine.Random.Range(-180, 180), 0);

                GameObject walls = Instantiate(_wallPrefabs, _ground.transform.parent);
                walls.transform.localPosition = randomPosition;
                walls.transform.localRotation = Quaternion.Euler(randomRotation);

                SetRandomColor(walls);
            }
        }

        private void SetRandomColor(GameObject target)
        {
            target.GetComponentsInChildren<MeshRenderer>().ToList().ForEach((MeshRenderer meshRenderer) =>
            {
                meshRenderer.material.color = UnityEngine.Random.ColorHSV();
            });
        }
    }
}