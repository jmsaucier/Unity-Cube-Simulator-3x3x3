using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlattenedCube 
{
    private FaceColors[,,] _faces;
    private int _size;

    public FlattenedCube(int size)
    {
        _size = size;
        _faces = new FaceColors[6, size, size];
        for (int face = 0; face < 6; face++)
        {
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    _faces[face, i, j] = (FaceColors)face;
                }
            }
        }
    }

    public void RotateFace(FaceColors face, int slice)
    {
        FaceColors[,] tempFace = new FaceColors[_size, _size];

        for (int i = 0; i < _size; i++)
        {
            for (int j = 0; j < _size; j++)
            {
                tempFace[_size - 1 - i, _size - 1 - j] = _faces[(int)face, i, j];
            }
        }

        for (int i = 0; i < _size; i++)
        {
            for (int j = 0; j < _size; j++)
            {
                _faces[(int)face, i, j] = tempFace[i,j];
            }
        }

        List<FaceColors> neighbors = GetNeighbors(face);
        List<Vector3Int> neighborLocations = new List<Vector3Int>();
        foreach (FaceColors neighbor in neighbors)
        {
            List<FaceColors> neighborNeighbors = GetNeighbors(neighbor);
            int ourIndex = neighborNeighbors.IndexOf(face);
            switch(ourIndex)
            {
                case 0:
                    neighborLocations.Add(new Vector3Int(-1, 0, -1));
                    break;
                case 1:
                    neighborLocations.Add(new Vector3Int(_size - 1, -1, -1));
                    break;
                case 2:
                    neighborLocations.Add(new Vector3Int(-1, _size - 1, 1));
                    break;
                default:
                    neighborLocations.Add(new Vector3Int(0, -1, 1));
                    break;
            }
        }

        for (int i = 0; i < _size; i++)
        {
            FaceColors[] tempColors = new FaceColors[4];
            List<Vector2Int> faceLocations = new List<Vector2Int>();
            for (int j = 0; j < neighbors.Count; j++)
            {
                Vector3Int neighborLocation = neighborLocations[i];
                Vector2Int faceLocation = new Vector2Int();
                if(neighborLocation.x == -1)
                {
                    if(neighborLocation.z == 1)
                    {
                        faceLocation.x = i;
                    }
                    else
                    {
                        faceLocation.x = _size - 1 - i;
                    }

                    faceLocation.y = neighborLocation.y;

                }
                else
                {
                    if(neighborLocation.z == 1)
                    {
                        faceLocation.y = i;
                    }
                    else
                    {
                        faceLocation.x = _size - 1 - i;
                    }
                    faceLocation.x = neighborLocation.x;
                }
                faceLocations.Add(faceLocation);
            }
            Vector2Int lastFaceLocation = faceLocations[faceLocations.Count - 1];
            FaceColors lastFaceColor = _faces[(int)neighbors[neighbors.Count - 1], lastFaceLocation.x, lastFaceLocation.y];
            for (int j = 0; j < faceLocations.Count; j++)
            {
                Vector2Int faceLocation = faceLocations[j];
                FaceColors tempFaceColor = _faces[(int)neighbors[j], faceLocation.x, faceLocation.y];

                _faces[(int)neighbors[j], faceLocation.x, faceLocation.y] = lastFaceColor;
                lastFaceColor = tempFaceColor;
            }
        }

    }

    public FaceColors[,] GetFace(FaceColors face)
    {
        FaceColors[,] retValue = new FaceColors[_size, _size];

        for (int i = 0; i < _size; i++)
        {
            for (int j = 0; j < _size; j++)
            {
                retValue[i, j] = _faces[(int)face, i, j];
            }
        }

        return retValue;
    }

    private List<FaceColors> GetNeighbors(FaceColors face)
    {
        switch(face)
        {
            case FaceColors.White:
                return new List<FaceColors> { FaceColors.Orange, FaceColors.Blue, FaceColors.Red, FaceColors.Green };
            case FaceColors.Red:
                return new List<FaceColors> { FaceColors.White, FaceColors.Blue, FaceColors.Yellow, FaceColors.Green };
            case FaceColors.Blue:
                return new List<FaceColors> { FaceColors.White, FaceColors.Orange, FaceColors.Yellow, FaceColors.Red };
            case FaceColors.Orange:
                return new List<FaceColors> { FaceColors.White, FaceColors.Green, FaceColors.Yellow, FaceColors.Blue };
            case FaceColors.Green:
                return new List<FaceColors> { FaceColors.White, FaceColors.Red, FaceColors.Yellow, FaceColors.Yellow };
            case FaceColors.Yellow:
            default:
                return new List<FaceColors> { FaceColors.Red, FaceColors.Blue, FaceColors.Orange, FaceColors.Green };
        }
    }
}
