using UnityEngine;
using UnityEngine.AI;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class NavMeshAreaAssigner : MonoBehaviour
{
    [Header("Area Assignment")]
    [SerializeField] private int navMeshAreaIndex = 0;
    [SerializeField] private bool applyToChildren = true;
    
    [Header("Area Reference")]
    [SerializeField] private string areaNameReference = "0=Walkable, 1=NotWalkable, 2=Jump, 3=PreferredPath, 4=AvoidArea";
    
    #if UNITY_EDITOR
    [ContextMenu("Apply NavMesh Area")]
    private void ApplyNavMeshArea()
    {
        if (applyToChildren)
        {
            MeshRenderer[] renderers = GetComponentsInChildren<MeshRenderer>();
            foreach (MeshRenderer renderer in renderers)
            {
                GameObjectUtility.SetNavMeshArea(renderer.gameObject, navMeshAreaIndex);
                GameObjectUtility.SetStaticEditorFlags(renderer.gameObject, StaticEditorFlags.NavigationStatic);
                Debug.Log($"Set {renderer.gameObject.name} to NavMesh Area {navMeshAreaIndex}");
            }
        }
        else
        {
            GameObjectUtility.SetNavMeshArea(gameObject, navMeshAreaIndex);
            GameObjectUtility.SetStaticEditorFlags(gameObject, StaticEditorFlags.NavigationStatic);
            Debug.Log($"Set {gameObject.name} to NavMesh Area {navMeshAreaIndex}");
        }
        
        Debug.Log($"<color=green>Applied NavMesh Area {navMeshAreaIndex} to objects. Remember to Bake NavMesh!</color>");
    }
    #endif
}
