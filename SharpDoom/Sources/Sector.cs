using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace SharpDoom
{
    public struct Sector
    {
        public float floorHeight;
        public float ceilHeight;

        public int numPoints;
        public List<int> neighbors;
        public List<int> vertices;
    }

    public struct SectorRendering
    {
        public SectorRendering(int sectorNumber, float minX, float maxX)
        {
            sectorNo = sectorNumber;
            sx1 = minX;
            sx2 = maxX;
        }

        public int sectorNo;
        public float sx1, sx2;
    };
}