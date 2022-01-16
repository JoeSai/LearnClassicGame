using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class Level : MonoBehaviour
{
    [SerializeField] private VoidEventChannelSO _birdDiedEvent = default;
    [SerializeField] private VoidEventChannelSO _startGameEvent = default;
    
    private static Level _instance;
    public static Level GetInstance()
    {
        return _instance;
    }
    
    private const float AREA_HEIGHT = 50f;
    private const float PIPE_WIDTH = 7.8f;
    private const float PIPE_HEAD_HEIGHT = 3.7f;
    private const float PIPE_DESTROY_X_POSITION = -120f;
    private const float PIPE_SPAWN_X_POSITION = +120f;
    
    private const float GROUND_END_X_POSITION = -200f;
    private const float GROUND_START_X_POSITION = +400f;
    private const float GROUND_Y_POSITION = -47.5f;
    // private const float GROUND_WIDTH = 200f;
    
    private const float BIRD_X_POSITION = 0f;
    private const float BIRD_MOVE_SPEED = 25f;

    private const float CLOUD_SPAWN_X_POSITION = 200f;
    private const float CLOUD_DESTROY_X_POSITION = -200f;

    private List<Pipe> _pipeList = new List<Pipe>();
    private List<Transform> _groundList = new List<Transform>();
    private List<Transform> _cloudList = new List<Transform>();
    
    private float pipeSpawnTimer;
    private float pipeSpawnTimerMax = 3f;
    private float gapSize = 30f;

    private float cloudSpawnTimer;
    private float cloudSpawnTimeMax = 3f;

    private int pipesSpawned; //生成的管道数
    private int pipesPassed; //通过的管道数
    
    public enum Difficulty
    {
        Easy,
        Medium,
        Hard,
        Impossible,
    }

    private GameState _state;
    private enum GameState
    {
        WaitingToState,
        Playing,
        End,
    }

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        
    }

    void Start()
    {
        SetDifficulty(Difficulty.Easy);
        _state = GameState.WaitingToState;
        
        HandleGroundCreate();
    }

    private void OnEnable()
    {
        _birdDiedEvent.OnEventRaised += OnBirdDied;
        _startGameEvent.OnEventRaised += OnStartGame;
    }

    private void OnDisable()
    {
        _birdDiedEvent.OnEventRaised -= OnBirdDied;
        _startGameEvent.OnEventRaised -= OnStartGame;
    }

    private void OnStartGame()
    {
        _state = GameState.Playing;
    }

    private void OnBirdDied()
    {
        _state = GameState.End;
    }

    private void Restart()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("GameScene");
    }


    // Update is called once per frame
    void Update()
    {
        if (_state == GameState.Playing)
        {
            HandlePipeCreate();
            HandlePipeMovment();
            
            HandleGroundMovement();

            HandleCloudCreate();
            HandleCloudMovement();
        }
    }

    private void HandleCloudMovement()
    {
        for(int i = 0 ; i < _cloudList.Count ; i++)
        {
            Transform cloudTransform = _cloudList[i];
            cloudTransform.position += new Vector3(-1, 0, 0) * BIRD_MOVE_SPEED * Time.deltaTime * 0.5f;
            if (cloudTransform.position.x <= CLOUD_DESTROY_X_POSITION)
            {
                Destroy(cloudTransform.gameObject);
                _cloudList.RemoveAt(i);
                i--;
            }
        }
    }

    private void HandleCloudCreate()
    {
        cloudSpawnTimer -= Time.deltaTime;
        if (cloudSpawnTimer < 0)
        {
            cloudSpawnTimeMax = Random.Range(10f, 30f);
            cloudSpawnTimer = cloudSpawnTimeMax;

            int cloudIndex = Random.Range(0, GameAssets.GetInstance().clouds.Length);

            float cloud_y_positon = Random.Range(30f, 50f);
            
            Transform cloudTransform = Instantiate(GameAssets.GetInstance().clouds[cloudIndex] , new Vector3(CLOUD_SPAWN_X_POSITION,cloud_y_positon,0) , quaternion.identity);
            _cloudList.Add(cloudTransform);
        }
    }

    private void HandleGroundMovement()
    {
        foreach (var groundTransform in _groundList)
        {
            groundTransform.position += new Vector3(-1, 0, 0) * BIRD_MOVE_SPEED * Time.deltaTime;
            if (groundTransform.position.x <= GROUND_END_X_POSITION)
            {
                groundTransform.position = new Vector3(GROUND_START_X_POSITION,GROUND_Y_POSITION,groundTransform.position.z);
            }
        }
    }

    private void HandleGroundCreate()
    {
        Transform groundTransform;
        groundTransform = Instantiate(GameAssets.GetInstance().ground, new Vector3(0,GROUND_Y_POSITION,0),quaternion.identity);
        _groundList.Add(groundTransform);
        
        groundTransform = Instantiate(GameAssets.GetInstance().ground, new Vector3(GROUND_START_X_POSITION / 2,GROUND_Y_POSITION,0),quaternion.identity);
        _groundList.Add(groundTransform);
        
        groundTransform = Instantiate(GameAssets.GetInstance().ground, new Vector3(GROUND_START_X_POSITION,GROUND_Y_POSITION,0),quaternion.identity);
        _groundList.Add(groundTransform);
    }

    public int GetPipesPassed()
    {
        return pipesPassed;
    }

    private void SetDifficulty(Difficulty difficulty)
    {
        switch (difficulty)
        {
            case Difficulty.Easy:
                gapSize = 50f;
                pipeSpawnTimerMax = 3f;
                break;
            case Difficulty.Medium:
                gapSize = 40f;
                pipeSpawnTimerMax = 2.6f;
                break;;
            case Difficulty.Hard:
                gapSize = 30f;
                pipeSpawnTimerMax = 2.2f;
                break;;
            case Difficulty.Impossible:
                pipeSpawnTimerMax = 1.8f;
                gapSize = 20f;
                break;;
        }
    }
    
    private Difficulty GetDifficulty()
    {
        if (pipesSpawned >= 50) return Difficulty.Impossible;
        if (pipesSpawned >= 25) return Difficulty.Hard;
        if (pipesSpawned >= 10) return Difficulty.Medium;
        return Difficulty.Easy;
    }

    private void HandlePipeCreate()
    {
        if (pipeSpawnTimer < 0)
        {
            pipeSpawnTimer = pipeSpawnTimerMax;


            float heightLimit = 10;
            float maxHeight = AREA_HEIGHT - gapSize / 2 - heightLimit;
            float minHeight = -AREA_HEIGHT + gapSize / 2 + heightLimit;
            float height = Random.Range(minHeight, maxHeight);
            
            CreateGapPipes(height, gapSize, PIPE_SPAWN_X_POSITION);
        }

        pipeSpawnTimer -= Time.deltaTime;
    }

    private void HandlePipeMovment()
    {
        for (int i = 0; i < _pipeList.Count; i++)
        {
            Pipe pipe = _pipeList[i];
            bool isToTheRightOfBird = pipe.GetXPosition() > BIRD_X_POSITION;
            pipe.Move();
            if (isToTheRightOfBird && pipe.GetXPosition() <= BIRD_X_POSITION && pipe.IsBottom())
            {
                //pipe passed bird
                pipesPassed++;
                
                MusicMgr.GetInstance().PlaySound("Score");
            }
            if (pipe.GetXPosition() < PIPE_DESTROY_X_POSITION)
            {
                pipe.DestroySelf();
                _pipeList.Remove(pipe);
                i--;
            }
        }

    }

    private void CreateGapPipes(float gapY, float gapSize, float xPosition)
    {
        gapY += AREA_HEIGHT;
        CreatePipe(gapY - gapSize / 2 , xPosition , true);
        CreatePipe(AREA_HEIGHT * 2 - gapY - gapSize / 2 , xPosition , false);
        pipesSpawned++;
        SetDifficulty(GetDifficulty());
    }

    private void CreatePipe(float height, float xPosition , bool isBottom)
    {
        Transform pipeHead = Instantiate(GameAssets.GetInstance().pipeHead);
        Transform pipeBody = Instantiate(GameAssets.GetInstance().pipeBody);
        pipeBody.GetComponent<SpriteRenderer>().size = new Vector2(PIPE_WIDTH, height);
        pipeBody.GetComponent<BoxCollider2D>().size = new Vector2(PIPE_WIDTH, height);
        pipeBody.GetComponent<BoxCollider2D>().offset = new Vector2(0, height / 2);
        if (isBottom)
        {
            pipeHead.position = new Vector3(xPosition, -AREA_HEIGHT + height - PIPE_HEAD_HEIGHT / 2);
            pipeBody.position = new Vector3(xPosition, -AREA_HEIGHT);
        }
        else
        {
            pipeHead.position = new Vector3(xPosition, AREA_HEIGHT - height + PIPE_HEAD_HEIGHT / 2);
            pipeBody.position = new Vector3(xPosition, AREA_HEIGHT);
            pipeBody.localScale = new Vector3(1, -1, 1);
        }

        Pipe pipe = new Pipe(pipeHead, pipeBody,isBottom);
        _pipeList.Add(pipe);
    }
    
    
    private class Pipe
    {
        private Transform pipeHead;
        private Transform pipeBody;
        private bool isBottom;

        public Pipe(Transform pipeHead , Transform pipeBody , bool isBottom)
        {
            this.pipeHead = pipeHead;
            this.pipeBody = pipeBody;
            this.isBottom = isBottom;
        }

        public void Move()
        {
            pipeHead.position += new Vector3(-1, 0, 0) * BIRD_MOVE_SPEED * Time.deltaTime;
            pipeBody.position += new Vector3(-1, 0, 0) * BIRD_MOVE_SPEED * Time.deltaTime;
        }

        public bool IsBottom()
        {
            return isBottom;
        }

        public float GetXPosition()
        {
            return pipeBody.position.x;
        }

        public void DestroySelf()
        {
            Destroy(pipeHead.gameObject);
            Destroy(pipeBody.gameObject);
        }
    }
}



