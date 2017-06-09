using UnityEngine;
using UnityEngine.UI;

public class AddToHexDebugManager : MonoBehaviour
{    
    void Start()
    {
        HexDebugManager.Instance.hexSystemText.Add(GetComponent<Text>());
        this.gameObject.SetActive(false);
    }
}
