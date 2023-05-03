using Network;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace View
{
    public class NodeTraversal : MonoBehaviour, INode
    {
        public GameObject[] neighbourNodes;
        private SpriteRenderer spriteRenderer;
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
        public void Click()
        {
            MovementSystem.Instance.ClickNode(this);
        }
        // Start is called before the first frame update
        void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }
        void Start()
        {
            Steps steps = GetComponent<Steps>();
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
    }
}