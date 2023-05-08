using Network;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace View
{
    public class NodeTraversal : MonoBehaviour, INode
    {
        [SerializeField] private GameObject[] neighbourNodes;
        [SerializeField] private SpriteRenderer spriteRenderer;
        private Animator animator;
        [field: SerializeField] public int Id { get; private set; }
        public GameObject[] relatedPlayerTransformButtons;

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
        // Start is called before the first frame update
        void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            animator = GetComponent<Animator>();
        }
        void Start()
        {
            Steps steps = GetComponent<Steps>();
            SetInteractable(false);
        }

        // Update is called once per frame
        void Update()
        {
            
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

        internal void ShowTransformButtons()
        {
            foreach (var playerTransformButton in relatedPlayerTransformButtons)
            {
                playerTransformButton.SetActive(false);
                playerTransformButton.SetActive(true);
            }
        }

        internal void HideTransformButtons()
        {
            foreach (var playerTransformButton in relatedPlayerTransformButtons)
            {
                playerTransformButton.SetActive(false);
            }
        }

        internal void HideNeighbouringTransformButtons()
        {
            foreach (var node in GetNeighbourScripts())
            {
                node.HideTransformButtons();
            }
        }
    }
}