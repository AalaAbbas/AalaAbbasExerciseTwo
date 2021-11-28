using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OldVoxelGridManager : MonoBehaviour
{
    [SerializeField]
    private Vector3Int _gridDimensions = new Vector3Int(10, 1, 10);
    
    [SerializeField]
    public float prob = 0.3f;

    private OldVoxelGrid startGrid;
    private OldVoxelGrid prevGrid;
    public int counter = 1;

    void Start()
    {
        startGrid = new OldVoxelGrid(_gridDimensions, prob);
        prevGrid = startGrid;
        counter++; 
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
        OldVoxelGrid curGrid  = new OldVoxelGrid(_gridDimensions, prevGrid, counter);
        curGrid.GOF();

        prevGrid = curGrid;
        counter++;
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

   

