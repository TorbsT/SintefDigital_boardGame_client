using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class NodeTraversal : MonoBehaviour, INode
    {
        [SerializeField] private GameObject[] neighbourNodes;
        [SerializeField] private SpriteRenderer spriteRenderer;
        private Animator animator;
        [field: SerializeField] public int Id { get; private set; }

        public ICollection<INode> GetNeighbours()
        {
            List<INode> result = new();
            foreach (GameObject neighbourNode in neighbourNodes)
            {
                if (neighbourNode == null) continue;
                result.Add(neighbourNode.GetComponent<INode>());
            }
            return result;
        }

        internal List<NodeTraversal> GetNeighbourScripts()
        {
            List<NodeTraversal> result = new();
            foreach (GameObject neighbourNode in neighbourNodes)
            {
                if (neighbourNode == null) continue;
                result.Add(neighbourNode.GetComponent<NodeTraversal>());
            }
            return result;
        }
        public void SetInteractable(bool show)
        {
            animator.SetBool("active", show);
        }
        public void Click()
        {
            MovementSystem.Instance.ClickNode(this);
        }

        void Awake()
        {
            animator = GetComponent<Animator>();
        }
        void Start()
        {
            SetInteractable(false);
        }

        private void OnMouseEnter()
        {
            spriteRenderer.color = Color.cyan;
        }

        private void OnMouseExit()
        {
            spriteRenderer.color = Color.white;
        }

        void OnMouseDown()
        {
            Click();
        }
    }
}