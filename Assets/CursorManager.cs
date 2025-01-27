using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorManager : MonoBehaviour
{
    public Texture2D cursorTexture; // Assign your cursor texture in the Inspector
    public Vector2 hotSpot = Vector2.zero; // The point in the texture that will be the cursor's "click" point

    void Start() {
        // Set the cursor
        Cursor.SetCursor(cursorTexture, hotSpot, CursorMode.Auto);
    }

    void OnDisable() {
        // Reset the cursor when the object is disabled
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
    }
}
