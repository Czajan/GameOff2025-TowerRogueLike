using UnityEngine;

public class CompilationTest : MonoBehaviour
{
    private void Start()
    {
        Debug.Log("=== COMPILATION TEST ===");
        
        Debug.Log("Testing PlayerStats...");
        if (PlayerStats.Instance != null)
        {
            Debug.Log($"PlayerStats.GetMoveSpeedLevel() = {PlayerStats.Instance.GetMoveSpeedLevel()}");
        }
        
        Debug.Log("Testing GameProgressionManager...");
        if (GameProgressionManager.Instance != null)
        {
            Debug.Log($"GameProgressionManager.Currency = {GameProgressionManager.Instance.Currency}");
        }
        
        Debug.Log("Testing ShopNPC type...");
        ShopNPC testNPC = FindFirstObjectByType<ShopNPC>();
        if (testNPC != null)
        {
            Debug.Log($"ShopNPC.GetNPCName() = {testNPC.GetNPCName()}");
            Debug.Log($"ShopNPC.GetNPCType() = {testNPC.GetNPCType()}");
        }
        
        Debug.Log("Testing UpgradeType enum...");
        UpgradeType testType = UpgradeType.MoveSpeed;
        Debug.Log($"UpgradeType.MoveSpeed = {testType}");
        
        Debug.Log("Testing ShopNPCType enum...");
        ShopNPCType testShopType = ShopNPCType.WeaponVendor;
        Debug.Log($"ShopNPCType.WeaponVendor = {testShopType}");
        
        Debug.Log("=== ALL TESTS PASSED ===");
    }
}
