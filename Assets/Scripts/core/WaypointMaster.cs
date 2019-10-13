using GAME.Movable;
using System;
using UnityEngine;
using UnityEngine.UI;


namespace GAME.Core {
    public class WaypointMaster : MonoBehaviour {
        [SerializeField]
        Text wayPointSequence = null;
        private Waypoint[] waypoints;
        private bool areVisible = true;
        private bool sequential = true;

        // Start is called before the first frame update
        void Start( ) {
            waypoints = FindObjectsOfType<Waypoint>( );

            if( wayPointSequence == null ) {
                Text[] texts = FindObjectsOfType<Text>( );
                foreach( Text text in texts ) {
                    if( text.name.Equals( "WayPointSequence" ) ) {
                        wayPointSequence = text;
                    }
                }
            }
        }

        // Update is called once per frame
        void Update( ) {
            CheckKeyBoardInput( );
            SetVisibility( );
            UpdateText( );
        }

        private void UpdateText( ) {
            if( sequential ) {
                wayPointSequence.text = "Waypoint order: Sequential";
            } else {
                wayPointSequence.text = "Waypoint order: Random";
            }
        }

        public Waypoint GetNextWaypoint( Waypoint previous ) {
            Waypoint next;
            int previousLocation = 0;
            if( sequential ) {
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
                waypoint.IsVisible = areVisible;
            }
        }

        private void CheckKeyBoardInput( ) {
            if( Input.GetKeyDown( KeyCode.H ) ) {
                areVisible = !areVisible;
            }
            if( Input.GetKeyDown( KeyCode.J ) ) {
                sequential = !sequential;
            }
        }
    }
}
