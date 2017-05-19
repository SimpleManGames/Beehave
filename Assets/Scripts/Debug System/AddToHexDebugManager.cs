using UnityEngine;
using UnityEngine.UI;

public class AddToHexDebugManager : MonoBehaviour
{    
    void Awake()
    {
        HexDebugManager.Instance.hexSystemText.Add(GetComponent<Text>());
    }
}
