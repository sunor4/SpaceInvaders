using Microsoft.Xna.Framework;
using System;
using Infrastructure.ObjectModel;
using SharpDX.MediaFoundation.DirectX;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace SpaceInvaders
{
    public class EnemyMatrix : CompositeDrawableComponent<Invader>
    {
        private const bool v_ForceGameOver = true;
        private const int k_MaxNumOfBulletEnemyOnScreen = 5;
        private const int k_NumOfPinkRows = 1;
        private const int k_NumOfBlueRows = 2;
        private const int k_NumOfWhiteRows = 2;
        private const int k_NumOfRows = 5;
        private int m_InitNumOfCols = 9;
        private int m_CurrentNumOfCols = 9;
        private int m_DeathCounter = 0;
        private int m_CurrentNumOfBulletsOnScreen = 0;
        private Invader[,] m_EnemyMatrix;
        private bool m_CompensateOnNextFrame = false;
        private float m_DistanceToCompensateOnNextFrame = 0;
        private int m_ExtraScorePerInvader = 0;
        public event Action LevelCleared;

        // Gets reference to an enemy so we could use its texture/size/position properties.
        private Invader m_DefaultInvader;

        public int ExtraScorePerInvader
        {
            get { return m_ExtraScorePerInvader; }
            set { m_ExtraScorePerInvader = value; }
        }

        public int InitNumOfCols
        {
            get { return m_InitNumOfCols; }
        }

        public int CurrentNumOfCols
        {
            get { return m_CurrentNumOfCols; }
            set { m_CurrentNumOfCols = value; }
        }

        public Invader[,] EnemyMatrixCollection
        {
            get { return m_EnemyMatrix; }
            private set { m_EnemyMatrix = value; }
        }

        private int CurrentNumOfBulletsOnScreen
        {
            get { return m_CurrentNumOfBulletsOnScreen; }
            set
            {
                value = MathHelper.Clamp(value, 0, k_MaxNumOfBulletEnemyOnScreen);
                m_CurrentNumOfBulletsOnScreen = value;
            }
        }

        public EnemyMatrix(Game i_Game) : base(i_Game)
        {
        }

        public override void Initialize()
        {
            base.Initialize();
            initializeEnemyMatrix();
            initializeEnemyPositions();
        }

        public override void Update(GameTime i_GameTime)
        {
            base.Update(i_GameTime);
            if (Enabled && m_CompensateOnNextFrame)
            {
                foreach (Invader invader in EnemyMatrixCollection)
                {
                    if (invader.Enabled)
                    {
                        invader.IsCompensateOnNextFrame = true;
                        invader.DistanceToCompensateOnNextFrame = m_DistanceToCompensateOnNextFrame;
                    }
                }

                m_CompensateOnNextFrame = false;
                m_DistanceToCompensateOnNextFrame = 0;
            }
        }

        // Initializes the enemy matrix and all enemies within it.
        private void initializeEnemyMatrix()
        {
            const int k_NumOFCellsPerInvader = 2;
            EnemyMatrixCollection = new Invader[k_NumOfRows, m_CurrentNumOfCols];

            for (int i = 0; i < k_NumOfRows; i++)
            {
                for (int j = 0; j < m_CurrentNumOfCols; j++)
                {
                    if (i < k_NumOfPinkRows)
                    {
                        EnemyMatrixCollection[i, j] = new Invader(this.Game, eInvaderType.Pink);
                    }
                    else if (i < k_NumOfPinkRows + k_NumOfBlueRows)
                    {
                        EnemyMatrixCollection[i, j] = new Invader(this.Game, eInvaderType.Blue);
                    }
                    else
                    {
                        EnemyMatrixCollection[i, j] = new Invader(this.Game, eInvaderType.Yellow);
                    }

                    Invader currentInvader = EnemyMatrixCollection[i, j];
                    currentInvader.CurrentCellIndex = i % k_NumOFCellsPerInvader;
                    currentInvader.ScoreOnKill += ExtraScorePerInvader;
                    this.Add(currentInvader);
                    subscribeToInvader(currentInvader);
                }
            }

            m_DefaultInvader = EnemyMatrixCollection[0, 0];
        }

        // Subscribes to events of each created invader in order to control them as a group by signals.
        private void subscribeToInvader(Invader i_Invader)
        {
            i_Invader.MatrixChange += Invader_MatrixChange;
            i_Invader.ReachBottom += Invader_ReachBottom;
            i_Invader.ReachBoundsOnNextFrame += Invader_ReachBoundsOnNextFrame;
            i_Invader.BulletShot += Invader_BulletShot;
            i_Invader.BulletDisappear += Invader_BulletDisappear;
        }

        // Handles event of shot bullet of invader in order to make sure there are only k_MaxNumOfBulletEnemyOnScreen
        // bullets on screen at any given time.
        private void Invader_BulletShot()
        {
            CurrentNumOfBulletsOnScreen++;
            if (CurrentNumOfBulletsOnScreen >= k_MaxNumOfBulletEnemyOnScreen)
            {
                foreach (Invader invader in EnemyMatrixCollection)
                {
                    invader.IsAllowedToShoot = false;
                }
            }
        }

        private void Invader_BulletDisappear()
        {
            CurrentNumOfBulletsOnScreen--;
            if (CurrentNumOfBulletsOnScreen < k_MaxNumOfBulletEnemyOnScreen)
            {
                foreach (Invader invader in EnemyMatrixCollection)
                {
                    invader.IsAllowedToShoot = true;
                }
            }
        }

        // Initializes the bounds of the enemy matrix formation.
        private void initializeEnemyPositions()
        {
            // Take any enemy's width and height.
            float enemyHeight = m_DefaultInvader.SourceRectangle.Height;
            float enemyWidth = m_DefaultInvader.SourceRectangle.Width;

            // Compute the starting position of the matrix:
            float startingPositionX = 0;
            float startingPositionY = enemyHeight * 3;
            Vector2 matrixStartingPosition = new Vector2(startingPositionX, startingPositionY);
            float spaceBetweenEnemies = enemyWidth * 0.6f;

            // Define variables for calculation of enemy location in loops.
            float enemyPositionX;
            float enemyPositionY;
            Vector2 enemyPosition;

            for (int i = 0; i < k_NumOfRows; i++)
            {
                for (int j = 0; j < m_CurrentNumOfCols; j++)
                {
                    // Calculate x,y initial position of each enemy:
                    enemyPositionX = matrixStartingPosition.X + (j * (enemyWidth + spaceBetweenEnemies));
                    enemyPositionY = matrixStartingPosition.Y + (i * (enemyHeight + spaceBetweenEnemies));
                    enemyPosition = new Vector2(enemyPositionX, enemyPositionY);
                    EnemyMatrixCollection[i, j].Position = enemyPosition;
                }
            }
        }

        // Handles case of invader death in order to terminate the game when all invaders are gone.
        private void Invader_MatrixChange()
        {
            m_DeathCounter++;

            if (m_DeathCounter == EnemyMatrixCollection.Length)
            {
                OnLevelCleared();
            }
        }

        private void OnLevelCleared()
        {
            if (LevelCleared != null)
            {
                LevelCleared.Invoke();
            }
        }

        // Handles case of invaders getting to the screen X-boundaries, in order to trigger jump down.
        private void Invader_ReachBoundsOnNextFrame(float i_DistanceToCompensate)
        {
            if (!m_CompensateOnNextFrame)
            {
                m_DistanceToCompensateOnNextFrame = i_DistanceToCompensate;
                m_CompensateOnNextFrame = true;
            }
        }

        // Handles case of invaders reaching bottom of the screen.
        private void Invader_ReachBottom()
        {
            (Game as BaseGame).GameOver(v_ForceGameOver);
        }

        public void Reset()
        {
            this.Clear();
            m_DeathCounter = 0;
            m_CurrentNumOfBulletsOnScreen = 0;
            m_CompensateOnNextFrame = false;
            m_DistanceToCompensateOnNextFrame = 0;
        }
    }
}
