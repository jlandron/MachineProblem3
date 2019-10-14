using GAME.Movable;
using UnityEngine;
using UnityEngine.UI;


namespace GAME.Core {
    public class EnemySpawner : MonoBehaviour {

        [SerializeField]
        Text planeCounter = null;
        [SerializeField]
        Text planesDestroyed = null;
        [SerializeField]
        EnemyBehavior enemyPrefab = null;
        [SerializeField]
        private int numberOfEnemies = 25;
        [SerializeField]
        private Camera chaseCam = null;
        [SerializeField]
        private Text chaseCamText = null;
        [SerializeField]
        private GameObject chaseCamBlocker = null;

        private int _planesAwake = 0;
        private int _planesKilled = 0;
        private bool _isChasing = false;

        private EnemyBehavior[] enemies;


        // Start is called before the first frame update
        void Start( ) {
            CheckConnections( );
        }

        

        // Update is called once per frame
        void Update( ) {
            enemies = FindObjectsOfType<EnemyBehavior>( );
            _planesAwake = enemies.Length;
            if( _planesAwake < numberOfEnemies ) {
                EnemyBehavior enemy = Instantiate( enemyPrefab );
                enemy.transform.parent = gameObject.transform;
                _planesAwake++;
                _planesKilled++;
            }
            UpdateTexts( );
            CheckChasing( );
        }
        private void LateUpdate( ) {
            if( !_isChasing ) {
                chaseCam.gameObject.SetActive( false );
            }
        }

        private void CheckChasing( ) {
            Vector3 newPosition;
            EnemyBehavior[] enemies = FindObjectsOfType<EnemyBehavior>( );
            foreach( EnemyBehavior enemy in enemies ) {
                if( enemy.isChasing ) {
                    chaseCam.gameObject.SetActive( true );
                    _isChasing = true;
                    newPosition = new Vector3( enemy.hero.transform.position.x + enemy.transform.position.x,
                        enemy.hero.transform.position.y + enemy.transform.position.y,
                        transform.position.z );
                    chaseCam.transform.position = newPosition * 0.5f;
                    chaseCam.orthographicSize = Mathf.Max( Vector3.Magnitude( enemy.hero.transform.position - enemy.transform.position ), 4 );
                    return;
                }
            }
            newPosition = new Vector3( chaseCamBlocker.transform.position.x, chaseCamBlocker.transform.position.y, transform.position.z );
            chaseCam.transform.position = newPosition;
            _isChasing = false;
        }

        private void UpdateTexts( ) {
            planeCounter.text = "Number of Planes: " + _planesAwake;
            planesDestroyed.text = "Planes Destroyed: " + _planesKilled;

            if( _isChasing ) {
                chaseCamText.text = "";
                chaseCamText.text = "Waypoint Camera: Active";
            } else {

                chaseCamText.text = "Waypoint Camera: Inactive";
            }
        }
        private void CheckConnections( ) {
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
            enemies = FindObjectsOfType<EnemyBehavior>( );
            _planesAwake = enemies.Length;
            while( _planesAwake < numberOfEnemies ) {
                EnemyBehavior enemy = Instantiate( enemyPrefab );
                enemy.transform.parent = gameObject.transform;
                _planesAwake++;
            }
        }
    }
}
