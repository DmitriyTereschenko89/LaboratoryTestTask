namespace EventBus.Messages.Extentions;

public static class RetryHelperTask
	{
		public static async Task<TResult> RetryOnExceptionResultSimpleAsync<TResult>(int times, TimeSpan delay, Func<Task<TResult>> operation)
		{
			return await RetryOnExceptionResultAsync<Exception, TResult>(times, delay, operation);
		}

		public static async Task<TResult> RetryOnExceptionResultAsync<TException, TResult>(int times, TimeSpan delay, Func<Task<TResult>> operation) where TException : Exception
		{
			var attempts = 0;
			while (true)
			{
				try
				{
					attempts++;
					return await operation();
				}
				catch (TException ex) when (times < 0 || attempts != times)
				{
					await CreateDelayForException(attempts, delay, ex);
				}
			}
		}

		private static Task CreateDelayForException(int attempts, TimeSpan delay, Exception ex)
		{
			int __delayInSeconds = delay == TimeSpan.FromSeconds(0)
												? IncreasingDelayInSeconds(attempts, ex.Message)
												: (int)delay.TotalSeconds;

			return Task.Delay(TimeSpan.FromSeconds(__delayInSeconds));
		}

		internal static readonly int[] DelayPerAttemptInSeconds =
		{
			(int) TimeSpan.FromSeconds(2).TotalSeconds,
			(int) TimeSpan.FromSeconds(5).TotalSeconds,
			(int) TimeSpan.FromSeconds(10).TotalSeconds,
			(int) TimeSpan.FromSeconds(15).TotalSeconds,
			(int) TimeSpan.FromSeconds(30).TotalSeconds
		};

		private static int IncreasingDelayInSeconds(int failedAttempts, string exMessage)
		{
			if (failedAttempts <= 0)
			{
				throw new ArgumentOutOfRangeException(exMessage);
			}

			return failedAttempts > DelayPerAttemptInSeconds.Length ? DelayPerAttemptInSeconds[^1] : DelayPerAttemptInSeconds[failedAttempts];
		}
	}
