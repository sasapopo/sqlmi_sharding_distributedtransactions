// This code sample shows how you can within a single distributed transactions 
// connect to different shards and move the data from one to another.
//
internal void ReassignOrder(int orderId, int fromCustomerId, int toCustomerId)
{
	string connectionString = string.Format("User ID={0};Password={1}", Login, P(Password));
	shardMapManager = GetShardMapManager(ShardMapServerName, Login, Password, ShardMapDatabaseName);

	// Within a single distributed transaction, connect to two different shards hosted on different
	// Azure SQL Managed Instance and move the data from one shard to another.
	//
	using (TransactionScope scope = new TransactionScope())
	{
		Order order = new Order();
		var shardMap = GetListShardMap<int>(shardMapManager, "ShardMapDemo");

		// Connect to the source shard and get the data from it.
		//
		using (var connectionToSourceShard = shardMap.OpenConnectionForKey(key: fromCustomerId, connectionString: connectionString))
		{
			DeleteOrderFromSource(orderId, toCustomerId, order, connectionToSourceShard);
		}

		// Connect to the destination shard and insert the data.
		//
		using (var connectionToDestinationShard = shardMap.OpenConnectionForKey(key: toCustomerId, connectionString: connectionString))
		{
			InsertOrderIntoDestination(order, connectionToDestinationShard);
		}

		scope.Complete();
	}
}