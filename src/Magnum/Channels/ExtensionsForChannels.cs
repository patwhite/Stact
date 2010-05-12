// Copyright 2007-2008 The Apache Software Foundation.
//  
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use 
// this file except in compliance with the License. You may obtain a copy of the 
// License at 
// 
//     http://www.apache.org/licenses/LICENSE-2.0 
// 
// Unless required by applicable law or agreed to in writing, software distributed 
// under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR 
// CONDITIONS OF ANY KIND, either express or implied. See the License for the 
// specific language governing permissions and limitations under the License.
namespace Magnum.Channels
{
	using System;
	using System.Collections.Generic;
	using Configuration;

	public static class ExtensionsForChannels
	{
		public static ChannelSubscription Subscribe<T>(this Channel<T> channel, Action<SubscriptionConfigurator> subscriberActions)
		{
			var subscriber = new TypedSubscriptionConfigurator<T>(channel);

			subscriberActions(subscriber);

			return subscriber.Complete();
		}

		public static ChannelSubscription Subscribe(this UntypedChannel channel, Action<SubscriptionConfigurator> subscriberActions)
		{
			var subscriber = new UntypedSubscriptionConfigurator(channel);

			subscriberActions(subscriber);

			return subscriber.Complete();
		}

		public static IEnumerable<Channel> Flatten(this UntypedChannel channel)
		{
			return new FlattenChannelVisitor().Flatten(channel);
		}

		public static IEnumerable<Channel> Flatten<T>(this Channel<T> channel)
		{
			return new FlattenChannelVisitor().Flatten(channel);
		}
	}
}