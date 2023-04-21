using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GeneticAlgorithm : MonoBehaviour {
    public GameObject main_agent;
    public struct NNinfo {
        public float[][] weights1;
        public float[] biases1;
        public float[][] weights2;
        public float[] biases2;
        public float fitness;
    }

    float mutationStrength;
    float mutationChance;

    int populationSize = 2;
    List<NNinfo> population = new List<NNinfo>();
    List<NNinfo> newPopulation;

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
            Vector3 spawnPosition = main_agent.transform.position;
            spawnPosition.x += Random.Range(-3, 3); 
            Instantiate(main_agent, spawnPosition, Quaternion.identity);
        }
    }

    void evolve() {
        List<NNinfo> sortedPopulation = population.OrderByDescending(individual => individual.fitness).ToList();
        List<NNinfo> breedingPool = sortedPopulation.Take(20).ToList();
        
        for (int i=0; i<populationSize-1; i++) {
            crossover(breedingPool[Random.Range(0, 19)], breedingPool[Random.Range(0, 19)]);
        }
        newPopulation.Add(breedingPool[0]); //keep the king of previous gen to prevent devolving

    }

    NNinfo child1;
    NNinfo child2;
    void crossover(NNinfo a, NNinfo b) {

        int crossPoint = Random.Range(0, a.weights1.Length-1);

        newPopulation.Add(child1);
        newPopulation.Add(child2);
    }
    // Update is called once per frame
    void Update() {
        //TODO: create agents and run evolve when all have
        if (population.Count == populationSize) {
            viewAgents(population);
        }
    }
}