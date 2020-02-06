using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerScripts
{
    public class GameHandler : MonoBehaviour
    {

        // handles the resource nodes and storage nodes
        // technically it will be a structure

        private static GameHandler instance;

        [SerializeField] private Transform goldNode1Transform;
        [SerializeField] private Transform goldNode2Transform;
        [SerializeField] private Transform goldNode3Transform;
        [SerializeField] private Transform storageTransform;

        private List<ResourceNode> resourceNodeList;

        private void Awake()
        {
            instance = this;

            resourceNodeList = new List<ResourceNode>();
            resourceNodeList.Add(new ResourceNode(goldNode1Transform));
            resourceNodeList.Add(new ResourceNode(goldNode2Transform));
            resourceNodeList.Add(new ResourceNode(goldNode3Transform));
        }

        private ResourceNode GetResourceNode()
        {
            // simple resource node randomization
            List<ResourceNode> tmpResourceNodeList = new List<ResourceNode>(resourceNodeList);
            return tmpResourceNodeList[UnityEngine.Random.Range(0, tmpResourceNodeList.Count)];

        }

        public static ResourceNode GetResourceNode_Static()
        {
            return instance.GetResourceNode();
        }

        private Transform GetStorage()
        {
            return storageTransform;
        }

        public static Transform GetStorage_Static()
        {
            return instance.GetStorage();
        }
    }

}
