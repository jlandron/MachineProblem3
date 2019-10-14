using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistantAudio : MonoBehaviour
{
    public static PersistantAudio _instance = null;

    private void Awake( ) {
        if( _instance == null ) {
            _instance = this;
            DontDestroyOnLoad( this.gameObject );
        } else {
            Destroy( this );
        }
    }
}
