using System;
using UnityEngine;
using UnityEngine.UI;
using GAME.Core;

namespace GAME.Movable {
    public class HeroMover : MonoBehaviour {
        [SerializeField]
        private float rotationDegrees = 90f;
        [SerializeField]
        private float rotationTime = 2f;
        [SerializeField]
        private EggSpawner eggSpawner = null;
        [SerializeField]
        Text heroHitCounter = null;

        private float _timesHitByChaser = 0;
        private float _rotationSpeed;
        private Bounds _screenBounds;

        // Start is called before the first frame update
        void Start( ) {
            if( eggSpawner == null ) {
                eggSpawner = FindObjectOfType<EggSpawner>( );
            }
            if( heroHitCounter == null ) {
                Text[] texts = FindObjectsOfType<Text>( );
                foreach( Text text in texts ) {
                    if( text.name.Equals( "HeroHitCounter" ) ) {
                        heroHitCounter = text;
                    }
                }
            }
            _rotationSpeed = rotationDegrees / rotationTime;
        }
        // Update is called once per frame
        void Update( ) {
            GetKeyboardInput( );
            GetMouseInput( );
            UpdateText( );
        }

        private void UpdateText( ) {
            heroHitCounter.text = "Hero has been hit " + _timesHitByChaser + " times";
        }

        private void OnTriggerEnter2D( Collider2D collision ) {
            if( collision.gameObject.tag == "Enemy" ) {
                _timesHitByChaser++;
            }
        }
        private void GetMouseInput( ) {
            Vector3 position = Camera.main.ScreenToWorldPoint( Input.mousePosition );
            transform.position = new Vector3( position.x, position.y, 0 );

        }
        private void GetKeyboardInput( ) {

            if( Input.GetAxis( "Horizontal" ) > 0 ) {
                transform.Rotate( Vector3.forward * -1 * _rotationSpeed * Time.deltaTime );
            } else if( Input.GetAxis( "Horizontal" ) < 0 ) {
                transform.Rotate( Vector3.forward * _rotationSpeed * Time.deltaTime );
            }
            if( Input.GetKey( KeyCode.Space ) ) {
                eggSpawner.SpawnEggs( transform );
            }

            if( Input.GetKeyDown( KeyCode.Q ) ) {
                Application.Quit( );
            }
        }
    }
}

