using System;
using System.Threading;
using Grpc.Net.Client;

namespace StatsCompanion
{
    /// <summary>
    /// A class for handling the connection to SNI and requesting memory reads.
    /// </summary>
    internal class SniConnection
    {
        private const string Sni_Address = "http://localhost:8191/";
        private readonly GrpcChannel sniChannel = GrpcChannel.ForAddress(Sni_Address);

        // Devices client.
        private readonly Devices.DevicesClient devicesClient;

        // Memory client
        private readonly DeviceMemory.DeviceMemoryClient memoryClient;

        // Reading memory requires passing 2 messages to the service: ReadMemoryRequest and SingleReadMemoryRequest.
        private readonly ReadMemoryRequest readMemoryRequest;
        private readonly SingleReadMemoryRequest singleReadMemoryRequest;

        public SniConnection()
        {
            devicesClient = new Devices.DevicesClient(sniChannel);

            memoryClient = new DeviceMemory.DeviceMemoryClient(sniChannel);

            readMemoryRequest = new ReadMemoryRequest
            {
                RequestMemoryMapping = MemoryMapping.HiRom,
            };

            singleReadMemoryRequest = new SingleReadMemoryRequest
            {
                Uri = "",
                Request = readMemoryRequest
            };
        }

        /// <summary>
        /// Method that attempts to retrieve the first device connected to SNI.
        /// If it succeeds, it will set the request URI for that device.
        /// Otherwise, it will keep retrying until it succeeds.
        /// </summary>
        public void ResetConnection()
        {
            try
            {
                var devicesList = devicesClient.ListDevices(new DevicesRequest { }).Devices[0];
                singleReadMemoryRequest.Uri = devicesList.Uri;
                readMemoryRequest.RequestAddressSpace = AddressSpace.SnesAbus;
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Connection to SNI successful!");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine($"Tracking device URI: {singleReadMemoryRequest.Uri}");
                Console.WriteLine($"Address space: {readMemoryRequest.RequestAddressSpace}");
            }
            catch (ArgumentOutOfRangeException)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Device not found or connection lost! Make sure your device/emulator is correctly connected to SNI.");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("Retrying in 10 seconds...");
                Thread.Sleep(10000);
                ResetConnection();
            }
            catch (Grpc.Core.RpcException)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Error - SNI not found! Make sure it's open and connected to your device/emulator.");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("Retrying in 10 seconds...");
                Thread.Sleep(10000);
                ResetConnection();
            }
        }

        /// <summary>
        /// Method that reads a specific memory address in the game through the gRPC API and returns its value.
        /// </summary>
        /// <param name="address">The memory address to read.</param>
        /// <param name="size">The size of the request in bytes.</param>
        /// <returns>A byte array containing the data of each byte.</returns>
        public byte[] ReadMemory(uint address, uint size)
        {
            try
            {
                readMemoryRequest.RequestAddress = address;
                readMemoryRequest.Size = size;
                byte[] response = memoryClient.SingleRead(singleReadMemoryRequest).Response.Data.Memory.ToArray();
                return response;
            }
            catch
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Error while reading memory! Attempting reconnection.");
                ResetConnection();
                return ReadMemory(address, size);
            }
        }
    }
}
