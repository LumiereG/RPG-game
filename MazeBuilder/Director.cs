﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game2
{
    public class Director
    {
        private IMazeBuilder _builder;

        public IMazeBuilder Builder
        {
            set { _builder = value; }
        }
        public object BuildBasicMaze()
        {
            return _builder
                .CreateFilledMaze(Constants.MapWidth, Constants.MapHeight)
                .GeneratePaths();
        }

        public object BuildBasikMazeWithItems()
        {
            return _builder
                .CreateFilledMaze(Constants.MapWidth, Constants.MapHeight)
                .GeneratePaths()
                .AddItems(30)
                .AddWeapons();
        }

        public object BuildMazeWithCentralRoom()
        {
            return _builder
                .CreateFilledMaze(Constants.MapWidth, Constants.MapHeight)
                .GeneratePaths()
                .AddCentralRoom()
                .AddItems(30)
                .AddWeapons()
                .AddModifiedWeapons();
        }

        public object BuildMazeWithEnemies()
        {
            return _builder
                .CreateFilledMaze(Constants.MapWidth, Constants.MapHeight)
                .GeneratePaths()
                .AddRooms()
                .AddCentralRoom()
                .AddItems(40)
                .AddWeapons()
                .AddModifiedWeapons()
                .AddElixirs()
                .AddEnemies();
        }

        public object BuildEmptyMaze()
        {
            return _builder
                .CreateEmptyMaze(Constants.MapWidth, Constants.MapHeight)
                .AddItems(30)
                .AddWeapons();
        }

    }
}
