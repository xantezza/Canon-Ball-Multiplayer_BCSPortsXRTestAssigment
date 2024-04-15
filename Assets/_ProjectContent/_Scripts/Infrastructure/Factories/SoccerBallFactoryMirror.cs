using Gameplay.SoccerBall;
using Mirror;
using UnityEngine;

namespace Infrastructure.Factories
{
    public class SoccerBallFactoryMirror : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private SoccerBall prefab;

        private int currentCount;
        private Pool<SoccerBall> pool;

        private void Start()
        {
            InitializePool();
            NetworkClient.RegisterPrefab(prefab.gameObject, SpawnHandler, UnspawnHandler);
        }

        private GameObject SpawnHandler(SpawnMessage msg) => Create(msg.position, msg.rotation).gameObject;

        private void UnspawnHandler(GameObject spawned) => Despawn(spawned.GetComponent<SoccerBall>());

        private void OnDestroy()
        {
            NetworkClient.UnregisterPrefab(prefab.gameObject);
        }

        public void Despawn(SoccerBall soccerBall)
        {
            soccerBall.gameObject.SetActive(false);
            pool.Return(soccerBall);
        }

        public SoccerBall Create(Vector3 position, Quaternion rotation)
        {
            var next = pool.Get();
            next.transform.position = position;
            next.transform.rotation = rotation;
            next.gameObject.SetActive(true);
            return next;
        }

        private void InitializePool()
        {
            pool = new Pool<SoccerBall>(CreateNew, 5);
        }

        private SoccerBall CreateNew()
        {
            var next = Instantiate(prefab, transform);
            next.name = $"{prefab.name}_pooled_{currentCount}";
            next.gameObject.SetActive(false);
            currentCount++;
            return next;
        }
    }
}