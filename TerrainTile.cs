using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace wpf3d
{
    class TerrainTile
    {
        private short[] heights;
        private int xSize = 1201;
        private int zSize = 1201;
        private int spacing = 90;

        public static TerrainTile FromFile(string filename)
        {
            byte[] raw = File.ReadAllBytes(filename);
            TerrainTile tile = new TerrainTile();
            tile.heights = new short[raw.Length / 2];

            int h = 0;
            for (int i = 0; i < raw.Length; i += 2)
            {
                tile.heights[h++] = (short)((raw[i] << 8) | raw[i + 1]);
            }

            return tile;
        }

        public short GetHeight(int x, int z)
        {
            return heights[z * 1201 + x];
        }

        public MeshGeometry3D GetMesh()
        {
            var mesh = new MeshGeometry3D();

            mesh.Positions = new Point3DCollection(xSize * zSize);
            mesh.TriangleIndices = new Int32Collection((xSize - 1) * (zSize - 1) * 6);

            int h = 0;
            for (int z = 0; z < zSize; z++)
            {
                for (int x = 0; x < xSize; x++)
                {
                    short y = heights[h++];
                    if (y < 0) y = -10;

                    mesh.Positions.Add(new Point3D(x * spacing, y, z * spacing));
                }
            }

            int v = 0;
            for (int x = 0; x < xSize - 1; x++)
            {
                for (int z = 0; z < zSize - 1; z++)
                {
                    mesh.TriangleIndices.Add(v); mesh.TriangleIndices.Add(v + xSize); mesh.TriangleIndices.Add(v + 1);
                    mesh.TriangleIndices.Add(v + 1); mesh.TriangleIndices.Add(v + xSize); mesh.TriangleIndices.Add(v + xSize + 1);

                    v++;
                }
                v++;
            }

            return mesh;
        }
    }
}
