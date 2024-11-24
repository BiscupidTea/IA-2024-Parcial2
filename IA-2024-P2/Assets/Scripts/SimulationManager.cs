using System.Collections.Generic;
using IA_Library;
using IA_Library_FSM;
using IA_Library.Brain;
using UnityEngine;

public class SimulationManager : MonoBehaviour
{
    private Simulation simulation;
    
    public Material plantMaterial;
    public Material herbivoreMaterial;
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

    private float Bias = 0.5f;
    private float P = 0.5f;

    private GridManager NewGrid;

    private void OnEnable()
    {
        NewGrid = new GridManager(20, 20, 2);

        herbivoreMainBrain = new BrainData(11, new int[] { 7, 5, 3 }, 3, Bias, P);
        herbivoreMoveEatBrain = new BrainData(4, new int[] { 5, 4 }, 4, Bias, P);
        herbivoreMoveEscapeBrain = new BrainData(5, new int[] { 3 }, 1, Bias, P);
        herbivoreEatBrain = new BrainData(8, new int[] { 5, 3 }, 4, Bias, P);

        carnivoreMainBrain = new BrainData(5, new int[] { 3, 2 }, 2, Bias, P);
        carnivoreMoveEatBrain = new BrainData(4, new int[] { 3, 2 }, 2, Bias, P);
        carnivoreEatBrain = new BrainData(5, new int[] { 2, 2 }, 1, Bias, P);

        scavengerMainBrain = new BrainData(5, new int[] { 3, 5 }, 3, Bias, P);
        scavengerFlockingBrain = new BrainData(8, new int[] { 5, 5, 5 }, 4, Bias, P);

        List<BrainData> herbivoreData = new List<BrainData>
            { herbivoreMainBrain, herbivoreMoveEatBrain, herbivoreMoveEscapeBrain, herbivoreEatBrain };
        List<BrainData> carnivoreData = new List<BrainData>
            { carnivoreMainBrain, carnivoreMoveEatBrain, carnivoreEatBrain };
        List<BrainData> scavengerData = new List<BrainData> { scavengerMainBrain, scavengerFlockingBrain };

        simulation = new Simulation(NewGrid, herbivoreData, carnivoreData, scavengerData, 10, 10, 10, 5, 10, 10, 35);
    }

    private void Update()
    {
        simulation.UpdateSimulation(Time.deltaTime);
    }

    private void DrawEntities()
    {
        foreach (AgentPlant agent in simulation.Plants)
        {
            DrawSquare(new Vector3(agent.position.Y, agent.position.Y, 0), plantMaterial, 1);
        }

        foreach (AgentHerbivore agent in simulation.Herbivore)
        {
            DrawSquare(new Vector3(agent.position.X, agent.position.Y, 1), herbivoreMaterial, 1);
        }

        foreach (AgentCarnivore agent in simulation.Carnivore)
        {
            DrawSquare(new Vector3(agent.position.X, agent.position.Y, 2), carnivoreMaterial, 1);
        }

        foreach (AgentScavenger agent in simulation.Scavenger)
        {
            DrawSquare(new Vector3(agent.position.X, agent.position.Y, 3), scavengerMaterial, agent.radius);
        }
    }

    private void OnDrawGizmos()
    {
        if (!Application.isPlaying)
            return;

        Gizmos.color = Color.black;

        for (float x = 0;
             x <= simulation.gridManager.size.X * simulation.gridManager.cellSize;
             x += simulation.gridManager.cellSize)
        {
            Gizmos.DrawLine(new Vector3(x, 0, 0),
                new Vector3(x, simulation.gridManager.size.Y * simulation.gridManager.cellSize, 0));
        }

        for (float y = 0;
             y <= simulation.gridManager.size.Y * simulation.gridManager.cellSize;
             y += simulation.gridManager.cellSize)
        {
            Gizmos.DrawLine(new Vector3(0, y, 0),
                new Vector3(simulation.gridManager.size.X * simulation.gridManager.cellSize, y, 0));
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
}