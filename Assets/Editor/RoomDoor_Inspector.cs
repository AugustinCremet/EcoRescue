using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(RoomDoor), true)]
public class RoomDoor_Inspector : Editor
{
    SerializedProperty _roomToload;
    SerializedProperty _leadsAnotherRoom;

    private void OnEnable()
    {
        _roomToload = serializedObject.FindProperty("_roomToLoad");
        _leadsAnotherRoom = serializedObject.FindProperty("_loadsAnotherRoom");
    }

    public override void OnInspectorGUI()
    {
        RoomDoor door = (RoomDoor)target;

        serializedObject.Update();

        EditorGUILayout.PropertyField(_leadsAnotherRoom);

        if (door.LoadsAnotherRoom)
        {
            EditorGUILayout.PropertyField(_roomToload);
        }

        serializedObject.ApplyModifiedProperties();
    }
}
