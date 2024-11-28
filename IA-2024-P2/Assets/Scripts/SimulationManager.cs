using System;
using System.Collections.Generic;
using IA_Library;
using IA_Library_FSM;
using IA_Library.Brain;
using UnityEngine;

public class SimulationManager : MonoBehaviour
{
    private Simulation simulation;

    public string fileToLoad;
    public string filePath = "/Saves/Genomes";
    public string fileExtension = ".citosina";

    [SerializeField] private Vector2Int gridSize;
    [SerializeField] private float cellSize;

    [SerializeField] private int totalHerbivores;
    [SerializeField] private int totalCarnivores;
    [SerializeField] private int totalScavengers;

    [SerializeField] private int mutationChance;
    [SerializeField] private int mutationRate;

    [SerializeField] private int totalElites;

    [SerializeField] private int generationTime;

    public Material plantMaterial;
    public Material deadPlantMaterial;
    public Material herbivoreMaterial;
    public Material deadHerbivoreMaterial;
    public Material carnivoreMaterial;
    public Material scavengerMaterial;

    public Mesh CubeMesh;

    private BrainData herbivoreMainBrain;
    private BrainData herbivoreMoveEatBrain;
    private BrainData herbivoreMoveEscapeBrain;
    private BrainData herbivoreEatBrain;
    private BrainData carnivoreMainBrain;
    private BrainData carnivoreMoveEatBrain;
    private BrainData carnivoreEatBrain;
    private BrainData scavengerMainBrain;
    private BrainData scavengerFlockingBrain;

    public float herbivoreBias = 0.5f;
    public float herbivoreP = 0.5f;

    public float carnivoreBias = 0.5f;
    public float carnivoreP = 0.5f;

    public float scavengerBias = 0.5f;
    public float scavengerP = 0.5f;

    private GridManager NewGrid;

    private void OnEnable()
    {
        NewGrid = new GridManager(gridSize.x, gridSize.y, cellSize);

        herbivoreMainBrain = new BrainData(11, new int[] { 9, 7, 5, 3 }, 3, herbivoreBias, herbivoreP);
        herbivoreMoveEatBrain = new BrainData(4, new int[] { 8, 8, 6, 4 }, 4, herbivoreBias, herbivoreP);
        herbivoreMoveEscapeBrain = new BrainData(8, new int[] { 8, 6, 4, 4 }, 4, herbivoreBias, herbivoreP);
        herbivoreEatBrain = new BrainData(5, new int[] { 3, 3, 2 }, 1, herbivoreBias, herbivoreP);

        carnivoreMainBrain = new BrainData(5, new int[] { 3, 2 }, 2, carnivoreBias, carnivoreP);
        carnivoreMoveEatBrain = new BrainData(4, new int[] { 6, 4, 4 }, 2, carnivoreBias, carnivoreP);
        carnivoreEatBrain = new BrainData(5, new int[] { 6, 4, 2 }, 1, carnivoreBias, carnivoreP);

        scavengerMainBrain = new BrainData(5, new int[] { 8, 6, 4, 6 }, 2, scavengerBias, scavengerP);
        scavengerFlockingBrain = new BrainData(8, new int[] { 10, 8, 6, 6 }, 6, scavengerBias, scavengerP);

        List<BrainData> herbivoreData = new List<BrainData>
            { herbivoreMainBrain, herbivoreMoveEatBrain, herbivoreMoveEscapeBrain, herbivoreEatBrain };
        List<BrainData> carnivoreData = new List<BrainData>
            { carnivoreMainBrain, carnivoreMoveEatBrain, carnivoreEatBrain };
        List<BrainData> scavengerData = new List<BrainData> { scavengerMainBrain, scavengerFlockingBrain };

        simulation = new Simulation(NewGrid, herbivoreData, carnivoreData, scavengerData, totalHerbivores,
            totalCarnivores, totalScavengers, totalElites, mutationChance, mutationRate, generationTime)
        {
            filepath = Application.dataPath + filePath,
            fileExtension = fileExtension,
            fileToLoad = fileToLoad
        };

        simulation.OnFitnessCalculated += LogFitness;
    }

    private void OnDisable()
    {
        simulation.OnFitnessCalculated -= LogFitness;
    }

    private void Update()
    {
        simulation.UpdateSimulation(Time.deltaTime);
    }

    private void LogFitness(int nH, float FH, int nC, float FC, int nS, float FS)
    {
        // Debug.Log("--- Average Fitness ---");
        // Debug.Log("Herbivore - Alive = " + nH + " / fitness = " + FH);
        // Debug.Log("Carnivore - Alive = " + nC + " / fitness = " + FC);
        // Debug.Log("Scavenger - Alive = " + nS + " / fitness = " + FS);
    }

    private void DrawEntities()
    {
        foreach (AgentPlant agent in simulation.Plants)
        {
            if (agent.CanBeEaten())
            {
                DrawSquare(new Vector3(agent.position.X, agent.position.Y, 0), plantMaterial, 1);
            }
            else
            {
                DrawSquare(new Vector3(agent.position.X, agent.position.Y, 0), deadPlantMaterial, 1);
            }
        }

        foreach (AgentHerbivore agent in simulation.Herbivore)
        {
            if (agent.lives <= 0)
            {
                DrawSquare(new Vector3(agent.position.X, agent.position.Y, 0), deadHerbivoreMaterial, 1);
            }
            else
            {
                DrawSquare(new Vector3(agent.position.X, agent.position.Y, 0), herbivoreMaterial, 1);
            }
        }

        foreach (AgentCarnivore agent in simulation.Carnivore)
        {
            DrawSquare(new Vector3(agent.position.X, agent.position.Y, 0), carnivoreMaterial, 1);
        }

        foreach (AgentScavenger agent in simulation.Scavenger)
        {
            DrawSquare(new Vector3(agent.position.X, agent.position.Y, 0), scavengerMaterial, 1);
        }
    }

    private void OnDrawGizmos()
    {
        if (!Application.isPlaying)
            return;

        Gizmos.color = Color.black;

        for (int x = 0; x < simulation.gridManager.size.X; x++)
        {
            for (int y = 0; y < simulation.gridManager.size.Y; y++)
            {
                Gizmos.DrawSphere(new Vector3(x, y, 0), 0.2f);
            }
        }
    }

    private void DrawSquare(Vector3 position, Material color, float squareSize)
    {
        color.SetPass(0);
        Matrix4x4 matrix =
            Matrix4x4.TRS(position, Quaternion.identity, new Vector3(squareSize, squareSize, squareSize));
        Graphics.DrawMeshNow(CubeMesh, matrix);
    }

    private void OnRenderObject()
    {
        DrawEntities();
    }

    [ContextMenu("Load Save")]
    private void Load()
    {
        simulation.fileToLoad = Application.dataPath + filePath + fileToLoad + "." + fileExtension;
        simulation.filepath = Application.dataPath + filePath;
        simulation.fileExtension = fileExtension;
        simulation.Load();
    }
}