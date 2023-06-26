using Grpc.Net.Client;

namespace StatsCompanion
{
    /// <summary>
    /// A class for handling the connection to SNI and requesting memory reads.
    /// </summary>
    internal class SniConnection
    {
        private const string SniAddress = "http://localhost:8191/";
        private readonly GrpcChannel _sniChannel = GrpcChannel.ForAddress(SniAddress);

        // Devices client.
        private readonly Devices.DevicesClient _devicesClient;

        // Memory client
        private readonly DeviceMemory.DeviceMemoryClient _memoryClient;

        // Reading memory requires passing 2 messages to the service: ReadMemoryRequest and SingleReadMemoryRequest.
        private readonly ReadMemoryRequest _readMemoryRequest;
        private readonly SingleReadMemoryRequest _singleReadMemoryRequest;

        private int _requestTimer;

        public int RequestTimer { get; set; }

        public SniConnection()
        {
            _devicesClient = new Devices.DevicesClient(_sniChannel);

            _memoryClient = new DeviceMemory.DeviceMemoryClient(_sniChannel);

            _readMemoryRequest = new ReadMemoryRequest
            {
                RequestMemoryMapping = MemoryMapping.HiRom,
            };

            _singleReadMemoryRequest = new SingleReadMemoryRequest
            {
                Uri = "",
                Request = _readMemoryRequest
            };
            
            RequestTimer = 0;
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
                var devicesList = _devicesClient.ListDevices(new DevicesRequest { }).Devices[0];
                _singleReadMemoryRequest.Uri = devicesList.Uri;
                _readMemoryRequest.RequestAddressSpace = AddressSpace.SnesAbus;
                Log.ConnectionSuccessful(_singleReadMemoryRequest.Uri, _readMemoryRequest.RequestAddressSpace.ToString());
            }
            catch (System.ArgumentOutOfRangeException)
            {
                string message = "Device not found! Make sure your device/emulator is correctly connected to SNI.";
                Log.ConnectionError(message);
                ResetConnection();
            }
            catch (Grpc.Core.RpcException)
            {
                string message = "Error - SNI not found! Make sure it's open and connected to your device/emulator.";
                Log.ConnectionError(message);
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
                _readMemoryRequest.RequestAddress = address;
                _readMemoryRequest.Size = size;
                byte[] response = _memoryClient.SingleRead(_singleReadMemoryRequest).Response.Data.Memory.ToArray();
                return response;
            }
            catch
            {
                ResetConnection();
                return ReadMemory(address, size);
            }
        }
    }
}
