﻿using System;
using UnityEngine;

namespace PhenomTools
{
    [Serializable]
    public struct BoundsInt_Serializable
    {
        public Vector3Int_Serializable position;
        public Vector3Int_Serializable size;

        public BoundsInt_Serializable(int xMin, int yMin, int zMin, int sizeX, int sizeY, int sizeZ)
        {
            position = new Vector3Int_Serializable(xMin, yMin, zMin);
            size = new Vector3Int_Serializable(sizeX, sizeY, sizeZ);
        }

        public BoundsInt_Serializable(Vector3Int_Serializable position, Vector3Int_Serializable size)
        {
            this.position = position;
            this.size = size;
        }

        public BoundsInt_Serializable(BoundsInt bounds)
        {
            position = new Vector3Int_Serializable(bounds.position);
            size = new Vector3Int_Serializable(bounds.size);
        }

        public BoundsInt ToBoundsInt_Unserializable()
        {
            return new BoundsInt(position.ToUnserializable(), size.ToUnserializable());
        }
    }
}