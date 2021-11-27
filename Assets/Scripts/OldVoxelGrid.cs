using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OldVoxelGrid
{
    
    public Vector3Int GridDimensions { get; private set; }
    private OldVoxel[,,] _voxels;
   

    public OldVoxelGrid(Vector3Int gridDimensions, float prob1)
    {
        GridDimensions = gridDimensions;
       
        
        MakeVoxels(prob1);
    }

    //Any live cell with fewer than two live neighbours dies.
    //Any live cell with two or three live neighbours lives on to the next generation.
    //Any live cell with more than three live neighbours dies.
    //Any dead cell with exactly three live neighbours becomes a live cell.

    public void GOF()
    {
        for (int x = 0; x < GridDimensions.x; x++)
        {
            for (int z = 0; z < GridDimensions.z; z++)
            {

                OldVoxel vo = _voxels[x, 0, z];

                List<OldVoxel> neis = vo.GetNeighbourList();

                int liveNCount = 0;
                foreach (OldVoxel nei in neis)
                {
                    if (nei.Alive)
                    {
                        liveNCount++;
                    }
                }
                vo.lnc = liveNCount;
            }
        }

        for (int x = 0; x < GridDimensions.x; x++)
        {
            for (int z = 0; z < GridDimensions.z; z++)
            {
                Correction(x, z);
            }
        }
    
    }

    public void Correction(int x, int z)
    {
        OldVoxel vo = _voxels[x, 0, z];

        int liveNCount = vo.lnc;
        if (vo.Alive)
        {
            if (liveNCount < 2)
            {
                vo.Alive = false;
            }
            if (liveNCount > 3)
            {
                vo.Alive = false;
            }
        }
        else if (liveNCount == 3)
        {
            vo.Alive = true;
        }
    }
    
    private void MakeVoxels(float prob)
    {
        _voxels = new OldVoxel[GridDimensions.x, 1, GridDimensions.z];
        for (int x = 0; x < GridDimensions.x; x++)
        {
            for (int z = 0; z < GridDimensions.z; z++)
            {
                _voxels[x, 0, z] = new OldVoxel(x, 0, z, this, prob);
            }
        }
    }

    public static bool CheckInBounds(Vector3Int gridDimensions, Vector3Int index)
    {
        if (index.x < 0 || index.x >= gridDimensions.x) return false;
        if (index.y < 0 || index.y >= gridDimensions.y) return false;
        if (index.z < 0 || index.z >= gridDimensions.z) return false;

        return true;
    }

    public OldVoxel GetVoxelByIndex(Vector3Int index)
    {
        if(!CheckInBounds(GridDimensions,index)||_voxels[index.x, index.y, index.z]==null)
        {
            Debug.Log($"A Voxel at {index} doesn't exist");
            return null;
        }
        return _voxels[index.x, index.y, index.z];
    }
    
}
