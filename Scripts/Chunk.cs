using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SimplexNoise;

//using UnityEditor;

[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshCollider))]
[RequireComponent(typeof(MeshFilter))]
public class Chunk : MonoBehaviour {

    public static List<Chunk> chunks = new List<Chunk>();
    public static float width {
        get { return World.currentWorld.chunkWidth; }
    }
    public static float height {
        get { return World.currentWorld.chunkHeight; }
    }
    public byte[,,] map;
    protected MeshRenderer meshRenderer;
    protected MeshCollider meshCollider;
    protected MeshFilter meshFilter;

    private List<float> pos = new List<float>();

    // Use this for initialization
    void Start() {
        QualitySettings.vSyncCount = 0;
        chunks.Add(this);

        meshRenderer = GetComponent<MeshRenderer>();
        meshCollider = GetComponent<MeshCollider>();
        meshFilter = GetComponent<MeshFilter>();

        StartCoroutine(CalculateMapFromScratch());
        StartCoroutine(CreateVisualMesh());

    }

    // Update is called once per frame
    void Update() {
        
    }

    public virtual byte GetTheoreticalByte(Vector3 pos) {
        Random.seed = World.currentWorld.seed;

        Vector3 grain0Offset = new Vector3(Random.value * 10000, Random.value * 10000, Random.value * 10000);
        Vector3 grain1Offset = new Vector3(Random.value * 10000, Random.value * 10000, Random.value * 10000);
        Vector3 grain2Offset = new Vector3(Random.value * 10000, Random.value * 10000, Random.value * 10000);

        return GetTheoreticalByte(pos, grain0Offset, grain1Offset, grain2Offset);
    }

    public virtual byte GetTheoreticalByte(Vector3 pos, Vector3 offset0, Vector3 offset1, Vector3 offset2) {

        if(pos.y == 0){
            return (byte)Block.ID.BEDROCK;
        }
        

        float heightBase = 40;
        float maxHeight = height - 20;
        float heightSwing = maxHeight - heightBase;

        byte brick = (byte)Block.ID.AIR + 1;

        float blobValue = CalculateNoiseValue(pos, offset1, 0.09f);
        float clusterValue = CalculateNoiseValue(pos, offset2, 0.02f);
        float mountainValue = CalculateNoiseValue(pos, offset0, 0.04f);
        /*
        if ((mountainValue == 0) && (blobValue < 0.2f))
            brick = 2;
        else if (clusterValue > 0.9f)
            brick = 3;
        else if (clusterValue > 0.8f)
            brick = 1;*/

        


        mountainValue *= mountainValue * 10;
        mountainValue *= heightSwing;
        mountainValue += heightBase;
        mountainValue += (blobValue * 10) - 5;
        
        if (mountainValue >= pos.y){
            int v = (int)pos.y;
            if(v == 40){
                return (byte)Block.ID.GRASS;
            }else if(v < 40 && v > 35){
                return (byte)Block.ID.DIRT;
            }else if(v < 36){
                return (byte)Block.ID.STONE;
            }else{
                return (byte)Block.ID.AIR;
            }
        }
            
        return 0; 
    }

