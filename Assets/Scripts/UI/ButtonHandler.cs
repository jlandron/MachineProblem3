using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GAME.Ui {

    public class ButtonHandler : MonoBehaviour {
        public void LoadLevel( ) {
            SceneManager.LoadScene( 1 );
        }

        public void QuitGame( ) {
            Application.Quit( );
        }
    }
}
