using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SongDataContainer))]
public class SongDataContainerEditor : Editor
{
    SongDataContainer SongDataContainer;

    private void OnEnable()
    {
        SongDataContainer = target as SongDataContainer;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("AddItemToLines"))
        {
            SongDataContainer.songNoteDatas[0].AddItemToLines();
        }

        if(GUILayout.Button("Log"))
        {
            SongDataContainer.songNoteDatas[0].LogLine();
        }
    }


}
