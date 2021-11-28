using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OldVoxel
{
        //index can not be set outside of the scope of this class
    public Vector3Int Index { get; private set; }
    public int lnc;
    private GameObject _goVoxelTrigger;
    private OldVoxelGrid _grid;
    private bool _alive;
    public int Counter;

    public bool Alive
    {
        get
        {
            return _alive;
        }
        set
        {
            if(_goVoxelTrigger!=null)
            {
                MeshRenderer renderer = _goVoxelTrigger.GetComponent<MeshRenderer>();
                renderer.enabled = value;
            }
            _alive = value;
        }
    }

    public static List<Vector3Int> Directions = new List<Vector3Int>
    {
        new Vector3Int(-1,0,0),// min x
        new Vector3Int(1,0,0),// plus 
        new Vector3Int(0,0,-1),// min z
        new Vector3Int(0,0,1),// plus z
        new Vector3Int(-1,0,1),// min x
        new Vector3Int(1,0,1),// plus 
        new Vector3Int(1,0,-1),// min z
        new Vector3Int(-1,0,-1)// plus z
    };


    public OldVoxel(int x, int y, int z, OldVoxelGrid grid, float prob)
    {
        Index = new Vector3Int(x, y, z);
        _grid = grid;
        
        CreateGameobject();
        Alive = Random.value < prob ? true : false;

    }

    public OldVoxel(int x, int y, int z, OldVoxelGrid grid, int counter)
    {
        Counter = counter;
        Index = new Vector3Int( x, 0, z) ;
        _grid = grid;
        CreateGameobject();
        Alive = true;
    }

    public void CreateGameobject()
    {
        Vector3Int Index1 = Index + new Vector3Int(0, Counter, 0);
        _goVoxelTrigger = GameObject.CreatePrimitive(PrimitiveType.Cube);
        _goVoxelTrigger.name = $"Voxel {Index1}";
        _goVoxelTrigger.tag = "Voxel";
        _goVoxelTrigger.transform.position = Index1;
        _goVoxelTrigger.transform.localScale = Vector3.one * 0.95f;
        OldVoxelTrigger trigger = _goVoxelTrigger.AddComponent<OldVoxelTrigger>();
        trigger.AttachedVoxel = this;
    }

    public List<OldVoxel> GetNeighbourList()
    {
        List<OldVoxel> neighbours = new List<OldVoxel>();
        foreach (var direction in Directions)
        {
            Vector3Int neighbourIndex = Index + direction;
            if (OldVoxelGrid.CheckInBounds(_grid.GridDimensions, neighbourIndex))
            {
                neighbours.Add(_grid.GetVoxelByIndex(neighbourIndex));
            }
        }

        return neighbours;
    }

    public void ToggleNeighbours()
    {
        List<OldVoxel> neighbours = GetNeighbourList();

        foreach (var neighbour in neighbours)
        {
            neighbour.Alive = !neighbour.Alive;
        }
    }

 
}
