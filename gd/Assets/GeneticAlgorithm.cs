using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GeneticAlgorithm : MonoBehaviour {
    public struct NNinfo {
        public float[][] weights1;
        public float[] biases1;
        public float[][] weights2;
        public float[] biases2;
        public float fitness;
    }

    float mutationStrength;
    float mutationChance;

    int populationSize = 100;
    List<NNinfo> population = new List<NNinfo>();
    List<NNinfo> newPopulation;

    public void addAgentInfo(NNinfo nnInfo) {
        population.Add(nnInfo);
    }

    void Start() {
        Debug.Log(population);
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
        //TODO: create agents and run evolve when all have died

    }
}