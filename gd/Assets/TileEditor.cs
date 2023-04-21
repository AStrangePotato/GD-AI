using UnityEngine;
using UnityEditor;

public class TilePlacer : EditorWindow {
    private GameObject tilePrefab; // The prefab you want to place
    private bool isPlacing; // Whether the user is currently placing the tile
    private Vector3 mousePosition; // The current mouse position in the scene view

    [MenuItem("Tools/Tile Placer")]
    public static void ShowWindow() {
        EditorWindow.GetWindow(typeof(TilePlacer));
    }

    void OnGUI() {
        tilePrefab = EditorGUILayout.ObjectField("Tile Prefab", tilePrefab, typeof(GameObject), false) as GameObject;

        if (tilePrefab == null) {
            EditorGUILayout.HelpBox("Please assign a prefab to place.", MessageType.Warning);
            return;
        }

        if (GUILayout.Button("Place Tile")) {
            isPlacing = true;
        }
    }

    void Update() {
        if (isPlacing) {
            if (Event.current.type == EventType.MouseDown && Event.current.button == 0) {
                // User has clicked left mouse button to place the tile
                GameObject tile = PrefabUtility.InstantiatePrefab(tilePrefab) as GameObject;
                tile.transform.position = mousePosition;
                isPlacing = false;
            } else {
                // User is still dragging the mouse to position the tile
                mousePosition = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition).origin;
                mousePosition.z = 0; // Make sure the tile is placed on the 2D plane
                SceneView.RepaintAll();
            }
        }
    }
}