    public virtual IEnumerator CalculateMapFromScratch() {
        map = new byte[World.currentWorld.chunkWidth, World.currentWorld.chunkHeight, World.currentWorld.chunkWidth];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                for (int z = 0; z < width; z++)
                {
                    
                    //map[x, y, z] = GetTheoreticalByte(new Vector3(x, y,z)+transform.position);
					if(y > 25){
                        map[x, y, z] = (byte)Block.ID.AIR;
                    }else if(y == 25){
                        map[x, y, z] = 1;
                    }else if(y < 25 && y > 0){
                        map[x, y, z] = (byte)Block.ID.STONE;
                    }else if(y == 0){
                        map[x, y, z] = (byte)Block.ID.BEDROCK;
                    }else{
                        map[x, y, z] = (byte)Block.ID.DIRT;
                    }
                }
            }
        }

        for(int x = 0; x < width; x++){
            for(int z = 0; z < width; z++){
                if(Random.Range(0, 300) == 1){
                    TreeGen.makeTree(x, 25, z, map);
                }
            }
        }
        yield return 0;
    }

    public static float CalculateNoiseValue(Vector3 pos,Vector3 offset,float scale) {
        float noiseX = Mathf.Abs((pos.x + offset.x) * scale);
        float noiseY = Mathf.Abs((pos.y + offset.y) * scale);
        float noiseZ = Mathf.Abs((pos.z + offset.z) * scale);

        return Mathf.Max(0, Noise.Generate(noiseX, noiseY, noiseZ));
    }

	
	public virtual IEnumerator CreateVisualMesh() {
		
		List<Vector3> verts = new List<Vector3>();
		List<Vector2> uvs = new List<Vector2>();
		List<int> tris = new List<int>();
		
		
		for (int x = 0; x < width; x++)
		{
			for (int y = 0; y < height; y++)
			{
				for (int z = 0; z < width; z++)
				{
					if (map[x,y,z] == 0) continue;
					
					Block.ID block = (Block.ID) map[x,y,z];
					// Left wall
					if (IsTransparent(x - 1, y, z))
						BuildFace (block, Block.Face.WEST, new Vector3(x, y, z), Vector3.up, Vector3.forward, false, verts, uvs, tris);
					// Right wall
					if (IsTransparent(x + 1, y , z))
						BuildFace (block, Block.Face.EAST, new Vector3(x + 1, y, z), Vector3.up, Vector3.forward, true, verts, uvs, tris);
					
					// Bottom wall
					if (IsTransparent(x, y - 1 , z))
						BuildFace (block, Block.Face.BOTTOM, new Vector3(x, y, z), Vector3.forward, Vector3.right, false, verts, uvs, tris);
					// Top wall
					if (IsTransparent(x, y + 1, z))
						BuildFace (block, Block.Face.TOP, new Vector3(x, y + 1, z), Vector3.forward, Vector3.right, true, verts, uvs, tris);
					
					// Back
					if (IsTransparent(x, y, z - 1))
						BuildFace (block, Block.Face.NORTH, new Vector3(x, y, z), Vector3.up, Vector3.right, true, verts, uvs, tris);
					// Front
					if (IsTransparent(x, y, z + 1))
						BuildFace (block, Block.Face.SOUTH, new Vector3(x, y, z + 1), Vector3.up, Vector3.right, false, verts, uvs, tris);
					
					
				}
			}
		}

        Mesh mesh =meshFilter.mesh;
        mesh.Clear();
        mesh.vertices = verts.ToArray();
        mesh.triangles = tris.ToArray();
        meshCollider.sharedMesh = mesh;
        mesh.SetUVs(0, uvs);
        //MeshUtility.Optimize(mesh);
        mesh.RecalculateNormals();
					
		

        yield return 0;
	}
	public virtual void BuildFace(Block.ID id, Block.Face face, Vector3 corner, Vector3 up, Vector3 right, bool reversed, List<Vector3> verts, List<Vector2> uvs, List<int> tris)
	{
		int index = verts.Count;
		
		verts.Add (corner);
		verts.Add (corner + up);
		verts.Add (corner + up + right);
		verts.Add (corner + right);


        Vector2 uvWidth = new Vector2(1f/16f, 1f/16f);
       // Vector2 uvCorner = new Vector2(0.0001f,0.77f);

       Vector2 uvCorner = Texture.getTexture(id, face);

        //uvCorner.x += (float)(1 - 1) / 16;

		uvs.Add(uvCorner);
        uvs.Add(new Vector2(uvCorner.x,uvCorner.y + uvWidth.y));
        uvs.Add(new Vector2(uvCorner.x + uvWidth.x, uvCorner.y + uvWidth.y));
        uvs.Add(new Vector2(uvCorner.x + uvWidth.x, uvCorner.y));


        if (reversed)
		{
			tris.Add(index + 0);
			tris.Add(index + 1);
			tris.Add(index + 2);
			tris.Add(index + 2);
			tris.Add(index + 3);
			tris.Add(index + 0);
		}
		else
		{
			tris.Add(index + 1);
			tris.Add(index + 0);
			tris.Add(index + 2);
			tris.Add(index + 3);
			tris.Add(index + 2);
			tris.Add(index + 0);
		}
		
	}
	public virtual bool IsTransparent (int x, int y, int z)
	{
        if (y < 0) return false;
		byte brick = GetByte(x,y,z);
        if(brick == (byte)Block.ID.GLASS || brick == (byte)Block.ID.OAK_LEAVES){
            return true;
        }
		switch (brick)
		{
		case 0: 
			return true;
        default:
            return false;
        }
	}
	public virtual byte GetByte (int x, int y , int z)
	{
        if ((y < 0) || (y >= height))
            return 0;

        if ((x < 0) || (z < 0) || (x >= width) || (z >= width))
        {
            Vector3 worldPos = new Vector3(x, y, z) + transform.position;
            Chunk chuck = Chunk.FindChunk(worldPos);
            if (chuck == this) return 0;
            if (chuck == null) {
                return GetTheoreticalByte(worldPos);
            }
            return chuck.GetByte(worldPos);
        }
		return map[x,y,z];
	}
    public virtual byte GetByte(Vector3 worldPos) {
        worldPos -= transform.position;
        int x = Mathf.FloorToInt(worldPos.x);
        int y = Mathf.FloorToInt(worldPos.y);
        int z = Mathf.FloorToInt(worldPos.z);
        return GetByte(x, y, z);
    }

    public static Chunk FindChunk(Vector3 pos) {
        for (int a = 0; a < chunks.Count; a++) {
            Vector3 cpos = chunks[a].transform.position;

            if ((pos.x < cpos.x) || (pos.z < cpos.z) || (pos.x >= cpos.x + width) || (pos.z >= cpos.z + width)) continue;
            return chunks[a];
        }
        return null;
    }

    public bool SetBrick(byte brick, Vector3 worldPos)
    {
        worldPos -= transform.position;
        if(worldPos.x <= width && worldPos.y <= height && worldPos.z <= width){
            if(map[(int)worldPos.x, (int)worldPos.y, (int)worldPos.z] == (byte)Block.ID.BEDROCK){
                return false;
            }
        }
        return SetBrick(brick, Mathf.FloorToInt(worldPos.x), Mathf.FloorToInt(worldPos.y), Mathf.FloorToInt(worldPos.z));
    }

    public bool SetBrick(byte brick, int x, int y, int z) {
        if(z >= width){
            Chunk chunk = FindChunk(new Vector3(transform.position.x, transform.position.y, transform.position.z + width));
            if(chunk != null)
                chunk.SetBrick(brick, x, y, z-(int)width);
        }

        if(x >= width){
            Chunk chunk = FindChunk(new Vector3(transform.position.x + width, transform.position.y, transform.position.z));
            if(chunk != null)
                chunk.SetBrick(brick, x-(int) width, y, z);
        }

        if(z < 0){
            Chunk chunk = FindChunk(new Vector3(transform.position.x, transform.position.y, transform.position.z - width));
            if(chunk != null)
                chunk.SetBrick(brick, x, y, z+(int)width);
        }

        if(x < 0){
            Chunk chunk = FindChunk(new Vector3(transform.position.x - width, transform.position.y, transform.position.z));
            if(chunk != null)
                chunk.SetBrick(brick, x+(int) width, y, z);
        }

        if ((x < 0) || (y < 0) || (z < 0) || (x >= width) || (y >= height) || (z >= width)) {
            return false;
        }

        

        if (map[x, y, z] == brick) return false;
        map[x, y, z] = brick;
        StartCoroutine(CreateVisualMesh());
        if (x == 0) {
            Chunk chunk = FindChunk(new Vector3(x - 2, y, z) + transform.position);
            if(chunk != null)
                StartCoroutine(chunk.CreateVisualMesh());
        }
        if (x == width - 1)
        {
            Chunk chunk = FindChunk(new Vector3(x + 2, y, z) + transform.position);
            if (chunk != null)
                StartCoroutine(chunk.CreateVisualMesh());
        }
        if (z == 0)
        {
            Chunk chunk = FindChunk(new Vector3(x, y, z - 2) + transform.position);
            if (chunk != null)
                StartCoroutine(chunk.CreateVisualMesh());
        }
        if (z == width - 1)
        {
            Chunk chunk = FindChunk(new Vector3(x, y, z + 2) + transform.position);
            if (chunk != null)
                StartCoroutine(chunk.CreateVisualMesh());
        }

        return true;
    }
	
}


