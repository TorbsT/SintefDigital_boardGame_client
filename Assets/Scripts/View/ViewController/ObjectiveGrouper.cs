using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace View
{
    internal class ObjectiveGrouper : MonoBehaviour
    {
        private enum GroupMode
        {
            LINE,
            CIRCLE
        }
        public static ObjectiveGrouper Instance { get; private set; }
        [SerializeField] private float groupLineDistance = 0.5f;
        [SerializeField] private float cooldown = 0.5f;
        [SerializeField, Range(0f, 5f)] private float circleRadius = 1f;
        [SerializeField] private GroupMode groupMode = GroupMode.CIRCLE;
        private Dictionary<int, List<Transform>> nodeToTransforms = new();
        private Dictionary<Transform, int> transformToNode = new();
        private float currentCooldown;
        public void Move(Transform trans, int? nodeId)
        {
            int? prevId = GetNode(trans);
            Unpair(trans, prevId);
            //if (prevId.HasValue) RefreshNodeGroup(prevId.Value);
            if (nodeId == null) return;  // Only remove requested
            int node = nodeId.Value;
            Pair(trans, node);
            //RefreshNodeGroup(node);
        }
        private void Awake()
        {
            Instance = this;
        }
        private void Update()
        {
            currentCooldown -= Time.deltaTime;
            if (currentCooldown <= 0f)
            {
                currentCooldown = cooldown;
                foreach (int id in nodeToTransforms.Keys)
                    RefreshNodeGroup(id);
            }
        }
        private void Pair(Transform trans, int node)
        {
            transformToNode.Add(trans, node);
            if (!nodeToTransforms.ContainsKey(node))
                nodeToTransforms.Add(node, new());
            nodeToTransforms[node].Add(trans);

            Transform nodeTrans = GraphManager.Instance.GetNode(node).gameObject.transform;
            trans.SetParent(nodeTrans, true);
        }
        private void Unpair(Transform trans, int? node)
        {
            if (node == null) return;
            nodeToTransforms[node.Value].Remove(trans);
            if (nodeToTransforms[node.Value].Count == 0)
                nodeToTransforms.Remove(node.Value);
            transformToNode.Remove(trans);

            trans.SetParent(null, true);
        }
        private void RefreshNodeGroup(int node)
        {
            if (!nodeToTransforms.ContainsKey(node)) return;
            ICollection<Transform> transes = nodeToTransforms[node];
            int count = transes.Count;
            
            List<Vector2> poses = new();
            if (groupMode == GroupMode.CIRCLE)
            {
                poses = ExtraMath.GetCircleArrangement(count, circleRadius);
            } else
            {
                int j = 0;
                foreach (Transform trans in transes)
                {
                    Vector2 endPos = (j - (count - 1) / 2f) * groupLineDistance * Vector2.right;
                    poses.Add(endPos);
                    j++;
                }
            }
            int i = 0;
            foreach (Transform trans in transes)
            {
                DoAnimation(trans, poses[i]);
                i++;
            }
        }
        public void DoAnimation(Transform trans, Vector2 endPos)
        {
            Animation<Vector2> moveAnim = new()
            {
                Action = value => trans.localPosition = value,
                Curve = AnimationPresets.Instance.PlayerMoveCurve,
                Duration = AnimationPresets.Instance.PlayerMoveDuration,
                StartValue = trans.localPosition,
                EndValue = endPos
            };
            moveAnim.Start();
        }
        private int? GetNode(Transform trans)
        {
            if (!transformToNode.ContainsKey(trans)) return null;
            return transformToNode[trans];
        }
    }
}
