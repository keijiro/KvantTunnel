//
// Noise scroller script for Tunnel
//
using UnityEngine;

namespace Kvant
{
    [RequireComponent(typeof(Tunnel))]
    [AddComponentMenu("Kvant/Tunnel Noise Scroller")]
    public class TunnelNoiseScroller : MonoBehaviour
    {
        [SerializeField]
        float _speed;

        public float speed {
            get { return _speed; }
            set { _speed = value; }
        }

        [SerializeField]
        float _rotation;

        public float rotation {
            get { return _rotation; }
            set { _rotation = value; }
        }

        void Update()
        {
            GetComponent<Tunnel>().noiseOffset +=
                new Vector2(_rotation, _speed) * Time.deltaTime;
        }
    }
}
