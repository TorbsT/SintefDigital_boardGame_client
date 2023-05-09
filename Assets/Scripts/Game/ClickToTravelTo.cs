using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Game
{
    internal class ClickToTravelTo : MonoBehaviour
    {
        private new Collider2D collider;
        private void Awake()
        {
            collider = GetComponent<Collider2D>();
        }
        private void OnMouseDown()
        {
            if (collider == null) collider = GetComponent<Collider2D>();
            NodeTraversal t = transform.parent.GetComponent<NodeTraversal>();
            if (t != null) t.Click();
        }
    }
}
