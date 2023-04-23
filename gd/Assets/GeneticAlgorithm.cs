using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GeneticAlgorithm : MonoBehaviour {
    public GameObject main_agent;
    NNinfo child1;
    NNinfo child2;


    public struct NNinfo {
        public float[][] weights1;
        public float[] biases1;
        public float[][] weights2;
        public float[] biases2;
        public float fitness;
    }

    float mutationStrength = 0.01f;
    float mutationChance = 0.05f;

    int populationSize = 25; //should be odd
    List<NNinfo> population = new List<NNinfo>();
    List<NNinfo> newPopulation = new List<NNinfo>();


    void createAgent(NNinfo param=default) {
        Vector3 spawnPosition = main_agent.transform.position;
        spawnPosition.x += Random.Range(-3f, 3f);
        if (EqualityComparer<NNinfo>.Default.Equals(param, default(NNinfo))) {
            Instantiate(main_agent, spawnPosition, Quaternion.identity);
        }
        else {
            GameObject agent = Instantiate(main_agent, spawnPosition, Quaternion.identity);
            Agent agentScript = agent.GetComponent<Agent>();
            agentScript.layer1.weights = param.weights1;
            agentScript.layer1.biases = param.biases1;
            agentScript.layer2.weights = param.weights2;
            agentScript.layer2.biases = param.biases2;
            agentScript.fitness = 0;
        }
    }


    void viewAgents(List<NNinfo> myNNInfoList) {
        string nnInfoListString = "";
        foreach (NNinfo nnInfo in myNNInfoList) {
            nnInfoListString += "Fitness: " + nnInfo.fitness + "\n";
            nnInfoListString += "Weights1:\n";
            for (int i = 0; i < nnInfo.weights1.Length; i++) {
                nnInfoListString += string.Join(", ", nnInfo.weights1[i].Take(3)) + " ...\n";
            }
            nnInfoListString += "Biases1: " + string.Join(", ", nnInfo.biases1) + "\n";
            nnInfoListString += "Weights2:\n";
            for (int i = 0; i < nnInfo.weights2.Length; i++) {
                nnInfoListString += string.Join(", ", nnInfo.weights2[i]) + "\n";
            }
            nnInfoListString += "Biases2: " + string.Join(", ", nnInfo.biases2) + "\n";
            nnInfoListString += "\n";
        }

        Debug.Log(nnInfoListString);
    }
    
    public void addAgentInfo(NNinfo nnInfo) {
        population.Add(nnInfo);
    }

    void Start() {
        for (int i=0; i<populationSize; i++) {
            createAgent();
        }
    }

    void evolve() {
        List<NNinfo> sortedPopulation = population.OrderByDescending(individual => individual.fitness).ToList();
        List<NNinfo> breedingPool = sortedPopulation.Take(20).ToList();

        Debug.Log(breedingPool.Count + "breeding pool size");

        for (int i=0; i<(populationSize-1)/2; i++) {
            crossover(breedingPool[Random.Range(0, 20)], breedingPool[Random.Range(0, 20)]);
        }
        newPopulation.Add(breedingPool[0]); //keep the king of previous gen to prevent devolving
        Debug.Log(newPopulation.Count + "new generation size");
    }

    void crossover(NNinfo a, NNinfo b) {
        int crossPoint = Random.Range(0, a.weights1.Length-1);

        child1 = a;
        child2 = b;

        newPopulation.Add(mutate(a));
        newPopulation.Add(mutate(b));
    }

    NNinfo mutate(NNinfo agent) {
        if (Random.Range(0f, 1f) <= mutationChance) {
            return agent;
        }
        else {
            return agent;
        }
    }
    // Update is called once per frame
    void Update() {
        //TODO: create agents and run evolve when all have
        if (population.Count == populationSize) {
            viewAgents(population);
            evolve();
            population.Clear();
        }
    }
}