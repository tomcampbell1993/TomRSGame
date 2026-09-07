using UnityEngine;
using System.Collections.Generic;

public class ResourceController : MonoBehaviour
{
    public GameObject resourcePrefab;
    public TileController tileController;
    public List<GameObject> resources;

    public GameObject treePrefab;
    public GameObject stonePrefab;
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void CreateResource(int xPosition, int zPosition, Resource.ResourceType type)
    {

        resourcePrefab = null;

        switch (type)
        {
            case Resource.ResourceType.Tree:
                resourcePrefab = treePrefab;
                break;

            case Resource.ResourceType.Stone:
                resourcePrefab = stonePrefab;
                break;
        }
        GameObject resource = Instantiate(resourcePrefab, transform);
        Tile targetTile = tileController.GetTile(xPosition, zPosition);

        Resource resourceScript = resource.GetComponent<Resource>();
        resourceScript.Initialize(tileController, targetTile, type);
        resource.transform.position = targetTile.transform.position + Vector3.up;
        resources.Add(resource);
    }
}
