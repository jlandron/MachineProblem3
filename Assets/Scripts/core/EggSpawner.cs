using UnityEngine;
using UnityEngine.UI;
using GAME.Movable;

namespace GAME.Core {
    public class EggSpawner : MonoBehaviour {

        [Header( "Prefabs" )]
        [SerializeField]
        private GameObject eggPrefab = null;
        [Header( "Text output" )]
        [SerializeField]
        private Text eggCounter = null;

        [SerializeField]
        private Image cooldownBar = null;

        private float _maxEggs = 60f;
        private float _cooldown = 0.2f;
        private float _timeSinceLastEggSpawned = 0;
        private float _eggCount = 0;

        private void Start( ) {
            CheckConnections( );
        }

        // Update is called once per frame
        void Update( ) {
            _eggCount = FindObjectsOfType<EggBehavior>( ).Length;
            eggCounter.text = "Number of Eggs: " + _eggCount;
            _timeSinceLastEggSpawned += Time.deltaTime;
            float scale = ( 1 - ( _timeSinceLastEggSpawned / _cooldown ) );
            scale = Mathf.Clamp( scale, 0, 1 );
            Vector2 vector2 = new Vector2( scale , 0.1f );
            cooldownBar.rectTransform.localScale = vector2;
        }
        public bool SpawnEggs( Transform l_Transform ) {

            if( ( _timeSinceLastEggSpawned >= _cooldown ) && ( _eggCount < _maxEggs ) ) {
                Instantiate( eggPrefab,
                            l_Transform.position,
                            l_Transform.rotation );
                _timeSinceLastEggSpawned = 0;
                return true;
            }
            return false;

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
            if(cooldownBar == null ) {
                Image[] images = FindObjectsOfType<Image>( );
                foreach(Image image in images ) {
                    if( image.name.Equals( "CooldownBar" ) ) {
                        cooldownBar = image;
                        break;
                    }
                }
            }
        }
    }
}
