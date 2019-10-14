using GAME.Movable;
using UnityEngine;
using UnityEngine.UI;


namespace GAME.Core {
    public class WaypointMaster : MonoBehaviour {
        [SerializeField]
        Text wayPointSequence = null;
        [SerializeField]
        GameObject waypointCamera = null;
        [SerializeField]
        Text cameraText = null;

        private Waypoint[] waypoints;
        private bool _areVisible = true;
        private bool _sequential = true;
        private bool _cameraIsActive = false;



        // Start is called before the first frame update
        void Start( ) {
            waypoints = FindObjectsOfType<Waypoint>( );

            if( wayPointSequence == null || cameraText == null ) {
                Text[] texts = FindObjectsOfType<Text>( );
                foreach( Text text in texts ) {
                    if( text.name.Equals( "WayPointSequence" ) ) {
                        wayPointSequence = text;
                    }
                    if( text.name.Equals( "WaypointCameraText" ) ) {
                        cameraText = text;
                    }
                }
            }
            //waypointCamera.SetActive( false );
        }

        // Update is called once per frame
        void Update( ) {
            CheckKeyBoardInput( );
            SetVisibility( );
            UpdateText( );
        }

        private void LateUpdate( ) {
            checkShaking( );
        }

        private void checkShaking( ) {

            foreach( Waypoint waypoint in waypoints ) {
                Shaker shaker = waypoint.GetComponent<Shaker>( );
                if( shaker.IsShaking ) {
                    waypointCamera.SetActive( true );
                    _cameraIsActive = true;
                    Vector3 newPosition = new Vector3( shaker.OriginalPosition.x, shaker.OriginalPosition.y, transform.position.z );
                    waypointCamera.transform.position = newPosition;
                    return;
                }
            }
            waypointCamera.SetActive( false );
            _cameraIsActive = false;
        }

        private void UpdateText( ) {
            if( _sequential ) {
                wayPointSequence.text = "";
                wayPointSequence.text = "Waypoint order: Sequential";
            } else {
                wayPointSequence.text = "";
                wayPointSequence.text = "Waypoint order: Random";
            }
            if( _cameraIsActive) {
                cameraText.text = "";
                cameraText.text = "Waypoint Camera: Active";
            } else {
                cameraText.text = "Waypoint Camera: Inactive";
            }
        }

        public Waypoint GetNextWaypoint( Waypoint previous ) {
            Waypoint next;
            int previousLocation = 0;
            if( _sequential ) {
                for( int i = 0; i < waypoints.Length; i++ ) {

                    if( previous.Equals( waypoints[ i ] ) ) {
                        previousLocation = i;
                    }
                }
                return waypoints[ ( previousLocation < ( waypoints.Length - 1 ) ? previousLocation + 1 : 0 ) ];
            } else {
                return waypoints[ UnityEngine.Random.Range( 0, waypoints.Length - 1 ) ];
            }
        }

        private void SetVisibility( ) {
            foreach( Waypoint waypoint in waypoints ) {
                waypoint.IsVisible = _areVisible;
            }
        }

        private void CheckKeyBoardInput( ) {
            if( Input.GetKeyDown( KeyCode.H ) ) {
                _areVisible = !_areVisible;
            }
            if( Input.GetKeyDown( KeyCode.J ) ) {
                _sequential = !_sequential;
            }
        }
    }
}
