using System;
using UnityEngine;
using UnityEngine.UI;
using GAME.Movable;


namespace GAME.Core {
    public class EnemySpawner : MonoBehaviour {

        [SerializeField]
        Text planeCounter = null;
        [SerializeField]
        Text planesDestroyed = null;
        [SerializeField]
        EnemyBehavior enemyPrefab = null;

        private int _planesAwake = 0;
        private int _planesKilled = 0;


        // Start is called before the first frame update
        void Start( ) {
            Text[] texts = FindObjectsOfType<Text>( );
            if( planeCounter == null ) {
                foreach( Text text in texts ) {
                    if( text.name.Equals( "PlaneCounter" ) ) {
                        planeCounter = text;
                    }
                }
            }
            if( planesDestroyed == null ) {
                foreach( Text text in texts ) {
                    if( text.name.Equals( "PlanesDestroyed" ) ) {
                        planesDestroyed = text;
                    }
                }
            }
            if( enemyPrefab == null ) {
                enemyPrefab = Resources.Load( "Prefabs/Enemy" ) as EnemyBehavior;
            }
            _planesAwake = FindObjectsOfType<EnemyBehavior>( ).Length;
            while( _planesAwake < 25 ) {
                Instantiate( enemyPrefab );
                _planesAwake++;
            }
        }

        // Update is called once per frame
        void Update( ) {
            _planesAwake = FindObjectsOfType<EnemyBehavior>( ).Length;
            if( _planesAwake < 25 ) {
                Instantiate( enemyPrefab );
                _planesAwake++;
                _planesKilled++;
            }
            UpdateTexts( );
        }

        private void UpdateTexts( ) {
            planeCounter.text = "Number of Planes: " + _planesAwake;
            planesDestroyed.text = "Planes Destroyed: " + _planesKilled;
        }
    }
}
