using System;
using System.Linq;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Threading.Tasks;

using System.Drawing;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;

using System.Xml;
using static SharpDoom.Tools;

namespace SharpDoom
{
    public static class World
    {
        public static List<QPoint> vertices = new List<QPoint> { };
        public static List<Sector> sectors = new List<Sector> { };
        public static float mapScale = 10.0f;

        public static void Init()
        {
            LoadMap();
            CreateEntities();
        }

        private static void CreateEntities()
        {
            
        }

        private static void LoadMap()
        {
            XmlDocument doc = LoadDocument();
            LoadVertices(doc);
            LoadSectors(doc);
            LoadPlayer(doc);
        }

        private static XmlDocument LoadDocument()
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(@"C:\Users\Jake\source\repos\SharpDoom\SharpDoom\Resources\Map1.xml");

            return doc;
        }

        private static void LoadVertices(XmlDocument doc)
        {
            XmlNode xmlVertices = doc.DocumentElement.ChildNodes[0];
            foreach (XmlNode xmlNode in xmlVertices.ChildNodes)
            {
                //Console.WriteLine("'{0}' '{1}'", vert.Attributes[0].Value, vert.Attributes[1].Value);
                vertices.Add(new QPoint(StrToFloat(xmlNode.Attributes[0].Value) * mapScale, StrToFloat(xmlNode.Attributes[1].Value) * mapScale));
            }
        }
        private static void LoadSectors(XmlDocument doc)
        {
            XmlNode xmlSectors = doc.DocumentElement.ChildNodes[1];

            foreach (XmlNode xmlNode in xmlSectors.ChildNodes)
            {
                Sector sector = new Sector()
                {
                    numPoints = Convert.ToInt32(xmlNode.Attributes[0].Value),
                    floorHeight = StrToFloat(xmlNode.Attributes[1].Value),
                    ceilHeight = StrToFloat(xmlNode.Attributes[2].Value),
                    vertices = new List<int>() { },
                    neighbors = new List<int>() { }
                };

                foreach (XmlNode neigh in xmlNode.ChildNodes[0].ChildNodes)
                {
                    sector.neighbors.Add(Convert.ToInt32(neigh.Attributes[0].Value));
                }

                //sector.neighbors.Insert(0, sector.neighbors.Last());

                foreach (XmlNode vert in xmlNode.ChildNodes[1].ChildNodes)
                {
                    sector.vertices.Add(Convert.ToInt32(vert.Attributes[0].Value));
                    //Console.WriteLine(vert.Attributes[0].Value);
                }

                sector.vertices.Insert(0, sector.vertices.Last());
                sectors.Add(sector);
            }
        }

        private static void LoadPlayer(XmlDocument doc)
        {
            XmlNode xmlPlayer = doc.DocumentElement.ChildNodes[2];
            int sectorIndex = Convert.ToInt32(doc.DocumentElement.ChildNodes[2].Attributes[3].Value);

            Game.player = new Player()
            {
                pos = new QPoint(
                        StrToFloat(doc.DocumentElement.ChildNodes[2].Attributes[0].Value),
                        StrToFloat(doc.DocumentElement.ChildNodes[2].Attributes[1].Value)
                        ),
                viewAngle = StrToFloat(doc.DocumentElement.ChildNodes[2].Attributes[2].Value),
                sector = sectorIndex,
                height = sectors[sectorIndex].floorHeight
            };
        }
    }
}
