using GAME.Core;
using UnityEngine;

namespace GAME.Movable {
    public class EnemyBehavior : MonoBehaviour {
        [SerializeField]
        private float speed = 20f;
        [SerializeField]
        private float waypointTolorence = 25f;
        [SerializeField]
        private float turnSpeed = 0.05f;
        [SerializeField]
        private float chaseDistance = 40f;
        [SerializeField]
        private float rotationSpeed = 50f;
        [SerializeField]
        private float scaleSpeed = 1f;
        [SerializeField]
        private float sizeChangeTime = 1f;
        [SerializeField]
        private float rotationTime = 1f;

        [SerializeField]
        public HeroMover hero;
        [SerializeField]
        private Waypoint waypoint;
        [SerializeField]
        private WaypointMaster waypointMaster;
        [SerializeField]
        private Sprite stunnedSprite;
        [SerializeField]
        private Sprite crippledSprite;

        public bool isChasing;



        private float _stateTime = 0f;
        private SpriteRenderer _spriteRenderer;
        [SerializeField]
        private Vector3 _lerpTo;
        [SerializeField]
        private EnemyState _state = EnemyState.PATROL;
        private float _currentSpeed = 0;
        private float _accelerationRate = 1f;
        private int _pushDistance = 4;
        private bool _pushed = false;
        private Bounds _bounds;
        private float _minX;
        private float _maxX;
        private float _minY;
        private float _maxY;
        private int _timeAlive;
        private int lifeSpan;
        private bool _isDead;

        private void Start( ) {
            transform.rotation = Quaternion.identity;
            waypointMaster = FindObjectOfType<WaypointMaster>( );
            _spriteRenderer = GetComponent<SpriteRenderer>( );
            if( waypoint == null ) {
                waypoint = FindObjectOfType<Waypoint>( );
            }
            if( hero == null ) {
                hero = FindObjectOfType<HeroMover>( );
            }

            _bounds = Camera.main.GetWorldBounds( );
            _minX = -( _bounds.extents.x );
            _maxX = _bounds.extents.x;
            _minY = -( _bounds.extents.y );
            _maxY = _bounds.extents.y;
        }
        private EnemyState State { get => _state; set => _state = value; }

        enum EnemyState {
            PATROL,
            CCW_ROTATION,
            CW_ROTATION,
            CHASE,
            ENLARGE,
            SHRINK,
            STUNNED,
            CRIPPLED
        }
        // Update is called once per frame
        void Update( ) {
            switch( _state ) {
                case EnemyState.PATROL:
                ExecutePatrol( );
                break;
                case EnemyState.CCW_ROTATION:
                executeCCWRotation( );
                break;
                case EnemyState.CW_ROTATION:
                ExecuteCWRotation( );
                break;
                case EnemyState.CHASE:
                ExecuteChase( );
                break;
                case EnemyState.ENLARGE:
                ExecuteEnlarge( );
                break;
                case EnemyState.SHRINK:
                ExecuteShrink( );
                break;
                case EnemyState.STUNNED:
                ExecuteStunned( );
                break;
                case EnemyState.CRIPPLED:
                ExecuteCrippled( );
                break;
            }
            transform.rotation = Quaternion.Euler( 0, 0, transform.rotation.eulerAngles.z );
            CheckLife( );
            CheckPushed( );
        }

        private void CheckPushed( ) {
            if( _pushed ) {
                if( _state == EnemyState.STUNNED ) {
                    if( Vector3.Distance( transform.position, _lerpTo ) >= 0.1 ) {
                        transform.position = Vector3.Lerp( transform.position, _lerpTo, Time.deltaTime );
                    } else {
                        _pushed = false;
                    }
                } else {
                    if( Vector3.Distance( transform.position, _lerpTo ) >= 0.1 ) {
                        transform.position = Vector3.Lerp( transform.position, 2 * _lerpTo, Time.deltaTime );
                        transform.position = new Vector3( transform.position.x, transform.position.y, 0f );
                    } else {
                        _pushed = false;
                    }
                }
            }
        }

        private void ExecutePatrol( ) {
            _spriteRenderer.color = Color.white;
            if( Vector3.Distance( transform.position, waypoint.transform.position ) < waypointTolorence ) {
                waypoint = waypointMaster.GetNextWaypoint( waypoint );
            }
            PointAtPosition( waypoint.transform.position, turnSpeed * Time.deltaTime );
            _currentSpeed = Mathf.Lerp( _currentSpeed, speed, Time.deltaTime * _accelerationRate );
            transform.position += _currentSpeed * transform.up * Time.deltaTime;
        }

