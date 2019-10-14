using UnityEngine;


namespace GAME.Movable {
    public class Shaker : MonoBehaviour {
        [SerializeField]
        private Vector2 shakeDelta;
        [SerializeField]
        private float duration;
        [SerializeField]
        private float omega;

        private float _timeLeft;
        private Vector3 _originalPosition;
        private bool _isShaking = false;

        private float nextHarmonic = 0f;

        public bool IsShaking { get => _isShaking; set => _isShaking = value; }
        public Vector3 OriginalPosition { get => _originalPosition; set => _originalPosition = value; }

        private void Start( ) {
            omega = 90 * Mathf.PI;
        }
        
        // Update is called once per frame
        void Update( ) {

            if( IsShaking ) {
                if( _timeLeft <= 0.1 ) {
                    _isShaking = false;
                    return;
                }
                float frac = _timeLeft / duration;
                nextHarmonic = frac * frac * Mathf.Cos( ( 1 - frac ) * omega );
                _timeLeft -= Time.deltaTime;
                Vector3 c = OriginalPosition;
                float fx = Random.Range( 0f, 1f ) > 0.5f ? -nextHarmonic : nextHarmonic;
                float fy = Random.Range( 0f, 1f ) > 0.5f ? -nextHarmonic : nextHarmonic;
                c.x += shakeDelta.x * fx;
                c.y += shakeDelta.y * fx;
                transform.position = c;
            }
        }
        

        internal void StartShaking( Vector2 vector2, float l_duration ) {

            if( !IsShaking ) {
                OriginalPosition = transform.position;
            }
            shakeDelta = vector2;
            duration = l_duration;
            _timeLeft = duration;
            IsShaking = true;
        }
    }
}

