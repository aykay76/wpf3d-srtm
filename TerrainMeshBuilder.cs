using System.Windows.Media.Media3D;
using System.Windows.Media;
using System.IO;
using System;

namespace wpf3d
{
    public class TerrainMeshBuilder
    {
        private MeshGeometry3D mesh = null;

        public MeshGeometry3D Geometry
        {
            get
            {
                if (mesh == null)
                {
                    //int xSize = 3601;
                    //int zSize = 3601;
                    //int spacing = 30;
                    int xSize = 1201;
                    int zSize = 1201;
                    int spacing = 90;

                    Console.WriteLine("Building geometry");

                    mesh = new MeshGeometry3D();

                    mesh.Positions = new Point3DCollection(xSize * zSize);
                    mesh.TriangleIndices = new Int32Collection((xSize - 1) * (zSize - 1) * 6);

                    // for now, load one file and get the data, build a mesh and display it - MVP, refine later :)
                    // byte[] raw = File.ReadAllBytes(@"C:\Users\alank\git\aykay76\wpf3d\N29E095.hgt");
                    byte[] raw = File.ReadAllBytes(@"C:\Users\alank\git\aykay76\wpf3d\S02W079.hgt");
                    int b = 0;
                    short maxy = 0;
                    for (int z = 0; z < zSize; z++)
                    {
                        for (int x = 0; x < xSize; x++)
                        {
                            short y = (short)((raw[b] << 8) | raw[b + 1]);
                            if (y > maxy) maxy = y;
                            if (y < 0) y = -10;

                            b += 2;

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
                else
                {
                    return mesh;
                }
            }
        }
    }
}