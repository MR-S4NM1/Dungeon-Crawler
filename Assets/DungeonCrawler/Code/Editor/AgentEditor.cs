using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace MrSanmi.DungeonCrawler
{
    [CustomEditor(typeof(Agent))]
    public class AgentEditor : Editor
    {
        // void Update() for the rendering inspector;
        public override void OnInspectorGUI()
        {
            //base.OnInspectorGUI();
            DrawDefaultInspector();
            if (GUILayout.Button("Press Me!"))
            {
                Debug.LogWarning("I have been pressed!");
                Agent agent = (Agent)target;
                agent.InitializeAgent();
            }
        }
    }
}
