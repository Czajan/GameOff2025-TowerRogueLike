using UnityEngine;

public class MaterialHelper : MonoBehaviour
{
    public static Material CreateSimpleMaterial(Color color)
    {
        Material material = new Material(Shader.Find("Universal Render Pipeline/Lit"));
        material.color = color;
        return material;
    }
    
    public static void ApplyColorToRenderer(Renderer renderer, Color color)
    {
        if (renderer != null)
        {
            Material material = CreateSimpleMaterial(color);
            renderer.material = material;
        }
    }
    
    [ContextMenu("Apply Red Material")]
    public void ApplyRed()
    {
        Renderer renderer = GetComponentInChildren<Renderer>();
        ApplyColorToRenderer(renderer, Color.red);
    }
    
    [ContextMenu("Apply Blue Material")]
    public void ApplyBlue()
    {
        Renderer renderer = GetComponentInChildren<Renderer>();
        ApplyColorToRenderer(renderer, Color.blue);
    }
    
    [ContextMenu("Apply Green Material")]
    public void ApplyGreen()
    {
        Renderer renderer = GetComponentInChildren<Renderer>();
        ApplyColorToRenderer(renderer, Color.green);
    }
}
