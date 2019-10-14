using GAME.Movable;
using UnityEngine;

public class smoothFollowCam2D : MonoBehaviour {
    [SerializeField]
    private float FollowSpeed = 2f;
    [SerializeField]
    private Transform Target;

    private void Update( ) {
        Vector3 newPosition = Target.position;
        newPosition.z = -10;
        transform.position = Vector3.Slerp( transform.position, newPosition, FollowSpeed * Time.deltaTime );
    }
}