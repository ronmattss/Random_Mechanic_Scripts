using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;

namespace NavigationMap
{
    public class Spawn : MonoBehaviour
    {
        public GameObject prefabCubeGreen;
        public GameObject prefabCubeRed;
        public GameObject prefabCubeBlue;
        public GameObject seeker, target;
        public List<GameObject> prefabNodes = new List<GameObject>();
        public static Spawn instance;
        private Spawn _instance;
        void Start()
        {

            _instance = this;
            instance = _instance;
        }

        public void RandomPositions(FieldGrid gridPosition)
        {
            if (prefabNodes.Count != 0)
            {
                prefabNodes.ForEach(x => DestroyImmediate(x));
                prefabNodes.Clear();
            }
            seeker.transform.position = gridPosition.GetRandomFieldNodePoint().worldPosition;
            target.transform.position = gridPosition.GetRandomFieldNodePoint().worldPosition;
            if(target.transform.position == seeker.transform.position)
            {
                target.transform.position = gridPosition.GetRandomFieldNodePoint().worldPosition;
            }
        }
        public void ClearNodes()
        {
            if (prefabNodes.Count != 0)
            {
                prefabNodes.ForEach(x => DestroyImmediate(x));
                prefabNodes.Clear();
            }
        }

        public void placeVisualOpenNode(FieldNode node)
        {
            GameObject nodeObject;

            nodeObject = Instantiate(prefabCubeBlue, node.worldPosition, Quaternion.identity);
            prefabNodes.Add(nodeObject);
        }
        public void placeVisualCloseNode(FieldNode node)
        {
            GameObject nodeObject;
            nodeObject = Instantiate(prefabCubeRed, node.worldPosition, Quaternion.identity);
            DeleteVisualNode(node);
            prefabNodes.Add(nodeObject);
        }
        public void placeVisualCloseNode(Vector3 node)
        {
            GameObject nodeObject;
            nodeObject = Instantiate(prefabCubeRed, node, Quaternion.identity);
            prefabNodes.Add(nodeObject);
        }
        public void placeVisualPathNode(FieldNode node)
        {
            DeleteVisualNode(node);
            GameObject nodeObject;
            nodeObject = Instantiate(prefabCubeGreen, node.worldPosition, Quaternion.identity);
            prefabNodes.Add(nodeObject);
        }
        public void placeVisualPathNode(Vector3[] paths, FieldNode node)
        {
            DeleteVisualNode(node);
            GameObject nodeObject;
            for (int i = 0; i < paths.Length; i++)
            {
                nodeObject = Instantiate(prefabCubeGreen, paths[i], Quaternion.identity);
                prefabNodes.Add(nodeObject);
            }


        }
        public void placeVisualPathNode(Vector3[] paths)
        {

            GameObject nodeObject;
            for (int i = 0; i < paths.Length; i++)
            {
                nodeObject = Instantiate(prefabCubeGreen, paths[i], Quaternion.identity);
                DeleteVisualNode(paths[i]);
                prefabNodes.Add(nodeObject);
            }


        }
        public void placeVisualPathNode(Vector3 paths)
        {
            GameObject nodeObject;
            nodeObject = Instantiate(prefabCubeGreen, paths, Quaternion.identity);
            DeleteVisualNode(paths);
            prefabNodes.Add(nodeObject);



        }
        public void placeVisualCloseNode(Vector3[] paths)
        {

            GameObject nodeObject;
            for (int i = 0; i < paths.Length; i++)
            {
                nodeObject = Instantiate(prefabCubeRed, paths[i], Quaternion.identity);
                DeleteVisualNode(paths[i]);
                prefabNodes.Add(nodeObject);
            }


        }
        public void DeleteVisualNode(FieldNode node)
        {
            if (prefabNodes.Count != 0)
            {
                GameObject[] toBeDeleted = prefabNodes.Where(x => x.transform.position == node.worldPosition).ToArray();
                foreach (GameObject prefab in toBeDeleted)
                {
                    //  Destroy(prefab);
                }
            }
            else
            {

            }
        }
        public void DeleteVisualNode(Vector3 waypoint)
        {
            GameObject[] toBeDeleted = prefabNodes.Where(x => x.transform.position == waypoint).ToArray();
            foreach (GameObject prefab in toBeDeleted)
            {
                //      Destroy(prefab);
            }
        }



    }
}
