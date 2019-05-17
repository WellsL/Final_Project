﻿using Final_Project.Parking;
using Final_Project.Vehicules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Final_Project.Exceptions.Parking;

namespace Final_Project
{
    public class GestionFlotte
    {
        private readonly List<Vehicule> vehiculeList;
        private readonly List<Client> clientList;
        private readonly List<Trajet> trajetList;
        private readonly List<Parking.Parking> parkingList;



        public GestionFlotte()
        {
            vehiculeList = new List<Vehicule>();
            clientList = new List<Client>();
            trajetList = new List<Trajet>();
            parkingList = new List<Parking.Parking>();
        }


        /* Properties */

        public int LastNumVehicule => vehiculeList.Count - 1;
        public int LastNumClient => clientList.Count - 1;
        public int LastNumTrajet => trajetList.Count - 1;

        public List<Vehicule> VehiculeList => vehiculeList;
        public List<Client> ClientList => clientList;
        public List<Trajet> TrajetList => trajetList;

        public List<Parking.Parking> ParkingList => parkingList;


        /* Public Methodes */

        /**
         * recupérer un vehicule de la liste de véhicules du gestionnaire de flotte
         * @Params  nVehicule   int     : Numéro du vehicule à récupérer
         * @Returns Vehicule : Véhicule recherché
         */
        public Vehicule GetVehicule(int nVehicule)
        {
            return vehiculeList[nVehicule];
        }

        /**
         * recupérer un trajet de la liste de trajets
         * @Params  nTrajet   int     : Numéro du trajet à récupérer
         * @Returns Trajet  : Trajet recherché
         */
        public Trajet GetTrajet(int nTrajet)
        {
            return trajetList[nTrajet];
        }

        /**
         * recupérer un client de la liste de clients
         * @Params  nClient   int     : Numéro du client à récupérer
         * @Returns Client  : Client recherché
         */
        public Client GetClient(int nClient)
        {
            return clientList[nClient];
        }

        /**
         * Ajouter un vehicule à la liste de véhicule du gestionnaire de flotte
         * @Params vehicule   Vehicule : le vehicule à ajouter
         */
        public void AjoutVehicule(Vehicule vehicule)
        {
            vehicule.NVehicule = LastNumVehicule + 1;
            vehicule.Place = GetPlaceDisp();
            vehiculeList.Add(vehicule);
        }

        /**
         * Ajouter un client à la liste de client
         * @Params client   Client : le client à ajouter
         */
        public void AjoutClient(Client client)
        {
            client.NClient = LastNumClient + 1;
            clientList.Add(client);
        }

        /**
         * Ajouter un trajet à la liste de client
         * @Params trajet   Trajet : le trajet à ajouter
         */
        public void AjoutTrajet(Trajet trajet)
        {
            trajet.NTrajet = LastNumTrajet + 1;
            trajetList.Add(trajet);
        }


        /**
         * Supprime un vehicule de la liste de véhicule du gestionnaire de flotte
         * @Params nVehicule   int : numero du vehicule à supprimer
         */
        public void SupVehicule(int nVehicule)
        {
            vehiculeList[nVehicule].Supprimer();

            vehiculeList.RemoveAt(nVehicule);

            for (int i = nVehicule; i < vehiculeList.Count; i++)
                vehiculeList[i].NVehicule = i;
        }

        /**
         * Supprimer un client à la liste du gestionnaire de flotte
         * @Params nClient int : numéro du client à supprimer
         */
        public void SupClient(int nClient)
        {
            clientList[nClient].NClient = -1;

            clientList.RemoveAt(nClient);

            for (int i = nClient; i < clientList.Count; i++)
                clientList[i].NClient = i;
        }

        /**
         * Supprimer un trajet à la liste du gestionnaire de flotte
         * @Params nTrajet int : numéro du trajet à supprimer
         */
        public void SupTrajet(int nTrajet)
        {
            trajetList[nTrajet].Supprimer();

            trajetList.RemoveAt(nTrajet);

            for (int i = nTrajet; i < trajetList.Count; i++)
                trajetList[i].NTrajet = i;
        }

        public List<Parking.Parking> GetParkingsDisp()
        {
            List<Parking.Parking> list = new List<Parking.Parking>();

            parkingList.ForEach(parking =>
            {
                if (!parking.IsPlein)
                    list.Add(parking);
            });

            return list;
        }



        /* Private Methodes */

        private Place GetPlaceDisp()
        {
            int i = 0;
            while (i < parkingList.Count && parkingList[i].IsPlein) { i++; }

            if (i==parkingList.Count && parkingList[i-1].IsPlein)
                throw new ErreurPlaceIndisp();

            return i == parkingList.Count ? parkingList[i - 1].GetPlaceDisp() : parkingList[i].GetPlaceDisp();
        }


    }
}