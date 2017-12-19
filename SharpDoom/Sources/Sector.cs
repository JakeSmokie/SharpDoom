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
        public SectorRendering(int sectorNumber, int minX, int maxX)
        {
            sectorNo = sectorNumber;
            sx1 = minX;
            sx2 = maxX;
        }

        public int sectorNo;
        public int sx1, sx2;
    };
}