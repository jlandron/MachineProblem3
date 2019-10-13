using UnityEngine;

namespace GAME.Movable {
    public class EggBehavior : MonoBehaviour {
        [SerializeField]
        private float speed = 100f;
        [SerializeField]
        private float lifeSpan = 2f;

        private float _timeAlive = 0f;
        private Bounds _bounds;
        private float _minX;
        private float _maxX;
        private float _minY;
        private float _maxY;
        private bool _isDead = false;

        private void Start( ) {
            _bounds = Camera.main.GetWorldBounds( );
            _minX = -( _bounds.extents.x );
            _maxX = _bounds.extents.x;
            _minY = -( _bounds.extents.y );
            _maxY = _bounds.extents.y;
        }
        // Update is called once per frame
        void Update( ) {
            transform.position += speed * transform.up * Time.deltaTime;
            _timeAlive += Time.deltaTime;
            CheckLife( );
        }
        private void CheckLife( ) {
            bool isOutOfBounds = CheckIfOutOfBounds( );
            if( _timeAlive >= lifeSpan || isOutOfBounds ) {
                _isDead = true;
            }
            if( _isDead ) {
                Destroy( gameObject );
            }
        }
        private bool CheckIfOutOfBounds( ) {
            if( transform.position.x >= _maxX || transform.position.x <= _minX ||
                transform.position.y >= _maxY || transform.position.y <= _minY ) {
                return true;
            }
            return false;
        }
        public bool isDead( ) {
            return _isDead;
        }
        private void OnTriggerEnter2D( Collider2D collision ) {
            if( collision.gameObject.tag == "Enemy" || collision.gameObject.tag == "Waypoint" ) {
                _isDead = true;
            }
        }
    }
}