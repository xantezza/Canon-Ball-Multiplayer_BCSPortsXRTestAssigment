using System;
using System.Collections.Generic;
using System.Linq;
using Infrastructure.Providers.PlayerDataProvider;
using Infrastructure.Services;
using Mirror;
using UnityEngine;
using Zenject;

namespace MirrorRoom
{
    public class NetworkRoomPlayerExt : NetworkRoomPlayer
    {
        private static List<ColorEnum> SelectedColors = new();

        [SyncVar(hook = nameof(UpdatePlayerData))]
        private PlayerData _playerData;

        private IPlayerDataProvider _playerDataProvider;

        [Inject]
        private void Inject(IPlayerDataProvider playerDataProvider)
        {
            _playerDataProvider = playerDataProvider;
        }

        public override void Start()
        {
            base.Start();
            CmdInit();
        }

        public override void OnDisable()
        {
            base.OnDisable();
            if (SelectedColors.Contains(_playerData.PlayerColorEnum)) SelectedColors.Remove(_playerData.PlayerColorEnum);
        }

        public override void OnGUI()
        {
            base.OnGUI();

            if (!isLocalPlayer) return;

            var colors = Enum.GetValues(typeof(ColorEnum)).Cast<ColorEnum>().Except(SelectedColors).ToList();

            GUILayout.BeginArea(new Rect(20f + (_index * 100), 400, 180f, colors.Count() * 130f));

            if (_playerData != null && _playerData.PlayerColorEnum == ColorEnum.None)
            {
                foreach (var color in colors)
                {
                    if (color != ColorEnum.None && GUILayout.Button(color.ToString()))
                        SelectColor(color);
                }
            }

            GUILayout.EndArea();
        }

        protected override void DrawColor()
        {
            if (_playerData != null) GUILayout.Label(_playerData.PlayerColorEnum.ToString());
        }

        protected override void DrawPlayerReadyButton()
        {
            if (NetworkClient.active && isLocalPlayer)
            {
                GUILayout.BeginArea(new Rect(20f, 300f, 120f, 20f));

                if (readyToBegin)
                {
                    if (GUILayout.Button("Cancel"))
                        CmdChangeReadyState(false);
                }

                if (_playerData != null && _playerData.PlayerColorEnum != ColorEnum.None)
                {
                    if (GUILayout.Button("Ready"))
                        CmdChangeReadyState(true);
                }

                GUILayout.EndArea();
            }
        }

        private void UpdatePlayerData(PlayerData _, PlayerData playerData)
        {
            _playerDataProvider.PlayerData = playerData;
        }

        private void SelectColor(ColorEnum colorEnum)
        {
            CmdSelectColor(colorEnum);
        }

        public override void IndexChanged(int oldIndex, int newIndex)
        {
            base.IndexChanged(oldIndex, newIndex);
            CmdInit();
        }

        [Command]
        private void CmdSelectColor(ColorEnum colorEnum)
        {
            SelectedColors.Add(colorEnum);
            _playerData.PlayerColorEnum = colorEnum;
            _playerData.PlayerColor = colorEnum switch
            {
                ColorEnum.White => Color.white,
                ColorEnum.Blue => Color.blue,
                ColorEnum.Yellow => Color.yellow,
                ColorEnum.Magenta => Color.magenta,
                _ => throw new ArgumentOutOfRangeException(nameof(colorEnum), colorEnum, null)
            };

            RPCInit(SelectedColors, _playerData);
        }

        [Command]
        private void CmdInit()
        {
            _playerData ??= new PlayerData();

            _playerData.Name = $"Player{_index + 1}";
            _playerData.Index = _index;

            RPCInit(SelectedColors, _playerData);
        }

        [ClientRpc]
        private void RPCInit(List<ColorEnum> selectedColors, PlayerData playerData)
        {
            SelectedColors = selectedColors;
            _playerData = playerData;

            if (isOwned) _playerDataProvider.PlayerData = _playerData;
        }

        public enum ColorEnum
        {
            None,
            White,
            Blue,
            Yellow,
            Magenta,
        }
    }
}