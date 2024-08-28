using StackExchange.Redis;

namespace GarnetOOMIssue
{
	internal class Program
	{
		static void Main(string[] args)
		{
			int expire_in_sec_min = 5;
			int expire_in_sec_max = 15;
			int read_delay = 10;
			int read_delay_counter = 0;
			List<StringBlock> blocks = new List<StringBlock>();
			ConnectionMultiplexer redis = ConnectionMultiplexer.Connect("localhost");
			IDatabase db = redis.GetDatabase();
			while (true)
			{
				blocks.RemoveAll(x => x.expire <= DateTime.Now);
				int totalsize = 0;
				foreach (StringBlock item in blocks)
				{
					totalsize += item.length;
				}
				Console.WriteLine("Total size in KB: " + (totalsize/1000));
				for (int i = 0; i < 20; i++)
				{
					if (totalsize < 300 * 1000 * 1000)
					{
						int size = Random.Shared.Next(10, 90 * 1000 * 1000);
						StringBlock block = new StringBlock();
						block.data = Stringmockup.CreateLargeString(size, 'a');
						int expire_in_sec = Random.Shared.Next(expire_in_sec_min, expire_in_sec_max);
						block.expire = DateTime.Now.AddSeconds(expire_in_sec);
						blocks.Add(block);
						totalsize += block.length;
						db.StringSet($"{Guid.NewGuid()}", block.data, new TimeSpan(0, 0, expire_in_sec));
					}
				}
				
				read_delay_counter++;
				if (read_delay_counter >= read_delay)
				{
					read_delay_counter = 0;
					long memoryUsage = 0; 
					var keys = redis.GetServer("localhost:6379").Keys();
					
					// Display all keys and their values
					foreach (var key in keys)
					{
						long? size_key = (long?)db.Execute("MEMORY", "USAGE", key);
						if (size_key.HasValue)
						{
							memoryUsage += size_key.Value;
						}
						//var value = db.StringGet(key);					
					}
					Console.WriteLine("Read all Keys and their values");
					Console.WriteLine($"Size: {memoryUsage/1000} KB");
				}

				Thread.Sleep(1000);
			}
		}
	}
}
