using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Technoguyfication.Notpad.Shared.Net;
using Technoguyfication.Notpad.Shared.Net.Packets;
using Technoguyfication.Notpad.Shared.Net.Structs;

namespace Technoguyfication.Notpad.Net
{
	class ClientUser : NetworkedUser
	{
		private NetworkClient _client;

		public ClientUser()
		{

		}

		public override void SetUsername(string username)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Queries a remote server for information such as server name, motd, and user count
		/// </summary>
		/// <param name="endpoint">The endpoint of the server to query</param>
		/// <param name="timeout">How long to wait for the query before timing out with a <see cref="TimeoutException"/></param>
		/// <returns></returns>
		/// <exception cref="IncompatibleProtocolVersionException">Thrown if we try to query a server with an incompatible protocol version</exception>
		/// <exception cref="IOException">Thrown if we are unable to communicate with the server</exception>
		/// <exception cref="SocketException">Thrown if we are unable to establish a connection to the server</exception>
		/// <exception cref="TimeoutException">Thrown if the operation does not complete within the specified amount of time</exception>
		public static async Task<ServerInfo> QueryServer(IPEndPoint endpoint, TimeSpan timeout)
		{
			using var queryClient = new NetworkClient();

			// create task to run the query since we need to handle a timeout for this entire function
			var queryTask = Task.Run(async () =>
			{
				// try to connect to the server
				await queryClient.ConnectAsync(endpoint).ConfigureAwait(false);

				// send handshake with query intent
				queryClient.WritePacket(new SHandshakePacket()
				{
					Intent = SHandshakePacket.HandshakeIntent.Query,
					ProtocolVersion = Protocol.Version
				});

				// read reply
				var reply = queryClient.ReadPacket();

				// handle possible responses
				if (reply is CQueryResponsePacket qrpkt)
				{
					return qrpkt.ServerInfo;
				}
				else if (reply is CInvalidVersionPacket ivpkt)
				{
					throw new IncompatibleProtocolVersionException(ivpkt.ServerVersion);
				}
				else
				{
					throw new IOException($"Invalid packet received from server. It may be running a vastly incompatible protocol version that does not recognize handshake packets from version {Protocol.Version}");
				}
			});

			try
			{
				// this throws a TimeoutException if the Task does not complete within the timespan
				// but the Task will continue running in the background so we manually dispose of 
				// the network client
				return await queryTask.WaitAsync(timeout);
			}
			catch (TimeoutException)
			{
				queryClient.Close();
				throw;
			}
		}
	}
}
