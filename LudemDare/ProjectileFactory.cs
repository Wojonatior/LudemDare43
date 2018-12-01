﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace LudemDare
{

    public class ProjectileFactory
    {
        private static Texture2D Texture;

        public ProjectileFactory(Texture2D texture)
        {
            Texture = texture;
        }

        private GameObject genericProjectile(Vector2 position){
            return new GameObject
            {
                position = position,
                velocity = new Vector2(0, -100),
                texture = Texture,
                update = updateProjectile,
                addItem = addFromProjectile
            };
        }

        public GameObject CreateUp(Vector2 position)
        {
            GameObject projectile = genericProjectile(position);
            projectile.velocity = new Vector2(0, -100);
            return projectile;
        }

        public GameObject CreateDown(Vector2 position)
        {
            GameObject projectile = genericProjectile(position);
            projectile.velocity = new Vector2(0, 100);
            return projectile;
        }

        public GameObject CreateRight(Vector2 position)
        {
            GameObject projectile = genericProjectile(position);
            projectile.velocity = new Vector2(100, 0);
            return projectile;
        }


        public GameObject CreateLeft(Vector2 position)
        {
            GameObject projectile = genericProjectile(position);
            projectile.velocity = new Vector2(-100, 0);
            return projectile;
        }

        private static GameObject updateProjectile(KeyboardState kState, GameTime gameTime, GameObject projectile)
        {
            projectile.position.X += projectile.velocity.X * (float)gameTime.ElapsedGameTime.TotalSeconds;
            projectile.position.Y += projectile.velocity.Y * (float)gameTime.ElapsedGameTime.TotalSeconds;
            return projectile;
        }

        private static GameObject? addFromProjectile(KeyboardState kState, GameTime gameTime, GameObject projectile)
        {
            return null;
        }
    }
}