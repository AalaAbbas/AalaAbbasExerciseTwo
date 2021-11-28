using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OldVoxelGrid
{
    
    public Vector3Int GridDimensions { get; private set; }
    private OldVoxel[,,] _voxels;
    public float prob1;
    public int Counter;
    public OldVoxelGrid PreGrid;
   

    public OldVoxelGrid(Vector3Int gridDimensions, float prob)
    {
        GridDimensions = gridDimensions;
        prob1 = prob;
        Counter = 1;
        MakeVoxels();
    }

    public OldVoxelGrid(Vector3Int gridDimensions, OldVoxelGrid preGrid, int counter)
    {
        GridDimensions = gridDimensions;
        Counter = counter;
        PreGrid = preGrid;
        MakeVoxels();
    }

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
    
    private void MakeVoxels()
    {
        _voxels = new OldVoxel[GridDimensions.x, 1, GridDimensions.z];
        for (int x = 0; x < GridDimensions.x; x++)
        {
            for (int z = 0; z < GridDimensions.z; z++)
            {
                if (Counter == 1)
                {
                    _voxels[x, 0, z] = new OldVoxel(x, 0, z, this, prob1);
                }
                else
                {
                    _voxels[x, 0, z] = new OldVoxel(x, 0, z, this, Counter);
                    Vector3Int index = new Vector3Int(x, 0, z);
                    bool fate = PreGrid.GetVoxelByIndex(index).Alive;
                    _voxels[x, 0, z].Alive = fate;
                }
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
