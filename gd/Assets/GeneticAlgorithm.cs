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

    int populationSize = 19; //should be odd
    List<NNinfo> population = new List<NNinfo>();
    List<NNinfo> newPopulation = new List<NNinfo>();


    void createAgent(NNinfo param=default) {
        Vector3 spawnPosition = main_agent.transform.position;
        spawnPosition.x += Random.Range(-.3f, .3f);
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
        Application.runInBackground = true;
    }

    void evolve() {
        List<NNinfo> sortedPopulation = population.OrderByDescending(individual => individual.fitness).ToList();
        List<NNinfo> breedingPool = sortedPopulation.Take(60).ToList();
        //redundant 


        NNinfo champion = breedingPool[0];
        newPopulation.Add(champion); //keep the king of previous gen to prevent devolving
        for (int i = 0; i < populationSize - 1; i++) {
            newPopulation.Add(codeNoodles(population[i]));
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
        return Random.Range(-1f, 1f) / mutationStrength;
    }

    NNinfo codeNoodles(NNinfo agentInfo) {
        //holy shit
        NNinfo newInfo = new NNinfo();
        newInfo.weights1 = new float[][] { new float[] { 1, 2 }, new float[] { 2, 2 } };

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
            if (yes())
                newInfo.biases1[n] += baguette();
        }
        for (int n = 0; n < agentInfo.biases2.Length; n++) {
            if (yes())
                newInfo.biases2[n] += baguette();
        }

        return newInfo;
    }

    void crossover(NNinfo a, NNinfo b) {
        int length = a.weights1.Length;
        int crossPoint = Random.Range(0, length);

        child1 = a;
        child2 = b;
        float[][] c1neurons = child1.weights1;
        float[][] c2neurons = child2.weights1;

        for (int i=0; i<length; i++) {
            if (i < crossPoint) {
                c1neurons[i] = child1.weights1[i];
                c2neurons[i] = child2.weights1[i];
            } else {
                c1neurons[i] = child2.weights1[i];
                c2neurons[i] = child1.weights1[i];
            }
        }

        child1.weights1 = c1neurons;
        child2.weights1 = c2neurons;

        newPopulation.Add(mutate(child1));
        newPopulation.Add(mutate(child2));
    }
    NNinfo mutate(NNinfo agent) {
        if (Random.Range(0f, 1f) <= mutationChance) {
            if (Random.Range(0f, 1f) <= 0.5) { //weights2
                agent.weights2[Random.Range(0, agent.weights2.Length)] [Random.Range(0, 3)] += Random.Range(-1f, 1f) / mutationStrength;
            }
            else { //weights1
                agent.weights1[Random.Range(0, agent.weights1.Length)] [Random.Range(0, 2)] += Random.Range(-1f, 1f)/mutationStrength;
            }

            if (Random.Range(0f, 1f) <= 0.5) { //biases2
                agent.biases2[Random.Range(0, agent.biases2.Length)] += Random.Range(-1f, 1f) / mutationStrength;
            } else { //biases1
                agent.biases1[Random.Range(0, agent.biases1.Length)] += Random.Range(-1f, 1f) / mutationStrength;
            }

            return agent;
        }

        else { //dont mutate
            return agent;
        }
    }


    void Update() {
        //TODO: create agents and run evolve when all have
        if (population.Count == populationSize) {
            viewAgents(population);
            evolve();
        }
    }
}