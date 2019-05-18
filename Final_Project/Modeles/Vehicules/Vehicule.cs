﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Final_Project.Enums;
using Final_Project.Parking;

namespace Final_Project.Vehicules
{
    public abstract class Vehicule
    {

        private readonly int nVehicule;
        private readonly string marque;
        private readonly string modele;
        private int km;
        private string couleur;
        private bool isDisponible;
        private double cout;
        private Place place;
        private List<Intervention> interventionList;
        private int nTrajet;

        private static int nbVehicule = 0;


        /**
         * Constructeur pour vehicule d'occasion
         */

        protected Vehicule(string marque, string modele, int km, string couleur)
        {
            this.nVehicule = ++nbVehicule;
            this.marque = marque;
            this.modele = modele;
            this.km = km;
            this.couleur = couleur;
            this.isDisponible = true;
            this.place = null;
            nTrajet = -1;
            interventionList = new List<Intervention>();
            CalculerCout();
        }

        
        /* Properties */

        public string Marque => marque;
        public string Modele => modele;
 
        public int Km { get => km; set => km = value; }
        public string Couleur { get => couleur; set => couleur = value; }
        public bool IsDisponible { get => isDisponible; set => isDisponible = value; }
        public int NVehicule => nVehicule;
        public double Cout { get => cout; set => cout = value; }

        public Place Place
        {
            get => place;
            set
            {
                place = value;
                place.Vehicule = this;
            }
        }

        public static int NbVehicule { get => nbVehicule; set => nbVehicule = value; }
        public int NTrajet { get => nTrajet; set => nTrajet = value; }

        /* Public Methodes */

        public void Supprimer()
        {
            place.Vehicule = null;
            place = null;
        }

        public void AddIntervention(Intervention intervention)
        {
            interventionList.Add(intervention);
        }

        public void DelLastIntervantion()
        {
            interventionList.RemoveAt(interventionList.Count-1);
        }

        /* Protected Methodes */

        protected abstract void CalculerCout();
    }
}