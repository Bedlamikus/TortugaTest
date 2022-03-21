using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell
{
    public Cube cube;
    public bool destroy;
    public float x;
    public float y;
    public float z;
    public Cell()
    {
        destroy = false;
    }
}

public class Board : MonoBehaviour
{
    [SerializeField] private float m_width;
    [SerializeField] private float m_height;
    [SerializeField] private int m_Rows;
    [SerializeField] private int m_Cols;
    [SerializeField] private float m_swipeAnimationTime;
    [SerializeField] private Cube prefab;
    [SerializeField] private Collider BoardPlaneCollider;

    [SerializeField] private List<Material> materials;

    //Доска с кубами
    private Cell[,] board;

    //Переменные активного куба и его стартовых координат
    private Cube currentActiveCube;
    private Collider currentActiveCollider;
    private Vector3 CurrentActiveCubeStartPoint;

    private Cube swipeCube;
    private Direction swipeDirection;

    private bool magnetic;


    private void Start()
    {
        magnetic = false;
        board = new Cell[m_Rows, m_Cols];
        for (int i = 0; i < m_Rows; i++)
            for (int j = 0; j<m_Cols; j++)
            {
                board[i, j] = new Cell();
                var scale = prefab.GetScale();
                board[i, j].x = transform.position.x + j * m_width / m_Cols - m_width / 2 + scale.x * 2;
                board[i, j].y = transform.position.y;
                board[i, j].z = transform.position.z + i * m_height / m_Rows - m_height / 2;
                var point = new Vector3(board[i, j].x,
                                        board[i, j].y,
                                        board[i, j].z);
                board[i, j].cube = Instantiate(prefab, point, Quaternion.identity);
                board[i, j].cube.transform.parent = this.transform;
                var render = board[i, j].cube.GetComponentInChildren<Renderer>();
                if (render != null) render.material = materials[Random.Range(0,materials.Count)];
            }
        var inputcontroller = FindObjectOfType<InputController>();
        if (inputcontroller == null)
        {
            print("Not find <InputController>");
            return;
        }

        inputcontroller.mouseLeftDown.AddListener(SetActiveCube);
        inputcontroller.mouseLeftUp.AddListener(UnActiveCube);
        inputcontroller.mouseSwipe.AddListener(TrySwipe);
    }
    private void Update()
    {
        if (currentActiveCube == null) return;
        MagnetizeToMouse(currentActiveCollider);
    }

    private Cube GetCube(Collider collider)
    {
        Cube result = null;
        for (int i = 0; i < m_Rows; i++)
            for (int j = 0; j < m_Cols; j++)
                if (board[i, j].cube.HitOnMe(collider))
                {
                    result = board[i, j].cube;
                    break;
                }
        return result;
    }

    private Vector2 GetItem(Collider collider)
    {
        Vector2 result = new Vector2(-1, -1);
        for (int i = 0; i < m_Rows; i++)
            for (int j = 0; j < m_Cols; j++)
                if (board[i, j].cube.HitOnMe(collider))
                {
                    result = new Vector2(i, j);
                    break;
                }
        return result;
    }

    private Vector2 GetItem(Cube cube)
    {
        Vector2 result = new Vector2(-1, -1);
        for (int i = 0; i < m_Rows; i++)
            for (int j = 0; j < m_Cols; j++)
                if (board[i, j].cube == cube)
                {
                    result = new Vector2(i, j);
                    break;
                }
        return result;
    }

