using System;
using System.Collections.Generic;
using System.Linq;
using VoyageurDeCommerce.modele.distances;
using VoyageurDeCommerce.modele.lieux;

namespace VoyageurDeCommerce.modele.algorithmes.realisations
{
    /// <summary>
    /// Classe de l'algorithme Dijkstra
    /// </summary>
    public class AlgorithmeDijkstra : Algorithme
    {
        private Dictionary<Lieu, double> distances;
        private Dictionary<Lieu, bool> estVisite;
        private Dictionary<Lieu, Lieu> predecesseur;
        
        /// <summary>
        /// Propriété donnant le nom de l'algorithme
        /// </summary>
        public override string Nom => "Dijsktra";

        /// <summary>
        /// Constructeur de Dijkstra
        /// </summary>
        public AlgorithmeDijkstra()
        {
            distances = new Dictionary<Lieu, double>();
            estVisite = new Dictionary<Lieu, bool>();
            predecesseur = new Dictionary<Lieu, Lieu>(); 
        }
        
        /// <summary>
        /// Méthode de l'initialisation de l'algorithme
        /// </summary>
        /// <param name="listeLieux">La Liste des lieux a visité</param>
        private void Initialisation(List<Lieu> listeLieux)
        {
            foreach(Lieu l in listeLieux)
            {
                distances[l] = -1;
                estVisite[l] = false;
                predecesseur[l] = null;
            }
            distances[listeLieux[0]] = 0;
        }
        
        /// <summary>
        /// Méthode qui permet de mettre a jour les distances et les predecesseurs
        /// </summary>
        /// <param name="a">Lieu de départ</param>
        /// <param name="b">Lieu d'arrivée</param>
        private void Relachement(Lieu a, Lieu b)
        {
            if (distances[a] > (distances[b] + FloydWarshall.Distance(a, b)))
            {
                distances[b] = distances[a] + FloydWarshall.Distance(a, b);
                predecesseur[b] = a;
            }
        }

        /// <summary>
        /// Méthode donnant les voisins d'un lieu
        /// </summary>
        /// <param name="lieu">le lieu de départ</param>
        /// <param name="route">la liste de toutes les routes</param>
        /// <returns>La liste de tous les voisins de notre lieu</returns>
        private List<Lieu> RechercheVoisin(Lieu lieu, List<Route> route)
        {
            List<Lieu> voisins = new List<Lieu>();
            foreach (Route r in route)
            {
                if (lieu.Nom == r.Depart.Nom)
                {
                    voisins.Add(r.Arrivee);
                }
            }
            return voisins;
        }
        
        /// <summary>
        /// Méthode qui donne la distance minimaledes lieux
        /// </summary>
        /// <param name="listeLieu"></param>
        /// <returns></returns>
        private Lieu Minimum(List<Lieu> listeLieu)
        {
            Lieu rep = listeLieu[0]; 
            double min = distances[listeLieu[0]];
            foreach (Lieu lieu in listeLieu)
            {
                if (distances[lieu] < min && !estVisite[lieu])
                {
                    min = distances[lieu];
                    rep = lieu;
                }
            }
            return rep;
        }
        
        /// <summary>
        /// Méthode executant l'algorithme
        /// </summary>
        /// <param name="listeLieux">La Liste des Lieux</param>
        /// <param name="listeRoute">La liste des routes</param>
        public override void Executer(List<Lieu> listeLieux, List<Route> listeRoute)
        {
            //Initialisation
            FloydWarshall.calculerDistances(listeLieux, listeRoute);
            Initialisation(listeLieux);
            Lieu a = null;
            List<Lieu> voisins = null;
            List<Lieu> keys = new List<Lieu>(distances.Keys);
            
            
            //Calcul
            while (estVisite.ContainsValue(false))
            {
                a = Minimum(keys);
                estVisite[a] = true;
                voisins = RechercheVoisin(a, listeRoute);
                foreach (Lieu b in voisins)
                {
                    Relachement(a, b);
                }
            }
            this.NotifyPropertyChanged("Tournee") ;
        }
    }
}