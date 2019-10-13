using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GAME.Movable {
    public class CollisionBouncer : MonoBehaviour {
        private float _timeSinceLastBouce;
        private float _multipleBounceDelay;
        private float _maxX;
        private Bounds _screenBounds;
        private float _minX;
        private float _maxY;
        private float _minY;

        // Start is called before the first frame update
        void Start( ) {
            _screenBounds = Camera.main.GetWorldBounds( );
            _minX = -( _screenBounds.extents.x );
            _maxX = _screenBounds.extents.x;
            _minY = -( _screenBounds.extents.y );
            _maxY = _screenBounds.extents.y;
        }

        private void LateUpdate( ) {
            BouceOffWalls( );
        }
        private void BouceOffWalls( ) {
            _timeSinceLastBouce += Time.deltaTime;
            if( _timeSinceLastBouce > _multipleBounceDelay ) {
                if( transform.position.x >= _maxX || transform.position.x <= _minX ) {
                    transform.up = new Vector3( -transform.up.x,
                                               transform.up.y,
                                               transform.up.z );
                }
                if( transform.position.y >= _maxY || transform.position.y <= _minY ) {
                    transform.up = new Vector3( transform.up.x,
                                               -transform.up.y,
                                               transform.up.z );
                }
                _timeSinceLastBouce = 0f;
            }
        }
    }
}
