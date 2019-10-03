using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeGen 
{
    public static void makeTree(int x, int y, int z, byte[,,] data){
        data[x, y + 1, z] = (byte) Block.ID.OAK_LOG;
        data[x, y + 2, z] = (byte) Block.ID.OAK_LOG;
        data[x, y + 3, z] = (byte) Block.ID.OAK_LOG;
        data[x, y + 4, z] = (byte) Block.ID.OAK_LOG;

        for(int xx = -2; xx < 3; xx++){
            for(int zz = -2; zz < 3; zz++){
                if(xx == 0 && zz == 0) continue;
                data[x + xx, y + 3, z + zz] = (byte) Block.ID.OAK_LEAVES;
                data[x + xx, y + 4, z + zz] = (byte) Block.ID.OAK_LEAVES;
            }
        }

        for(int xx = -1; xx < 2; xx++){
            for(int zz = -1; zz < 2; zz++){
                data[x + xx, y + 5, z + zz] = (byte) Block.ID.OAK_LEAVES;
            }
        }
    }
}
