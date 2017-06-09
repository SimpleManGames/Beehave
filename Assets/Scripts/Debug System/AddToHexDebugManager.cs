using UnityEngine;
using UnityEngine.UI;

public class AddToHexDebugManager : MonoBehaviour
{    
    void Start()
    {
        HexDebugManager.Instance.hexSystemText.Add(GetComponent<Text>());
        this.gameObject.SetActive(false);
        transform.parent.eulerAngles -= transform.parent.eulerAngles;
        transform.parent.eulerAngles = new Vector3(90f, 0f, 0f);
    }
}
