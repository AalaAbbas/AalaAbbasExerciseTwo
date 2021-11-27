using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OldVoxelGridManager : MonoBehaviour
{
    [SerializeField]
    private Vector3Int _gridDimensions = new Vector3Int(10, 1, 10);
    private OldVoxelGrid _grid;
    [SerializeField]
    public float prob = 0.3f;

    void Start()
    {
        _grid = new OldVoxelGrid(_gridDimensions, prob);
    }

    void Update()
    {
        PerformRaycast();
      
    }

    public void Auto()
    {
        StartCoroutine(Auto1());
    }

    public IEnumerator Auto1()
    {
		while (true)
		{
            Next();
            yield return new WaitForSeconds(0.2f);
        }
    }


    public void Next()
    {
        _grid.GOF();
    }

    public void PerformRaycast()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform.tag == "Voxel")
                {
                    GameObject hitObject = hit.transform.gameObject;
                    var voxel = hitObject.GetComponent<OldVoxelTrigger>().AttachedVoxel;

                    voxel.ToggleNeighbours();
                }
            }
        }
    }
    
}

   

