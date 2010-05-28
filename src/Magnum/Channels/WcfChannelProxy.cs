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
	using System.ServiceModel;
	using Extensions;
	using Fibers;
	using Internal;
	using Serialization;

	public class WcfChannelProxy<T> :
		Channel<T>
	{
		private readonly EndpointAddress _address;
		private readonly Fiber _fiber;
		private readonly WcfChannel<T> _proxy;

		public WcfChannelProxy(Fiber fiber, Uri serviceUri, string pipeName)
		{
			_fiber = fiber;
			Serializer = new FastTextSerializer();
			ServiceUri = serviceUri;
			PipeName = pipeName;

			_address = new EndpointAddress(serviceUri.AppendPath(pipeName));
			_proxy = System.ServiceModel.ChannelFactory<WcfChannel<T>>.CreateChannel(new NetNamedPipeBinding(), _address);
		}

		public Serializer Serializer { get; private set; }

		public Uri ServiceUri { get; private set; }

		public string PipeName { get; private set; }

		public void Send(T message)
		{
			_fiber.Add(() => _proxy.Send(message));
		}
	}
}