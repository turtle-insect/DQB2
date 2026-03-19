using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace STGDAT
{
	internal class ChunkMesh
	{
		public MeshGeometry3D Build(uint chunkID, int offsetX, int offsetZ)
		{
			MeshGeometry3D mesh = new MeshGeometry3D
			{
				Positions = new Point3DCollection(100000),
				TriangleIndices = new Int32Collection(200000),
				Normals = new Vector3DCollection(100000)
			};

			int[] dims = { 32, 96, 32 };
			int[] x = new int[3];
			int[] q = new int[3];

			for (int d = 0; d < 3; d++)
			{
				int u = (d + 1) % 3;
				int v = (d + 2) % 3;

				q[0] = q[1] = q[2] = 0;
				q[d] = 1;

				int[,] mask = new int[dims[u], dims[v]];

				for (x[d] = -1; x[d] < dims[d];)
				{
					// マスク生成
					for (x[v] = 0; x[v] < dims[v]; x[v]++)
					{
						for (x[u] = 0; x[u] < dims[u]; x[u]++)
						{
							int a = (x[d] >= 0) ? Get(chunkID, x[0], x[1], x[2]) : 0;
							int b = (x[d] < dims[d] - 1) ? Get(chunkID, x[0] + q[0], x[1] + q[1], x[2] + q[2]) : 0;

							if ((a != 0) == (b != 0))
								mask[x[u], x[v]] = 0;
							else if (a != 0)
								mask[x[u], x[v]] = a;
							else
								mask[x[u], x[v]] = -b;
						}
					}

					x[d]++;

					for (int j = 0; j < dims[v]; j++)
					{
						for (int i = 0; i < dims[u];)
						{
							int c = mask[i, j];
							if (c == 0)
							{
								i++;
								continue;
							}

							// 幅を伸ばす
							int w;
							for (w = 1; i + w < dims[u] && mask[i + w, j] == c; w++) ;

							// 高さを伸ばす
							int h;
							bool done = false;
							for (h = 1; j + h < dims[v]; h++)
							{
								for (int k = 0; k < w; k++)
								{
									if (mask[i + k, j + h] != c)
									{
										done = true;
										break;
									}
								}
								if (done) break;
							}

							x[u] = i;
							x[v] = j;

							int[] du = { 0, 0, 0 };
							int[] dv = { 0, 0, 0 };

							du[u] = w;
							dv[v] = h;

							bool flip = c < 0;

							AddQuad(mesh, x[0] + offsetX, x[1], x[2] + offsetZ, du, dv, d, flip);

							// 使用済みクリア
							for (int l = 0; l < h; l++)
								for (int k = 0; k < w; k++)
									mask[i + k, j + l] = 0;

							i += w;
						}
					}
				}
			}

			return mesh;
		}

		private int Get(uint chunkID, int x, int y, int z)
		{
			uint address = 0x183FEF0 + chunkID * 0x30000;
			address += (uint)(y * 32 * 32 + z * 32 + x) * 2;

			return (int)SaveData.Instance().ReadNumber(address, 1);
		}

		private void AddQuad(MeshGeometry3D mesh, double x, double y, double z,
							int[] du, int[] dv, int axis, bool flip)
		{
			int start = mesh.Positions.Count;

			// 頂点生成
			Point3D p0 = new Point3D(x, y, z);
			Point3D p1 = new Point3D(x + du[0], y + du[1], z + du[2]);
			Point3D p2 = new Point3D(x + du[0] + dv[0], y + du[1] + dv[1], z + du[2] + dv[2]);
			Point3D p3 = new Point3D(x + dv[0], y + dv[1], z + dv[2]);

			mesh.Positions.Add(p0);
			mesh.Positions.Add(p1);
			mesh.Positions.Add(p2);
			mesh.Positions.Add(p3);

			// 法線設定
			Vector3D normal = axis switch
			{
				0 => flip ? new Vector3D(-1, 0, 0) : new Vector3D(1, 0, 0),
				1 => flip ? new Vector3D(0, -1, 0) : new Vector3D(0, 1, 0),
				2 => flip ? new Vector3D(0, 0, -1) : new Vector3D(0, 0, 1),
				_ => new Vector3D(0, 0, 0)
			};

			for (int i = 0; i < 4; i++)
				mesh.Normals.Add(normal);

			// 三角形インデックス
			if (!flip)
			{
				mesh.TriangleIndices.Add(start);
				mesh.TriangleIndices.Add(start + 1);
				mesh.TriangleIndices.Add(start + 2);
				mesh.TriangleIndices.Add(start);
				mesh.TriangleIndices.Add(start + 2);
				mesh.TriangleIndices.Add(start + 3);
			}
			else
			{
				mesh.TriangleIndices.Add(start);
				mesh.TriangleIndices.Add(start + 2);
				mesh.TriangleIndices.Add(start + 1);
				mesh.TriangleIndices.Add(start);
				mesh.TriangleIndices.Add(start + 3);
				mesh.TriangleIndices.Add(start + 2);
			}
		}
	}
}