    //MagnetizeToMouse примагничивает collider в одном кадре, чтобы продлить нужен цикл или Update
    private void MagnetizeToMouse(Collider collider)
    {
        if (collider == null) return;
        if (currentActiveCube == null) return;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (BoardPlaneCollider.Raycast(ray, out hit, 100.0f))
        {
            collider.transform.position = hit.point;
            magnetic = true;
        }
    }
    private void SetActiveCube(Collider collider)
    {
        var newcurrentActiveCube = GetCube(collider);
        if (newcurrentActiveCube == null) return;
        if (newcurrentActiveCube.mooving == true) return;
        if (newcurrentActiveCube == currentActiveCube) return;

        currentActiveCube = newcurrentActiveCube;
        var itemXZ = GetItem(collider);
        CurrentActiveCubeStartPoint = new Vector3(board[(int)itemXZ.x, (int)itemXZ.y].x,
                                                    board[(int)itemXZ.x, (int)itemXZ.y].y,
                                                    board[(int)itemXZ.x, (int)itemXZ.y].z);
        currentActiveCollider = collider;
    }
    private void UnActiveCube(Collider collider)
    {
        if (currentActiveCube == null) return;
        if (magnetic == false) return;
        if (magnetic == true)
        {
            if (Swipe())
            {
                swipeDirection = Direction.Idle;
                ClearActiveCube();
                return; 
            }
            ResetCube(currentActiveCollider, m_swipeAnimationTime);
            ClearActiveCube();
        }
    }
    private void ResetCube(Collider collider, float time)
    {
        if (collider == null) return;
        var cube = GetCube(collider);
        var point = GetPositionCell(collider);
        cube.Move(point, time);
    }

    private void SwipeCubes(Cube cubeOne, Cube cubeTwo, float time)
    {
        var itemOne = GetItem(cubeOne);
        var itemTwo = GetItem(cubeTwo);
        var positionOne = GetPositionCell(cubeOne);
        var positionTwo = GetPositionCell(cubeTwo);
        cubeOne.Move(positionTwo, time);
        cubeTwo.Move(positionOne, time);
        var transportCube = board[(int)itemOne.x, (int)itemOne.y].cube;
        board[(int)itemOne.x, (int)itemOne.y].cube = cubeTwo;
        board[(int)itemTwo.x, (int)itemTwo.y].cube = transportCube;
    }

    private bool Swipe()
    {
        if (currentActiveCollider == null) return false;
        var item = GetItem(currentActiveCollider);
        int x = (int)item.x;
        int y = (int)item.y;
        bool result = false;

        switch (swipeDirection)
        {
            case Direction.Down:
                if (x <= 0) break;

                swipeCube = board[x-1,y].cube;       //second swipe cube
                SwipeCubes(currentActiveCube, swipeCube, m_swipeAnimationTime);

                swipeCube = null;
                result = true;
                break;
                
            case Direction.Up:
                if (x >= m_Rows-1) break;

                swipeCube = board[x + 1, y].cube;       //second swipe cube
                SwipeCubes(currentActiveCube, swipeCube, m_swipeAnimationTime);

                swipeCube = null;
                result = true;
                break;
            case Direction.Left:
                if (y <= 0) break;

                swipeCube = board[x, y-1].cube;       //second swipe cube
                SwipeCubes(currentActiveCube, swipeCube, m_swipeAnimationTime);
                swipeCube = null;
                result = true;
                break;
            case Direction.Right:
                if (y >= m_Cols-1) break;

                swipeCube = board[x , y+1].cube;       //second swipe cube
                SwipeCubes(currentActiveCube, swipeCube, m_swipeAnimationTime);
                swipeCube = null;
                result = true;
                break;
                
        }
        return result;
    }
    private void TrySwipe(Collider collider, Direction direction)
    {
        swipeDirection = direction;
    }
    private void ClearActiveCube()
    {
        currentActiveCube = null;
        currentActiveCollider = null;
        CurrentActiveCubeStartPoint = Vector3.zero;
        magnetic = false;
    }

    private Vector3 GetPositionCell(Collider collider)
    {
        var itemXZ = GetItem(collider);
        return new Vector3(board[(int)itemXZ.x, (int)itemXZ.y].x,
                            board[(int)itemXZ.x, (int)itemXZ.y].y,
                            board[(int)itemXZ.x, (int)itemXZ.y].z);
    }
    private Vector3 GetPositionCell(Cube cube)
    {
        var itemXZ = GetItem(cube);
        return new Vector3(board[(int)itemXZ.x, (int)itemXZ.y].x,
                            board[(int)itemXZ.x, (int)itemXZ.y].y,
                            board[(int)itemXZ.x, (int)itemXZ.y].z);
    }
}
