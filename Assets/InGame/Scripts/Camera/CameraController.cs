using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform player;
    public float speed;
    public float offsetX = 0; // Added offsetX variable
    public float offsetY = 0; // Added offsetY variable
    public float offsetZ = 10;

    public void SetPlayer(Transform player)
    {
        this.player = player;
    }

    public void Resize()
    {
#if UNITY_EDITOR
        Vector2 size = UnityEditor.Handles.GetMainGameViewSize();
#else
        Vector2 size = new Vector2(Screen.width, Screen.height);
#endif
        int x = 5;

        float s1 = x * size.y / size.x * 0.5f + 0.5f;
        float s2 = (x + 0.5f) * size.y / size.x * 0.5f;

        float offset = size.x / size.y >= 1.9f ? s1 : s2;

        Camera.main.orthographicSize = offset;
    }

    private void Update()
    {
        if (player == null)
        {
            return;
        }

        // Updated to include offsetX and offsetY
        Vector3 t = new Vector3(player.position.x + offsetX, transform.position.y + offsetY, player.position.z - offsetZ);
        transform.position = Vector3.Lerp(transform.position, t, Time.deltaTime * speed);
    }
}