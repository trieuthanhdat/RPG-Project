using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Core
{
    public class GameStateManager
    {
        private static GameStateManager _instance;
        public delegate void GameStateChangeHandler(GameState newState);
        public event GameStateChangeHandler OnGameStateChanged;
        public GameState CurrentGameState{get; private set;}
        public void SetState(GameState newState)
        {
            if(newState == CurrentGameState) return;

            CurrentGameState = newState;
            OnGameStateChanged?.Invoke(newState);
        }
        
        public static GameStateManager Instance
        {
            get
            {
                if(_instance == null)
                    _instance = new GameStateManager();
                return _instance;
            }
        }

        private GameStateManager(){}

    }

}
