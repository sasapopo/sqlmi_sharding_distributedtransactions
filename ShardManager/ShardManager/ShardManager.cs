using Microsoft.Azure.SqlDatabase.ElasticScale.ShardManagement;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Text.Json;

namespace ShardManager
{
    class ShardManager
    {
        public ShardManager()
        {
            Login = null;
        }

        internal string ShardMapServer { get; set; }

        internal string ShardMapDatabase { get; set; }

        internal string Setup(JsonDocument shardMapDefinition)
        {
            ShardMapServer = shardMapDefinition.RootElement.GetProperty("shardmap").GetProperty("server").GetString();
            ShardMapDatabase = shardMapDefinition.RootElement.GetProperty("shardmap").GetProperty("database").GetString();

            string sqlShardMapManagerConnectionStringTemplate = "Server={0};User ID={1};Password={2};Initial Catalog = {3}";
            string connectionString = string.Format(
                sqlShardMapManagerConnectionStringTemplate,
                ShardMapServer,
                Login,
                Marshal.PtrToStringUni(Marshal.SecureStringToGlobalAllocUnicode(Password)), ShardMapDatabase);


            ShardMapManager shardMapManager = GetShardMapManager(connectionString);
            ListShardMap<int> shardMap = GetListShardMap<int>(shardMapManager, "ShardMapDemo");

            JsonElement shards = shardMapDefinition.RootElement.GetProperty("shardmap").GetProperty("shards");

            for (int i = 0; i < shards.GetArrayLength(); i++)
            {
                string server = shards[i].GetProperty("server").GetString();
                string database = shards[i].GetProperty("database").GetString();
                Shard shard = GetShard(shardMap, server: server, database: database);
                int minPoint = shards[i].GetProperty("min").GetInt32();
                int maxPoint = shards[i].GetProperty("max").GetInt32();
                for (int j = minPoint; j <= maxPoint; j++)
                {
                    CreateIfNotExistsPointMapping(shardMap, shard, point: j);
                }
            }

            StringBuilder result = new StringBuilder();
            result.AppendLine(shardMap.Name);
            foreach(var s in shardMap.GetShards())
            {
                result.AppendLine(s.Location.Server + " " + s.Location.Database);
                var mappings = shardMap.GetMappings(s);
                foreach (var m in mappings)
                {
                    result.AppendLine(m.Value.ToString());
                }
            }
            return result.ToString();
        }

        internal void DeleteAllShardMaps(string login, SecureString password)
        {
            string sqlShardMapManagerConnectionStringTemplate = "Server={0};User ID={1};Password={2};Initial Catalog = {3}";
            string connectionString = string.Format(
                sqlShardMapManagerConnectionStringTemplate,
                ShardMapServer,
                Login,
                Marshal.PtrToStringUni(Marshal.SecureStringToGlobalAllocUnicode(Password)), ShardMapDatabase);

            ShardMapManager shardMapManager = GetShardMapManager(string.Format(sqlShardMapManagerConnectionStringTemplate, login, password.ToString()));
            foreach (var sm in shardMapManager.GetShardMaps())
            {
                ListShardMap<int> listSm = (ListShardMap<int>)sm;

                foreach (PointMapping<int> m in listSm.GetMappings())
                {
                    PointMappingUpdate update = new PointMappingUpdate();
                    update.Status = MappingStatus.Offline;
                    var updatedMapping = listSm.UpdateMapping(m, update);
                    listSm.DeleteMapping(updatedMapping);
                }

                foreach (Shard s in listSm.GetShards())
                {
                    listSm.DeleteShard(s);
                }

                shardMapManager.DeleteShardMap(sm);
            }
        }

        private static Shard GetShard(ListShardMap<int> shardMap, string server, string database)
        {
            Shard shard;
            if (!shardMap.TryGetShard(new ShardLocation(server: server, database: database), out shard))
            {
                shard = shardMap.CreateShard(new ShardCreationInfo(new ShardLocation(server: server, database: database)));
            }

            return shard;
        }

        private static void CreateIfNotExistsPointMapping(ListShardMap<int> shardMap, Shard shard1, int point)
        {
            PointMapping<int> mapping;
            if (!shardMap.TryGetMappingForKey(point, out mapping))
            {
                shardMap.CreatePointMapping(point: point, shard: shard1);
            }
        }

        private static ShardMapManager GetShardMapManager(string sqlShardMapManagerConnectionString)
        {
            ShardMapManager shardMapManager;
            bool shardMapManagerExists = ShardMapManagerFactory.TryGetSqlShardMapManager(
                                 sqlShardMapManagerConnectionString,
                                 ShardMapManagerLoadPolicy.Lazy,
                                 out shardMapManager);

            if (!shardMapManagerExists)
            {
                // Create the Shard Map Manager.
                //
                ShardMapManagerFactory.CreateSqlShardMapManager(sqlShardMapManagerConnectionString);

                shardMapManager = ShardMapManagerFactory.GetSqlShardMapManager(
                        sqlShardMapManagerConnectionString,
                        ShardMapManagerLoadPolicy.Lazy);
            }

            return shardMapManager;
        }

        public static ListShardMap<T> GetListShardMap<T>(ShardMapManager shardMapManager, string shardMapName)
        {
            // Try to get a reference to the Shard Map.
            //
            ListShardMap<T> shardMap;
            bool shardMapExists = shardMapManager.TryGetListShardMap(shardMapName, out shardMap);

            if (!shardMapExists)
            {
                // The Shard Map does not exist, so create it
                //
                shardMap = shardMapManager.CreateListShardMap<T>(shardMapName);
            }

            return shardMap;
        }
        public string Login { get; set; }
        public SecureString Password { get; set; }
    }

}

