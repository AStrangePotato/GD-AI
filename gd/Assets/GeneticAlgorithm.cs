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

    float mutationStrength = 0.2f;
    float mutationChance = 0.5f;

    int populationSize = 15; //should be odd
    List<NNinfo> population = new List<NNinfo>();
    List<NNinfo> newPopulation = new List<NNinfo>();


    void createAgent(NNinfo param=default) {
        Vector3 spawnPosition = main_agent.transform.position;
        spawnPosition.x += Random.Range(-.5f, .5f);
        if (EqualityComparer<NNinfo>.Default.Equals(param, default(NNinfo))) {
            Instantiate(main_agent, spawnPosition, Quaternion.identity);
        }
        else {
            GameObject agent = Instantiate(main_agent, spawnPosition, Quaternion.identity);
            Agent agentScript = agent.GetComponentInChildren<Agent>();
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
                nnInfoListString += string.Join(", ", nnInfo.weights1[i]) + "\n";
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
        Application.runInBackground = true;
    }

    //chatgpt
    private int RandTriangular() {
        int n = populationSize-1;
        double u = Random.value;
        double v = Random.value;
        double x = Mathf.Min((float)u, (float)v) * n;
        if (u > v)
            x = n - x;
        return (int)x;
    }

    void evolve() {
        List<NNinfo> sortedPopulation = population.OrderByDescending(individual => individual.fitness).ToList();

        NNinfo champion = sortedPopulation[0];
        newPopulation.Add(champion); //keep the king of previous gen to prevent devolving
        for (int i = 0; i < populationSize - 1; i++) {
            newPopulation.Add(codeNoodles(sortedPopulation[RandTriangular()]));
        }

        foreach (NNinfo nninfo in newPopulation) {
            createAgent(nninfo);
        }
        population.Clear();
        newPopulation.Clear();
    }

    bool yes() {
        return Random.Range(0f, 1f) < mutationChance;
    }

    float baguette() {
        return Random.Range(-1f, 1f) * mutationStrength;
    }

    NNinfo codeNoodles(NNinfo agentInfo) {
        //holy shit
        NNinfo newInfo = agentInfo;

        for (int n=0; n<agentInfo.weights1.Length; n++) {
            for (int w=0; w<agentInfo.weights1[0].Length; w++) {
                if (yes())
                    newInfo.weights1[n][w] += baguette();
            }
        }
        for (int n = 0; n < agentInfo.weights2.Length; n++) {
            for (int w = 0; w < agentInfo.weights2[0].Length; w++) {
                if (yes())
                    newInfo.weights2[n][w] += baguette();
            }
        }
        for (int n = 0; n < agentInfo.biases1.Length; n++) {
            if (yes()) {
                newInfo.biases1[n] += baguette();
                newInfo.biases1[n] = Mathf.Clamp(newInfo.biases1[n], -10, 10);
            }
        }
        for (int n = 0; n < agentInfo.biases2.Length; n++) {
            if (yes()) {
                newInfo.biases2[n] += baguette();
                newInfo.biases2[n] = Mathf.Clamp(newInfo.biases2[n], -10, 10);
            }
        }
        return newInfo;
    }

    void Update() {
        //TODO: create agents and run evolve when all have
        if (population.Count == populationSize) {
            //viewAgents(population.OrderByDescending(individual => individual.fitness).ToList());
            evolve();
        }
    }
}