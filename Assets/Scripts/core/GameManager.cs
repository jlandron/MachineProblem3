using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
    [SerializeField]
    private Camera mainCamera;
    [SerializeField]
    private Camera waypointCamera;

    private bool _isPaused = false;

    public static GameManager _instance = null;

    private void Awake( ) {
        if( _instance == null ) {
            _instance = this;
            DontDestroyOnLoad( this.gameObject );
        } else {
            Destroy( this );
        }
    }
    private void Start( ) {
        if( waypointCamera != null ) {
            waypointCamera.enabled = false;
        }

    }
    private void Update( ) {
        if( Input.GetKeyDown( KeyCode.Q ) ) {
            Application.Quit( );
        }
        if( Input.GetKeyDown( KeyCode.Escape ) ) {
            SceneManager.LoadScene( 0 );
        }
        if( Input.GetKeyDown( KeyCode.P ) ) {
            if( !_isPaused ) {
                _isPaused = true;
                Time.timeScale = 0;
            } else {
                _isPaused = false;
                Time.timeScale = 1;
            }
        }
    }



    public void ShowWaypointCam( ) {
        waypointCamera.enabled = true;
    }

    public void ShowChaseCamera( Transform player, Transform enemy ) {

    }
}
