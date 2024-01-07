using System.Collections.Generic;
using Oxide.Core;
using UnityEngine;

namespace Oxide.Plugins
{
    [Info("StartCrates", "HaniXavi", "1.0")]
    [Description("Hackable Crates Event")]

    class StartCrates : RustPlugin
    {
        private const string permissionName = "startcrates.use";

        private void Init()
        {
            permission.RegisterPermission(permissionName, this);
        }

        [ChatCommand("startcrates")]
        private void StartCratesCommand(BasePlayer player, string command, string[] args)
        {
            if (!permission.UserHasPermission(player.UserIDString, permissionName))
            {
                SendReply(player, "You don't have permission to use this command.");
                return;
            }

            Vector3 spawnPosition = player.transform.position;
            SpawnLockedCrates(spawnPosition, player.transform.rotation);

            string playerName = player.displayName;
            string message = $"<color=#ff0000>Hackable crates event has been started !</color>";
            PrintToChat(message);
        }

        [ChatCommand("removecrates")]
        private void RemoveCratesCommand(BasePlayer player, string command, string[] args)
        {
            if (!permission.UserHasPermission(player.UserIDString, permissionName))
            {
                SendReply(player, "You don't have permission to use this command.");
                return;
            }

            RemoveSpawnedCrates();
            SendReply(player, "All spawned crates have been removed.");
        }

        private void SpawnLockedCrates(Vector3 spawnPosition, Quaternion rotation)
        {
            Vector3[] cratePositions = new Vector3[4];

            cratePositions[0] = spawnPosition + rotation * new Vector3(5, 0, 4);
            cratePositions[1] = spawnPosition + rotation * new Vector3(-5, 0, 4);
            cratePositions[2] = spawnPosition + rotation * new Vector3(-5, 0, -4);
            cratePositions[3] = spawnPosition + rotation * new Vector3(5, 0, -4);

            for (int i = 0; i < cratePositions.Length; i++)
            {
                SpawnCrate(cratePositions[i]);
            }
        }

        private void SpawnCrate(Vector3 position)
        {
            BaseEntity crate = GameManager.server.CreateEntity("assets/prefabs/deployable/chinooklockedcrate/codelockedhackablecrate.prefab", position);
            if (crate != null)
            {
                crate.Spawn();
            }
        }

        private void RemoveSpawnedCrates()
        {
            BaseEntity[] crates = UnityEngine.Object.FindObjectsOfType<BaseEntity>();
            foreach (BaseEntity entity in crates)
            {
                if (entity.PrefabName == "assets/prefabs/deployable/chinooklockedcrate/codelockedhackablecrate.prefab")
                {
                    entity.Kill();
                }
            }
        }
    }
}
