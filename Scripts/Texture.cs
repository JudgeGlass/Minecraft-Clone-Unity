using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Texture {
	private static Vector2[] grass = {
		new Vector2(2f / 16f, 15f / 16f), // Back side
		new Vector2(2f / 16f, 15f / 16f), // Front Side
		new Vector2(2f / 16f, 15f / 16f), // Right Side
		new Vector2(2f / 16f, 15f / 16f), // Left Side
		new Vector2(1f / 16f, 15f / 16f), // Top
		new Vector2(3f / 16f, 15f / 16f) // Bottom Side
	};

	private static Vector2[] dirt = {
		new Vector2(3f / 16f, 15f / 16f), // All
	};

	private static Vector2[] stone = {
		new Vector2(0f / 16f, 15f / 16f), // All
	};

	private static Vector2[] bedrock = {
		new Vector2(13f / 16f, 15f / 16f), // All
	};

	private static Vector2[] sand = {
		new Vector2(14f / 16f, 15f / 16f), // All
	};

	private static Vector2[] gravel = {
		new Vector2(15f / 16f, 15f / 16f), // All
	};

	private static Vector2[] cobblestone = {
		new Vector2(4f / 16f, 15f / 16f), // All
	};

	private static Vector2[] glass = {
		new Vector2(13f / 16f, 14f / 16f) // All
	};

	private static Vector2[] moss_stone = {
		new Vector2(10f / 16f, 10f / 16f) // All
	};

	private static Vector2[] moss_brick = {
		new Vector2(0f / 16f, 5f / 16f) // All
	};

	private static Vector2[] cracked_brick = {
		new Vector2(1f / 16f, 5f / 16f) // All
	};

	private static Vector2[] stone_bricks = {
		new Vector2(15f / 16f, 6f / 16f) // All
	};

	private static Vector2[] oak_planks = {
		new Vector2(5f / 16f, 15f / 16f), //ALL
	};

	private static Vector2[] oak_leaves = {
		new Vector2(8f / 16f, 14f / 16f) // All
	};

	private static Vector2[] bricks = {
		new Vector2(5f / 16f, 10f / 16f),
	};

	private static Vector2[] clay = {
		new Vector2(14f / 16f, 7f / 16f) // All
	}; 

	private static Vector2[] cursor = {
		new Vector2(0f/ 16f, 0f / 16f),
	};

	private static Vector2[] oak_log = {
		new Vector2(4f / 16f, 14f / 16f), // Back side
		new Vector2(4f / 16f, 14f / 16f), // Front Side
		new Vector2(4f / 16f, 14f / 16f), // Right Side
		new Vector2(4f / 16f, 14f / 16f), // Left Side
		new Vector2(3f / 16f, 14f / 16f), // Top
		new Vector2(3f / 16f, 14f / 16f) // Bottom Side
	};

	private static Vector2[] sandstone = {
		new Vector2(6f / 16f, 13f / 16f), // Back side
		new Vector2(6f / 16f, 13f / 16f), // Front Side
		new Vector2(6f / 16f, 13f / 16f), // Right Side
		new Vector2(6f / 16f, 13f / 16f), // Left Side
		new Vector2(4f / 16f, 13f / 16f), // Top
		new Vector2(5f / 16f, 13f / 16f) // Bottom Side
	};

	private static Vector2[] bookcase = {
		new Vector2(9f / 16f, 10f / 16f), // Back side
		new Vector2(9f / 16f, 10f / 16f), // Front Side
		new Vector2(9f / 16f, 10f / 16f), // Right Side
		new Vector2(9f / 16f, 10f / 16f), // Left Side
		new Vector2(5f / 16f, 15f / 16f), // Top
		new Vector2(5f / 16f, 15f / 16f) // Bottom Side
	};


	public static Vector2 getTexture(Block.ID id, Block.Face face){
		Vector2 texture = new Vector2();
		int index = (int) face;

		switch(id){
			case Block.ID.AIR:
				return texture;
			case Block.ID.GRASS:
				return grass[index];
			case Block.ID.DIRT:
				return dirt[0];
			case Block.ID.STONE:
				return stone[0];
			case Block.ID.BEDROCK:
				return bedrock[0];
			case Block.ID.COBBLESTONE:
				return cobblestone[0];
			case Block.ID.GLASS:
				return glass[0];
			case Block.ID.OAK_LOG:
				return oak_log[index];
			case Block.ID.OAK_PLANKS:
				return oak_planks[0];
			case Block.ID.BRICKS:
				return bricks[0];
			case Block.ID.OAK_LEAVES:
				return oak_leaves[0];
			case Block.ID.MOSS_STONE:
				return moss_stone[0];
			case Block.ID.MOSS_BRICK:
				return moss_brick[0];
			case Block.ID.CRACKED_BRICK:
				return cracked_brick[0];
			case Block.ID.STONE_BRICK:
				return stone_bricks[0];
			case Block.ID.CLAY:
				return clay[0];
			case Block.ID.SANDSTONE:
				return sandstone[index];
			case Block.ID.SAND:
				return sand[0];
			case Block.ID.GRAVEL:
				return gravel[0];
			case Block.ID.BOOKCASE:
				return bookcase[index];
			case Block.ID.CURSOR_1:
				return cursor[0];

		}


		return texture;
	}
	
}
