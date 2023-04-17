using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace View
{
    public class NodeTraversal : MonoBehaviour, INode
    {
        public GameObject player;
        public GameObject[] neighbourNodes;
        private SpriteRenderer spriteRenderer;
        // Start is called before the first frame update
        void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }
        void Start()
        {
            //Debug.Log(GameObject.Find("gameBoard/CitySquare").transform.GetSiblingIndex());
            //Debug.Log(GameObject.Find("gameBoard").transform.GetChild(2).GetType());
            //Debug.Log(transform.GetSiblingIndex());
            Steps steps = GetComponent<Steps>();
        }

        // Update is called once per frame
        void Update()
        {

        }
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
        private void OnMouseEnter()
        {
            if (neighbourNodes.Contains(player.transform.parent.gameObject))
            {
                //Debug.Log("COLOR!!!");
                spriteRenderer.color = Color.cyan;
            }
        }

        private void OnMouseExit()
        {
            //Debug.Log("Reset!!!");
            spriteRenderer.color = Color.white;
        }

        void OnMouseDown()
        {
            if (neighbourNodes.Contains(player.transform.parent.gameObject))
            {
                //Debug.Log("clicked");
                player.transform.parent = transform;
                Animation<Vector2> moveAnimation = new()
                {
                    StartValue = player.transform.position,
                    EndValue = transform.position,
                    Duration = AnimationPresets.Instance.PlayerMoveDuration,
                    Curve = AnimationPresets.Instance.PlayerMoveCurve,
                    Action = (value) => { player.transform.position = value; }
                };
                moveAnimation.Start();
                //player.transform.localPosition = new Vector3(0, 0, 0);
            }
        }
    }
}