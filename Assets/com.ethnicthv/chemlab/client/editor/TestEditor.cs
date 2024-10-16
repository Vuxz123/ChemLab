using UnityEditor;
using UnityEngine;

namespace com.ethnicthv.chemlab.client.editor
{
    [CustomEditor(typeof(Test))]
    public class TestEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            
        }
    }
}