using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class RoomGenerator : MonoBehaviour
{
    public List<GameObject> Room = new List<GameObject>();

    public GameObject BaseRoom;
    public GameObject WalkerRoom;


    //目标：随机种子
    //目标：多层立体结构，大小区块有区别。
    //目标：有重要性分层
    //目标：怪物房间区分。
    //目标：房间大小区分
    public void OnButtonClick()
    {
        GenerateRooms(4, 4);
        ConnectTheRoom(Room);
        PackAllRoomObject();
    }
    //GenerateGrids格子形式
    void GenerateRooms(int row, int col)
    {
        try
        {
            foreach (var item in GameObject.FindGameObjectsWithTag("Room"))
            {
                GameObject.Destroy(item);
            }
            Room.Clear();
        }
        catch (System.Exception)
        {

            throw;
        }

        for (int i = 0; i < row; i++)
        {
            for (int j = 0; j < col; j++)
            {
                
                if (Random.Range(0, 10) > 5)
                {

                    GameObject tempRoom = Instantiate(BaseRoom);

                    tempRoom.name = "Room(" + i + "," + j + ")";
                    tempRoom.transform.position = new Vector3(i, 0.5f, j);
                    tempRoom.tag = "Room";

                    Room.Add(tempRoom);
                }

            }
        }
    }
    //第一种连线法
    void ConnectTheRoom(List<GameObject> room)
    {
        //
        //宋子俊的柴火
        var ls = new List<GameObject>();
        room.ForEach(item =>
        {
            var obj = room.Where(r => r != item&&!ls.Contains(r)).
                    OrderBy(r => Vector3.Distance(item.transform.position, r.transform.position)).
                    FirstOrDefault();
            if (obj!=null)
            {
                GameObject tempwalker = Instantiate(WalkerRoom);

                tempwalker.name = "Room_walker";
                tempwalker.transform.position = Vector3.Lerp(obj.transform.position, item.transform.position,0.5f);
                tempwalker.transform.LookAt(obj.transform.position);

                tempwalker.transform.localScale = new Vector3(0.5f* tempwalker.transform.localScale.x, 0.5f* tempwalker.transform.localScale.y, 0.5f* tempwalker.transform.localScale.z);
                tempwalker.transform.localScale = new Vector3(tempwalker.transform.localScale.x, tempwalker.transform.localScale.y,Vector3.Distance(obj.transform.position, item.transform.position));
                tempwalker.tag = "Room";

                Debug.DrawLine(obj.transform.position, item.transform.position, Color.yellow, 3);
                ls.Add(item);
            }

        });

    }
    //打包所有房间到一个父物体
    void PackAllRoomObject()
    {
        GameObject packer = new GameObject();
        packer.name = "Rooms_Content";

        foreach (var item in GameObject.FindGameObjectsWithTag("Room"))
        {
            item.transform.SetParent(packer.transform);
        }
        packer.tag = "Room";

    }
    //第二种连线法
    void ConnectTheRoom_A(List<GameObject> room, List<GameObject> checkPoint)
    {

        List<GameObject> tempRoom = room;
        foreach (var item in room)
        {
            //遍历每一个与他没有关系的，计算他们之间的距离
            foreach (var c_item in tempRoom)
            {
                List<float> distanceBetween = new List<float>();

                if (Vector3.Distance(item.transform.position, c_item.transform.position) >= 0.5)
                {
                    distanceBetween.Add(Vector3.Distance(item.transform.position, c_item.transform.position));
                }

            }

            tempRoom.RemoveAt(tempRoom.Count - 1);

            checkPoint.Add(tempRoom[tempRoom.Count - 1]);

            //Debug.DrawLine(,Color.yellow,1000);
        }
    }

}
