// This code sample shows how you can update shard map metadata and customer data within one distributed transaction.
//
internal void MoveCustomer(
		int customerId,
		string sourceServer,
		string sourceDatabase,
		string destinationServer,
		string destinationDatabase)
{
	// Update ShardMap and move customer data within one distributed transaction accross
	// shardmap database and source and destination shard, all of which can be hosted on
	// different Azure SQL Managed Instances.
	//
	using (var scope = new TransactionScope())
	{
		int customerId = customerId;
		ListShardMap<int> shardMap = DemoShardMap;

		// Set customer mapping status to offline.
		// When customer status is offline, read-write connections cannot be opened via ShardMap.
		//
		PointMapping<int> mapping = ChangeShardMappingStatus(customerId, MappingStatus.Offline, shardMap);

		// Move customer data from source to destination database.
		//
		MoveCustomerData(customerId, sourceServer, sourceDatabase, destinationServer, destinationDatabase);

		// Delete original customer mapping and create a new one on the destination server.
		//
		shardMap.DeleteMapping(mapping);
		Shard destinationShard = shardMap.GetShard(new ShardLocation(destinationServer, destinationDatabase));
		shardMap.CreatePointMapping(customerId, destinationShard);

		// Commit transaction.
		//
		scope.Complete();
	}
}