using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerScripts
{
    public class SpawnManager : MonoBehaviour
    {
        //Spawn Manager:
        // manage spawn points
        // spawn points spawn enemies
        // disable enable spawn points
        // spawn spawn points on the grid inside the field
        // 
        public FieldGrid fieldGrid;
        public GameObject enemyEntity;
        public Transform playerTransform;
        [SerializeField]
        public Stack<FieldNode> nodeStack = new Stack<FieldNode>();
        public List<GameObject> entities;
        public int waveCount = 0;

        // Start is called before the first frame update
        void Start()
        {
            //populate Stack with nodes
            GenerateNodeStack();

            // SpawnFromStack();
            InvokeRepeating("CheckEnemies", 1f, 0.5f);

        }

        void Update()
        {



        }
        void GenerateNodeStack()
        {
            for (int i = 0; i < 2 + waveCount; i++)
            {

                nodeStack.Push(fieldGrid.GetRandomFieldNodePoint());
            }
            SpawnFromStack();

        }
        void SpawnFromStack()
        {
            while (nodeStack.Count != 0)
            {
                // Debug.Log($"Random Coordinates: {nodeStack.Pop().worldPosition}");
                GameObject spawnedEntity = Instantiate(enemyEntity, nodeStack.Pop().worldPosition, Quaternion.identity);
                spawnedEntity.GetComponent<EnemyAI>().target = playerTransform;
                entities.Add(spawnedEntity);

            }
        }
        void CheckEnemies()
        {
            foreach (GameObject g in entities)
            {
                if (entities.Count != 0)
                {
                    if (g == null)
                    {
                        entities.Remove(g);
                    }
                    else
                        return;
                }

            }
            if (entities.Count == 0)
            {
                waveCount++;
                GenerateNodeStack();
            }
        }
    }
}
