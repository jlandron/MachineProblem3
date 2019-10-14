using UnityEngine;

namespace GAME.Movable {
    public class Waypoint : MonoBehaviour {
        private Transform _initialPosition;
        private SpriteRenderer _spriteRenderer;
        private Color _color;
        private float _baseHealth = 4f;
        private float _health = 4f;
        private int _timesHit = 0;
        private float _timeLastHit = 0f;
        private const float _timeBetweenHits = 0.05f;
        private bool isVisible = true;

        private void Start( ) {
            _initialPosition = transform;
            _spriteRenderer = GetComponent<SpriteRenderer>( );
        }

        private void Update( ) {
            if( isVisible ) {
                _color = new Color( 1f, 1f, 1f, ( _health / _baseHealth ) );
                _spriteRenderer.color = _color;
            } else {
                _color = new Color( 1f, 1f, 1f, 0f );
                _spriteRenderer.color = _color;
            }


            if( _health == 0 ) {
                GetComponent<Shaker>( ).IsShaking = false;
                respawn( );
            }
            _timeLastHit += Time.deltaTime;
        }

        private void respawn( ) {
            Vector3 newPosition = new Vector3( ( _initialPosition.position.x + UnityEngine.Random.Range( -15f, 15f ) ),
                ( _initialPosition.position.y + UnityEngine.Random.Range( -15f, 15f ) ),
                _initialPosition.position.z );
            transform.position = newPosition;
            _health = _baseHealth;
            _timesHit = 0;
        }

        private void OnTriggerEnter2D( Collider2D collision ) {
            if( collision.gameObject.tag == "Projectile" && ( _timeLastHit > _timeBetweenHits ) ) {
                _timesHit++;
                if(_health > 1 ) {
                    GetComponent<Shaker>( ).StartShaking( new Vector2( _timesHit, _timesHit ), _timesHit );
                }
                _health -= 1f;
                
            }
        }
        public bool IsVisible {
            get => isVisible;
            set => isVisible = value;
        }
    }
}
