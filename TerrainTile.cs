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
        private int xSize = 1200;
        private int zSize = 1200;
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
            return heights[(z * (xSize + 1)) + x];
        }

        public MeshGeometry3D GetMesh(int fidelity)
        {
            if (fidelity < 1 || fidelity > 6)
            {
                throw new InvalidDataException("Fidelity must be between 1 and 6 inclusive.");
            }

            var mesh = new MeshGeometry3D();

            mesh.Positions = new Point3DCollection((xSize / fidelity + 1) * (zSize / fidelity + 1));
            mesh.TriangleIndices = new Int32Collection(xSize * zSize / fidelity * 6);

            int z = 0;
            int x = 0;
            for (z = 0; z <= zSize; z += fidelity)
            {
                for (x = 0; x <= xSize; x += fidelity)
                {
                    short y = GetHeight(x, z);
                    if (y < 0) y = -10;

                    mesh.Positions.Add(new Point3D(x * spacing, y, z * spacing));
                    Console.WriteLine($"{x}, {z}");
                }
            }

            int v = 0;
            for (x = 0; x < xSize / fidelity; x++)
            {
                for (z = 0; z < zSize / fidelity; z++)
                {
                    mesh.TriangleIndices.Add(v); mesh.TriangleIndices.Add(v + (xSize / fidelity) + 1); mesh.TriangleIndices.Add(v + 1);
                    mesh.TriangleIndices.Add(v + 1); mesh.TriangleIndices.Add(v + (xSize / fidelity) + 1); mesh.TriangleIndices.Add(v + (xSize / fidelity) + 2);

                    v++;
                }
                v++;
            }

            return mesh;
        }
    }
}
