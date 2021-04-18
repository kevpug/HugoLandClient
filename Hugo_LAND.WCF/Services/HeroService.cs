﻿using Hugo_LAND.Core.Models;

using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hugo_LAND.WCF.DTOs;

namespace Hugo_LAND.WCF.Services
{
    public partial class HugoLandService : IHeroService
    {
        private readonly Random _random = new Random();
        public async void CreeHero(string newNomHero, bool newConnection, int idClasse, int idCompteJoueur,
              int idMonde)
        {
            using (var context = new HugoLANDContext())
            {
                Monde monde = context.Mondes.Find(idMonde);
                Classe classe = context.Classes.Find(idClasse);
                CompteJoueur compteJoueur = context.CompteJoueurs.Find(idCompteJoueur);
                var hero = new Hero
                {
                    Niveau = 1,
                    Experience = 0,
                    x = 0,
                    y = 0,
                    StatStr = classe.StatBaseStr + _random.Next(0, 10),
                    StatDex = classe.StatBaseDex + _random.Next(0, 10),
                    StatInt = classe.StatBaseInt + _random.Next(0, 10),
                    StatVitalite = classe.StatBaseVitalite + _random.Next(0, 10),
                    NomHero = newNomHero,
                    EstConnecte = newConnection,
                    Classe = classe,
                    CompteJoueur = compteJoueur,
                    Monde = monde
                };
                context.Entry(hero).State = EntityState.Added;
                await context.SaveChangesAsync();

            }
        }


        public async void SupprimeHero(HeroDetailsDTO dto)
        {
            using (var context = new HugoLANDContext())
            {
                var hero = new Hero()
                {
                    Id = dto.Id,
                    NomHero = dto.NomHero
                };
                context.Entry(hero).State = EntityState.Deleted;
                await context.SaveChangesAsync();
            }
        }
        public async void DeplaceHero(HeroDetailsDTO dto, int newX, int newY)
        {
            using (var context = new HugoLANDContext())
            {
                //Hero hero = context.Heros.Find(dto.Id);
                //hero.x = newX;
                //hero.y = newY;
                //context.SaveChanges();

                var hero = new Hero()
                {
                    Id = dto.Id,
                    x = newX,
                    y = newY
                };
                context.Entry(hero).State = EntityState.Modified;
                await context.SaveChangesAsync();


            }
        }
        public List<HeroDetailsDTO> RetourneHerosCompte(int idCompteJoueur)
        {
            using (var context = new HugoLANDContext())
            {
                return context.CompteJoueurs
                .Where(p => p.Id == idCompteJoueur)
                .First().Heros
                .Select(m => new HeroDetailsDTO
                {
                    Id = m.Id,
                    Niveau = m.Niveau,
                    Experience = m.Experience,
                    x = m.x,
                    y = m.y,
                    StatStr = m.StatStr,
                    StatDex = m.StatDex,
                    StatInt = m.StatInt,
                    StatVitalite = m.StatVitalite,
                    NomHero = m.NomHero,
                    EstConnecte = m.EstConnecte
                }).ToList();
            }
        }

    }
}
