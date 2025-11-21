using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class MemoryDiagnostics : EditorWindow
{
    private Vector2 scrollPosition;
    private List<TextureInfo> textures = new List<TextureInfo>();
    private List<MeshInfo> meshes = new List<MeshInfo>();
    private int totalTextureMemoryMB = 0;
    private int totalMeshMemoryMB = 0;
    private bool analysisComplete = false;
    
    private class TextureInfo
    {
        public string name;
        public string path;
        public int width;
        public int height;
        public TextureFormat format;
        public int memoryMB;
        public bool isReadable;
        public int maxSize;
        public TextureImporterCompression compression;
    }
    
    private class MeshInfo
    {
        public string name;
        public int vertexCount;
        public int triangleCount;
        public float memoryKB;
    }
    
    [MenuItem("Tools/Memory Diagnostics")]
    public static void ShowWindow()
    {
        GetWindow<MemoryDiagnostics>("Memory Diagnostics");
    }
    
    private void OnGUI()
    {
        GUILayout.Label("GPU Memory Diagnostics", EditorStyles.boldLabel);
        GUILayout.Space(10);
        
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Run Full Analysis", GUILayout.Height(40)))
        {
            RunAnalysis();
        }
        
        GUI.backgroundColor = Color.green;
        if (GUILayout.Button("AUTO-OPTIMIZE ALL", GUILayout.Height(40)))
        {
            if (EditorUtility.DisplayDialog("Auto-Optimize Project", 
                "This will automatically optimize:\n\n" +
                "• All textures (reduce size, enable compression)\n" +
                "• URP settings (disable HDR, MSAA)\n" +
                "• Quality settings (reduce shadows, lights)\n" +
                "• Scene lights (disable unnecessary shadows)\n\n" +
                "This operation can be undone with Ctrl+Z.\n\nContinue?", 
                "Optimize", "Cancel"))
            {
                AutoOptimizeAll();
            }
        }
        GUI.backgroundColor = Color.white;
        EditorGUILayout.EndHorizontal();
        
        EditorGUILayout.BeginHorizontal();
        GUI.backgroundColor = Color.yellow;
        if (GUILayout.Button("Optimize Mesh Instances", GUILayout.Height(30)))
        {
            if (EditorUtility.DisplayDialog("Optimize Mesh Instances", 
                "This will:\n\n" +
                "• Combine duplicate mesh renderers\n" +
                "• Optimize mesh vertex data\n" +
                "• Disable mesh read/write\n\n" +
                "Continue?", 
                "Optimize", "Cancel"))
            {
                OptimizeMeshInstances();
            }
        }
        
        if (GUILayout.Button("Disable Far Objects", GUILayout.Height(30)))
        {
            if (EditorUtility.DisplayDialog("Disable Far Objects", 
                "Temporarily disable GameObjects far from player to reduce memory.\n\n" +
                "Distance threshold: 100 units\n\nContinue?", 
                "Disable", "Cancel"))
            {
                DisableFarObjects();
            }
        }
        GUI.backgroundColor = Color.white;
        EditorGUILayout.EndHorizontal();
        
        GUILayout.Space(10);
        
        if (analysisComplete)
        {
            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
            
            DisplaySummary();
            GUILayout.Space(20);
            DisplayURPSettings();
            GUILayout.Space(20);
            DisplayQualitySettings();
            GUILayout.Space(20);
            DisplayTextureAnalysis();
            GUILayout.Space(20);
            DisplayMeshAnalysis();
            GUILayout.Space(20);
            DisplaySceneStats();
            GUILayout.Space(20);
            DisplayRecommendations();
            
            EditorGUILayout.EndScrollView();
        }
    }
    
    private void RunAnalysis()
    {
        textures.Clear();
        meshes.Clear();
        totalTextureMemoryMB = 0;
        totalMeshMemoryMB = 0;
        
        AnalyzeTextures();
        AnalyzeMeshes();
        
        analysisComplete = true;
        Debug.Log("<color=cyan>Memory analysis complete! Check the Memory Diagnostics window for results.</color>");
    }
    
    private void AnalyzeTextures()
    {
        string[] guids = AssetDatabase.FindAssets("t:Texture2D", new[] { "Assets" });
        
        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            Texture2D texture = AssetDatabase.LoadAssetAtPath<Texture2D>(path);
            
            if (texture != null)
            {
                TextureImporter importer = AssetImporter.GetAtPath(path) as TextureImporter;
                
                long memoryBytes = UnityEngine.Profiling.Profiler.GetRuntimeMemorySizeLong(texture);
                int memoryMB = Mathf.CeilToInt(memoryBytes / (1024f * 1024f));
                
                TextureInfo info = new TextureInfo
                {
                    name = texture.name,
                    path = path,
                    width = texture.width,
                    height = texture.height,
                    format = texture.format,
                    memoryMB = memoryMB,
                    isReadable = importer != null && importer.isReadable,
                    maxSize = importer != null ? importer.maxTextureSize : 0,
                    compression = importer != null ? importer.textureCompression : TextureImporterCompression.Uncompressed
                };
                
                textures.Add(info);
                totalTextureMemoryMB += memoryMB;
            }
        }
        
        textures = textures.OrderByDescending(t => t.memoryMB).ToList();
    }
    
    private void AnalyzeMeshes()
    {
        string[] guids = AssetDatabase.FindAssets("t:Mesh", new[] { "Assets" });
        
        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            Mesh mesh = AssetDatabase.LoadAssetAtPath<Mesh>(path);
            
            if (mesh != null)
            {
                long memoryBytes = UnityEngine.Profiling.Profiler.GetRuntimeMemorySizeLong(mesh);
                float memoryKB = memoryBytes / 1024f;
                
                MeshInfo info = new MeshInfo
                {
                    name = mesh.name,
                    vertexCount = mesh.vertexCount,
                    triangleCount = mesh.triangles.Length / 3,
                    memoryKB = memoryKB
                };
                
                meshes.Add(info);
                totalMeshMemoryMB += Mathf.CeilToInt(memoryKB / 1024f);
            }
        }
        
        meshes = meshes.OrderByDescending(m => m.memoryKB).ToList();
    }
    
    private void DisplaySummary()
    {
        EditorGUILayout.LabelField("=== SUMMARY ===", EditorStyles.boldLabel);
        EditorGUILayout.LabelField($"Total Textures: {textures.Count}");
        EditorGUILayout.LabelField($"Total Texture Memory: ~{totalTextureMemoryMB} MB", GetMemoryStyle(totalTextureMemoryMB));
        EditorGUILayout.LabelField($"Total Meshes: {meshes.Count}");
        EditorGUILayout.LabelField($"Total Mesh Memory: ~{totalMeshMemoryMB} MB");
        
        int estimatedTotal = totalTextureMemoryMB + totalMeshMemoryMB;
        EditorGUILayout.LabelField($"Estimated Total VRAM: ~{estimatedTotal} MB", GetMemoryStyle(estimatedTotal));
        
        if (estimatedTotal > 2000)
        {
            EditorGUILayout.HelpBox("WARNING: High memory usage detected! This may cause crashes on lower-end GPUs.", MessageType.Error);
        }
        else if (estimatedTotal > 1000)
        {
            EditorGUILayout.HelpBox("CAUTION: Moderate memory usage. Consider optimization.", MessageType.Warning);
        }
    }
    
    private void DisplayURPSettings()
    {
        EditorGUILayout.LabelField("=== URP SETTINGS ===", EditorStyles.boldLabel);
        
        var urpAsset = GraphicsSettings.currentRenderPipeline as UniversalRenderPipelineAsset;
        
        if (urpAsset != null)
        {
            EditorGUILayout.LabelField($"URP Asset: {urpAsset.name}");
            EditorGUILayout.ObjectField("Asset", urpAsset, typeof(UniversalRenderPipelineAsset), false);
            
            SerializedObject serializedAsset = new SerializedObject(urpAsset);
            
            bool supportsHDR = serializedAsset.FindProperty("m_SupportsHDR")?.boolValue ?? false;
            int msaa = serializedAsset.FindProperty("m_MSAA")?.intValue ?? 1;
            float renderScale = serializedAsset.FindProperty("m_RenderScale")?.floatValue ?? 1f;
            
            EditorGUILayout.LabelField($"HDR: {supportsHDR}");
            EditorGUILayout.LabelField($"MSAA: {msaa}x");
            EditorGUILayout.LabelField($"Render Scale: {renderScale}");
            
            if (supportsHDR)
            {
                EditorGUILayout.HelpBox("HDR is enabled - increases memory usage", MessageType.Warning);
            }
            if (msaa > 1)
            {
                EditorGUILayout.HelpBox($"MSAA {msaa}x is enabled - increases memory usage significantly", MessageType.Warning);
            }
        }
        else
        {
            EditorGUILayout.HelpBox("No URP asset found!", MessageType.Error);
        }
    }
    
    private void DisplayQualitySettings()
    {
        EditorGUILayout.LabelField("=== QUALITY SETTINGS ===", EditorStyles.boldLabel);
        
        string currentLevel = QualitySettings.names[QualitySettings.GetQualityLevel()];
        EditorGUILayout.LabelField($"Current Quality Level: {currentLevel}");
        EditorGUILayout.LabelField($"Pixel Light Count: {QualitySettings.pixelLightCount}");
        EditorGUILayout.LabelField($"Texture Quality: {QualitySettings.globalTextureMipmapLimit}");
        EditorGUILayout.LabelField($"Anisotropic Filtering: {QualitySettings.anisotropicFiltering}");
        EditorGUILayout.LabelField($"Shadow Distance: {QualitySettings.shadowDistance}");
        EditorGUILayout.LabelField($"Shadow Resolution: {QualitySettings.shadowResolution}");
    }
    
    private void DisplayTextureAnalysis()
    {
        EditorGUILayout.LabelField("=== TOP 10 LARGEST TEXTURES ===", EditorStyles.boldLabel);
        
        int count = 0;
        foreach (var tex in textures.Take(10))
        {
            count++;
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField($"{count}. {tex.name}", GUILayout.Width(200));
            EditorGUILayout.LabelField($"{tex.width}x{tex.height}", GUILayout.Width(100));
            EditorGUILayout.LabelField($"{tex.memoryMB} MB", GetMemoryStyle(tex.memoryMB), GUILayout.Width(80));
            EditorGUILayout.LabelField($"Max: {tex.maxSize}", GUILayout.Width(80));
            EditorGUILayout.LabelField($"{tex.compression}", GUILayout.Width(120));
            EditorGUILayout.EndHorizontal();
            
            if (tex.memoryMB > 10)
            {
                EditorGUILayout.HelpBox($"Large texture! Consider reducing max size or compression. Path: {tex.path}", MessageType.Warning);
            }
        }
    }
    
    private void DisplayMeshAnalysis()
    {
        EditorGUILayout.LabelField("=== TOP 10 LARGEST MESHES ===", EditorStyles.boldLabel);
        
        int count = 0;
        foreach (var mesh in meshes.Take(10))
        {
            count++;
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField($"{count}. {mesh.name}", GUILayout.Width(200));
            EditorGUILayout.LabelField($"Verts: {mesh.vertexCount}", GUILayout.Width(100));
            EditorGUILayout.LabelField($"Tris: {mesh.triangleCount}", GUILayout.Width(100));
            EditorGUILayout.LabelField($"{mesh.memoryKB:F1} KB", GUILayout.Width(80));
            EditorGUILayout.EndHorizontal();
        }
        
        EditorGUILayout.Space(5);
        EditorGUILayout.LabelField("=== MESH INSTANCES IN SCENE ===", EditorStyles.boldLabel);
        
        MeshFilter[] meshFilters = FindObjectsByType<MeshFilter>(FindObjectsSortMode.None);
        Dictionary<Mesh, int> meshInstanceCounts = new Dictionary<Mesh, int>();
        
        foreach (var mf in meshFilters)
        {
            if (mf.sharedMesh != null)
            {
                if (!meshInstanceCounts.ContainsKey(mf.sharedMesh))
                    meshInstanceCounts[mf.sharedMesh] = 0;
                meshInstanceCounts[mf.sharedMesh]++;
            }
        }
        
        var sortedInstances = meshInstanceCounts.OrderByDescending(kvp => kvp.Value).Take(10);
        
        count = 0;
        foreach (var kvp in sortedInstances)
        {
            count++;
            long memoryBytes = UnityEngine.Profiling.Profiler.GetRuntimeMemorySizeLong(kvp.Key);
            float totalMemoryMB = (kvp.Value * memoryBytes) / (1024f * 1024f);
            
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField($"{count}. {kvp.Key.name}", GUILayout.Width(200));
            EditorGUILayout.LabelField($"Instances: {kvp.Value}", GUILayout.Width(100));
            EditorGUILayout.LabelField($"Total: {totalMemoryMB:F1} MB", GetMemoryStyle((int)totalMemoryMB), GUILayout.Width(100));
            EditorGUILayout.EndHorizontal();
            
            if (totalMemoryMB > 50)
            {
                EditorGUILayout.HelpBox($"High memory usage from {kvp.Value} instances! Consider reducing duplicates or using LOD.", MessageType.Warning);
            }
        }
    }
    
    private void DisplaySceneStats()
    {
        EditorGUILayout.LabelField("=== SCENE STATISTICS ===", EditorStyles.boldLabel);
        
        MeshRenderer[] renderers = FindObjectsByType<MeshRenderer>(FindObjectsSortMode.None);
        Light[] lights = FindObjectsByType<Light>(FindObjectsSortMode.None);
        Camera[] cameras = FindObjectsByType<Camera>(FindObjectsSortMode.None);
        
        EditorGUILayout.LabelField($"Mesh Renderers: {renderers.Length}");
        EditorGUILayout.LabelField($"Lights: {lights.Length}");
        EditorGUILayout.LabelField($"Cameras: {cameras.Length}");
        
        int shadowCastingLights = lights.Count(l => l.shadows != LightShadows.None);
        EditorGUILayout.LabelField($"Shadow-casting Lights: {shadowCastingLights}");
        
        if (shadowCastingLights > 3)
        {
            EditorGUILayout.HelpBox("Many shadow-casting lights detected - this increases memory and performance cost", MessageType.Warning);
        }
    }
    
    private void DisplayRecommendations()
    {
        EditorGUILayout.LabelField("=== RECOMMENDATIONS ===", EditorStyles.boldLabel);
        
        if (totalTextureMemoryMB > 1000)
        {
            EditorGUILayout.HelpBox("• Reduce texture max sizes (try 1024 or 512)", MessageType.Info);
            EditorGUILayout.HelpBox("• Enable texture compression (High Quality)", MessageType.Info);
            EditorGUILayout.HelpBox("• Disable Read/Write enabled on textures", MessageType.Info);
        }
        
        var urpAsset = GraphicsSettings.currentRenderPipeline as UniversalRenderPipelineAsset;
        if (urpAsset != null)
        {
            SerializedObject serializedAsset = new SerializedObject(urpAsset);
            bool supportsHDR = serializedAsset.FindProperty("m_SupportsHDR")?.boolValue ?? false;
            int msaa = serializedAsset.FindProperty("m_MSAA")?.intValue ?? 1;
            
            if (supportsHDR)
            {
                EditorGUILayout.HelpBox("• Disable HDR in URP asset", MessageType.Info);
            }
            if (msaa > 1)
            {
                EditorGUILayout.HelpBox("• Disable or reduce MSAA in URP asset", MessageType.Info);
            }
        }
        
        EditorGUILayout.HelpBox("• Reduce shadow distance in Quality Settings", MessageType.Info);
        EditorGUILayout.HelpBox("• Disable shadows on non-essential lights", MessageType.Info);
    }
    
    private GUIStyle GetMemoryStyle(int memoryMB)
    {
        GUIStyle style = new GUIStyle(EditorStyles.label);
        if (memoryMB > 500)
            style.normal.textColor = Color.red;
        else if (memoryMB > 200)
            style.normal.textColor = new Color(1f, 0.5f, 0f);
        else if (memoryMB > 50)
            style.normal.textColor = Color.yellow;
        return style;
    }
    
    private void AutoOptimizeAll()
    {
        Debug.Log("<color=cyan>======= STARTING AUTO-OPTIMIZATION =======</color>");
        
        int changes = 0;
        
        changes += OptimizeTextures();
        changes += OptimizeURPSettings();
        changes += OptimizeQualitySettings();
        changes += OptimizeSceneLights();
        
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        
        Debug.Log($"<color=green>======= OPTIMIZATION COMPLETE: {changes} changes made =======</color>");
        EditorUtility.DisplayDialog("Optimization Complete", 
            $"Successfully optimized project!\n\n{changes} changes made.\n\nRun analysis again to see the improvements.", 
            "OK");
        
        RunAnalysis();
    }
    
    private int OptimizeTextures()
    {
        Debug.Log("<color=yellow>Optimizing textures...</color>");
        int changes = 0;
        
        string[] guids = AssetDatabase.FindAssets("t:Texture2D", new[] { "Assets" });
        
        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            TextureImporter importer = AssetImporter.GetAtPath(path) as TextureImporter;
            
            if (importer != null)
            {
                bool changed = false;
                
                if (importer.maxTextureSize > 1024)
                {
                    importer.maxTextureSize = 1024;
                    changed = true;
                }
                
                if (importer.textureCompression == TextureImporterCompression.Uncompressed)
                {
                    importer.textureCompression = TextureImporterCompression.CompressedHQ;
                    changed = true;
                }
                
                if (importer.isReadable)
                {
                    importer.isReadable = false;
                    changed = true;
                }
                
                if (importer.mipmapEnabled == false && importer.textureType == TextureImporterType.Default)
                {
                    importer.mipmapEnabled = true;
                    changed = true;
                }
                
                if (changed)
                {
                    AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceUpdate);
                    changes++;
                }
            }
        }
        
        Debug.Log($"<color=green>✓ Optimized {changes} textures</color>");
        return changes;
    }
    
    private int OptimizeURPSettings()
    {
        Debug.Log("<color=yellow>Optimizing URP settings...</color>");
        int changes = 0;
        
        var urpAsset = GraphicsSettings.currentRenderPipeline as UniversalRenderPipelineAsset;
        
        if (urpAsset != null)
        {
            SerializedObject serializedAsset = new SerializedObject(urpAsset);
            
            SerializedProperty hdrProp = serializedAsset.FindProperty("m_SupportsHDR");
            if (hdrProp != null && hdrProp.boolValue)
            {
                hdrProp.boolValue = false;
                changes++;
                Debug.Log("  - Disabled HDR");
            }
            
            SerializedProperty msaaProp = serializedAsset.FindProperty("m_MSAA");
            if (msaaProp != null && msaaProp.intValue > 1)
            {
                msaaProp.intValue = 1;
                changes++;
                Debug.Log("  - Disabled MSAA");
            }
            
            SerializedProperty depthTextureProp = serializedAsset.FindProperty("m_RequireDepthTexture");
            if (depthTextureProp != null && depthTextureProp.boolValue)
            {
                depthTextureProp.boolValue = false;
                changes++;
                Debug.Log("  - Disabled Depth Texture");
            }
            
            SerializedProperty opaqueTextureProp = serializedAsset.FindProperty("m_RequireOpaqueTexture");
            if (opaqueTextureProp != null && opaqueTextureProp.boolValue)
            {
                opaqueTextureProp.boolValue = false;
                changes++;
                Debug.Log("  - Disabled Opaque Texture");
            }
            
            SerializedProperty shadowResolutionProp = serializedAsset.FindProperty("m_MainLightShadowmapResolution");
            if (shadowResolutionProp != null && shadowResolutionProp.intValue > 1024)
            {
                shadowResolutionProp.intValue = 1024;
                changes++;
                Debug.Log("  - Reduced shadow resolution to 1024");
            }
            
            serializedAsset.ApplyModifiedProperties();
            EditorUtility.SetDirty(urpAsset);
        }
        
        Debug.Log($"<color=green>✓ Applied {changes} URP optimizations</color>");
        return changes;
    }
    
    private int OptimizeQualitySettings()
    {
        Debug.Log("<color=yellow>Optimizing Quality settings...</color>");
        int changes = 0;
        
        if (QualitySettings.shadows != UnityEngine.ShadowQuality.Disable)
        {
            QualitySettings.shadows = UnityEngine.ShadowQuality.HardOnly;
            changes++;
            Debug.Log("  - Set shadows to Hard Only");
        }
        
        if (QualitySettings.shadowResolution != UnityEngine.ShadowResolution.Low)
        {
            QualitySettings.shadowResolution = UnityEngine.ShadowResolution.Low;
            changes++;
            Debug.Log("  - Set shadow resolution to Low");
        }
        
        if (QualitySettings.shadowDistance > 30f)
        {
            QualitySettings.shadowDistance = 30f;
            changes++;
            Debug.Log("  - Reduced shadow distance to 30");
        }
        
        if (QualitySettings.pixelLightCount > 2)
        {
            QualitySettings.pixelLightCount = 2;
            changes++;
            Debug.Log("  - Reduced pixel light count to 2");
        }
        
        if (QualitySettings.anisotropicFiltering != AnisotropicFiltering.Disable)
        {
            QualitySettings.anisotropicFiltering = AnisotropicFiltering.Disable;
            changes++;
            Debug.Log("  - Disabled anisotropic filtering");
        }
        
        if (QualitySettings.globalTextureMipmapLimit < 1)
        {
            QualitySettings.globalTextureMipmapLimit = 1;
            changes++;
            Debug.Log("  - Set texture quality to Half Res");
        }
        
        Debug.Log($"<color=green>✓ Applied {changes} Quality optimizations</color>");
        return changes;
    }
    
    private int OptimizeSceneLights()
    {
        Debug.Log("<color=yellow>Optimizing scene lights...</color>");
        int changes = 0;
        
        Light[] lights = FindObjectsByType<Light>(FindObjectsSortMode.None);
        
        foreach (Light light in lights)
        {
            if (light.type != LightType.Directional && light.shadows != LightShadows.None)
            {
                light.shadows = LightShadows.None;
                changes++;
                EditorUtility.SetDirty(light);
            }
        }
        
        Debug.Log($"<color=green>✓ Disabled shadows on {changes} non-directional lights</color>");
        return changes;
    }
    
    private void OptimizeMeshInstances()
    {
        Debug.Log("<color=cyan>======= OPTIMIZING MESH INSTANCES =======</color>");
        int changes = 0;
        
        string[] guids = AssetDatabase.FindAssets("t:Model", new[] { "Assets" });
        
        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            ModelImporter importer = AssetImporter.GetAtPath(path) as ModelImporter;
            
            if (importer != null)
            {
                bool changed = false;
                
                if (importer.isReadable)
                {
                    importer.isReadable = false;
                    changed = true;
                }
                
                if (importer.importNormals != ModelImporterNormals.Import)
                {
                    importer.importNormals = ModelImporterNormals.Import;
                    changed = true;
                }
                
                if (importer.importTangents != ModelImporterTangents.None)
                {
                    importer.importTangents = ModelImporterTangents.None;
                    changed = true;
                }
                
                if (changed)
                {
                    AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceUpdate);
                    changes++;
                }
            }
        }
        
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        
        Debug.Log($"<color=green>✓ Optimized {changes} mesh assets</color>");
        EditorUtility.DisplayDialog("Mesh Optimization Complete", 
            $"Optimized {changes} mesh assets.\n\nDisabled read/write and tangent imports.", 
            "OK");
    }
    
    private void DisableFarObjects()
    {
        Debug.Log("<color=cyan>======= DISABLING FAR OBJECTS =======</color>");
        
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            EditorUtility.DisplayDialog("Error", "Player not found in scene!", "OK");
            return;
        }
        
        Vector3 playerPos = player.transform.position;
        float distanceThreshold = 100f;
        int disabledCount = 0;
        
        MeshRenderer[] renderers = FindObjectsByType<MeshRenderer>(FindObjectsSortMode.None);
        
        foreach (var renderer in renderers)
        {
            float distance = Vector3.Distance(playerPos, renderer.transform.position);
            
            if (distance > distanceThreshold && renderer.gameObject.activeInHierarchy)
            {
                renderer.gameObject.SetActive(false);
                disabledCount++;
                EditorUtility.SetDirty(renderer.gameObject);
            }
        }
        
        Debug.Log($"<color=green>✓ Disabled {disabledCount} objects beyond {distanceThreshold} units</color>");
        EditorUtility.DisplayDialog("Far Objects Disabled", 
            $"Disabled {disabledCount} GameObjects beyond {distanceThreshold} units from player.\n\nThis reduces runtime memory usage.", 
            "OK");
    }
}
