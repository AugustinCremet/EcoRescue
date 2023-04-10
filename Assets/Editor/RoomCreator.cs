using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;

public class RoomCreator : EditorWindow
{
    private string _roomSceneName;
    private bool _shouldReset;
    private bool _hasExit;
    private EntranceDoor[] _entranceDoor;
    private ExitDoor[] _exitDoor;

    [MenuItem("Window/CustomWindows/RoomWindowEditor")]
    static void Init()
    {
        RoomCreator window = (RoomCreator)EditorWindow.GetWindow(typeof(RoomCreator));
        window.Show();
    }

    private void OnGUI()
    {
        _roomSceneName = SceneManager.GetActiveScene().name;

        EditorGUILayout.LabelField("Room Settings", EditorStyles.boldLabel);

        EditorGUILayout.Space(5);

        EditorGUILayout.LabelField("Room Scene Name : ", _roomSceneName);

        EditorGUILayout.Space(2.5f);

        _shouldReset = EditorGUILayout.Toggle("Should Reset",_shouldReset);

        EditorGUILayout.Space(2.5f);

        _hasExit = EditorGUILayout.Toggle("Room Has An Exit Door", _hasExit);

        EditorGUILayout.Space(2.5f);

        _entranceDoor = FindObjectsOfType<EntranceDoor>();

        if (_entranceDoor.Length != 1)
        {
            EditorGUILayout.HelpBox("There should be 1 entrance door", MessageType.Error);
        }

        if (_hasExit)
        {
            _exitDoor = FindObjectsOfType<ExitDoor>();

            if (_exitDoor.Length != 1)
            {
                EditorGUILayout.HelpBox("There should be 1 exit door", MessageType.Error);
            }
        }

        if (_entranceDoor.Length != 1) return;
        if (_hasExit && _exitDoor.Length != 1) return;

        if (GUILayout.Button("Generate Room Data"))
        {
            Room asset = CreateInstance<Room>();

            List<DoorTransform> positions = new List<DoorTransform>();
            DoorTransform pos;
            pos.Position = _entranceDoor[0].gameObject.transform.position;
            pos.Rotation = _entranceDoor[0].gameObject.transform.rotation;

            positions.Add(pos);

            if (_hasExit)
            {
                pos.Position = _exitDoor[0].gameObject.transform.position;
                pos.Rotation = _exitDoor[0].gameObject.transform.rotation;

                positions.Add(pos);
            }

            asset.Name = _roomSceneName;
            asset.name = _roomSceneName;
            asset.DoorTransform = positions;

            var uniquePath = "Assets/ScriptableObjects/Dungeons/LumberJack/Rooms/" + _roomSceneName + ".asset";

            if (AssetDatabase.FindAssets(asset.name).Length > 0)
            {
                AssetDatabase.DeleteAsset(uniquePath);
                AssetDatabase.CreateAsset(asset, uniquePath);
            }

            Selection.activeObject = asset;
        }       
    }
}
