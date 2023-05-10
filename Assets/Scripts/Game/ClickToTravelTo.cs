using UnityEngine;

namespace Game
{
    /// <summary>
    /// Use if you want players to be able to click package/goal/others in order to
    /// move to that node
    /// </summary>
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
