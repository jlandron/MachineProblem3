using UnityEngine;
using UnityEngine.UI;
using GAME.Movable;

namespace GAME.Core {
    public class EggSpawner : MonoBehaviour {

        [Header( "Prefabs" )]
        [SerializeField]
        private GameObject eggPrefab;
        [Header( "Text output" )]
        public Text eggCounter = null;

        private float _maxEggs = 60f;
        private float _cooldown = 0.2f;
        private float _timeSinceLastEggSpawned = 0;
        private float _eggCount = 0;

        private void Start( ) {
            CheckConnections( );
        }

        // Update is called once per frame
        void Update( ) {
            cullEggs( );

            eggCounter.text = "Number of Eggs: " + _eggCount;
            _timeSinceLastEggSpawned += Time.deltaTime;
        }
        public void SpawnEggs( Transform l_Transform ) {

            if( ( _timeSinceLastEggSpawned >= _cooldown ) && ( _eggCount < _maxEggs ) ) {
                Instantiate( eggPrefab,
                            l_Transform.position,
                            l_Transform.rotation );
                _timeSinceLastEggSpawned = 0;
            }

        }
        private void cullEggs( ) {
            EggBehavior[] eggs = FindObjectsOfType<EggBehavior>( );
            _eggCount = eggs.Length;
        }

        private void CheckConnections( ) {
            if( eggPrefab == null ) {
                eggPrefab = Resources.Load( "Prefabs/Egg" ) as GameObject;
                if( eggPrefab == null ) {
                    Debug.Log( "Failed to Load egg prefab" );
                } else {
                    Debug.Log( "Loaded prefab" );
                }
            }

            Text[] sceneTexts = Resources.FindObjectsOfTypeAll<Text>( );
            foreach( Text text in sceneTexts ) {
                if( eggCounter == null ) {
                    if( text.gameObject.name == "EggCounter" ) {
                        eggCounter = text;
                    }
                }
            }
        }
    }
}
