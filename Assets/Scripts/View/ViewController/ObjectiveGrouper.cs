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
        public static ObjectiveGrouper Instance { get; private set; }
        [SerializeField] private float groupDistance = 0.5f;

        private void Awake()
        {
            Instance = this;
        }


    }
}
