﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Final_Project.Enums;
using Final_Project.Parking;

namespace Final_Project.Vehicules
{
    public sealed class Voiture : Vehicule
    {
        private readonly int nbPortes;
        private readonly int puissance;
        private readonly TypeVoiture type;

        /**
        * Constructeur pour vehicule d'occasion
        */

        public Voiture(string marque, string modele, int km, string couleur, int nbPortes, int puissance, TypeVoiture type) : base(marque, modele, km, couleur)
        {
            this.nbPortes = nbPortes;
            this.puissance = puissance;
            this.type = type;
            CalculerCout();
        }

        public Voiture(int nVehicule, string marque, string modele, int km, string couleur, bool isDisponible,
            Place place, List<Intervention> interventionList, int nTrajet, int nbPortes, int puissance, TypeVoiture type) : base(
            nVehicule, marque, modele, km, couleur, isDisponible, place, interventionList, nTrajet)
        {
            this.nbPortes = nbPortes;
            this.puissance = puissance;
            this.type = type;
            CalculerCout();
        }

        /* Properties */

        public int NbPortes => nbPortes;

        public int Puissance => puissance;

        public TypeVoiture Type => type;

        protected override void CalculerCout()
        {
            if (type == TypeVoiture.Break)
                Cout = Puissance;
            else if (type == TypeVoiture.Berline)
                Cout = 1.5 * Puissance;
            else if (type == TypeVoiture.Monospace)
                Cout = 1.25 * Puissance;
        }
        public override string ToString()
        {
            return base.ToString() + $" NOMBRE DE PORTES : {nbPortes}\n PUISSANCE : {puissance}";
        }

        public override void Sauvegarder(StreamWriter fWriter, string before = "", string after = "")
        {
            base.Sauvegarder(fWriter, before);
            fWriter.WriteLine($"{before}\t\"vehicule\" : \"voiture\",");
            fWriter.WriteLine($"{before}\t\"nbPortes\" : \"{nbPortes}\",");
            fWriter.WriteLine($"{before}\t\"puissance\" : \"{puissance}\",");
            fWriter.WriteLine($"{before}\t\"type\" : \"{type}\"");
            fWriter.WriteLine(before+"}"+after);
        }
    }
}