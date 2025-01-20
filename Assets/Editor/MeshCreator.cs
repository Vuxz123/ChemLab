using com.ethnicthv.chemlab.client.model.util;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    public class MeshCreator
    {
        [MenuItem("Assets/Create Atom Mesh")]
        static void CreateAtom()
        {
            var filePath = EditorUtility.SaveFilePanelInProject(
                "Save Procedural Mesh",
                "AtomMesh",
                "asset",
                "",
                path: "Assets/com.ethnicthv/assets/mesh"
            );
            if (filePath == "") return;
            
            //create a sphere mesh

            var mesh = SphereModelUtil.GenerateIcoSphereMesh(1);
            
            AssetDatabase.CreateAsset(mesh, filePath);
        }
        
        [MenuItem("Assets/Create Single Bond Mesh")]
        static void CreateSingleBond()
        {
            var filePath = EditorUtility.SaveFilePanelInProject(
                "Save Procedural Mesh",
                "SingleBondMesh",
                "asset",
                "",
                path: "Assets/com.ethnicthv/assets/mesh"
            );
            if (filePath == "") return;
            
            //create a sphere mesh

            var mesh = BondModelUtil.GenerateSingleBond(0.5f, 1);
            
            AssetDatabase.CreateAsset(mesh, filePath);
        }

        [MenuItem("Assets/Create Double Bond Mesh")]
        static void CreateDoubleBond()
        {
            var filePath = EditorUtility.SaveFilePanelInProject(
                "Save Procedural Mesh",
                "DoubleBondMesh",
                "asset",
                "",
                path: "Assets/com.ethnicthv/assets/mesh"
            );
            if (filePath == "") return;

            //create a sphere mesh

            var mesh = BondModelUtil.GenerateDoubleBond(0.5f, 1);

            AssetDatabase.CreateAsset(mesh, filePath);
        }

        [MenuItem("Assets/Create Triple Bond Mesh")]
        static void CreateTripleBond()
        {
            var filePath = EditorUtility.SaveFilePanelInProject(
                "Save Procedural Mesh",
                "TripleBondMesh",
                "asset",
                "",
                path: "Assets/com.ethnicthv/assets/mesh"
            );
            if (filePath == "") return;

            //create a sphere mesh

            var mesh = BondModelUtil.GenerateTripleBond(0.5f, 1);

            AssetDatabase.CreateAsset(mesh, filePath);
        }
    }
}