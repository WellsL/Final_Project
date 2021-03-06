﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using Final_Project.Enums;
using Final_Project.Exceptions;
using Final_Project.Exceptions.Parking;
using Final_Project.Exceptions.Trajet;
using Final_Project.Exceptions.Voiture;
using Final_Project.Exceptions.Voiture.Camion;
using Final_Project.Exceptions.Voiture.Moto;
using Final_Project.Parking;
using Final_Project.Vehicules;
using Newtonsoft.Json.Linq;

namespace Final_Project
{
    public class Controller
    {
        private GestionFlotte gestionFlotte;

        public Controller(GestionFlotte gestionFlotte)
        {
            this.gestionFlotte = gestionFlotte;
        }

        /* Public Methodes */


        /***** Ajouter *****/

        /**
         * Ajouter un client à la liste du gestionnaire de flotte
         * @Params nom             string       : Nom du client
         * @Params prénom          string       : Prénom du client
         * @Params adresse         string       : adresse du client
         * @Params srtPermisList   List<string> : list des permis du client
         */
        public void AjoutClient(string nom, string prenom, string adresse, string nbAnneePermis, List<string> srtPermisList)
        {
            if (string.IsNullOrWhiteSpace(nom))
                throw new NomVide();
            if (string.IsNullOrWhiteSpace(prenom))
                throw new PrenomVide();
            if (string.IsNullOrWhiteSpace(adresse))
                throw new AdresseVide();

            int nbAnnee = -1;

            if (!int.TryParse(nbAnneePermis, out nbAnnee) || nbAnnee < 0) 
                throw new NotImplementedException("ERREUR : Nombre d'année de permis invalide");
            
            List<EPermis> permisList = new List<EPermis>();
            
            srtPermisList.ForEach(x => StrToEPermis(x, permisList));

            gestionFlotte.ClientList.Add(new Client(nom, prenom, adresse, nbAnnee, permisList));

        }

        /**
         * Verifie la validité des donnée et ajout une Voiture à la liste de véhicule du gestionnaire de flotte
         * @Params couleur   string   : couleur du véhicule
         * @Params km        int      : km au compteur
         * @Params marque    string   : marque du véhicule
         * @Params modele    string   : modele du véhicule
         * @Params nbPortes  int      : nombre de portes
         * @Params puissance int      : puissance moteur (nombre de chevaux)
         */
        public void AjoutVoiture(string couleur, string km, string marque, string modele, string nbPortes, string puissance, string type)
        {
            int iKm = -1;
            int iNbPortes = -1;
            int iPuissances = -1;
            TypeVoiture typeV;
            CheckVehicule(couleur, km, marque, modele, out iKm);


            if (!TypeVoiture.TryParse(type, true, out typeV))
                throw new ErreurType();
            if (!int.TryParse(nbPortes, out iNbPortes) || iNbPortes < 3 || iNbPortes > 5)
                throw new ErreurNBPortes();
            if (!int.TryParse(puissance, out iPuissances) || iPuissances < 70 || iPuissances > 650)
                throw new ErreurPuissance();

            gestionFlotte.AjoutVehicule(new Voiture(marque, modele, iKm, couleur, iNbPortes, iPuissances, typeV));
        }

        /**
         * Verifie la validité des donnée et ajout un camion à la liste de véhicule du gestionnaire de flotte
         * @Params couleur   string   : couleur du véhicule
         * @Params km        int      : km au compteur
         * @Params marque    string   : marque du véhicule
         * @Params modele    string   : modele du véhicule
         * @Params capacite  int      : capacité en m3 du camion
         */
        public void AjoutCamion(string couleur, string km, string marque, string modele, string capacite)
        {
            int iKm = -1;
            int iCapacite = -1;

            CheckVehicule(couleur, km, marque, modele, out iKm);

            if (!int.TryParse(capacite, out iCapacite) || iCapacite < 2.75 || iCapacite > 22)
                throw new ErreurCapacite();

            gestionFlotte.AjoutVehicule(new Camion(marque, modele, iKm, couleur, iCapacite));
        }

        /**
         * Verifie la validité des donnée et ajout une moto à la liste de véhicule du gestionnaire de flotte
         * @Params couleur   string   : couleur du véhicule
         * @Params km        int      : km au compteur
         * @Params marque    string   : marque du véhicule
         * @Params modele    string   : modele du véhicule
         * @Params cylindre  int      : cylindré du véhicule en cm3
         */
        public void AjoutMoto(string couleur, string km, string marque, string modele, string cylindre)
        {
            int iKm = -1;
            int iCylindre = -1;

            CheckVehicule(couleur, km, marque, modele, out iKm);

            if (!int.TryParse(cylindre, out iCylindre) || iCylindre < 50 || iCylindre > 1500)
                throw new ErreurCylindre();

            gestionFlotte.AjoutVehicule(new Moto(marque, modele, iKm, couleur, iCylindre));
        }


