using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerIO : MonoBehaviour {

    public static PlayerIO currentPlayerIO;
    public float maxInteractionRange = 8;
    public byte selectedInventory = 0;
    private byte currentBlock = 1;
    public GameObject text;

    public GameObject cursor;
    public Image imgSlider;
    public Image hotbarCanvas;

    public Text txtName;

    public Sprite hotbar1;
    public Sprite hotbar2;

    private int index = 0;
    private int hotbarIndex = 1;
    private byte[] hotbar1Data = {(byte) Block.ID.COBBLESTONE, (byte) Block.ID.STONE_BRICK, (byte) Block.ID.MOSS_BRICK, (byte) Block.ID.CRACKED_BRICK, (byte) Block.ID.MOSS_STONE, (byte) Block.ID.OAK_PLANKS, (byte) Block.ID.BRICKS, (byte) Block.ID.STONE, (byte) Block.ID.DIRT};
    private byte[] hotbar2Data = {(byte) Block.ID.GRASS, (byte) Block.ID.CLAY, (byte) Block.ID.SANDSTONE, (byte) Block.ID.SAND, (byte) Block.ID.GRAVEL, (byte) Block.ID.OAK_LOG, (byte) Block.ID.OAK_LEAVES, (byte) Block.ID.BOOKCASE, (byte) Block.ID.GLASS};

    private Vector2 pos;
    private bool skip = false;

    public Canvas canvas;
    private RectTransform rect;

    // Use this for initialization
    void Start () {
        currentPlayerIO = this;
        rect = canvas.GetComponent<RectTransform>();
	}
	
	// Update is called once per frame
	void Update () {
        //cursor.transform.position = worldPos1;
        if(Input.mouseScrollDelta.y < 0){
            if(index - 1 == -1){
                index = 8;
                skip = true;
            }else{
                index--;
            }
        }else if(Input.mouseScrollDelta.y > 0){
            if(index + 1 == 9){
                index = 0;
                skip = true;
            }else{
                index++;
            }
        } 
        pos = imgSlider.transform.position;
        int sx = (int)((rect.rect.width / 2) - 237);
        imgSlider.transform.position = new Vector2((index * 60) + sx, pos.y);

        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Q))
        {
            Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2.0f, Screen.height / 2.0f, 0));
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, maxInteractionRange))
            {
                Chunk chunk = hit.transform.GetComponent<Chunk>();
                if (chunk == null)
                {
                    Debug.Log("Clicked on " + hit.transform.name + " and it's not a chunk.");
                    return;
                }

                Debug.Log("Clicked on the chunk at " + chunk.transform.position);

                Vector3 p = hit.point;

                if (selectedInventory == 0)
                {
                    p -= hit.normal / 4;
                    if(transform.position == p){
                        return;
                    }
                    
                    chunk.SetBrick(0, p);
                }
            }
            else
            {
                Debug.Log("Clicked, but nothing was there!");
            }

        }
        if (Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.E))
        {
            RaycastHit hitInfo;
      if (Physics.Raycast(Camera.main.ScreenPointToRay(new Vector3((float) Screen.width / 2f, (float) Screen.height / 2f, 0.0f)), out hitInfo, this.maxInteractionRange))
      {
        Chunk component = hitInfo.transform.GetComponent<Chunk>();
        if ((Object) component == (Object) null)
          return;
        Vector3 worldPos = hitInfo.point + hitInfo.normal / 4f;
        if (component.GetByte(worldPos) != (byte) 0)
          return;
        if (worldPos != this.transform.position)
          component.SetBrick(this.currentBlock, worldPos);
      }
      else
        Debug.Log((object) "Clicked, but nothing was there!");

        }
        if (Input.GetKeyDown("1"))
        {
            hotbarIndex = 1;
            hotbarCanvas.sprite = hotbar1;
        }
        if (Input.GetKeyDown("2"))
        {
            hotbarIndex = 2;
            hotbarCanvas.sprite = hotbar2;
        }
        
        if(hotbarIndex == 1){
            currentBlock = hotbar1Data[index];
        }else if(hotbarIndex == 2){
            currentBlock = hotbar2Data[index];
        }
        
        txtName.text = ((Block.ID)currentBlock).ToString();
    }
}
