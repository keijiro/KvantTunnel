//
// Scroller script for Tunnel
//
using UnityEngine;

namespace Kvant
{
    [RequireComponent(typeof(Tunnel))]
    [AddComponentMenu("Kvant/Tunnel Scroller")]
    public class TunnelScroller : MonoBehaviour
    {
        [SerializeField]
        float _speed;

        public float speed {
            get { return _speed; }
            set { _speed = value; }
        }

        void Update()
        {
            GetComponent<Tunnel>().offset += _speed * Time.deltaTime;
        }
    }
}