        /**
         * Ajouter un trajet à la liste du gestionnaire de flotte
         * @Params strNClient    string : Numero du client
         * @Params strNVehicule  string : Numero du vehicule
         * @Params strDistance   string : distance en km
         */
        public void AjoutTrajet(string strNClient, string strNVehicule, string strDistance, string date)
        {
            int distance = -1;
            if (!int.TryParse(strDistance, out distance) || distance<=0)
                throw new ErreurDistance();

            Vehicule v = GetVehicule(strNVehicule);

            if(!v.IsDisponible)
                throw new NotImplementedException("ERREUR : Le vehicule n'est pas disponible");
            try
            {
                gestionFlotte.TrajetList.Add(new Trajet(GetClient(strNClient), v, distance, DateTime.Parse(date)));
            }
            catch (System.FormatException e)
            {
                throw new NotImplementedException("ERREUR : Le date saisie est invalide");
            }

            v.IsDisponible = false;
            v.NTrajet = Trajet.LastNTrajet;
        }

        /***** Fin Ajout *****/



        /***** Suppression *****/

        /**
         * LibPlace un client à la liste du gestionnaire de flotte
         * @Params nClient int : numéro du client à supprimer
         */
        public void SupClient(string strnClient)
        {
            gestionFlotte.ClientList.RemoveAll(client => client.NClient == CheckNClient(strnClient));
        }

        /**
         * LibPlace un véhicule à la liste du gestionnaire de flotte
         * @Params nVehicule int : numéro du véhicule à supprimer
         */
        public void SupVehicule(string strnVehicule)
        {
            int nVehicule = CheckNVehicule(strnVehicule);

            gestionFlotte.VehiculeList.Find(vehicule => vehicule.NVehicule == nVehicule).LibPlace();
            gestionFlotte.VehiculeList.RemoveAll(vehicule => vehicule.NVehicule == nVehicule);
        }

        /**
         * LibPlace un trajet à la liste du gestionnaire de flotte
         * @Params nTrajet int : numéro du trajet à supprimer
         */
        public void SupTrajet(string strnTrajet)
        {
            int nTrajet = CheckNTrajet(strnTrajet);

            gestionFlotte.TrajetList.Find(trajet => trajet.NTrajet == nTrajet).Supprimer();
            gestionFlotte.TrajetList.RemoveAll(trajet => trajet.NTrajet == nTrajet);
        }

        /***** Fin Suppression *****/

        /***** Affichage *****/

        /**
        * Afficher un client de la liste du gestionnaire de flotte
        * @Params nClient int : numéro du client à supprimer
        * @Returns : Client
        */
        public Client GetClient(string strnClient)
        {
            return gestionFlotte.ClientList.Find(client => client.NClient == CheckNClient(strnClient));
        }

        /**
         * Afficher un véhicule de la liste du gestionnaire de flotte
         * @Params nVehicule int : numéro du véhicule à supprimer
         * @Returns : Vehicule
         */
        public Vehicule GetVehicule(string strnVehicule)
        {
            return gestionFlotte.VehiculeList.Find(vehicule => vehicule.NVehicule == CheckNVehicule(strnVehicule));
        }

        /**
         * Afficher un trajet de la liste du gestionnaire de flotte
         * @Params nTrajet int : numéro du trajet à supprimer
         * @Returns : Trajet
         */
        public Trajet GetTrajet(string strnTrajet)
        {
            return gestionFlotte.TrajetList.Find(trajet => trajet.NTrajet == (CheckNTrajet(strnTrajet)));
        }

        /***** Fin Affichage *****/


        public Parking.Parking SelectParking(string nParking, List<Parking.Parking> list)
        {
            if (list == null)
                throw new NotImplementedException("Erreur Controller.SelectParking() : list null");

            Parking.Parking result = null;

            list.ForEach(parking =>
            {
                if (!parking.IsPlein && nParking.ToUpper().Equals(parking.Nom.ToUpper()))
                    result = parking;
            });

            if (result == null)
                throw new ErreurNomParking();
            return result;
        }

