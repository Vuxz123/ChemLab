using com.ethnicthv.chemlab.client.model.util;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    public class MeshCreator
    {
        [MenuItem("Assets/Create Procedural Mesh")]
        static void Create()
        {
            var filePath = EditorUtility.SaveFilePanelInProject(
                "Save Procedural Mesh",
                "Procedural Mesh",
                "asset",
                ""
            );
            if (filePath == "") return;
            
            //create a sphere mesh

            var mesh = SphereModelUtil.GenerateIcoSphereMesh(1);
            
            AssetDatabase.CreateAsset(mesh, filePath);
        }
    }
}