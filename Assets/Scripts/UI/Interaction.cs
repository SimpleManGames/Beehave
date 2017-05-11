using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interaction : Singleton<Interaction>
{
    Camera cam;
    GameObject selectedObj = null;
    public static GameObject objToPlace = null;
    public static List<Material> defaultMat = new List<Material>();
    int hexIndex;
    //RaycastHit[] hits;
    RaycastHit hit;
    Ray ray;
    public GameObject buildingScreen;
    public GameObject bumblrScreen;
    public LayerMask terrainLayer;
    public Material placingMat;

    public bool selecting;

    static Interaction instance;
    public List<int> CreepToPlaceList = new List<int>();

    public bool mouseDown = false;
    // Use this for initialization
    void Start()
    {
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        CheckHitInfo();
        if (objToPlace != null)
        {
            Material[] mats = objToPlace.GetComponent<Renderer>().materials;
            for (int i = 0; i < mats.Length; i++)
            {
                mats[i] = placingMat;
            }
            objToPlace.GetComponent<Renderer>().materials = mats;
            //hits = Physics.RaycastAll(ray, Mathf.Infinity, LayerMask.NameToLayer("Highlighted"));
            if (Input.GetKeyDown(KeyCode.R))
            {
                RotateObject(objToPlace);
            }
            //foreach (RaycastHit hit in hits)
            //{
            // Errors due to not all hit objs having a parent of a parent
            if (hit.collider.gameObject != objToPlace)
            {
                if (hit.collider.transform.GetComponentInParent(typeof(HexObject)))
                {
                    hexIndex = hit.collider.transform.GetComponentInParent<HexObject>().Index;
                    objToPlace.transform.localPosition = new Vector3(hit.collider.gameObject.transform.position.x, hit.point.y, hit.collider.gameObject.transform.position.z);
                }
            }
        }
        // }
        if (selecting)
        {
            SelectCreepToPlace();
        }
        if (!selecting && Input.GetMouseButtonDown(0))
        {
            if (objToPlace == null)
            {
                //foreach(RaycastHit hit in hits)
                //{
                if (hit.collider != null)
                    if (/*hit.collider.gameObject.GetComponent(typeof(IHighlightable)) && */!hit.collider.gameObject.transform.GetComponentInParent<Canvas>())
                    {
                        selectedObj = hit.collider.transform.root.gameObject;
                        OpenPopUp();
                    }
                //if (hit.collider.gameObject.GetComponent(typeof(BuildingHolder)))
                //{
                //    if (objToPlace == null)
                //    {
                //        GameObject obj = hit.collider.gameObject.GetComponent<BuildingHolder>().Building;
                //        objToPlace = Instantiate(obj);
                //    }
                //}
            }
            //}
            else
            {
                if (Grid.FindHexObject(hexIndex).IsCreep && !Grid.FindHexObject(hexIndex).HasBuilding)
                {
                    BuildingObject objToPlaceInfo = objToPlace.GetComponent<BuildingObject>();

                    // Errors due to material not being on base obj
                    Material[] mats = objToPlace.GetComponent<Renderer>().materials;
                    for (int i = 0; i < mats.Length; i++)
                    {
                        mats[i] = defaultMat[i];
                    }
                    objToPlace.GetComponent<Renderer>().materials = mats;
                    //objToPlace.GetComponent<Renderer>().materials = defaultMat
                    objToPlaceInfo.SetBuildingTile(hexIndex);
                    Grid.EditHeatMapData(hexIndex, objToPlaceInfo.layerType);
                    objToPlace = null;
                    defaultMat.Clear();
                }
                else
                    Destroy(objToPlace);
                if (objToPlace != null)
                    objToPlace = null;
                defaultMat.Clear();
            }
        }
    }
    void CheckHitInfo()
    {
        if(cam != null)
            ray = cam.ScreenPointToRay(Input.mousePosition);

        Physics.Raycast(ray, out hit);
    }
    void OpenPopUp()
    {
        Vector3 targetPos = Input.mousePosition;
        //targetPos = cam.WorldToViewportPoint(targetPos);
        if (selectedObj.gameObject.name.StartsWith("Bee"))
        {
            bumblrScreen.SetActive(true);
            bumblrScreen.GetComponent<RectTransform>().localPosition = new Vector2(targetPos.x - Screen.width / 1.5f, targetPos.y - Screen.height / 2.75f);
        }
        //if (selectedObj.gameObject.name.StartsWith("Building"))
        //{
        //    buildingScreen.SetActive(true);
        //    buildingScreen.GetComponent<RectTransform>().localPosition = new Vector2(targetPos.x - Screen.width / 1.7f, targetPos.y - Screen.height / 4.3f);
        //}
    }
    void SelectCreepToPlace()
    {
        if (Input.GetMouseButton(0))
        {
            mouseDown = true;
            if (hit.collider.GetComponentInParent<HexObject>())
            {
                if (!CreepToPlaceList.Contains(hit.collider.GetComponentInParent<HexObject>().Index))
                {
                    CreepToPlaceList.Add(hit.collider.GetComponentInParent<HexObject>().Index);
                    hit.transform.GetComponentInParent<HexObject>().IsCreep = true;
                    hit.collider.GetComponent<MeshRenderer>().enabled = true;
                }
            }
        }

        if (Input.GetMouseButtonUp(0) && mouseDown)
        {
            selecting = false;
            mouseDown = false;
            return;
        }
    }
    void RotateObject(GameObject obj)
    {
        Quaternion lR = obj.transform.localRotation;
        obj.transform.Rotate(new Vector3(lR.x, lR.y += 90f, lR.z), Space.Self);
    }

    public void ToggleSelect()
    {
        selecting = true;
    }
}
