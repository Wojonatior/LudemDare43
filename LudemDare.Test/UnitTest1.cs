using Microsoft.Xna.Framework;
using System;
using Xunit;
using LudemDare.Desktop;

namespace LudemDare.Test
{
    public class ProjectileFactoryTest
    {
        [Fact]
        public void CreateUp ()
        {
            var projectile = ProjectileFactory.CreateUp(new Vector2(10, 15));
            Assert(projectile.position.X = 10);
            Assert(projectile.position.X = 15);
        }
    }
}