        public Parking.Place SelectPlace(string nPlace, List<Place> listPlace)
        {
            if (listPlace == null)
                throw new NotImplementedException("Erreur Controller.SelectPlace() : listPlace null");

            int i = 0;
            while (i < listPlace.Count && nPlace.ToUpper().Equals("A" + i))
            {
                i++;
            }

            if (i == listPlace.Count && !nPlace.ToUpper().Equals("A" + (i - 1)))
                throw new ErreurNomParking();
            if(i == listPlace.Count)
                return listPlace[i - 1];
            return listPlace[i];
        }

        public void RendreVehicule(string strNTrajet, Place place)
        {
            int nTrajet = CheckNTrajet(strNTrajet);
            Vehicule vehicule = gestionFlotte.TrajetList.Find(trajet => trajet.NTrajet == nTrajet).Vehicule;

            vehicule.Place = place;

            if(vehicule is Voiture)
                gestionFlotte.ControleurV.Verifier(vehicule);
            else if (vehicule is Moto)
                gestionFlotte.ControleurM.Verifier(vehicule);
            else
                gestionFlotte.ControleurC.Verifier(vehicule);
        }


        public bool ChargerVehicules(JToken jToken)

        {
            jToken = jToken.First;
            bool erreur = false;
            int lastNVehicule = 0;
            int i = 0;
            while (jToken != null)
            {
                try
                {
                    int nVehicule = int.Parse(jToken["nVehicule"].ToString());
                    string marque = jToken["marque"].ToString();
                    string modele = jToken["modele"].ToString();
                    int km = int.Parse(jToken["km"].ToString());
                    string couleur = jToken["couleur"].ToString();
                    bool isDisponible = bool.Parse(jToken["isDisponible"].ToString());
                    string parking = jToken["parking"].ToString();
                    string strPlace = jToken["place"].ToString();

                    Place place;
                    if (strPlace == "null")
                        place = null;
                    else
                        place = gestionFlotte.ParkingList.FindLast(p => p.Nom == parking).Places[int.Parse(strPlace.Substring(1))];

                    JToken subtJToken = jToken["interventionList"].First;
                    List<Intervention> interventionList = new List<Intervention>();
                    while (subtJToken != null)
                    {
                        Intervention interv;
                        Intervention.TryParse(subtJToken["intervention"].ToString(), out interv);
                        interventionList.Add(interv);
                        subtJToken = subtJToken.Next;
                    }
                    int nTrajet = int.Parse(jToken["nTrajet"].ToString());
                    string vehiculeT = jToken["vehicule"].ToString();
                    if (vehiculeT.ToUpper() == "VOITURE")
                    {
                        int nbPortes = int.Parse(jToken["nbPortes"].ToString());
                        int puissance = int.Parse(jToken["puissance"].ToString());
                        TypeVoiture type;
                        TypeVoiture.TryParse(jToken["type"].ToString(), out type);
                        gestionFlotte.VehiculeList.Add(new Voiture(nVehicule, marque, modele, km, couleur, isDisponible, place, interventionList, nTrajet, nbPortes, puissance, type));
                    }
                    else if (vehiculeT.ToUpper() == "MOTO")
                    {
                        int cylindre = int.Parse(jToken["cylindre"].ToString());
                        gestionFlotte.VehiculeList.Add(new Moto(nVehicule, marque, modele, km, couleur, isDisponible, place, interventionList, nTrajet, cylindre));
                    }
                    else if (vehiculeT.ToUpper() == "CAMION")
                    {
                        int capacite = int.Parse(jToken["capacite"].ToString());
                        gestionFlotte.VehiculeList.Add(new Moto(nVehicule, marque, modele, km, couleur, isDisponible, place, interventionList, nTrajet, capacite));
                    }

                    if (lastNVehicule < nVehicule)
                        lastNVehicule = nVehicule;
                }
                catch (Exception e)
                {
                    erreur = true;
                }

                jToken = jToken.Next;
            }

            Vehicule.LastNVehicule = lastNVehicule;
            return erreur;
        }

        public bool ChargerClients(JToken jToken)
        {
            jToken = jToken.First;
            bool erreur = false;
            int lastNClient = 0;
            int i = 0;
            while (jToken != null)
            {
                try
                {
                    int nClient = int.Parse(jToken["nClient"].ToString());
                    string nom = jToken["nom"].ToString();
                    string prenom = jToken["prenom"].ToString();
                    string adresse = jToken["adresse"].ToString();
                    int nbAnneePermis = int.Parse(jToken["nbAnneePermis"].ToString());
                    int totalLoc = int.Parse(jToken["totalLoc"].ToString());

                    JToken subtJToken = jToken["permisList"].First;
                    List<EPermis> permisList = new List<EPermis>();
                    while (subtJToken != null)
                    {
                        EPermis permis;
                        EPermis.TryParse(subtJToken["permis"].ToString(), out permis);
                        permisList.Add(permis);
                        subtJToken = subtJToken.Next;
                    }

                    gestionFlotte.ClientList.Add(new Client(nClient, nom, prenom, adresse, nbAnneePermis, totalLoc, permisList));

                    if (lastNClient < nClient)
                        lastNClient = nClient;
                }
                catch (Exception e)
                {
                    erreur = true;
                }

                jToken = jToken.Next;
            }

            Client.LastNClient = lastNClient;
            return erreur;
        }


