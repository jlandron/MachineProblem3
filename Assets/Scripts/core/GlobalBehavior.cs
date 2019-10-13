using UnityEngine;

public static class GlobalBehavior {
    public static Bounds GetWorldBounds(this Camera camera ) {
        float aspectRatio = Screen.width / ( float )Screen.height;
        float cameraHeight = camera.orthographicSize * 2;
        Bounds bounds = new Bounds(
            camera.transform.position,
            new Vector3( cameraHeight * aspectRatio, cameraHeight, 0 ) );
        return bounds;
    }
}