        private void executeCCWRotation( ) {
            if( _stateTime > rotationTime ) {
                _state = EnemyState.CW_ROTATION;
                _stateTime = 0f;
            }
            _stateTime += Time.deltaTime;
            _spriteRenderer.color = Color.red;
            transform.Rotate( 0, 0, rotationSpeed * Time.deltaTime, Space.Self );
        }

        private void ExecuteCWRotation( ) {
            if( _stateTime > rotationTime ) {
                _state = EnemyState.CHASE;
                _stateTime = 0f;
            }
            _stateTime += Time.deltaTime;
            transform.Rotate( 0, 0, -rotationSpeed * Time.deltaTime, Space.Self );
        }

        private void ExecuteChase( ) {
            isChasing = true;
            if( Vector3.Distance( transform.position, hero.transform.position ) > chaseDistance ) {
                _state = EnemyState.ENLARGE;
                isChasing = false;
                _stateTime = 0f;
            }
            _stateTime += Time.deltaTime;
            Vector2 direction = new Vector2( ( hero.transform.position.x - transform.position.x ),
                ( hero.transform.position.y - transform.position.y ) );
            transform.up = direction;
            if( Vector2.Distance( hero.transform.position, transform.position ) > 0.1 ) {
                transform.position += _currentSpeed * transform.up * Time.deltaTime;
            }

        }

        private void ExecuteEnlarge( ) {
            if( _stateTime > sizeChangeTime ) {
                _state = EnemyState.SHRINK;
                _stateTime = 0f;
            }
            _stateTime += Time.deltaTime;
            float scale = transform.localScale.x;
            scale += scaleSpeed * Time.deltaTime;
            transform.localScale = new Vector3( scale, scale, 1 );
        }
        private void ExecuteShrink( ) {
            if( _stateTime > sizeChangeTime ) {
                _state = EnemyState.PATROL;
                _stateTime = 0f;
            }
            _stateTime += Time.deltaTime;
            float scale = transform.localScale.x;
            scale -= scaleSpeed * Time.deltaTime;
            transform.localScale = new Vector3( scale, scale, 1 );
        }

        private void ExecuteStunned( ) {
            _spriteRenderer.sprite = stunnedSprite;
            transform.Rotate( 0, 0, rotationSpeed * Time.deltaTime, Space.Self );
        }

        private void ExecuteCrippled( ) {
            _spriteRenderer.sprite = crippledSprite;
        }

        private void OnTriggerEnter2D( Collider2D collision ) {
            isChasing = false;
            if( collision.gameObject.tag == "Projectile" ) {
                if( _state == EnemyState.STUNNED ) {
                    GetPushInfo( collision );
                    _state = EnemyState.CRIPPLED;
                } else if( _state == EnemyState.CRIPPLED ) {
                    Destroy( gameObject );
                } else {
                    GetPushInfo( collision );
                    _state = EnemyState.STUNNED;
                }
            }
            if( collision.gameObject.tag == "Player" ) {
                //check states and move to anger
                if( _state == EnemyState.PATROL ) {
                    _stateTime = 0;
                    _state = EnemyState.CCW_ROTATION;
                }
            }
        }

        private void GetPushInfo( Collider2D collision ) {
            _pushed = true;
            Vector3 heading = collision.gameObject.transform.up;
            float distance = heading.magnitude;
            Vector3 directionToMove = heading / distance;
            _lerpTo = transform.position + _pushDistance * directionToMove;
        }

        private void PointAtPosition( Vector3 p, float r ) {
            Vector3 v = p - transform.position;
            transform.up = Vector3.LerpUnclamped( transform.up, v, r );
        }
        private bool CheckIfOutOfBounds( ) {
            if( transform.position.x > _maxX + 1 || transform.position.x < _minX - 1 ||
                transform.position.y > _maxY + 1 || transform.position.y < _minY - 1 ) {
                return true;
            }
            return false;
        }
        private void CheckLife( ) {
            bool isOutOfBounds = CheckIfOutOfBounds( );
            if( isOutOfBounds ) {
                _isDead = true;
            }
            if( _isDead ) {
                Destroy( gameObject );
            }
        }
    }
}
