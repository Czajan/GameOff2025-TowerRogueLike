using UnityEngine;
using UnityEditor;
using System.IO;

public class CreateSolidSpriteTexture : MonoBehaviour
{
    [MenuItem("Tools/Create Solid White Sprite")]
    private static void CreateSprite()
    {
        int size = 32;
        Texture2D texture = new Texture2D(size, size, TextureFormat.RGBA32, false);
        
        Color[] pixels = new Color[size * size];
        for (int i = 0; i < pixels.Length; i++)
        {
            pixels[i] = Color.white;
        }
        
        texture.SetPixels(pixels);
        texture.Apply();
        
        byte[] bytes = texture.EncodeToPNG();
        
        string directory = "Assets/Sprites";
        if (!AssetDatabase.IsValidFolder(directory))
        {
            AssetDatabase.CreateFolder("Assets", "Sprites");
        }
        
        string path = directory + "/SolidWhiteSprite.png";
        File.WriteAllBytes(path, bytes);
        
        AssetDatabase.Refresh();
        
        TextureImporter importer = AssetImporter.GetAtPath(path) as TextureImporter;
        if (importer != null)
        {
            importer.textureType = TextureImporterType.Sprite;
            importer.spriteImportMode = SpriteImportMode.Single;
            importer.spriteBorder = Vector4.zero;
            importer.spritePixelsPerUnit = 100;
            importer.filterMode = FilterMode.Point;
            importer.mipmapEnabled = false;
            importer.SaveAndReimport();
        }
        
        Debug.Log($"Solid white sprite created at: {path}");
        
        Selection.activeObject = AssetDatabase.LoadAssetAtPath<Sprite>(path);
        EditorGUIUtility.PingObject(Selection.activeObject);
    }
}