        public bool ChargerTrajets(JToken jToken)
        {
            jToken = jToken.First;
            bool erreur = false;
            int lastNTrajet = 0;
            int i = 0;
            while (jToken != null)
            {
                try
                {
                    int nTrajet = int.Parse(jToken["nTrajet"].ToString());
                    DateTime date = DateTime.Parse(jToken["date"].ToString());
                    int distance = int.Parse(jToken["distance"].ToString());

                    int nClient = int.Parse(jToken["NClient"].ToString());
                    Client client = gestionFlotte.ClientList.FindLast(c => c.NClient == nClient);

                    int nVehicule = int.Parse(jToken["NVehicule"].ToString());
                    Vehicule vehicule = gestionFlotte.VehiculeList.FindLast(v => v.NVehicule == nVehicule);

                    gestionFlotte.TrajetList.Add(new Trajet(nTrajet, client, vehicule, date, distance));

                    if (lastNTrajet < nClient)
                        lastNTrajet = nClient;
                }
                catch (Exception e)
                {
                    erreur = true;
                }

                jToken = jToken.Next;
            }

            Trajet.LastNTrajet = lastNTrajet;
            return erreur;
        }


        /* Private Methodes */


        /**
         * Verifie les attribues commun au types de véhicules
         * @Params couleur   string   : couleur du véhicule
         * @Params km        int      : km au compteur
         * @Params marque    string   : marque du véhicule
         * @Params modele    string   : modele du véhicule
         */
        private void CheckVehicule(string couleur, string km, string marque, string modele, out int iKm)
        {

            if (string.IsNullOrWhiteSpace(couleur))
                throw new ErreurCouleur();
            if (!int.TryParse(km, out iKm) || iKm < 0)
                throw new ErreurKm();
            if (string.IsNullOrWhiteSpace(marque))
                throw new ErreurMarque();
            if (string.IsNullOrWhiteSpace(modele))
                throw new ErreurModele();
        }

        /**
         * Convertie un string en strPermis et l'ajout à une list de strPermis
         * @Params  strPermi    string : string à convertir
         * @Params  listPermis      : List<EPermis>  list de strPermis à remplir
         */
        private void StrToEPermis(string strPermis, List<EPermis> permisList)
        {
            EPermis permis;
            if (!EPermis.TryParse(strPermis, true, out permis))
                throw new ErreurPermis();

            permisList.Add(permis);
        }

        /**
         * Convertie un string en int et vérifie que le numéro du véhicule est correct
         * @Params  strNVehicule    string : string à convertir
         * * @Returns   int : numero du véhicule
         */
        private int CheckNVehicule(string strNVehicule)
        {
            int nVehicule = -1;
            if (!int.TryParse(strNVehicule, out nVehicule) ||
                !gestionFlotte.VehiculeList.Exists(vehicule => vehicule.NVehicule == nVehicule)) 
                throw new ErreurNVehicule();
            return nVehicule;
        }

        /**
         * Convertie un string en int et vérifie que le numéro du trajet est correct
         * @Params  strNTrajet    string : string à convertir
         * * @Returns   int : numero du trajet
         */
        private int CheckNTrajet(string strNTrajet)
        {
            int nTrajet = -1;
            if (!int.TryParse(strNTrajet, out nTrajet) ||
                !gestionFlotte.TrajetList.Exists(trajet => trajet.NTrajet == nTrajet))
                throw new ErreurNTrajet();
            return nTrajet;
        }

        /**
         * Convertie un string en int et vérifie que le numéro du client est correct
         * @Params  strNClient    string : string à convertir
         * * @Returns   int : numero du client
         */
        private int CheckNClient(string strNClient)
        {
            int nClient = -1;
            if (!int.TryParse(strNClient, out nClient) ||
                !gestionFlotte.ClientList.Exists(client => client.NClient == nClient))
                throw new ErreurNClient();
            return nClient;
        }
    }
}